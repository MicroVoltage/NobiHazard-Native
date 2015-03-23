using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class MessageEvent : MonoBehaviour {
	public string[] imageNames;
	public string[] messages;

	private InputController inputController;
	private MessageController messageController;

	private bool showingMessage = false;
	private int messageIndex = 1;

	void Start () {
		gameObject.name = gameObject.name + "-message";

		inputController = GameController.inputController;
		messageController = GameController.messageController;

		for (int i=0; i<imageNames.Length; i++) {
			if (imageNames[i] == "") {
				imageNames[i] = imageNames[i-1];
			}
		}
	}

	public void OnEvent () {
		Debug.Log(gameObject.name + " - get message event");

		showingMessage = true;
		messageController.ShowMessage(imageNames[0], messages[0]);

		inputController.cancel = false;
	}

	void Update () {
		if (!showingMessage || !inputController.cancel) {
			return;
		}

		if (messageIndex < messages.Length) {
			messageController.ShowMessage(imageNames[messageIndex], messages[messageIndex]);
			messageIndex++;
		} else {
			messageController.HideMessage();
			showingMessage = false;
			messageIndex = 1;

			gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
		}
	}
}
