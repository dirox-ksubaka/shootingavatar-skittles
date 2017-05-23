using System;
using UnityEngine;
using System.Collections;

namespace betomorrow.tmn.shootinggallery
{
    public class TargetController : MonoBehaviour
    {

        public event Action<TargetController> destroyed;

        public int targetId = 0;
        public float falloutPoint;

        public bool isDestroyed { get { return !gameObject.activeSelf; } }

        public int ID;
        public int orderedProductID = 0;                            //Product ID which the customer wants. if left to 0, it randomly chooses a product.
        //set anything but 0 to override it.
        
        public Material[] customerMoods;
        private int moodIndex;
        public int mySeat;                          //Do not modify this.
        public Vector3 destination, startingPos;
        public bool  isCloseEnoughToDeliveryObject;         //Do not modify this.
        
        public float currentPatience;       //current patience of the customer
        Material mat;
        public bool  isLeaving;
		public GameObject Ni_Icon;
        void Awake ()
        {
            GetComponentInParent<TargetPool> ().RegisterTarget (this);

            
            isCloseEnoughToDeliveryObject = false;
            
            mat = GetComponentInChildren<Renderer> ().material;
            moodIndex = 0;
            
            isLeaving = false;

            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject,Vector3.one,.5f).tweenType = LeanTweenType.easeSpring;

            ResetPatience ();

            StartCoroutine (Sit ());

        }

        public void Update ()
        {
            if (transform.position.y < falloutPoint) {
                
            }

      //      updateCustomerMood (); 
        }

        void NotifyDestroyed ()
        {
			if (destroyed != null) {
				destroyed (this);
			}
            gameObject.SetActive (false);
            
        }

       
        
        void OnDestroy ()
        {
			NotifyDestroyed ();
            print ("destroy: " + name);
        }
        
        IEnumerator Sit ()
        {
            
            StartCoroutine (patienceBar ());     
            
            //        StartCoroutine (open (1, requestBubble, .3f, .7f));     
            yield return new WaitForSeconds (.1f);
            
        }

        IEnumerator patienceBar ()
        {
            //        patienceBarSliderFlag = false;
            while (currentPatience > 0) {
                //stop timer in assist mode, but run timer in tutorial mode
                //if (!TutorialStateImpl.assistMode || TutorialStateImpl.tutMode) 
                {
                    currentPatience -= 0.5f;// Time.deltaTime * Application.targetFrameRate * 0.02f;
                    //   print("patience="+currentPatience);
                    //  circleProgress.fillAmount = currentPatience / WaitorLeaveTime;
                }
                
                
                yield return new WaitForSeconds (.5f);
            }
            
            if (currentPatience <= 0) {
                bool hasCreatedInMiddleSit = false;
//              if (TutorialStateImpl.tutMode) 
//              {
//                  //in tut mode, must create customer before leave(destroy)
//                  //so that the tutorialCustomer won't be null 
//                  MainGameController.Instance.CreateCustomerInTutorialMode ();
//                  hasCreatedInMiddleSit = true;
//              }
                StartCoroutine (leave (false, !hasCreatedInMiddleSit));//leave, but should NOT free the seat, since there're already a new customer replaced
                
            }

            
        }

      

        public void ResetPatience ()
        {
            currentPatience = GameManager.Instance.WaitorLeaveTime;
//          circleProgress.fillAmount = 1.0f;
        }

   
        
        public IEnumerator leave (bool immediately=false, bool shouldFreeSeatIndex=true)
        {
            
            //prevent double animation
            if (isLeaving && !immediately)
                yield break;
            
            //set the leave flag to prevent multiple calls to this function
            isLeaving = true;
            
            
            if (shouldFreeSeatIndex)
                GameManager.Instance.availableSeatForCustomers [mySeat] = true;
            
            if (immediately) {
                Destroy (gameObject);
                yield break;
            }
            

            LeanTween.scale(gameObject,Vector3.zero,.2f).setOnComplete (() => {
                Destroy (gameObject);
            }).tweenType =LeanTweenType.easeOutBounce;
            
        }
        
        
    

    }
}
