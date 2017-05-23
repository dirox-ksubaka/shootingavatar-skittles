using UnityEngine;
using System.Collections;

public class UIAdjustToOriginalTextureSize : MonoBehaviour
{

	UITexture container = null;

	Texture oldTexture = null;
	// Use this for initialization
	void Awake ()
	{
		container = GetComponent<UITexture> ();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (container.mainTexture != oldTexture) {
			oldTexture = container.mainTexture;

			FitScaleToTexture (oldTexture);
		}
	}

	void FitScaleToTexture (Texture texture)
	{
		container.MakePixelPerfect ();
		
		container.width = texture.width;
		container.height = texture.height;
		//enabled = false;
	}
}
