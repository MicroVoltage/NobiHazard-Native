using UnityEngine;
using System.Collections;

public class EmptyEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;

	void OnSceneEnter () {
		if (autoStart) {
			OnEmptyEvent();
		}
	}

	void OnApproach () {
		if (approachStart) {
			OnEmptyEvent();
		}
	}

	void OnExam () {
		if (examStart) {
			OnEmptyEvent();
		}
	}

	void OnChainEnter () {
		OnEmptyEvent();
	}

	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	void Start () {
		gameObject.name = gameObject.name + "-empty";

	}

	void OnEmptyEvent () {

		CallNextEvents();
	}
}
