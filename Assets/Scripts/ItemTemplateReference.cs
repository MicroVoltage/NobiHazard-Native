using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemTemplateReference : MonoBehaviour {
	public Image itemIcon;
	public Text itemName;
	public Text itemCount;

	// Set by the MenuController
	public int itemIndex;

	public void OnItemSelected () {
		GameController.menuController.itemPreview.sprite = GameController.inventoryController.items[itemIndex].preview;
		GameController.menuController.itemDescription.text = GameController.inventoryController.items[itemIndex].description;
		GameController.menuController.applyItemText.text = GameController.inventoryController.items[itemIndex].applyText;

		GameController.menuController.selectedItemIndex = itemIndex;
	}
}
