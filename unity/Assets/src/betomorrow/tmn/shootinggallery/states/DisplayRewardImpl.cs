using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using ksubaka;
using ksubaka.core.databinding;
using ksubaka.core.unity.extensions;
using ksubaka.core.utils;
using ksubaka.sdk;
using ksubaka.states;

namespace betomorrow.tmn.shootinggallery {

public class DisplayRewardImpl : MonoBehaviour, RewardState {

	[SerializeField] GameObject[] _rewardScreenGameObjects;

	[SerializeField] GameObject _rewardScreenRoot;

	public bool useKeypad { get; set; }

	ScoreManager _scoreManager;
		public CTAController ctaController;
		public UI2DSpriteAnimation qrCodeAnim ;
		public GameObject qrCodeImage;
		public GameObject packshotCamera;
		public GameObject scoreObject;
		public GameObject rewardpicture;
		Context _context;
		public Animation scoreObjectAnim;

	public void OnStart(Context context) {
			_context = context;
		_scoreManager = context.scoreManager;
		InstantiateGameObjects(_rewardScreenGameObjects);
		_rewardScreenRoot.SetVisible(true);		
			ctaController.setupCTA(context, false);


	}

	public void GetSMSPadArgs(SMSPadArgsAvailableCallback callback) {
		if (useKeypad) { StartCoroutine(WaitForSMSPadArgs(callback)); }
		else { callback(null); }
	}

	void InstantiateGameObjects(GameObject[] gameObjects) {
		gameObjects.Where(go => go != null).ForEach(go => Object.Instantiate(go));
	}

	public void OnStop() {
			CustomStatEventSender.SendAvatarsFed(GameManager.Instance.avatarFed);

		}

	IEnumerator WaitForSMSPadArgs(SMSPadArgsAvailableCallback callback) {
		KeypadController keypadController;
		while ((keypadController = _rewardScreenRoot.GetComponentInChildren<DefaultKeypadController>()) == null) { yield return null; }
		var smsContentDataBinder = new KeyValuePairDataBinder(new Dictionary<string, object>() {
			{ "score", _scoreManager.currentScore },
			{ "highScore", _scoreManager.highScores.ElementAt(0) }
		});
		callback(keypadController, smsContentDataBinder);
	}

		public void startAnimQr(){
			qrCodeAnim.enabled = true;
			float duration = qrCodeAnim.frames.Length/qrCodeAnim.framesPerSecond;
			Invoke("OnAnimQrEnd", duration);
		}

		void OnAnimQrEnd(){
			ctaController.setupCTA(_context, true);
			qrCodeImage.SetActive(true);
		}

		public void OnAnimPackshotFinished(){
			rewardpicture.SetActive(true);
			packshotCamera.SetActive(false);
			Destroy(packshotCamera);

		}
}
}
