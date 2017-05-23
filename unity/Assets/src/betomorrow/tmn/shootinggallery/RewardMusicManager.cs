using UnityEngine;
using System.Collections;

using ksubaka;

namespace betomorrow.tmn.shootinggallery {

using ksubaka.core.logging;

[RequireComponent(typeof(AudioSource))]
public class RewardMusicManager : MonoBehaviour {

	[SerializeField] AudioClip _musicClip;

	Logger _logger = LoggerManager.GetLogger(typeof(RewardMusicManager));

	// UserFeedbackManager _userFeedbackManager;

	AudioSource _audioSource;

	void Awake() {
		_audioSource = GetComponent<AudioSource>();
	}

	void Start () {
		StartCoroutine(InitializeAfterFrame());
	}

	IEnumerator InitializeAfterFrame() {
		yield return null;
		// _userFeedbackManager = FindObjectOfType<UserFeedbackManager>();
		_audioSource.clip = _musicClip;
		_audioSource.Play();
		// if (_userFeedbackManager != null) {
		// 	_userFeedbackManager.surveyStarted += StopMusic;
		// } else {
		// 	_logger.Warn("Couldn't find user feedback manager.");
		// }
	}


	void StopMusic() {
		_audioSource.Stop();
		UnregisterListener();
	}

	void OnDestroy() {
		StopMusic();
	}

	void UnregisterListener() {
		// if (_userFeedbackManager != null) {
		// 	_userFeedbackManager.surveyStarted -= StopMusic;
		// }
	}
}
}
