using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameController : MonoBehaviour {
	public Sprite[] frames;

	public GameObject frameContainer;
	public Image frame;
	public Image background;

	public bool showingFrame;
	
	private GameController gameController;
	
	
	void Awake () {
		if (GameController.frameController == null) {
			GameController.frameController = this;
		} else if (GameController.frameController != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		gameController = GameController.gameController;
	}

	public void ShowFrame (string frameName, string backgroundName) {
		gameController.gameState = GameController.stateFrame;
		
		frame.sprite = frames[GetFrameIndex(frameName)];
		if (backgroundName == "") {
			background.sprite = null;
			background.color = Color.black;
		} else {
			background.sprite = frames[GetFrameIndex(backgroundName)];
			background.color = Color.white;
		}
		
		frameContainer.SetActive(true);
		showingFrame = true;
	}
	
	public void HideFrame () {
		gameController.gameState = GameController.stateSearch;
		
		frameContainer.SetActive(false);
		showingFrame = false;
	}
	
	int GetFrameIndex(string frameName) {
		for (int i=0; i<frames.Length; i++) {
			if (frames[i].name == frameName) {
				return i;
			}
		}
		
		Debug.LogError(frameName + " - frame not exist");
		return -1;
	}
}
