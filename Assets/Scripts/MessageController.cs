using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageController : MonoBehaviour {
	public Sprite[] images;

	public Image image;
	public Text message;

	public bool showingMessage;

	private GameController gameController;
	private int originalGameState;
	

	void Awake () {
		if (GameController.messageController == null) {
			GameController.messageController = this;
		} else if (GameController.messageController != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		gameController = GameController.gameController;

		HideMessage();
	}

	public void ShowMessage (string imageName, string newMessage) {
		gameController.gameState = 2;

		image.sprite = images[GetImageIndex(imageName)];
		message.text = newMessage;

		gameObject.SetActive(true);
		showingMessage = true;
	}

	public void HideMessage () {
		gameController.gameState = 0;

		gameObject.SetActive(false);
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
