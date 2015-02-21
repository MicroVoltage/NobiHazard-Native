using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageController : MonoBehaviour {
	public Sprite[] images;

	public Image image;
	public Text message;

	public bool showingMessage = false;
	

	void Awake () {
		if (GameController.messageController == null) {
			GameController.messageController = this;
		} else if (GameController.messageController != this) {
			Destroy(gameObject);
		}

	}

	void Start () {
		HideMessage();
	}

	public void ShowMessage (string imageName, string newMessage) {
		image.sprite = images[FindImage(imageName)];
		message.text = newMessage;

		gameObject.SetActive(true);
		showingMessage = true;
	}

	public void HideMessage () {
		gameObject.SetActive(false);
		showingMessage = false;
	}

	int FindImage(string imageName) {
		for (int i=0; i<images.Length; i++) {
			if (images[i].name == imageName) {
				return i;
			}
		}

		Debug.LogError(imageName + " - image not exist");
		return -1;
	}
}
