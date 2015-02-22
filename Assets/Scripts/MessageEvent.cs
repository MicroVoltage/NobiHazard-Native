using UnityEngine;
using System.Collections;

public class MessageEvent : MonoBehaviour {
	public string imageName;
	public string[] messages;

	private InputController inputController;

	private bool showingMessage = false;
	private int messageIndex = 1;

	void Start () {
		inputController = GameController.inputController;
	}

	void OnExam () {
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
		}

	}
}
