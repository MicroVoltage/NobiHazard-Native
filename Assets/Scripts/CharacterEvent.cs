using UnityEngine;
using System.Collections;

public class CharacterEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	
	public enum Comparation {Equal, Less, More};
	public string[] requiredIntNames;
	public Comparation[] requiredIntComparations;
	public int[] requiredInts;
	public string[] requiredIntBoolNames;
	
	public GameObject[] nextEvents;
	
	public void OnSceneEnter () {
		if (autoStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnApproach () {
		if (approachStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnExam () {
		if (examStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnChainEnter () {
		if (!MeetRequirements()) {
			return;
		}
		OnEvent();
	}
	
	public bool MeetRequirements () {
		for (int i=0; i<requiredIntNames.Length; i++) {
			switch (requiredIntComparations[i]) {
			case Comparation.Equal:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) == requiredInts[i])) {
					return false;
				}
				break;
			case Comparation.Less:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) < requiredInts[i])) {
					return false;
				}
				break;
			case Comparation.More:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) > requiredInts[i])) {
					return false;
				}
				break;
			}
		}
		
		for (int i=0; i<requiredIntBoolNames.Length; i++) {
			if (!GameController.stateController.GetIntBool(requiredIntBoolNames[i])) {
				return false;
			}
		}
		
		return true;
	}
	
	public void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}
	
	/******************************* Event Alike *******************************/


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

	void OnEvent () {
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
