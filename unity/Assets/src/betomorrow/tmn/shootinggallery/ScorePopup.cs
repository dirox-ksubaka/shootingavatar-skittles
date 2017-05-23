using UnityEngine;
using System;

using ksubaka.sdk;

namespace betomorrow.tmn.shootinggallery {

public interface ScorePopup {

	event Action skipped;

	GameObject gameObject { get; }

	void Initialize(ScoreManager score, Locale locale);
}
}
