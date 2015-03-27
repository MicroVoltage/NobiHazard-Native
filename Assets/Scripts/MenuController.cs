using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {
	public GameObject itemTemplate;

	public GameObject menuContainer;
	public Transform itemContainer;
	public Image itemPreview;
	public Text itemDescription;
	public Text applyItemText;

	public int selectedItemIndex;

	public bool showingMenu;


	InventoryController inventoryController;


	void Awake () {
		if (GameController.menuController == null) {
			GameController.menuController = this;
		} else if (GameController.menuController != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		inventoryController = GameController.inventoryController;
	}

	public void ShowMenu () {
		selectedItemIndex = 0;
		RefreshItemList();
		
		menuContainer.SetActive(true);
		showingMenu = true;
	}
	
	public void HideMenu () {
		menuContainer.SetActive(false);
		showingMenu = false;
	}
	
	public void RefreshItemList () {
		for (int i=0; i<itemContainer.childCount; i++) {
//			Destroy(itemContainer.GetChild(i).gameObject);
		}

		for (int i=0; i<inventoryController.items.Length; i++) {
			if (inventoryController.itemCounts[i] > 0) {
				GameObject newItem = (GameObject)Instantiate(itemTemplate);
				newItem.transform.SetParent(itemContainer);
				
				ItemTemplateReference newItemReference = newItem.GetComponent<ItemTemplateReference>();
				newItemReference.itemIcon.sprite = inventoryController.items[i].icon;
				newItemReference.itemName.text = inventoryController.items[i].name;
				newItemReference.itemCount.text = inventoryController.itemCounts[i].ToString();

				newItemReference.itemIndex = i;
			}
		}
	}

	// Apply Item Button Call
	public void OnItemApply () {
		GameObject itemEventInstance = (GameObject)Instantiate(inventoryController.items[selectedItemIndex].itemEvent);
		itemEventInstance.SendMessage("OnChainEnter", SendMessageOptions.RequireReceiver);
	}
}
