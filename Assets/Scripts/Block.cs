using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public Transform placeablePrefab;

	public void OnCollisionEnter2D (Collision2D collision) {
		InventoryManager inventory = collision.gameObject.GetComponent<InventoryManager> ();
		if (inventory != null) {
			int index = inventory.AddItemToInventory (placeablePrefab);
			if (index >= 0) {
				Destroy (gameObject);
			}
		}
	}

}
