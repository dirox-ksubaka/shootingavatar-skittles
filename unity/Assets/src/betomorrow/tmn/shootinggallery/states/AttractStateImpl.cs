using UnityEngine;

using ksubaka.sdk;
using ksubaka.states;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace betomorrow.tmn.shootinggallery {

public class AttractStateImpl : MonoBehaviour, AttractState {

		[SerializeField]
		public string attractText;
		[SerializeField]
		public UILabel
			attractTextLable;
		[SerializeField]
		public UITexture
			tapToStartPic, tapToStartPic_CN, tapToStartPic_EN;
		
		[SerializeField]
		public UITexture
			LogoBanner, attractBanner;
		public static bool isAttractMode = true;



		//CoroutineEx _impulseCoroutine;

	public void OnStart(Context context) {
		FindObjectOfType<GameManager>().SetContext(context);
			attractTextLable.text = attractText;
			if (!string.IsNullOrEmpty(attractTextLable.text) && attractTextLable.text.StartsWith ("0x") || attractTextLable.text.StartsWith ("#")) {
				int i = 0;
				for(; i < attractTextLable.text.Length; i++)
				{
					if(attractTextLable.text[i].Equals(' '))
						break;
					if(i == 10)
						break;
				}
				
				attractTextLable.color = hexToColor (attractTextLable.text.Substring(0,i+1));
				if(attractTextLable.text[i].Equals(' '))
					attractTextLable.text = attractTextLable.text.Substring (i+1);
				else
					attractTextLable.text = attractTextLable.text.Substring (i);
			}
			if (tapToStartPic_CN != null && tapToStartPic_EN != null) {
							if (context.locale.identifier == "CN")
								tapToStartPic.mainTexture = tapToStartPic_CN.mainTexture;
							else
								tapToStartPic.mainTexture = tapToStartPic_EN.mainTexture;
						}
			//move in
			LeanTween.moveLocalY (attractBanner.gameObject, 960, 1f);

	}
		public static Color hexToColor (string hex)
		{
			hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
			hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
			byte a = 255;//assume fully visible unless specified in hex
			byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
			//Only use alpha if the string has enough characters
			if (hex.Length >= 8) {
				a = byte.Parse (hex.Substring (6, 2), System.Globalization.NumberStyles.HexNumber);
			}
			return new Color32 (r, g, b, a);
		}

	public void OnStop() {
			isAttractMode = false;
			TutorialStateImpl.Instance.isTutorialState = true;
			//tapToStartPic.gameObject.SetActive (false);
			attractTextLable.gameObject.SetActive (false);
			
			//move out attract banner
			LeanTween.moveLocalY (attractBanner.gameObject, 960 + attractBanner.height, 1f).setOnComplete (() => {
				//if (!PlayStateImpl.isClickWhatCookingBtn)
				if(LogoBanner!= null)
				{

					LogoBanner.gameObject.SetActive(true);
//					LeanTween.moveLocalY (LogoBanner.gameObject, 960, 1f);//move in Logo banner
				}
			});
		}
}
}
