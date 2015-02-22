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
			showingMessage = false;
			messageIndex = 1;
		}

	}
}
