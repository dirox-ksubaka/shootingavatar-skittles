using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using ksubaka.sdk;

namespace betomorrow.tmn.shootinggallery {

public class SimpleScorePopup : MonoBehaviour, ScorePopup {

	public event Action skipped;

	[SerializeField] UIButton _skipButton;
	public UIButton skipButton { get { return _skipButton; } }

	[SerializeField] UILabel _highScoreLabel;
	public UILabel highScoreLabel { get { return _highScoreLabel; } }

	[SerializeField] UILabel[] _scoreLabels;
	public UILabel[] scoreLabel { get { return _scoreLabels; } }

	public void Initialize(ScoreManager scoreManager, Locale locale) {
		scoreManager.sortOrder = ScoreSortOrder.Ascending;
		foreach (UILabel scoreLabel in _scoreLabels) { 
			if (scoreLabel != null) { 
				scoreLabel.text = string.Format(scoreLabel.text, scoreManager.currentScore);
			}
		}
		highScoreLabel.text = string.Format(highScoreLabel.text, scoreManager.highScores.First());
		EventDelegate.Add(_skipButton.onClick, OnSkipButtonClicked);
	}

	void OnDestroy() {
		EventDelegate.Remove(_skipButton.onClick, OnSkipButtonClicked);
	}

	public void OnSkipButtonClicked() {
		if (skipped != null) { skipped(); }
	}
}
}
