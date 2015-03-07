using UnityEngine;
using System.Collections;

public class GenericEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;

	public void OnSceneEnter () {
		if (autoStart) {
			OnEvent();
		}
	}

	public void OnApproach () {
		if (approachStart) {
			OnEvent();
		}
	}

	public void OnExam () {
		if (examStart) {
			OnEvent();
		}
	}

	public void OnChainEnter () {
		OnEvent();
	}

	public void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}
	

	public virtual void OnEvent () {

		CallNextEvents();
	}
}
