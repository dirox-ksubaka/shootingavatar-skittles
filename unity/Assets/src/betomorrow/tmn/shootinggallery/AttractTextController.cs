using UnityEngine;

namespace betomorrow.tmn.shootinggallery {
	
using ksubaka.core.unity.extensions;

public class AttractTextController : MonoBehaviour {
	
	[SerializeField] GameObject _attractTextPopup;
	
	UILabel _attractText;
	
	public string attractText {
		get {
			if (_attractText == null) {
				//_attractText = _attractTextPopup.GetComponentInChildren<UILabel>(true);
			}
			return _attractText.text;
		}
		set {
			if (attractText != value) {
				_attractText.text = value;
				UpdateVisibility();
			}
		}
	}

	void Awake() {
		UpdateVisibility();
	}

	void UpdateVisibility() {
		_attractTextPopup.gameObject.SetVisible(!string.IsNullOrEmpty(attractText));
	}
}
}
