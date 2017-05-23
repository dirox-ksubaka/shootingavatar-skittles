using UnityEngine;
using System.Collections;

public class OutfitsController : MonoBehaviour {

	public GameObject normal, press;
	public Texture borderBackground;
	public UITexture mainBorder;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClicked()
	{
		DisableAllItem();
		gameObject.GetComponentInParent<ListPositionCtrl>()._currentID = gameObject.GetComponent<ListBox>().listBoxID;
		mainBorder.mainTexture = borderBackground;
		normal.SetActive(false);
		press.SetActive(true);
	}
	void DisableAllItem()
	{
		OutfitsController[] allItem = transform.parent.gameObject.GetComponentsInChildren<OutfitsController>();
		foreach(OutfitsController outfit in allItem)
		{
			outfit.OnClickedOther();
		}

	}
	public void OnClickedOther()
	{
		normal.SetActive(true);
		press.SetActive(false);
	}
}
