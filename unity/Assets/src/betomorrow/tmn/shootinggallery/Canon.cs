using System;
using System.Collections.Generic;
using UnityEngine;

using ksubaka.sdk;
using ksubaka.physics;

using Touch = ksubaka.input.Touch;

namespace betomorrow.tmn.shootinggallery {

[RequireComponent(typeof(BallGenerator), typeof(AimingAssistance))]
public class Canon : MonoBehaviour {

	public event Action<Canon, BallController> ballSent;

	[SerializeField] TargetPool _targetPool;

	public AnimationCurve gestureSpeedToTravelDistanceMapping;

	public AnimationCurve gestureSizeToElevationMapping;

	public InputManager inputManager { get; set; }

	public bool useStrictBallPicking { get; set; }

	AimingAssistance _aimingAssistance;

	BallGenerator _ballGenerator;

	Touch _gestureStartTouch;

	float _gestureStartTime;

	BallController _pickedBall;

	ICollection<BallController> _sentBalls = new List<BallController>();

	void Start() {
		_sentBalls.Clear();
		_aimingAssistance = GetComponent<AimingAssistance>();
		_ballGenerator = GetComponent<BallGenerator>();

		_targetPool.targetDestroyed += OnTargetDestroyed;
		_ballGenerator.ballSpawned += ball => _sentBalls.Remove(ball);
	}

	void OnDestroy() {
		_targetPool.targetDestroyed -= OnTargetDestroyed;
	}

	void OnTargetDestroyed (TargetController target) {
		_aimingAssistance.ResetAssist();
	}

	void Update() {
		if ((inputManager == null) || (inputManager.touchCount == 0)) { return; }

		Touch touch = inputManager.GetTouch(0);
		if (touch.phase == TouchPhase.Began) {
			_pickedBall = TryToPickBall(touch);
		} else if ((_pickedBall != null) && (touch.phase == TouchPhase.Ended) && !AvatarCreationController.isCameraPanel) {
			TryToLaunchBall(_pickedBall, touch);
			_pickedBall = null;
		}
	}

	BallController TryToPickBall(Touch touch) {
		BallController pickedBall = GetTouchedBall(touch, BallCanBePicked);
		if ((pickedBall == null) && !useStrictBallPicking) {
			pickedBall = GetNearestVisibleBall(_ballGenerator.GetBalls(BallCanBePicked), touch);
		}
		if (pickedBall != null) {
			_gestureStartTouch = touch;
			_gestureStartTime = Time.time;
			pickedBall.NotifyPicked();
		}
		return pickedBall;
	}

	void TryToLaunchBall(BallController ball, Touch touch) {
		Vector2 gestureVector = GetGestureVector(touch);
		if (gestureVector.y <= 0.0f) {
			ball.NotifyReleased();
			return;
		}

		Vector2 screenSpaceGestureVector = GetScreenSpaceGestureVector(gestureVector);
		float targetTravelDistance = GetTargetTravelDistance(screenSpaceGestureVector);
		float targetApogee = GetTargetApogee(screenSpaceGestureVector);
		Vector3 launchDirection = GetLaunchDirection(touch, gestureVector);

		_aimingAssistance.Assist(ball, ref targetTravelDistance, ref targetApogee, ref launchDirection);
		ball.rigidbody.isKinematic = false;
		ball.rigidbody.constraints =RigidbodyConstraints.FreezeRotationY| RigidbodyConstraints.FreezeRotationX;
		ball.rigidbody.velocity = BallisticHelper.GetStartVelocity(launchDirection, targetTravelDistance, targetApogee);
		ball.NotifySent();
		_sentBalls.Add(ball);

		if (ballSent != null) { ballSent(this, ball); }
	}			

	Vector2 GetGestureVector(Touch touch) {
		return touch.position - _gestureStartTouch.position;
	}

	Vector2 GetScreenSpaceGestureVector(Vector2 gestureVector) {
		return new Vector2(gestureVector.x / Screen.width, gestureVector.y / Screen.height);
	}

	float GetGestureSpeed(Vector2 screenSpaceGestureVector) {
		float gestureDuration = Time.time - _gestureStartTime;
		return screenSpaceGestureVector.magnitude / gestureDuration;
	}

	float GetTargetTravelDistance(Vector2 screenSpaceGestureVector) {
		return gestureSpeedToTravelDistanceMapping.Evaluate(GetGestureSpeed(screenSpaceGestureVector));
	}

	float GetTargetApogee(Vector2 screenSpaceGestureVector) {
		return gestureSizeToElevationMapping.Evaluate(screenSpaceGestureVector.magnitude);
	}

	Vector3 GetLaunchDirection(Touch touch, Vector2 gestureVector) {
		Vector3 ballPosition = _pickedBall.transform.position;
		Vector3 targetPosition = GetTargetPosition(touch, gestureVector, ballPosition);
		Vector3 launchDirection = targetPosition - ballPosition;
		launchDirection.y = 0;

		return launchDirection.normalized;
	}

	Vector3 GetTargetPosition(Touch touch, Vector2 gestureVector, Vector3 ballPosition) {
		RaycastHit hit;
		if (touch.Raycast(out hit, _ => !HitObjectIsABall(_))) { return hit.point; }

		Vector2 ballScreenPosition = Camera.main.WorldToScreenPoint(ballPosition);
		Vector3 targetScreenPosition = ballScreenPosition + gestureVector;
		targetScreenPosition.z = _targetPool.transform.position.z - Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(targetScreenPosition);
	}

	bool HitObjectIsABall(RaycastHit hit) {
		return hit.collider.GetComponent<BallController>() != null;
	}

	bool BallCanBePicked(BallController ball) {
		return !_sentBalls.Contains(ball);
	}

	BallController GetTouchedBall(Touch touch, Func<BallController, bool> filter) {
		var touchedBall = touch.GetTouchedObject<BallController>(HitObjectIsABall);
		return filter(touchedBall) ? touchedBall : null;
	}

	BallController GetNearestVisibleBall(ICollection<BallController> balls, Touch touch) {
		float smallestDistanceToBall = float.MaxValue;
		BallController nearestBall = null;
		foreach (var ball in balls) {
			var ballScreenPosition = (Vector2) Camera.main.WorldToScreenPoint(ball.transform.position);
			var distanceToBall = (touch.position - ballScreenPosition).magnitude;
			if ((smallestDistanceToBall > distanceToBall) && ball.renderer.isVisible) {
				nearestBall = ball;
				smallestDistanceToBall = distanceToBall;
			}
		}
		return nearestBall;
	}
}
}
