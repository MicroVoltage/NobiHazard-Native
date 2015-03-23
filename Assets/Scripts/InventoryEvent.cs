using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class InventoryEvent : MonoBehaviour {
	public string[] setItemNames;
	public int[] addNumbers;
	
	private InventoryController inventoryController;
	
	void Start () {
		gameObject.name = gameObject.name + "-inventory";
		
		inventoryController = GameController.inventoryController;
	}
	
	public void OnEvent() {
		for (int i=0; i<setItemNames.Length; i++) {
			inventoryController.AddItem(inventoryController.GetItemIndex(setItemNames[i]), addNumbers[i]);
		}
		
		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
