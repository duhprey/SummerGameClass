using UnityEngine;
using System.Collections;

public class UseEquipment : MonoBehaviour {

	public Transform aimObject;
	public float range = 1;
	public LayerMask targetLayers;
	public Transform sparksPrefab;

	public float timeBetweenHits = 1.0f;
	private float nextHit = 0.0f;

	private void Update () {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		Vector3 direction = mousePosition - transform.position;
		  
		direction.x = Mathf.Clamp (direction.x, -range, range);
		direction.y = Mathf.Clamp (direction.y, -range, range);
		direction.z = 0;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, 1.0f, targetLayers);
		if (hit.collider != null) {
			if (Input.GetMouseButton (0) && Time.time > nextHit) {
				HitTarget (hit);
				nextHit = Time.time + timeBetweenHits;
			}
			aimObject.position = hit.point;
		} else {
			aimObject.position = transform.position + direction * range;
		}
	}

	private void HitTarget (RaycastHit2D hit) {
		Instantiate (sparksPrefab, hit.point, Quaternion.identity);
	}
}
