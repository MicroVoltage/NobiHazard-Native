using UnityEngine;
using System.Collections;

public class EmptyEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;

	void OnSceneEnter () {
		if (autoStart) {
			EmptyMethod();
		}
	}

	void OnApproach () {
		if (approachStart) {
			EmptyMethod();
		}
	}

	void OnExam () {
		if (examStart) {
			EmptyMethod();
		}
	}

	void OnChainEnter () {
		EmptyMethod();
	}

	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	void EmptyMethod () {

		CallNextEvents();
	}
}
