using UnityEngine;
using System.Collections;

public class CharacterEvent : MonoBehaviour {
	public bool autoStart;
	public bool specifyEnterScene;
	public int specifyEnterSceneIndex;

	public bool approachStart;
	public bool examStart;

	public GameObject[] nextEvents;
	
	void OnSceneEnter (int enterSceneIndex) {
		if (autoStart) {
			if (specifyEnterScene && specifyEnterSceneIndex != enterSceneIndex) {
				return;
			}
			OnCharacterEvent();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			OnCharacterEvent();
		}
	}
	
	void OnExam () {
		if (examStart) {
			OnCharacterEvent();
		}
	}
	
	void OnChainEnter () {
		OnCharacterEvent();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	public enum CharaterEventType {AddCharacter, AddHero, DestroyCharacter, TeleportHero};
	public CharaterEventType eventType;
	public int charaterIndex;
	public int orientationIndex;
	public int newSceneIndes;
	public Vector2 startPosition;
	
	private CharacterManager characterManager;
	private InputController inputController;
	private GameController gameController;

	void Start () {
		gameObject.name = gameObject.name + "-character";

		characterManager = GameController.characterManager;
		inputController = GameController.inputController;
		gameController = GameController.gameController;
	}

	void OnCharacterEvent () {
		Debug.Log(gameObject.name + " - get animation event");

		switch (eventType) {
		case CharaterEventType.AddCharacter:
			characterManager.AddCharacter(charaterIndex, transform.position);
			characterManager.characterInstances[charaterIndex].GetComponent<AnimationController>().orientationIndex = orientationIndex;

			break;
		case CharaterEventType.AddHero:
			characterManager.AddHero(charaterIndex, transform.position);
			inputController.orientationIndex = orientationIndex;

			break;
		case CharaterEventType.DestroyCharacter:
			characterManager.DestroyCharacter(charaterIndex);

			break;
		case CharaterEventType.TeleportHero:
			gameController.CloseScene(gameController.sceneIndex);
			gameController.OpenScene(newSceneIndes, startPosition);

			break;
		}
		
		CallNextEvents();
	}


}
