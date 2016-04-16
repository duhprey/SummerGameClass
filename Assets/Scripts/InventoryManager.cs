using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	public Toggle[] inventorySlots;
	public Text[] inventoryLabels;

	int firstEmpty = 0;

	class InventoryItem {
		public int index;
		public int count;
	}
	private Dictionary<Transform, InventoryItem> inventory;

	private void Start () {
		inventory = new Dictionary<Transform, InventoryItem> ();
		for (int i = 0; i < inventoryLabels.Length; i ++) {
			inventoryLabels[i].text = "";
		}
	}

	public void OnCollisionEnter2D (Collision2D collision) {
		Block block = collision.gameObject.GetComponent<Block> ();
		if (block != null) {
			Transform prefab = block.placeablePrefab;
			if (inventory.ContainsKey (prefab) && inventory[prefab].count < 99) {
				inventory[prefab].count ++;
			} else {
				InventoryItem item = new InventoryItem ();
				item.count = 1;
				item.index = firstEmpty;
				firstEmpty ++;
				inventory.Add (prefab, item);
			}
			UpdateGUI (prefab);
			Destroy (block.gameObject);
		}
	}

	private void UpdateGUI (Transform prefab) {
		int index = inventory[prefab].index;
		inventorySlots[index].graphic.GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
		inventoryLabels[index].text = "" + inventory[prefab].count;
	}
}
