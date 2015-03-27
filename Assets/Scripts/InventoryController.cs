using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Item {
	public string name;

	public Sprite icon;
	public Sprite preview;

	public string description;
	public string applyText;

	public int Int;
	public float Float;

	/* Standard event prefeb
	 * Will be instantiated and called OnChainEnter
	 * Must detele itself after execution
	 */
	public GameObject itemEvent;
}

public class InventoryController : MonoBehaviour {
	public float maxHealth = 100.0f;
	public float health = 100.0f;

	public int inventorySize = 200;

	public Item[] items;
	public int[] itemCounts;


	void Awake () {
		if (GameController.inventoryController == null) {
			GameController.inventoryController = this;
		} else if (GameController.inventoryController != this) {
			Destroy(gameObject);
		}
	}

	public int GetItemIndex (string itemName) {
		for (int i=0; i<items.Length; i++) {
			if (items[i].name == itemName) {
				return i;
			}
		}
		
		Debug.LogError(itemName + " - item not exist");
		return -1;
	}

	public int GetItemInt (int itemIndex) {
		return items[itemIndex].Int;
	}

	public float GetItemFloat (int itemIndex) {
		return items[itemIndex].Float;
	}

	public void AddItem (int itemIndex) {
		itemCounts[itemIndex]++;
	}
	public void AddItem (int itemIndex, int addNumber) {
		itemCounts[itemIndex] += addNumber;
	}
	public bool SubItem (int itemIndex) {
		itemCounts[itemIndex]--;
		if (HasItem(itemIndex)) {
			itemIndex = 0;
		}
		return HasItem(itemIndex);
	}
	public void CleanItem (int itemIndex) {
		itemCounts[itemIndex] = 0;
	}
	public bool HasItem (int itemIndex) {
		return itemCounts[itemIndex] > 0;
	}
	public int ItmeCount (int itemIndex) {
		return itemCounts[itemIndex];
	}

	public void AddHealth (float value) {
		health += value;
		if (health > maxHealth) {
			health = maxHealth;
		}
	}
	public bool SubHealth (float value) {
		health -= value;
		if (!HasHealth()) {
			health = 0;
		}
		return HasHealth();
	}
	public void CleanHealth (int itemIndex) {
		health = 0;
	}
	public bool HasHealth () {
		return health > 0;
	}
	public float HealthValue () {
		return health;
	}
}
