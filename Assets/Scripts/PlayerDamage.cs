using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamage : MonoBehaviour {

	public float maxHealth;
	private float currentHealth;

	public Slider healthSlider;

	public Transform spawnPoint;

	public Transform bloodPrefab;

	public void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Enemy") {
			if (bloodPrefab != null) {
				Instantiate (bloodPrefab, collision.contacts[0].point, Quaternion.identity); 
			}
			currentHealth -= 10.0f;

			if (currentHealth <= 0) {
				currentHealth = maxHealth;
				healthSlider.value = 1;
				transform.position = spawnPoint.position;
			} else {
				healthSlider.value = currentHealth / maxHealth;
			}
		}
	}
}
