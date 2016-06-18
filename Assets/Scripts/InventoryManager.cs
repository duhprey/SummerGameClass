using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
	public GameObject hud;
	public Image[] hudIcons;
	public Text[] hudLabels;

	public GameObject inventoryWindow;
	public GameObject backpack;
	private Image[] backpackIcons;
	private Text[] backpackLabels;

	private int activeSlot;

	public class InventoryItem {
		public Transform itemPrefab;
		public int count;
		public int index;
	}
	private InventoryItem[] inventory;
	private InventoryItem holding;

	public Image holdingImage;

	private CraftingManager crafting;

	private void Start () {
		int backpackSize = backpack.transform.childCount;
		inventory = new InventoryItem[backpackSize];
		backpackIcons = new Image[backpackSize];
		backpackLabels = new Text[backpackSize];
		for (int i = 0; i < backpackSize; i ++) {
			backpackLabels[i] = backpack.transform.GetChild(i).Find("Text ").GetComponent<Text>();
			backpackLabels[i].text = "";
			backpackIcons[i] = backpack.transform.GetChild(i).Find("Image").GetComponent<Image>();
			backpackIcons[i].color = Color.clear;
		}

		for (int i = 0; i < hudIcons.Length; i ++) {
			hudIcons[i].color = Color.clear;
		}

		for (int i = 0; i < hudLabels.Length; i ++) {
			hudLabels[i].text = "";
		}

		hud.SetActive (true);
		inventoryWindow.SetActive (false);

		holding = null;
		UpdateHolding ();

		crafting = GetComponent<CraftingManager> ();
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			if (!inventoryWindow.activeSelf) {
				GetComponent<UseEquipment>().enabled = false;
				hud.SetActive (false);
				inventoryWindow.SetActive (true);
			} else {
				GetComponent<UseEquipment>().enabled = true;
				hud.SetActive (true);
				// Return the item being held to the first available slot
				if (holding != null) {
					int index = FirstEmptyIndex ();
					inventory[index] = holding;
					UpdateGUI (index);
					holding = null;
					UpdateHolding ();
				}
				inventoryWindow.SetActive (false);
			}
		}

		if (inventoryWindow.activeSelf) {
			holdingImage.transform.position = Input.mousePosition;
		}
	}

	public int AddItemToInventory (Transform prefab) {
		int index = -1;
		if ((index = IndexOf (prefab)) >= 0 && inventory[index].count < 99) {
			inventory[index].count ++;
			if (crafting != null) {
				crafting.UpdateSats (inventory[index]);
			}
		} else if ((index = FirstEmptyIndex ()) >= 0) {
			InventoryItem item = new InventoryItem ();
			item.itemPrefab = prefab;
			item.count = 1;
			item.index = index;
			inventory[index] = item;
			SetActiveSlot (activeSlot);
			if (crafting != null) {
				crafting.UpdateSats (inventory[index]);
			}
		}
		if (index >= 0) {
			UpdateGUI (index);
		}
		return index;
	}

	public void RemoveItem (Transform item, int amount = 1) {
		int index = -1;
		if (item != null && (index = IndexOf (item)) >= 0) {
			RemoveItem (index, amount);
		}
	}

	public void RemoveItem (int index, int amount = 1) {
		inventory[index].count -= amount;
		if (crafting != null) {
			crafting.UpdateSats (inventory[index]);
		}
		int count = inventory[index].count;
		if (count <= 0) {
			inventory[index] = null;
			SetActiveSlot (activeSlot);
		}
		UpdateGUI (index);
	}

	private int IndexOf (Transform prefab) {
		for (int i = 0; i < inventory.Length; i ++) {
			if (inventory[i] != null && inventory[i].itemPrefab == prefab) {
				return i;
			}
		}
		return -1;
	}

	private int FirstEmptyIndex () {
		for (int i = 0; i < inventory.Length; i ++) {
			if (inventory[i] == null) {
				return i;
			}
		}
		return -1;
	}

	private void UpdateGUI (int index) {
		Sprite sprite;
		Color color;
		string label;
		if (inventory[index] != null) {
			sprite = inventory[index].itemPrefab.GetComponent<SpriteRenderer>().sprite;
			color = Color.white;
			label = "" + inventory[index].count;
		} else {
			sprite = null;
			color = Color.clear;
			label = "";
		}

		backpackIcons[index].sprite = sprite;
		backpackIcons[index].color = color;
		backpackLabels[index].text = label;

		if (index < hudIcons.Length) {
			hudIcons[index].sprite = sprite;
			hudIcons[index].color = color;
		}
		if (index < hudLabels.Length) {
			hudLabels[index].text = label;
		}
	}

	public void Slot1Active (bool on) { if (on) SetActiveSlot (0); }
	public void Slot2Active (bool on) { if (on) SetActiveSlot (1); }
	public void Slot3Active (bool on) { if (on) SetActiveSlot (2); }
	public void Slot4Active (bool on) { if (on) SetActiveSlot (3); }
	public void Slot5Active (bool on) { if (on) SetActiveSlot (4); }

	private void SetActiveSlot (int index) {
		activeSlot = index;
		if (inventory[index] != null) {
			GetComponent<UseEquipment>().SetCurrentEquipment (inventory[index].itemPrefab);
		} else {
			GetComponent<UseEquipment>().SetCurrentEquipment (null);
		}
	}

	public void InventoryClicked (int index) {
		InventoryItem nowHolding = inventory[index];
		if (holding != null) 
			holding.index = index;
		inventory[index] = holding;
		UpdateGUI (index);
		SetActiveSlot (activeSlot);
		holding = nowHolding;
		if (holding != null)
			holding.index = -1;
		UpdateHolding ();
	}

	private void UpdateHolding () {
		if (holding != null) {
			holdingImage.sprite = holding.itemPrefab.GetComponent<SpriteRenderer>().sprite; 
			holdingImage.color = Color.white;
		} else {
			holdingImage.sprite = null;
			holdingImage.color = Color.clear;
		}
	}

}
