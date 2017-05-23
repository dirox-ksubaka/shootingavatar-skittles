using UnityEngine;
using System.Collections;

public class BlinkingButton : MonoBehaviour {

	public bool isPulsingGlow = false;
	public float minValue = 150f, maxValue = 255f;
	private float currentValue = 0;
	private Color defaultColor;
	private bool flag = true, flag2 = true, isImage = false;
	public int speed = 50;
	float timePulsingGlow = 0f;
	void Start () {
		if(gameObject.GetComponent<UITexture>()!=null)
		{
			currentValue = gameObject.GetComponent<UITexture>().color.a * 255f;
			defaultColor = gameObject.GetComponent<UITexture>().color;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isPulsingGlow)
		{
			flag2 = true;
			if(currentValue >= maxValue - 1)
				flag = false;
			if(currentValue <= minValue +1)
				flag = true;
			if(!flag)
				currentValue -= Time.deltaTime*speed;
			else
				currentValue += Time.deltaTime*speed;
			currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
			gameObject.GetComponent<UITexture>().color = new Color32(255,255, 255, (byte)(currentValue));
			timePulsingGlow += Time.deltaTime;
		}
		else
		{
			if(flag2)
			{
//				timePulsingGlow = 0f;
				gameObject.GetComponent<UITexture>().color = new Color32(255,255, 255, 255);
				flag2 = false;
			}
			
		}
	}
	void OnEnable()
	{
		StartCoroutine(WaitForBlink());
	}
	IEnumerator WaitForBlink()
	{
		yield return new WaitForSeconds(3f);
		isPulsingGlow = true;
	}
	void OnDisable()
	{
		StopCoroutine("WaitForBlink");
		gameObject.GetComponent<UITexture>().color = new Color32(255,255, 255, 255);
		isPulsingGlow = false;
	}
}
