using UnityEngine;
using System.Collections;

public class UseEquipment : MonoBehaviour {

	public Transform aimObject;
	public float range = 1;
	public LayerMask targetLayers;
	public LayerMask groundLayer;
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
			aimObject.position = hit.point;
		} else {
			aimObject.position = transform.position + direction * mag;
		}

		if (Input.GetMouseButtonDown (1)) {
			if (HasGroundNeighbor (aimObject.position)) {
				PlaceObject (aimObject.position);
			}
		}
	}

	private void HitTarget (RaycastHit2D hit) {
		Destroyable destroyable = hit.collider.GetComponent<Destroyable> ();
		
		if (destroyable != null) {
			Instantiate (sparksPrefab, hit.point, Quaternion.identity);
			destroyable.TakeDamage (damage);
		}
	}

	private bool HasGroundNeighbor (Vector3 position) {
		RaycastHit2D hit;
		hit = Physics2D.Raycast (position, Vector3.up, gridSize.y, groundLayer);
		if (hit.collider != null) { return true; }
		hit = Physics2D.Raycast (position, Vector3.down, gridSize.y, groundLayer);
		if (hit.collider != null) { return true; }
		hit = Physics2D.Raycast (position, Vector3.right, gridSize.x, groundLayer);
		if (hit.collider != null) { return true; }
		hit = Physics2D.Raycast (position, Vector3.left, gridSize.x, groundLayer);
		if (hit.collider != null) { return true; }
		return false;
	}

	private void PlaceObject (Vector3 position) {
		if (currentItem != null) {
			position.x -= gridOffset.x;
			position.y -= gridOffset.y;
			int i =	Mathf.RoundToInt (position.x / gridSize.x);
			int j =	Mathf.RoundToInt (position.y / gridSize.y);
			position.x = i * gridSize.x + gridOffset.x;
			position.y = j * gridSize.y + gridOffset.y;
			Instantiate (currentItem, position, Quaternion.identity);
			GetComponent<InventoryManager>().RemoveOne (currentItem);
		}
	}

	public void SetCurrentEquipment (Transform item) {
		currentItem = item;
	}
}
