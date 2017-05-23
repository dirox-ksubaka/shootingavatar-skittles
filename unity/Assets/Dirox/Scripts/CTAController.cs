using UnityEngine;
using System.Collections;
using ksubaka.sdk;

using ksubaka.core.unity.extensions;
using ksubaka.states;

public class CTAController : MonoBehaviour {

	public GameObject groupDefaultCN, groupDefaultEN;
	public GameObject groupNewCN, groupNewEN;
	// Use this for initialization
	void Start () {
	
	}

	public void setupCTA(Context context, bool showDefault){
		if(context != null){
			if(context.locale.identifier == "CN"){
				if(showDefault){
					groupDefaultCN.SetActive(true);
					groupDefaultEN.SetActive(false);
					disableGroupDefault();
				}else{
					groupNewCN.SetActive(true);
					groupNewEN.SetActive(false);
					disableGroupNew();
				}
			}else{
				if(showDefault){
					groupDefaultCN.SetActive(false);
					groupDefaultEN.SetActive(true);
					disableGroupNew();
				}else{
					groupNewCN.SetActive(false);
					groupNewEN.SetActive(true);
					disableGroupDefault();
				}
			}
		}
	}

	void disableGroupDefault(){
		groupDefaultCN.SetActive(false);
		groupDefaultEN.SetActive(false);
	}

	void disableGroupNew(){
		groupNewCN.SetActive(false);
		groupNewEN.SetActive(false);
	}
}
