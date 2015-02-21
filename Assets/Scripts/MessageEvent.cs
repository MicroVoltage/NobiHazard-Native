using UnityEngine;
using System.Collections;

public class MessageEvent : MonoBehaviour {
	public string faceName;
	public string[] messages;

	private bool showingMessage = false;
	private int messageIndex = 1;

	void OnExam () {
		Debug.Log(gameObject.name + " - get message event");

		showingMessage = true;
		GameController.inputController.mode = 1;
		GameController.messageController.ShowMessage(faceName, messages[0]);
	}

	void Update () {
		if (!showingMessage || !GameController.inputController.B) {
			return;
		}

		if (messageIndex < messages.Length) {
			GameController.messageController.ShowMessage(faceName, messages[messageIndex]);
			messageIndex++;
		} else {
			GameController.messageController.HideMessage();
			GameController.inputController.mode = 0;
			showingMessage = false;
			messageIndex = 1;
		}

	}
}
