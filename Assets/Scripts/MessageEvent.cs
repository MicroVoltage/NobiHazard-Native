using UnityEngine;
using System.Collections;

public class MessageEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;
	
	void OnSceneEnter () {
		if (autoStart) {
			OnMessageEvent();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			OnMessageEvent();
		}
	}
	
	void OnExam () {
		if (examStart) {
			OnMessageEvent();
		}
	}
	
	void OnChainEnter () {
		OnMessageEvent();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	
	public string imageName;
	public string[] messages;

	private InputController inputController;
	private MessageController messageController;

	private bool showingMessage = false;
	private int messageIndex = 1;

	void Start () {
		gameObject.name = gameObject.name + "-message";

		inputController = GameController.inputController;
		messageController = GameController.messageController;
	}

	void OnMessageEvent () {
		Debug.Log(gameObject.name + " - get message event");

		showingMessage = true;
		messageController.ShowMessage(imageName, messages[0]);

		inputController.cancel = false;
	}

	void Update () {
		if (!showingMessage || !inputController.cancel) {
			return;
		}

		if (messageIndex < messages.Length) {
			messageController.ShowMessage(imageName, messages[messageIndex]);
			messageIndex++;
		} else {
			messageController.HideMessage();
			showingMessage = false;
			messageIndex = 1;

			CallNextEvents();
		}
	}
}
