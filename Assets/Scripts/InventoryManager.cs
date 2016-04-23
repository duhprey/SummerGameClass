using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	public GameObject hud;
	public Image[] hudIcons;
	public Text[] hudLabels;

	public GameObject inventoryWindow;
	public GameObject backpack;
	private Image[] backpackIcons;
	private Text[] backpackLabels;

	private int activeSlot;

	class InventoryItem {
		public Transform itemPrefab;
		public int count;
	}
	private InventoryItem[] inventory;
	private InventoryItem holding;

	public Image holdingImage;

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

	public void OnCollisionEnter2D (Collision2D collision) {
		Block block = collision.gameObject.GetComponent<Block> ();
		if (block != null) {
			Transform prefab = block.placeablePrefab;
			int index = -1;
			if ((index = IndexOf (prefab)) >= 0 && inventory[index].count < 99) {
				inventory[index].count ++;
			} else if ((index = FirstEmptyIndex ()) >= 0) {
				InventoryItem item = new InventoryItem ();
				item.itemPrefab = prefab;
				item.count = 1;
				inventory[index] = item;
				SetActiveSlot (activeSlot);
			}
			if (index >= 0) {
				UpdateGUI (index);
				Destroy (block.gameObject);
			}
		}
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

	public void RemoveOne (Transform item) {
		int index = -1;
		if (item != null && (index = IndexOf (item)) >= 0) {
			inventory[index].count --;
			int count = inventory[index].count;
			if (count > 0) {
				hudLabels[index].text = "" + count;
			} else {
				inventory[index] = null;
				UpdateGUI (index);
				SetActiveSlot (activeSlot);
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
		if (inventory[index] != null) {
			GetComponent<UseEquipment>().SetCurrentEquipment (inventory[index].itemPrefab);
		} else {
			GetComponent<UseEquipment>().SetCurrentEquipment (null);
		}
	}

	public void InventoryClicked (int index) {
		InventoryItem nowHolding = inventory[index];
		inventory[index] = holding;
		UpdateGUI (index);
		SetActiveSlot (activeSlot);
		holding = nowHolding;
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
