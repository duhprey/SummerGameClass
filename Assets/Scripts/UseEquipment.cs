using UnityEngine;
using System.Collections;

public class UseEquipment : MonoBehaviour {

	public Transform aimObject;
	public float range = 1;
	public LayerMask targetLayers;
	public Transform sparksPrefab;

	public float timeBetweenHits = 1.0f;
	private float nextHit = 0.0f;

	public float damage = 10.0f;

	public Vector2 gridOffset;
	public Vector2 gridSize;
	private Transform currentItem;

	private void Update () {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		Vector3 direction = mousePosition - transform.position;
		  
		direction.x = Mathf.Clamp (direction.x, -range, range);
		direction.y = Mathf.Clamp (direction.y, -range, range);
		direction.z = 0;

		float mag = direction.magnitude;
		direction /= mag;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, mag, targetLayers);
		if (hit.collider != null) {
			if (Input.GetMouseButton (0) && Time.time > nextHit) {
				HitTarget (hit);
				nextHit = Time.time + timeBetweenHits;
			}
			if (Input.GetMouseButtonDown (1) && hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
				PlaceObject (hit);
			}
			aimObject.position = hit.point;
		} else {
			aimObject.position = transform.position + direction * mag;
		}
	}

	private void HitTarget (RaycastHit2D hit) {
		Destroyable destroyable = hit.collider.GetComponent<Destroyable> ();
		
		if (destroyable != null) {
			Instantiate (sparksPrefab, hit.point, Quaternion.identity);
			destroyable.TakeDamage (damage);
		}
	}

	private void PlaceObject (RaycastHit2D hit) {
		Vector3 position = hit.collider.transform.position;
		float dx = hit.point.x - position.x;
		float dy = hit.point.y - position.y;
		if (Mathf.Abs (dx) > Mathf.Abs (dy)) {
			position += Mathf.Sign (dx) * gridSize.x * Vector3.right;
		} else {
			position += Mathf.Sign (dy) * gridSize.y * Vector3.up;
		}
		Instantiate (currentItem, position, Quaternion.identity);
		// TODO remove one from the inventory
	}

	public void SetCurrentEquipment (Transform item) {
		currentItem = item;
	}
}
