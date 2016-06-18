using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingManager : MonoBehaviour 
{
	public Transform craftWindowParent;
	public Transform craftSlotPrefab;
	public CraftingDatabase database;
	private class CraftSat {
		public CraftingDatabase.Entry databaseEntry;
		public InventoryManager.InventoryItem[] reqsSatisfiedByItem;
		public Transform craftWindowInstance;
	}
	private CraftSat[] sats;

	private InventoryManager inventory;

	private void Start () {
		inventory = GetComponent<InventoryManager> ();

		sats = new CraftSat[database.crafts.Length];
		for (int i = 0; i < database.crafts.Length; i ++) {
			sats[i] = new CraftSat ();
			sats[i].databaseEntry = database.crafts[i];
			sats[i].reqsSatisfiedByItem = new InventoryManager.InventoryItem[database.crafts[i].requiredItems.Length];
			for (int j = 0; j < sats[i].reqsSatisfiedByItem.Length; j ++) {
				sats[i].reqsSatisfiedByItem[j] = null;
			}
			sats[i].craftWindowInstance = null;
		}
	}

	public void UpdateSats (InventoryManager.InventoryItem updatedItem) {
		foreach (CraftSat sat in sats) {
			int reqIndex = -1;
			for (int i = 0; i < sat.databaseEntry.requiredItems.Length; i ++) {
				if (sat.databaseEntry.requiredItems[i] == updatedItem.itemPrefab) {
					reqIndex = i;
					break;
				}
			}
			if (reqIndex >= 0) {
				if (sat.reqsSatisfiedByItem[reqIndex] == null && updatedItem.count > 0) {
					sat.reqsSatisfiedByItem[reqIndex] = updatedItem;
					if (RequirementsSatisfied (sat)) {
						sat.craftWindowInstance = Instantiate (craftSlotPrefab);
						sat.craftWindowInstance.parent = craftWindowParent;
						sat.craftWindowInstance.GetComponent<Image>().sprite = sat.databaseEntry.itemPrefab.GetComponent<SpriteRenderer>().sprite;
        				sat.craftWindowInstance.GetComponent<Button>().onClick.AddListener(delegate { CraftItem(sat); });
					}
				} else if (sat.reqsSatisfiedByItem[reqIndex] == updatedItem && updatedItem.count <= 0) {
					// TODO search through the inventory to find another that might satisfy
					sat.reqsSatisfiedByItem[reqIndex] = null;
					if (sat.craftWindowInstance != null) {
						Destroy (sat.craftWindowInstance.gameObject);
					}
				}
			}
		}
	}

	private bool RequirementsSatisfied (CraftSat sat) {
		bool ret = true;
		for (int i = 0; i < sat.reqsSatisfiedByItem.Length; i ++) {
			if (sat.reqsSatisfiedByItem[i] == null) {
				ret = false;
				break;
			}
		}
		return ret;
	}

	private void CraftItem (CraftSat sat) {
		Debug.Log ("Trying to add " + sat.databaseEntry.itemPrefab.gameObject.name + " to inventory");
		inventory.AddItemToInventory (sat.databaseEntry.itemPrefab);
		for (int i = 0; i < sat.databaseEntry.requiredItems.Length; i ++) {
			inventory.RemoveItem (sat.reqsSatisfiedByItem[i].index, sat.databaseEntry.requiredCounts[i]);
		}
	}

}
