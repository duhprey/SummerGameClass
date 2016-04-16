using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {

	public float maxHitpoints = 100.0f;
	private float currentHitpoints;

	public Sprite damaged1;
	public Sprite damaged2;
	public Sprite damaged3;

	public Transform[] collectablePrefabs;

	public float CurrentHitpoints {
		get { return currentHitpoints; }
	}

	private void Start () {
		currentHitpoints = maxHitpoints;
	}

	public void TakeDamage (float damage) {
		currentHitpoints -= damage;

		if (currentHitpoints / maxHitpoints < .25f && damaged3 != null) {
			GetComponent<SpriteRenderer> ().sprite = damaged3;
		} else if (currentHitpoints / maxHitpoints < 0.5f && damaged2 != null) {
			GetComponent<SpriteRenderer> ().sprite = damaged2;
		} else if (currentHitpoints / maxHitpoints < 0.75f && damaged1 != null) {
			GetComponent<SpriteRenderer> ().sprite = damaged1;
		}

		if (currentHitpoints <= 0) {
			Collider2D col = GetComponent<Collider2D>();
			if (collectablePrefabs != null) {
				foreach (Transform prefab in collectablePrefabs) {
					if (prefab != null) {
						Instantiate (prefab, transform.position + Vector3.Scale (col.bounds.size * 0.5f, Random.insideUnitCircle), Quaternion.identity);
					}
				}
			}
			Destroy (gameObject);
		}
	}

}
