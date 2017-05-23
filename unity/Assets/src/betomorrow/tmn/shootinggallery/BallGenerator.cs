using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using ksubaka.core.utils;
using ksubaka.core.unity.extensions;

namespace betomorrow.tmn.shootinggallery {

public class BallGenerator : MonoBehaviour {

		[SerializeField]	BallRing ballRing;
        const int BallPoolSize = 1;

	public event Action<BallController> ballSpawned;

	public int generatedBallCount { get; private set; }

	public BallController ballPrefab;
	
	public Transform spawnPoint;

	public float spawnPeriod;

	public Vector3 spawnForce;

    public float falloutPoint;

	List<BallController> _balls = new List<BallController>();

	List<BallController> _deadBalls = new List<BallController>();

	IList<Texture> _ballTextures = new List<Texture>();

	void Start() {
		// Pre-populate ball pool
		_balls.Clear();
		_deadBalls.Clear();
		for (int i = 0; i < BallPoolSize; i++) { CreateBall(); }
		for (int i = BallPoolSize - 1; i >= 0; i--) { DestroyBall(_balls[i]); }

		SpawnBall();
	}

	void Update() {
		DestroyDeadBalls();
            if (GameObject.FindGameObjectsWithTag("ball").Length ==0 ){
                SpawnBall();
                ballRing.DecreaseRing();
             
            }
	}

	void OnDisable() {
	//	CancelInvoke("SpawnBall");
		DestroyDeadBalls();
	}

	public void AddBallTexture(Texture texture) {
		if (!_ballTextures.Contains(texture)) { _ballTextures.Add(texture); }
	}

	void SpawnBall() {
		var ball = CreateBall();

		if (_ballTextures.Count > 0) {
			ball.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = _ballTextures.GetRandom();
		}
		ball.transform.localPosition = Vector3.zero;
		ball.transform.localScale = Vector3.one;
		ball.transform.localRotation = Quaternion.identity;
		if ((generatedBallCount++) % 2 == 1) {
			ball.transform.localRotation = UnityEngine.Random.rotation;
		}

		ball.rigidbody.velocity = Vector3.zero;
		ball.rigidbody.angularVelocity = Vector3.zero;
	//	ImpulseBall(ball);

		if (ballSpawned != null) { ballSpawned(ball); }
	//	Invoke("SpawnBall", spawnPeriod);
	}

	BallController CreateBall() {
		BallController ball;
		if (_deadBalls.Count > 0) {
			ball = _deadBalls[0];
			_deadBalls.Remove(ball);
		} else {
			ball = GameObject.Instantiate(ballPrefab) as BallController;
			ball.transform.parent = spawnPoint;
		}
		ball.gameObject.SetActive(true);
		ball.released += ImpulseBall;
		_balls.Add(ball);
		return ball;
	}

	void DestroyBall(BallController ball) {
		ball.released -= ImpulseBall;
		_balls.Remove(ball);
		_deadBalls.Add(ball);
		if (!ball.IsDestroyed()) {
			ball.gameObject.SetActive(false);
		}
	}

	void ImpulseBall(BallController ball) {
//		ball.rigidbody.AddForce(spawnForce);

		
	}

	void DestroyDeadBalls() {
		for (int i = _balls.Count - 1; i >= 0; i--) {
			BallController ball = _balls[i];
			if (ball.IsDestroyed() || (ball.transform.position.y < falloutPoint)) {
				DestroyBall(ball);
			}
		}
	}

	public ICollection<BallController> GetBalls(Func<BallController, bool> filter) {
		return _balls.Where(ball => filter(ball)).ToList();
	}
}
}
