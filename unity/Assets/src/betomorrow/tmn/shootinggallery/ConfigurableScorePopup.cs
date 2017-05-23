using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ksubaka.core.unity.ui;
using ksubaka.core.unity.extensions;
using ksubaka.sdk;
using ksubaka.tools;

namespace betomorrow.tmn.shootinggallery {

public class ConfigurableScorePopup : DisplayableObject, ScorePopup {

	[Serializable]
	public class ScoreBasedAnimation {
		public bool mandatory;
		public int minScore;
		public AnimationClip animationToPlay;
	}

	[Serializable]
	public class ScoreBasedPopupConfiguration {
		public int minScore;
		public string headTitleKey;
		public string headMessageKey;
		public string rewardPictureName;
	}

	public event Action skipped;

	[SerializeField] AnimationClip _hideAnimation;

	[SerializeField] ScoreSortOrder _scoreSortOrder;

	[SerializeField] UIButton _skipButton;
	public UIButton skipButton { get { return _skipButton; } }

	[SerializeField] UILabel _highScoreLabel;
	public UILabel highScoreLabel { get { return _highScoreLabel; } }

	[SerializeField] UILabel _scoreLabel;
	public UILabel scoreLabel { get { return _scoreLabel; } }

	[SerializeField] UILabel _headTitle;

	[SerializeField] UILabel _headMessage;

	[SerializeField] UISprite _rewardPicture;

	[SerializeField] List<ScoreBasedAnimation> _animationsToPlay;

	[SerializeField] List<ScoreBasedPopupConfiguration> _configurations;

	[SerializeField] AnimationPlayer _player;

	ScoreManager _scoreManager;

	Locale _locale;
		public int scoreBGTransition;

	public void Initialize(ScoreManager scoreManager, Locale localisationManager) {
		_scoreManager = scoreManager;
		_locale = localisationManager;
		scoreManager.sortOrder = _scoreSortOrder;
		scoreLabel.text = string.Format(scoreLabel.text, scoreManager.currentScore);
		highScoreLabel.text = string.Format(highScoreLabel.text, scoreManager.highScores.First());
		EventDelegate.Add(_skipButton.onClick, OnSkipButtonClicked);
	
		ApplyConfigurationFromScore(scoreManager.currentScore);

	}

	void ApplyConfigurationFromScore(int score) {
		foreach (ScoreBasedPopupConfiguration configuration in _configurations) {
			if (IsScoreSuitable(configuration.minScore, score)) {
				_headTitle.text = _locale[configuration.headTitleKey];
				_headMessage.text = _locale[configuration.headMessageKey];
				_rewardPicture.spriteName = configuration.rewardPictureName;
			}
		}
	}

	void PopulateAnimationsFromScore(int score) {
		List<AnimationClip> clips = new List<AnimationClip>();
		foreach (ScoreBasedAnimation animation in _animationsToPlay) {
			if (IsScoreSuitable(animation.minScore, score) || animation.mandatory) {
				clips.Add(animation.animationToPlay);
			}
		}
		_player.animationClips = clips.ToArray();
	}

	bool IsScoreSuitable(int minScore, int score) {
		return (minScore <= score && _scoreSortOrder == ScoreSortOrder.Descending) ||
				(minScore >= score && _scoreSortOrder == ScoreSortOrder.Ascending);
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		EventDelegate.Remove(_skipButton.onClick, OnSkipButtonClicked);
	}

	public void OnSkipButtonClicked() {
		if (skipped != null) { skipped(); }
	}

	protected override void StartShow(Action endAction) {
		_player.gameObject.SetVisible(true);
		PopulateAnimationsFromScore(_scoreManager.currentScore);
		_player.Play();
		StartCoroutine(_player.gameObject.animation.WaitForAnimationEnd(_ => endAction()));
	}

	protected override void StartHide(Action endAction) {
		_player.gameObject.animation.AddClip(_hideAnimation, _hideAnimation.name);
		_player.gameObject.animation.Play(_hideAnimation.name);
		StartCoroutine(_player.gameObject.animation.WaitForAnimationEnd(_ => endAction()));
		_player.gameObject.SetVisible(false);
	}
		/*public void ClearLevelAnimationLeftToRight ()
		{

			LeanTween.moveLocalX (this.gameObject, 5f, .7f).setEase (LeanTweenType.linear).setOnComplete (() => {
				LeanTween.moveLocalX (this.gameObject, 0f, .3f).setEase (LeanTweenType.easeOutBounce);
			});
			
		}*/
		

}
}
