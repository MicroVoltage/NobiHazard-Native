using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageController : MonoBehaviour {
	public Sprite[] images;

	public GameObject messageContainer;
	public Image image;
	public Text message;

	public bool showingMessage;

	private GameController gameController;


	void Awake () {
		if (GameController.messageController == null) {
			GameController.messageController = this;
		} else if (GameController.messageController != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		gameController = GameController.gameController;
	}

	public void ShowMessage (string imageName, string newMessage) {
		gameController.gameState = GameController.stateMessage;

		image.sprite = images[GetImageIndex(imageName)];
		message.text = newMessage;

		messageContainer.SetActive(true);
		showingMessage = true;
	}

	public void HideMessage () {
		gameController.gameState = GameController.stateSearch;

		messageContainer.SetActive(false);
		showingMessage = false;
	}

	int GetImageIndex(string imageName) {
		for (int i=0; i<images.Length; i++) {
			if (images[i].name == imageName) {
				return i;
			}
		}

		Debug.LogError(imageName + " - image not exist");
		return -1;
	}
}
