using UnityEngine;
using System.Collections;
using ksubaka.core;
using ksubaka.sdk;
public class QrCodeController : MonoBehaviour {

	public GameObject handQr;
	public float duration = 3;
	bool isUserClickedCoolbox = false;

	public void OnEnable(){
		//start coroutine
		StartCoroutine(StartCountdown());
	}
	public IEnumerator StartCountdown()
	{
		float currentCountDown = 0;
		while(currentCountDown <= duration){
			yield return new WaitForSeconds(1f);
			currentCountDown++;
		}
		if(!isUserClickedCoolbox){
			handQr.SetActive(true);
		}
	}
	public void OnUserClickCoolerBox(){
		if(!isUserClickedCoolbox){
			isUserClickedCoolbox = true;
			handQr.SetActive(false);
			//stop coroutine
			StopCoroutine(StartCountdown());
		}
	}
	

}
