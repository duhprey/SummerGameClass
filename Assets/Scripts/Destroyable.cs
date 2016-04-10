using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {

	public float maxHitpoints = 100.0f;
	private float currentHitpoints;

	public Sprite damaged1;
	public Sprite damaged2;
	public Sprite damaged3;

	public float CurrentHitpoints {
		get { return currentHitpoints; }
	}

	private void Start () {
		currentHitpoints = maxHitpoints;
	}

	public void TakeDamage (float damage) {
		currentHitpoints -= damage;

		if (currentHitpoints / maxHitpoints < .25f) {
			GetComponent<SpriteRenderer> ().sprite = damaged3;
		} else if (currentHitpoints / maxHitpoints < 0.5f) {
			GetComponent<SpriteRenderer> ().sprite = damaged2;
		} else if (currentHitpoints / maxHitpoints < 0.75f) {
			GetComponent<SpriteRenderer> ().sprite = damaged1;
		}

		if (currentHitpoints <= 0) {
			// TODO instantiate the inventory block
			Destroy (gameObject);
		}
	}

}
