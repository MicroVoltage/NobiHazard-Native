using UnityEngine;
using System.Collections;

public class StateEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;
	
	void OnSceneEnter () {
		if (autoStart) {
			OnStateEvent();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			OnStateEvent();
		}
	}
	
	void OnExam () {
		if (examStart) {
			OnStateEvent();
		}
	}
	
	void OnChainEnter () {
		OnStateEvent();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	public string[] setIntNames;
	public int[] setInts;
	public bool[] addInt;

	private StateController stateController;

	void Start () {
		stateController = GameController.stateController;
	}

	public void OnStateEvent() {
		for (int i=0; i<setIntNames.Length; i++) {
			if (addInt[i]) {
				stateController.SetInt(setIntNames[i], stateController.GetInt(setIntNames[i]) + setInts[i]);
			} else {
				stateController.SetInt(setIntNames[i], setInts[i]);
			}
		}

		CallNextEvents();
	}
}
