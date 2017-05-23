using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ksubaka.states;
using betomorrow.tmn.shootinggallery;
public class BallRing : MonoBehaviour
{
    const float radiusOfCircle = .184f;
    [SerializeField]
    GameObject[] skittlePrefab;
    public int numberOfItemsOnGUI = 3;
    List<Transform> skittleList;
    private List<Vector3> positionList;
	public int ThrowsB4AvatarCreation = -1;
	public GameObject AvatarCreationPanel;
    // Use this for initialization
    void Start ()
    {

        CalculateListPositions (numberOfItemsOnGUI);
        SpwanSkittlesList (numberOfItemsOnGUI);
        ArrangePile (skittleList);

    }

    public void ArrangePile (List<Transform> list, float duration=.3f)
    {
        for (int i=0; i<list.Count; i++) {      
            LeanTween.moveLocal (list [i].gameObject, positionList [i], duration);
        }
        
    }

    public void DecreaseRing ()
    {
    	if(GameManager.Instance.isTutorialPass)
		{
			GameObject.Destroy (skittleList [0].gameObject);
			skittleList.RemoveAt (0);
			CalculateListPositions (skittleList.Count);
			ArrangePile (skittleList);
			if (skittleList.Count <= 0) {
				betomorrow.tmn.shootinggallery.PlayStateImpl.Instance.OnOutOfSkittles ();
				
			}
			if(ThrowsB4AvatarCreation != -1)
			{
				if(ThrowsB4AvatarCreation == 0)
				{
					//Avatar creation
					AvatarCreationPanel.SetActive(true);
					ThrowsB4AvatarCreation--;
					Time.timeScale = 0;
				}
				else
				{
					ThrowsB4AvatarCreation--;
				}
			}
		}

       
            
    }

    void SpwanSkittlesList (int number)
    {
        if (skittleList != null)
            skittleList.Clear ();
        skittleList = new  List<Transform> (number);
    
        for (int i=0; i<number; i ++) {
            GameObject o = GameObject.Instantiate (skittlePrefab [i % skittlePrefab.Length]) as GameObject;
            o.transform.parent = transform;
            o.SetActive (true);
            skittleList.Add (o.transform);
        }
    }

    void CalculateListPositions (int number)
    {
        if (positionList != null)
            positionList.Clear ();

        positionList = new List<Vector3> (number);
        for (int i=0; i< number; i++) {
            positionList.Add (PositionForItem (i, number));
        
        }
    }

    protected Vector3 PositionForItem (int index, int capacity)
    {
        float alpha = 2 * Mathf.PI / capacity; 
        alpha *= (index + 1);
        float y = radiusOfCircle * Mathf.Sin (alpha);
        float x = y / Mathf.Tan (alpha);

        return new Vector3 (x, y, 0);
    }
    

}
