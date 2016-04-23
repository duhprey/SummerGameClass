using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	public Toggle[] inventorySlots;
	public Image[] inventoryIcons;
	public Text[] inventoryLabels;

	private int activeSlot;

	class InventoryItem {
		public int index;
		public int count;
	}
	private Dictionary<Transform, InventoryItem> inventory;

	private void Start () {
		inventory = new Dictionary<Transform, InventoryItem> ();
		for (int i = 0; i < inventoryIcons.Length; i ++) {
			inventoryIcons[i].color = Color.clear;
		}

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
				item.index = FirstEmptyIndex ();
				inventory.Add (prefab, item);
				if (activeSlot == item.index) {
					GetComponent<UseEquipment>().SetCurrentEquipment (prefab);
				}
			}
			UpdateGUI (prefab);
			Destroy (block.gameObject);
		}
	}

	private int FirstEmptyIndex () {
		for (int i = 0; i < inventoryIcons.Length; i ++) {
			if (inventoryIcons[i].color == Color.clear) {
				return i;
			}
		}
		return -1;
	}

	private void UpdateGUI (Transform prefab) {
		int index = inventory[prefab].index;
		if (index >= 0)
		{
			inventoryIcons[index].sprite = prefab.GetComponent<SpriteRenderer>().sprite;
			inventoryIcons[index].color = Color.white;
			inventoryLabels[index].text = "" + inventory[prefab].count;
		}
	}

	private Transform FindItem (int index) {
		foreach (Transform item in inventory.Keys) {
			if (inventory[item] != null && inventory[item].index == index) {
				return item;
			}
		}
		return null;
	}

	public void RemoveOne (Transform item) {
		if (item != null && inventory.ContainsKey (item)) {
			inventory[item].count --;
			int count = inventory[item].count;
			int index = inventory[item].index;
			if (count > 0) {
				inventoryLabels[index].text = "" + count;
			} else {
				if (index >= 0) {
					inventoryIcons[index].color = Color.clear;
					inventoryLabels[index].text = "";
				}
				inventory.Remove (item);
				GetComponent<UseEquipment>().SetCurrentEquipment (null);
			}
		}
	}

	public void Slot1Active (bool on) { if (on) SetActiveSlot (0); }
	public void Slot2Active (bool on) { if (on) SetActiveSlot (1); }
	public void Slot3Active (bool on) { if (on) SetActiveSlot (2); }
	public void Slot4Active (bool on) { if (on) SetActiveSlot (3); }
	public void Slot5Active (bool on) { if (on) SetActiveSlot (4); }

	private void SetActiveSlot (int index) {
		activeSlot = index;
		Transform item = FindItem (index);
		GetComponent<UseEquipment>().SetCurrentEquipment (item);
	}
}
