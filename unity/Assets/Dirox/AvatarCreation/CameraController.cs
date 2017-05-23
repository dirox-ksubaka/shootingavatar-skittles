using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using betomorrow.tmn.shootinggallery;
public class CameraController : MonoBehaviour
{
	public WebCamTexture mCamera = null;
	WebCamDevice[] deviceCameras;
	public GameObject plane;
	public AudioSource cameraShutter;

	public GameObject cameraButton, yesButton, retakeButton;
//	public UITexture PhotoTaken = null;
	
	// Use this for initialization
	void Start ()
	{
		deviceCameras = WebCamTexture.devices;
		Debug.Log ("Script has been started");
		//plane = GameObject.FindWithTag ("Player");

		mCamera = new WebCamTexture ();
		if(deviceCameras.Length == 2)
		{
			mCamera.deviceName = deviceCameras[1].name;
		}
		else
		{
			mCamera.deviceName = deviceCameras[0].name;
		}
		
		plane.GetComponent<UITexture>().mainTexture = mCamera;
//		PhotoTaken.mainTexture = mCamera;
		mCamera.Play ();
//		if(plane.GetComponent<Image>()!=null)
//		{
//			plane.GetComponent<Image>().sprite = Sprite.Create ((Texture2D)PhotoTaken.mainTexture, new Rect (0, 0, PhotoTaken.mainTexture.width, PhotoTaken.mainTexture.height), new Vector2 (0.5f, 0.5f));
//		}


		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("XXXXXXXXXXXXXXXXXXX Mouse Position = "+Input.mousePosition);
		}
	}
	public void TakePhoto()
	{
		cameraShutter.Play();
//		PhotoTaken.mainTexture = plane.GetComponent<UITexture>().mainTexture;
		mCamera.Stop();
		cameraButton.SetActive(false);
		yesButton.SetActive(true);
		retakeButton.SetActive(true);
//		StartCoroutine(CreatingPNG());
	}
	public void RetakePhoto()
	{
		cameraButton.SetActive(true);
		yesButton.SetActive(false);
		retakeButton.SetActive(false);
		mCamera.Play();

	}
	public void Confirm()
	{
		//do something here
		Time.timeScale = 1f;
		AvatarCreationController.isCameraPanel = false;
		GameManager.Instance.avatarID = gameObject.GetComponentInChildren<ListPositionCtrl>()._currentID;
		GameManager.Instance.isCreateAvatar = true;
		GameManager.Instance.avatarCustomerTexture = plane.GetComponent<UITexture>().mainTexture;
		gameObject.SetActive(false);
	}
//	IEnumerator CreatingPNG()
//	{
//		yield return new WaitForEndOfFrame();
//		int width = plane.GetComponent<UITexture>().width;
//		int height = plane.GetComponent<UITexture>().height;
//		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
//
//		// Read screen contents into the texture
//		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//		tex.Apply();
//		//		PhotoTaken = new Texture2D(mCamera.width, mCamera.height);
//		//		PhotoTaken = (Texture2D) plane.GetComponent<UITexture>().mainTexture;
//		byte[] bytes = tex.EncodeToPNG();
//		Object.Destroy(tex);
//		
//		// For testing purposes, also write to a file in the project folder
//		File.WriteAllBytes(Application.dataPath + "/SavedScreen.png", bytes);
//		Debug.Log("XXXXXXXXXXXXXX"+Application.dataPath);
//	}

}