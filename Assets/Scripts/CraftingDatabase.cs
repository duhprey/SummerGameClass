using UnityEngine;
using System.Collections;

public class CraftingDatabase : MonoBehaviour {

	[System.Serializable]
	public class Entry {
		public Transform itemPrefab;
		public int countProduced;
		public Transform[] requiredItem;
		public int requiredCount;
	};
	public Entry[] crafts;

	static private CraftingDatabase _instance = null;
	static public CraftingDatabase instance {
		get {
			if (_instance == null) {
				_instance = (CraftingDatabase) FindObjectOfType (typeof(CraftingDatabase));
				if (_instance == null) { 
					Debug.LogError ("There is no CraftingDatabase in the scene!");
				}
			}
			return _instance;
		}
	}

	void Awake () {
		if (_instance != null) {
			Debug.Log ("There are more than one CraftingDatabases in the scene!");
			DestroyImmediate (gameObject);
		} else {
			_instance = this;
		}
	}
}
