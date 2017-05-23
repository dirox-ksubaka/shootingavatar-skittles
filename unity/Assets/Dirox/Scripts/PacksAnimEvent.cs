using UnityEngine;
using System.Collections;
using betomorrow.tmn.shootinggallery;

public class PacksAnimEvent : MonoBehaviour {

	public DisplayRewardImpl displayReward;

	// Use this for initialization
	void OnAnimStart(){

	}
	void OnAnimEnd(){
		displayReward.OnAnimPackshotFinished();
	}
}
