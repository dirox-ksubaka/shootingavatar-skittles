using System;
using UnityEngine;

using ksubaka.sdk;

using Object = UnityEngine.Object;

using ksubaka.core.unity.extensions;
using ksubaka.states;

namespace betomorrow.tmn.shootinggallery {

[RequireComponent(typeof(AudioSource))]
public class DisplayScoreImpl : MonoBehaviour, CongratsState {

	public event SkipEvent skip;

	[SerializeField] AudioClip popupInSound;

	[SerializeField] GameManager _gameManager;

	Context _context;

	AudioSource _audioSource;

	ScorePopup _scorePopup;
		public UITexture scoreBanner;		
		public GameObject scoreBackground;
		public GameObject scoreObject;
		public GameObject scoreRoot;
		public int scoreBGTransition;
		public GameObject packshotCamera;

	public void OnStart(Context context) {
		FadeOutVolumeBGM();

		context.scoreManager.SaveCurrentScore();
		_audioSource = GetComponent<AudioSource>();
		_context = context;		
		if (popupInSound != null) {
			_audioSource.PlayOneShot(popupInSound);
		}

		_gameManager.scorePopup = gameObject;

			scoreBackground.SetActive(true);		
			if(scoreBanner != null){
				scoreBanner.gameObject.SetActive(true);
				LeanTween.moveLocalY (scoreBanner.gameObject, -960, 1f).setLoopType (LeanTweenType.punch).loopCount = 1;
			}
			if(scoreBGTransition == 1){
				TweenPosition tweenPos = scoreObject.GetComponent<TweenPosition>();
				if(tweenPos != null){
					tweenPos.enabled = true;
				}
				scoreObject.SetActive(true);
			}else{
				scoreObject.SetActive(true);
				scoreRoot.SetActive(true);
				initialScorePopUp();
			}

	}
	void FadeOutVolumeBGM()
	{
		TweenVolume volume = TutorialStateImpl.Instance._bgm.gameObject.AddComponent("TweenVolume") as TweenVolume;
		volume.from = 1f;
		volume.to = 0f;
		volume.duration = 4f;

	}
	public void OnStop() {
		_scorePopup.skipped -= OnContinue;
		_scorePopup.gameObject.SetVisible(false);
			scoreBackground.SetActive(false);
			packshotCamera.SetActive(true);
			if(scoreBanner != null){
				LeanTween.moveLocalY (scoreBanner.gameObject, -1920 , 3f );//.setLoopType (LeanTweenType.punch).loopCount = 1;
			}
	}

	void OnContinue() {
		if (skip != null) {	skip(); }
	}
		public void ScoreObjectAnimFinish(){
			if(!scoreRoot.activeSelf){
				scoreRoot.SetActive(true);
				initialScorePopUp();
			}
		}
		void initialScorePopUp(){
			_scorePopup = ObjectExtensions.FindAssignableObjectOfType<ScorePopup>();
			_scorePopup.Initialize(_context.scoreManager, _context.locale);
			_scorePopup.skipped += OnContinue;
			_scorePopup.gameObject.SetActive(false);
			_scorePopup.gameObject.SetVisible(true);
		}
}
}
