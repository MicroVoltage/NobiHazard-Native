using UnityEngine;
using System.Collections;

public class MessageEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;
	
	void OnSceneEnter () {
		if (autoStart) {
			ShowMessage();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			ShowMessage();
		}
	}
	
	void OnExam () {
		if (examStart) {
			ShowMessage();
		}
	}
	
	void OnChainEnter () {
		ShowMessage();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	
	public string imageName;
	public string[] messages;

	private InputController inputController;

	private bool showingMessage = false;
	private int messageIndex = 1;

	void Start () {
		inputController = GameController.inputController;
	}

	void ShowMessage () {
		Debug.Log(gameObject.name + " - get message event");

		showingMessage = true;
		GameController.messageController.ShowMessage(imageName, messages[0]);
	}

	void Update () {
		if (!showingMessage || !inputController.cancel) {
			return;
		}

		if (messageIndex < messages.Length) {
			GameController.messageController.ShowMessage(imageName, messages[messageIndex]);
			messageIndex++;
		} else {
			GameController.messageController.HideMessage();
			showingMessage = false;
			messageIndex = 1;

			CallNextEvents();
		}
	}
}
