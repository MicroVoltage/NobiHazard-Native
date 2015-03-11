using UnityEngine;
using System.Collections;

public partial class GenericEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool arriveStart;
	public bool examStart;
	
	public enum Comparation {Equal, Less, More};
	public string[] requiredIntNames;
	public Comparation[] requiredIntComparations;
	public int[] requiredInts;
	
	public string[] requiredIntBoolNames;
	public string[] requiredInversedIntBoolNames;
	
	public string[] requiredItemNames;
	public bool deleteRequiredItems;
	
	public AudioClip sound;
	
	public GameObject[] nextEvents;
	
	public void OnSceneEnter () {
		if (autoStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnApproach () {
		if (approachStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnArrive () {
		if (arriveStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnExam () {
		if (examStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnChainEnter () {
		if (!MeetRequirements()) {
			return;
		}
		OnEvent();
	}
	
	public bool MeetRequirements () {
		for (int i=0; i<requiredIntNames.Length; i++) {
			switch (requiredIntComparations[i]) {
			case Comparation.Equal:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) == requiredInts[i])) {
					return false;
				}
				break;
			case Comparation.Less:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) < requiredInts[i])) {
					return false;
				}
				break;
			case Comparation.More:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) > requiredInts[i])) {
					return false;
				}
				break;
			}
		}
		
		for (int i=0; i<requiredIntBoolNames.Length; i++) {
			if (!GameController.stateController.GetIntBool(requiredIntBoolNames[i])) {
				return false;
			}
		}
		
		for (int i=0; i<requiredInversedIntBoolNames.Length; i++) {
			if (GameController.stateController.GetIntBool(requiredInversedIntBoolNames[i])) {
				return false;
			}
		}
		
		for (int i=0; i<requiredItemNames.Length; i++) {
			if (!GameController.inventoryController.HasItem(GameController.inventoryController.GetItemIndex(requiredItemNames[i]))) {
				return false;
			}
			if (deleteRequiredItems) {
				GameController.inventoryController.SubItem(GameController.inventoryController.GetItemIndex(requiredItemNames[i]));
			}
		}
		
		AudioSource.PlayClipAtPoint(sound, transform.position);
		
		return true;
	}
	
	public void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}
	
	/******************************* Event Alike *******************************/
	

	public virtual void OnEvent () {

		CallNextEvents();
	}
}
