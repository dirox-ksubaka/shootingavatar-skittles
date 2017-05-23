using System;

using UnityEngine;

using ksubaka.sdk;
using ksubaka.states;

namespace betomorrow.tmn.shootinggallery {

public class TutorialStateImpl : MonoBehaviour, TutorialState {

	public event FailEvent fail;// { add{} remove{} }

	public event SuccessEvent success;

	public static TutorialStateImpl Instance;

	public bool isTutorialState = false;

	public AudioSource _bgm;
	TargetPool _targetPool;

	void Awake()
	{
		Instance = this;
	}

	public void OnStart(Context context) {
		_targetPool = FindObjectOfType<TargetPool>();
		_targetPool.targetDestroyed += OnTargetDestroyed;
		_bgm.Play();
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			OnFail();
		}
	}
	public void OnStop() {
		_targetPool.targetDestroyed -= OnTargetDestroyed;
	}

	void OnTargetDestroyed (TargetController target) {
		
	}
	public void OnWin()
	{
		isTutorialState = false;
		GameManager.Instance.isTutorialPass = true;
		GameManager.Instance._gameProgressionTracker.gameProgression = 10;
		if (success != null) 
		{
			success(); 
		}
	}
	public void OnFail()
	{
		if(fail != null)
		{
			fail();
			print("Send Tutorial Fail Event");
		}
	}

}
}
