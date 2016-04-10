using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {

	public float maxHitpoints = 100.0f;
	private float currentHitpoints;

	public float CurrentHitpoints {
		get { return currentHitpoints; }
	}

	private void Start () {
		currentHitpoints = maxHitpoints;
	}

	public void TakeDamage (float damage) {
		currentHitpoints -= damage;

		// TODO replace sprites with damaged versions

		if (currentHitpoints <= 0) {
			// TODO instantiate the inventory block
			Destroy (gameObject);
		}
	}

}
