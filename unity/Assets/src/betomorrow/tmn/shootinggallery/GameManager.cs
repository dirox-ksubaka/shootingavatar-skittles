using UnityEngine;
using System.Collections;
using ksubaka.sdk;
using ksubaka.propertystore;
using ksubaka.core.unity;
using System.Collections.Generic;

namespace betomorrow.tmn.shootinggallery
{
	[RequireComponent(typeof(BallGenerator), typeof(Canon))]
	public class GameManager : MonoBehaviour
	{

		[SerializeField]
		TargetPool _targetPool;

		//[SerializeField] AttractTextController _attractTextController;

		[SerializeField]
		[Type(typeof(bool))]
		Property _useStrictBallPicking;
		[SerializeField]
		[Type(typeof(bool))]
		Property _hideHighScore;
		[SerializeField]
		[Type(typeof(string))]
		Property _attractText;
		public
		bool[] availableSeatForCustomers;
		[SerializeField]
		Transform[] seatPositions;
		public bool  canCreateNewCustomer;  //flag to prevent double calls to functions

        [HideInInspector]
        public float WaiterSpawnTime=2f;    //interval of each spawn wave 
        [HideInInspector]
          public  float WaitorLeaveTime=4f  ;             //life time

		static GameManager instance;
		Vector3 waitorStartingPos ;
        public GameObject[] customersPrefabList;    //list of all available customers (different patience and textures)
		[SerializeField] Property _numSkittles;
		public bool isTutorialPass = false;
		public bool isCreateAvatar = false;
		public int avatarID;
		public Texture avatarCustomerTexture;
		public static GameManager Instance {
			get { return instance; }
		}
		public int avatarFed = 0;
        void Awake ()
        {
            instance = this;
            _ballGenerator = GetComponent<BallGenerator> ();
            
            _canon = GetComponent<Canon> ();
            _canon.useStrictBallPicking = _useStrictBallPicking.GetValue<bool> ();
            _canon.ballSent += OnBallSent;
            
            _targetPool.targetDestroyed += OnTargetDestroyed;
            
            //delay = 12; //Optimal value should be between 5 (Always crowded) and 15 (Not so crowded) seconds. 
            //      yield return new WaitForSeconds (.05f);
            canCreateNewCustomer = true;
        }


     

        void Start()
        {
			avatarCustomerTexture = new Texture();
            canCreateNewCustomer = true;
                 StartCoroutine (CheckCustomerCreationRoutine ());
        }

   
        int currentMaxSpawnEachTime=3;
        public IEnumerator CheckCustomerCreationRoutine ()
        {
            //while (TutorialStateImpl.tutMode || PlayStateImpl.playMode) 
//            {
//                CheckEachSeatToCreateNewCustomer (false/* TutorialStateImpl.tutMode*/);
//                yield return new WaitForSeconds (.5f);
//            }

            while(true)
            {
                for (int i=0;i<Random.Range(1, currentMaxSpawnEachTime+1);i++) {
                    CheckToCreateNewCustomer(true);
                }

                yield return new WaitForSeconds (WaiterSpawnTime);
               // yield return new WaitForSeconds (.5f);
            }
        }

        void CheckToCreateNewCustomer (bool createImmediately=false)
        {
            if (canCreateNewCustomer || createImmediately) {
                int numseatAvail = monitorAvailableSeats ();
                if (numseatAvail > 0) {
                    if (numseatAvail == 5)
                        createImmediately = true;
                    StartCoroutine (createCustomer (freeSeatIndex [Random.Range (0, freeSeatIndex.Count)], createImmediately));
                } 
            } 
            
        }
//        void CheckEachSeatToCreateNewCustomer (bool createImmediately=false)
//		{
//			       if (canCreateNewCustomer || createImmediately) 
//			{
//				for (int i = 0; i < availableSeatForCustomers.Length; i++) {
//					if (availableSeatForCustomers [i] == true) {
//						StartCoroutine (createCustomer (i, false/*createImmediately*/));
//						
//					}
//                    
//				}
//			} 
//            
//		}

    

		private List<int> freeSeatIndex = new List<int> ();
		
		int monitorAvailableSeats ()
		{
			freeSeatIndex = new List<int> ();
			for (int i = 0; i < availableSeatForCustomers.Length; i++) {
				if (availableSeatForCustomers [i] == true)
					freeSeatIndex.Add (i);
			}
			
			//debug
			//print("Available seats: " + freeSeatIndex);
			
			return freeSeatIndex.Count;
            
		}

		IEnumerator createCustomer (int _seatIndex, bool createImmediately =false)
		{
            print ("=================create avatar at: "+_seatIndex);
			//set flag to prevent double calls 
			canCreateNewCustomer = false;
            Vector3 seat = seatPositions [_seatIndex].position;
			availableSeatForCustomers [_seatIndex] = false;
			
//			if (!createImmediately)
//				yield return new WaitForSeconds (WaiterSpawnTime);
			
			//if (PlayStateImpl.playMode || AttractStateImpl.attractMode || TutorialStateImpl.tutMode) 
			{
				int customerId = LookForAvaiableCustomerId ();//1,2,3
				if (customerId > 0) {
					GameObject tmpCustomer = customersPrefabList [customerId - 1];
					GameObject newCustomer = Instantiate (tmpCustomer) as GameObject;
                    newCustomer.transform.parent = _targetPool.transform;
                    newCustomer.transform.position  = seat ;//+ transform.position;
                    newCustomer.SetActive(true);
					if(isCreateAvatar)
					{
						int idOfAvatar = newCustomer.GetComponent<TargetController>().targetId;
						CreateAvatarForCustomer(idOfAvatar, newCustomer);
					}
//					if (TutorialStateImpl.tutMode)
					//	tutorialCustomer = newCustomer.GetComponent<TargetController> ();
//					newCustomer.transform.SetParent (GameObject.FindGameObjectWithTag ("worldCanvas").transform);
					newCustomer.GetComponent<TargetController> ().mySeat = _seatIndex;
					StartCoroutine (reactiveCustomerCreation ());
				}
			}

            yield return null;
		}
		void CreateAvatarForCustomer(int id, GameObject customer)
		{
			if(avatarID == id)
			{
				Texture tex = new Texture();
				tex = avatarCustomerTexture;
				customer.gameObject.GetComponent<TargetController>().Ni_Icon.SetActive(true);
				if(customer.gameObject.GetComponentInChildren<MeshRenderer>()!=null)
				{
					customer.gameObject.GetComponentInChildren<MeshRenderer>().materials[0].mainTexture = tex;
					Debug.Log("XXXXXXXXXXXXXXXXXXXXXX Change Avatar");
				}
				else
				{
					Debug.LogError("XXXXXXXXXXXX Can't change Avatar");
				}

			}
		}
        IEnumerator reactiveCustomerCreation ()
        {
           // yield return new WaitForSeconds (WaiterSpawnTime);
            //immediately allow
            canCreateNewCustomer = true;
            yield break;
        }

		int[]customerAvailableID = {1,2,3};
		int customerNumber = 0;
        public int LookForAvaiableCustomerId ()
		{
			return customerAvailableID [customerNumber++ % customerAvailableID.Length];
		
		}
		public GameObject scorePopup {
			set {
				if (!_hideHighScore.GetValue<bool> ()) {
					return;
				}
                
				var highScoreRoot = FindChild (value.transform, "Best score label");
				if (highScoreRoot != null) {
					GameObject.Destroy (highScoreRoot.gameObject);
				}

				var sprite = FindChild (value.transform, "Sprite");
				if (sprite != null) {
					sprite.transform.localPosition = sprite.transform.localPosition + Vector3.down * 100;
				}
			}
		}

		BallGenerator _ballGenerator;
		Canon _canon;
		public GameProgressionTracker _gameProgressionTracker;
		ScoreManager _scoreManager;

		public void SetContext (Context context)
		{
			_scoreManager = context.scoreManager;
			_scoreManager.sortOrder = ScoreSortOrder.Ascending;
			_gameProgressionTracker = context.gameProgressionTracker;
			_canon.inputManager = context.inputManager;

			/*var attractText = _attractText.GetValue<string>();
		if (!string.IsNullOrEmpty(attractText)) {
			_attractTextController.attractText = attractText;
		}*/
		}

		
        
		void OnBallSent (Canon canon, BallController ball)
		{
			if(TutorialStateImpl.Instance.isTutorialState)
			{
				_gameProgressionTracker.gameProgression = 5;
				Debug.Log("XXXXXXXXXXXXXXXXXXX Progression = 5");
			}
			else if(isTutorialPass)
			{
				_scoreManager.currentScore++;
				//				Debug.Log("XXXXXXXXXXXXXXXXXXXScore = "+_scoreManager.currentScore+"   Skittles = "+_numSkittles.GetValue<int>());
				_gameProgressionTracker.gameProgression = 10 + (int)(90*(1f*_scoreManager.currentScore/_numSkittles.GetValue<int>()));
				Debug.Log("XXXXXXXXXXXXXXXXXXX Progression = "+_gameProgressionTracker.gameProgression);
			}

		}

		void OnTargetDestroyed (TargetController target)
		{
//			if(TutorialStateImpl.Instance.isTutorialState)
//			{
//				isTutorialPass = true;
//				_gameProgressionTracker.gameProgression = 10;
//				Debug.Log("XXXXXXXXXXXXXXXXXXX Progression = 10");
//			}

//			_gameProgressionTracker.gameProgression = (100 * _targetPool.destroyedTargetCount) / _targetPool.targetCount;
		}

		public void NotifyWin ()
		{
			_ballGenerator.enabled = false;
			_canon.enabled = false;
			_canon.ballSent -= OnBallSent;
			_targetPool.targetDestroyed -= OnTargetDestroyed;
		}

		Transform FindChild (Transform root, string name)
		{
			Transform child = null;
			for (int i = 0; (i < root.childCount) && (child == null); i++) {
				child = root.GetChild (i);
				if (!string.Equals (child.name, name)) {
					child = FindChild (child, name);
				}
			}
			return child;
		}
	}
}
