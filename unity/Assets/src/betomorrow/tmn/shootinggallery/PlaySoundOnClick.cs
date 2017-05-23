using UnityEngine;
using System.Collections;

namespace betomorrow.tmn.shootinggallery {

[RequireComponent(typeof(AudioSource), typeof(UIButton))]
public class PlaySoundOnClick : MonoBehaviour {
	
	[SerializeField] AudioClip soundOnClick;
		
	AudioSource _source;
	
	void Start() {
		_source = GetComponent<AudioSource>();
		EventDelegate.Add(GetComponent<UIButton>().onClick, OnClick);
	}
	
	void OnClick() {
		_source.PlayOneShot(soundOnClick);
	}
}
}