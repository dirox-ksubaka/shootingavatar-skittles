using UnityEngine;
using System.Collections;

public class AvatarCreationController : MonoBehaviour {
	static AvatarCreationController instance;
	public static AvatarCreationController Instance{
		get{return instance;}
	}

	public GameObject AvatarCreation;
	public static bool isCameraPanel = false;
	void Awake()
	{
		instance = this;
	}
	void OnEnable()
	{
		isCameraPanel = true;
	}
	public void OnYesClick()
	{
		AvatarCreation.SetActive(true);
		gameObject.SetActive(false);
		CustomStatEventSender.SendAvatarCreated(CustomStatEventSender.STATUS_PASS);
	}
	public void OnNoClick()
	{
		Time.timeScale = 1f;
		isCameraPanel = false;
		gameObject.SetActive(false);
		CustomStatEventSender.SendAvatarCreated(CustomStatEventSender.STATUS_FAIL);
	}

}
