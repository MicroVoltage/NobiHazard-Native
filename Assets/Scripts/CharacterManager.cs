using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {
	public GameObject[] characters;

	public GameObject[] characterInstances;

	public int heroIndex;

	private GameController gameController;
	private SceneController sceneController;
	private CameraController cameraController;

	void Awake () {
		if (GameController.characterManager == null) {
			GameController.characterManager = this;
		} else if (GameController.characterManager != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		gameController = GameController.gameController;
		cameraController = GameController.cameraController;

		characterInstances = new GameObject[characters.Length];
	}

	public void AddCharacter (int characterIndex, Vector2 position) {
		if (characterInstances[characterIndex]) {
			Debug.LogError(characterIndex + " - adding exsiting character");
		}

		sceneController = gameController.sceneControllers[gameController.sceneIndex];
		GameObject character = (GameObject)Instantiate(characters[characterIndex], position, transform.rotation);
		characterInstances[characterIndex] = character;
		character.transform.parent = sceneController.eventLayer;
	}

	public void DestroyCharacters () {
		for (int i=0; i<characterInstances.Length; i++) {
			if (characterInstances[i]) {
				Destroy(characterInstances[i]);
				characterInstances[i] = null;
			}
		}
	}

	public void AddHero (int characterIndex, Vector3 position) {
		AddCharacter(characterIndex, position);
		ChangeHero(characterIndex);
		RefreshCharacterControllers();
	}

	public void ChangeHero (int characterIndex) {
		heroIndex = characterIndex;
		if (cameraController.focus != characterInstances[characterIndex].transform) {
			cameraController.SetCameraFocus(characterInstances[characterIndex].transform);
			cameraController.SetCameraPosition(characterInstances[characterIndex].transform.position);
		}
		RefreshCharacterControllers();
	}


	void RefreshCharacterControllers () {
		for (int i=0; i<characterInstances.Length; i++) {
			if (characterInstances[i]) {
				if (i == heroIndex) {
					characterInstances[i].GetComponent<PlayerController>().enabled = true;
					characterInstances[i].GetComponent<AnimationController>().enabled = false;
				} else {
					characterInstances[i].GetComponent<PlayerController>().enabled = false;
					characterInstances[i].GetComponent<AnimationController>().enabled = true;
				}
			}
		}
	}
}
