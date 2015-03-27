using UnityEngine;
using System.Collections;

public partial class GenericEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	
	public string[] needSwitchNames;
	public bool[] needSwitchValues;

	public string[] setSwitchNames;
	public bool[] setSwitchValues;

	public string[] needItems;
	public bool deleteItems;
	
	public AudioClip enterSound;
	public AudioClip exitSound;
	
	public GameObject[] nextEvents;
	
	public void OnSceneEnter () {
		if (autoStart) {
			if (!MeetRequirements()) {
				return;
			}
			StartEvent();
		}
	}
	
	public void OnApproach () {
		if (approachStart) {
			if (!MeetRequirements()) {
				return;
			}
			StartEvent();
		}
	}

	public void OnExam () {
		if (examStart) {
			if (!MeetRequirements()) {
				return;
			}
			StartEvent();
		}
	}
	
	public void OnChainEnter () {
		if (!MeetRequirements()) {
			return;
		}
		StartEvent();
	}
	
	bool MeetRequirements () {
		for (int i=0; i<needSwitchNames.Length; i++) {
			if (GameController.stateController.GetSwitch(needSwitchNames[i]) != needSwitchValues[i]) {
				return false;
			}
		}
		
		for (int i=0; i<needItems.Length; i++) {
			if (!GameController.inventoryController.HasItem(GameController.inventoryController.GetItemIndex(needItems[i]))) {
				return false;
			}
		}

		/* End of The Test */
		for (int i=0; i<setSwitchNames.Length; i++) {
			GameController.stateController.SetSwitch(setSwitchNames[i], setSwitchValues[i]);
		}

		if (deleteItems) {
			for (int i=0; i<needItems.Length; i++) {
				GameController.inventoryController.SubItem(GameController.inventoryController.GetItemIndex(needItems[i]));
			}
		}

		return true;
	}

	public void StartEvent () {
		if (enterSound) {
			AudioSource.PlayClipAtPoint(enterSound, transform.position);
		}

		gameObject.BroadcastMessage("OnEvent", SendMessageOptions.RequireReceiver);
	}

	public void EventCallBack () {
		if (exitSound) {
			AudioSource.PlayClipAtPoint(exitSound, transform.position);
		}

		CallNextEvents();
	}

	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	/* Editor Function */
	public void NewNextEvent (GameObject nextEvent) {
		nextEvents = new GameObject[1];
		nextEvents[0] = nextEvent;
	}
	public void AddNextEvent (GameObject nextEvent) {
		GameObject[] nextEventsX = nextEvents;
		nextEvents = new GameObject[nextEventsX.Length + 1];
		System.Array.Copy (nextEventsX, nextEvents, Mathf.Min (nextEventsX.Length, nextEvents.Length));
		
		nextEvents[nextEvents.Length-1] = nextEvent;
	}
}
