using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour {
	public float maxHealth = 100.0f;
	public float health = 100.0f;

	public int inventorySize = 200;

	public int[] itemCounts;
	public string[] itemNames;
	public string[] itemDescriptions;
	public int[] itemInts;
	public float[] itemFloats;
	

	public void ResizeInventory () {
		int[] itemCountsX = itemCounts;
		string[] itemNamesX = itemNames;
		string[] itemDescriptionsX = itemDescriptions;
		int[] itemIntsX = itemInts;
		float[] itemFloatsX = itemFloats;

		itemCounts = new int[inventorySize];
		itemNames = new string[inventorySize];
		itemDescriptions = new string[inventorySize];
		itemInts = new int[inventorySize];
		itemFloats = new float[inventorySize];

		System.Array.Copy(itemCountsX, itemCounts, Mathf.Min(itemCountsX.Length, itemCounts.Length));
		System.Array.Copy(itemNamesX, itemNames, Mathf.Min(itemNamesX.Length, itemNames.Length));
		System.Array.Copy(itemDescriptionsX, itemDescriptions, Mathf.Min(itemDescriptionsX.Length, itemDescriptions.Length));
		System.Array.Copy(itemIntsX, itemInts, Mathf.Min(itemIntsX.Length, itemInts.Length));
		System.Array.Copy(itemFloatsX, itemFloats, Mathf.Min(itemFloatsX.Length, itemFloats.Length));
	}

	void Awake () {
		if (GameController.inventoryController == null) {
			GameController.inventoryController = this;
		} else if (GameController.inventoryController != this) {
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


public class Item {
	public int itemCount;
	public string itemName;
	public string itemDescription;
	public int itemInt;
	public float itemFloat;
}
