using System;
using UnityEngine;
using ksubaka.tools;

using ksubaka.core.events;
using ksubaka.core.services;
using ksubaka.help;

namespace betomorrow.tmn.shootinggallery {

[RequireComponent(typeof(SoundOnCollisionPlayer))]
public class BallController : MonoBehaviour {

	public event Action<BallController> picked;

	public event Action<BallController> released;

	public event Action<BallController> sent;
	
	public bool isSent { get; private set; }
	
	public bool isPicked { get; private set; }

	public AnimationCurve startSpeedCurve;

	public float pickedDrag;

	public AudioClip sendSound;

	public GameObject smokeEffect;

	bool _firstCollision;

	float _startDrag;

	void Reset() {
		startSpeedCurve = AnimationCurve.Linear(-1.0f, 0.05f, 1.0f, 0.05f);
	}

	void Awake() {
		_startDrag = rigidbody.drag;
	}

	void OnEnable() {
		_firstCollision = true;
		isPicked = false;
		isSent = false;
		GetComponent<SoundOnCollisionPlayer>().enabled = false;
	}

	void FixedUpdate () {
//		if (isSent || isPicked) { return; }
//
//		float startSpeed = startSpeedCurve.Evaluate(transform.position.x);
//		rigidbody.AddForce(Vector3.right * startSpeed);
	}

	public void NotifyPicked() {
		isSent = false;
		isPicked = true;
		rigidbody.drag = pickedDrag;
		if (picked != null) { picked(this); }
	}

	public void NotifyReleased() {
		isSent = false;
		isPicked = false;
		rigidbody.drag = _startDrag;
		if (released != null) { released(this); }
	}

	public void NotifySent() {
		isSent = true;
		isPicked = false;
		rigidbody.drag = _startDrag;
		GetComponent<SoundOnCollisionPlayer>().enabled = true;
		audio.volume = 1.0f;
		audio.clip = sendSound;
		audio.Play();

		ServiceRepository.GetService<EventBus>().Publish(new HelpNeedEvent(false));

		if (sent != null) { sent(this); }
	}

	void OnCollisionEnter(Collision collision) {
		if (isSent && _firstCollision) {
			_firstCollision = false;
			Instantiate(smokeEffect, collision.contacts[0].point, new Quaternion());
                if (collision.gameObject.tag == "avatar"){
					if(TutorialStateImpl.Instance.isTutorialState)
					{
						TutorialStateImpl.Instance.OnWin();
					}
                    print("hit: " + collision.gameObject.name);
                    collision.gameObject.GetComponent<TargetController>().currentPatience = 0;//end life
					GameManager.Instance.avatarFed++;
                }
		}
	}
}
}
