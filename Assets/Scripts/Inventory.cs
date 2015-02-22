using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public float maxHealth = 100.0f;
	public float health = 100.0f;

	public int[] itemCounts;

	public string[] itemNames;
	public string[] itemDescriptions;
	public int[] itemInts;
	public float[] itemFloats;

	void Awake () {
		if (GameController.inventory == null) {
			GameController.inventory = this;
		} else if (GameController.inventory != this) {
			Destroy(gameObject);
		}
	}

	public int GetItemIndex (string itemName) {
		for (int i=0; i<itemNames.Length; i++) {
			if (itemNames[i] == itemName) {
				return i;
			}
		}
		
		Debug.LogError(itemName + " - item not exist");
		return -1;
	}

	public int GetItemInt (int itemIndex) {
		return itemInts[itemIndex];
	}

	public float GetItemFloat (int itemIndex) {
		return itemFloats[itemIndex];
	}

	public void AddItem (int itemIndex) {
		itemCounts[itemIndex]++;
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
		if (HasHealth()) {
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
