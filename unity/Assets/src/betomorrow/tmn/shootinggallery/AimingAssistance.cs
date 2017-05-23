using UnityEngine;

namespace betomorrow.tmn.shootinggallery {

using ksubaka.core.unity;

public class AimingAssistance : MonoBehaviour {

	public int minFailureCount;

	public int maxFailureCount;

	public AnimationCurve maxAssistanceDistance;

	public AnimationCurve assistanceLevel;

	public float minApogee;

	int _assistCount;

	public void Assist(BallController ball, ref float targetTravelDistance, ref float targetApogee, ref Vector3 launchDirection) {
		_assistCount++;
		if (_assistCount < minFailureCount) { return; }

		float assistanceNeed = Mathf.Clamp01((float) (_assistCount - minFailureCount) / (maxFailureCount - minFailureCount));

		float _maxAssistanceDistance = maxAssistanceDistance.Evaluate(assistanceNeed);

		Vector3 ballPosition = ball.transform.position;
		Collider assistedTarget = GetAssistedTarget(targetTravelDistance, targetApogee, launchDirection,
													ballPosition, _maxAssistanceDistance);
		if (assistedTarget == null) { 
                Debug.LogError("assistedTarget == null");
                return; 
            }

		Vector3 ballToTargetVector = assistedTarget.bounds.center - ballPosition;
		float _assistanceLevel = assistanceLevel.Evaluate(assistanceNeed);
		targetTravelDistance = GetAssistedTargetTravelDistance(targetTravelDistance, ballToTargetVector, _assistanceLevel);
		targetApogee = GetAssistedTargetApogee(targetApogee, ballToTargetVector, _assistanceLevel);
		launchDirection = GetAssistedLaunchDirection(launchDirection, ballToTargetVector, _assistanceLevel);
	}

	public void ResetAssist() {
		_assistCount = 0;
	}

	Collider GetAssistedTarget(float targetTravelDistance, float targetApogee, Vector3 launchDirection,
							   Vector3 startPosition, float maxAimingAssistance) {
		RaycastHit hit;
		// We should take ballistic trajectory into account instead of just throwing a ray in ball's direction
		RaycastHit[] hits = Physics.SphereCastAll(startPosition, maxAimingAssistance, launchDirection);
		if (PhysicsHelper.GetNearestRaycastHit(startPosition, hits, out hit, HitObjectIsATarget)) {
			return hit.collider;
		}
		return null;
	}

	float GetAssistedTargetTravelDistance(float targetTravelDistance, Vector3 ballToTargetVector, float assistanceLevel) {
		float perfectTargetTravelDistance = ballToTargetVector.magnitude;
		return assistanceLevel * perfectTargetTravelDistance + (1.0f - assistanceLevel) * targetTravelDistance;
	}

	float GetAssistedTargetApogee(float targetApogee, Vector3 ballToTargetVector, float assistanceLevel) {
		float perfectTargetApogee = ballToTargetVector.y * 2.0f + minApogee;
//            float perfectTargetApogee = ballToTargetVector.y * .5f + minApogee;
		return assistanceLevel * perfectTargetApogee + (1.0f - assistanceLevel) * targetApogee;
	}

	Vector3 GetAssistedLaunchDirection(Vector3 launchDirection, Vector3 ballToTargetVector, float assistanceLevel) {
		 Vector3 perfectLaunchDirection = new Vector3(ballToTargetVector.x, 0.0f, ballToTargetVector.z).normalized;
		return assistanceLevel * perfectLaunchDirection + (1.0f - assistanceLevel) * launchDirection;
	}

	bool HitObjectIsATarget(RaycastHit hit) {
		return hit.collider.GetComponent<TargetController>() != null;
	}
}
}
