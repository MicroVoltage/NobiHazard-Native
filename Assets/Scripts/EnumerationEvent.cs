using UnityEngine;
using System.Collections;

public class EnumerationEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;

	public void OnSceneEnter () {
		if (autoStart) {
			OnEnumerationEvent();
		}
	}
	
	public void OnApproach () {
		if (approachStart) {
			OnEnumerationEvent();
		}
	}
	
	public void OnExam () {
		if (examStart) {
			OnEnumerationEvent();
		}
	}
	
	public void OnChainEnter () {
		OnEnumerationEvent();
	}
	
	public void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	public GameObject[] events;
	public bool[] sameTime;

	public int eventIndex;
	
	public void OnEnumerationEvent () {
		if (eventIndex >= events.Length) {
			eventIndex = 0;
			CallNextEvents();
		} else {
			events[eventIndex].SendMessage("OnChainEnter");

			if (sameTime[eventIndex]) {
				for (int i=eventIndex + 1; i<events.Length; i++) {
					if (sameTime[i]) {
						events[i].SendMessage("OnChainEnter");
						if (i == events.Length - 1) {
							eventIndex = i;
						}
					} else {
						eventIndex = i - 1;
						return;
					}
				}
			}
		}
	}
}
