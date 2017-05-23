using System;
using System.Collections.Generic;
using UnityEngine;

namespace betomorrow.tmn.shootinggallery {

public class TargetPool : MonoBehaviour {

	public event Action<TargetController> targetDestroyed;

	IList<TargetController> _liveTargets = new List<TargetController>();

	IList<TargetController> _destroyedTargets = new List<TargetController>();

	public int liveTargetCount { get { return _liveTargets.Count; } }

	public int destroyedTargetCount { get { return _destroyedTargets.Count; } }

	public int targetCount { get { return liveTargetCount + destroyedTargetCount; } }

	public void RegisterTarget(TargetController target) {
		_liveTargets.Add(target);
		_destroyedTargets.Remove(target);
		target.destroyed -= OnTargetDestroyed;
		target.destroyed += OnTargetDestroyed;
	}

	void OnTargetDestroyed(TargetController target) {
		target.destroyed -= OnTargetDestroyed;

		_liveTargets.Remove(target);
		if (!_destroyedTargets.Contains(target)) { _destroyedTargets.Add(target); }

		if (targetDestroyed != null) { targetDestroyed(target); }
	}
}
}
