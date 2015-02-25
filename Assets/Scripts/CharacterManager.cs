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

		RefreshCharacterControllers();
	}

	public void DestroyCharacter (int characterIndex) {
		if (characterInstances[characterIndex]) {
			Destroy(characterInstances[characterIndex]);
			characterInstances[characterIndex] = null;
		}
	}

	public void DestroyCharacters () {
		for (int i=0; i<characterInstances.Length; i++) {
			DestroyCharacter(i);
		}
	}

	public void AddHero (int characterIndex, Vector2 position) {
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
					Debug.Log(i + " - set hero");
					characterInstances[i].GetComponent<PlayerController>().enabled = true;
					characterInstances[i].GetComponent<AnimationController>().enabled = false;
				} else {
					Debug.Log(i + " - set character");
					characterInstances[i].GetComponent<PlayerController>().enabled = false;
					characterInstances[i].GetComponent<AnimationController>().enabled = true;
				}
			}
		}
	}
}
