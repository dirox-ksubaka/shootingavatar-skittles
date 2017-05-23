using ksubaka.core.unity.io;
using ksubaka.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace betomorrow.tmn.shootinggallery {
public class ScoreBasedPrefabInstancier : MonoBehaviour {

	public Action<GameObject> objectInstantiated;
	
	[SerializeField] string _defaultPrefabUri;

	[SerializeField] List<ScoreGameObjectPair> _scorePrefabs;

	[Serializable]
	struct ScoreGameObjectPair {
		public int score;
		public string gameObjectUri;
	}
	
	public void InstantiatePrefab(int score) {
		string prefabUri = _defaultPrefabUri;
		if (_scorePrefabs.Count != 0) {
			_scorePrefabs = _scorePrefabs.OrderBy(pair => pair.score).ToList();
			foreach (ScoreGameObjectPair pair in _scorePrefabs) {
				if (score < pair.score) {
					prefabUri = pair.gameObjectUri;
					break;
				}
			}
		}
		var factory = new GameObjectFactory();
		factory.objectInstantiated += OnObjectInstantiated;
		factory.CreateGameObject(new Uri(prefabUri).AbsolutePath);
	}

	void OnObjectInstantiated(GameObject obj) {
		if (objectInstantiated != null) {
			objectInstantiated(obj);
		}
	}
}
}