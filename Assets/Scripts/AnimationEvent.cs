using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour {
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


	public bool startAnimationSequence;
	public bool endAnimationSequence;

	public int animationEventSize;
	[HideInInspector]
	public int[] characterIndexes;
	[HideInInspector]
	public Vector2[] positions;

	private CharacterManager characterManager;
	private AnimationController animationController;
	private GameController gameController;

	private bool playingAnimation;
	private int animationIndex;

	public void ResizeAnimationEvent () {
		int[] characterIndexesX = characterIndexes;
		Vector2[] positionX = positions;

		characterIndexes = new int[animationEventSize];
		positions = new Vector2[animationEventSize];

		System.Array.Copy(characterIndexesX, characterIndexes, Mathf.Min(characterIndexesX.Length, characterIndexes.Length));
		System.Array.Copy(positionX, positions, Mathf.Min(positionX.Length, positions.Length));
	}

	void Start () {
		gameObject.name = gameObject.name + "-animation";

		characterManager = GameController.characterManager;
		gameController = GameController.gameController;
	}
	
	void OnEvent () {
		Debug.Log(gameObject.name + " - get animation event");
		if (startAnimationSequence) {
			Debug.Log(gameObject.name + " - start animation sequence");
			gameController.gameState = GameController.stateAnimation;
		}

		PlayAnimation(0);
		playingAnimation = true;
		animationIndex = 1;
	}

	void PlayAnimation (int animationIndex) {
		if (!characterManager.characterInstances[characterIndexes[animationIndex]]) {
			Debug.LogError(characterIndexes[animationIndex] + " - character not instantiated");
		}
		animationController = characterManager.characterInstances[characterIndexes[animationIndex]].GetComponent<AnimationController>();
		animationController.MoveTo((Vector2)transform.position + positions[animationIndex] * GameController.gameScale);
	}

	void Update () {
		if (!playingAnimation || animationController.playing) {
			return;
		}

		if (animationIndex < positions.Length) {
			Debug.Log(animationIndex + " - play animation");
			PlayAnimation(animationIndex);
			animationIndex++;
		} else {
			playingAnimation = false;
			animationIndex = 1;

			if (endAnimationSequence) {
				Debug.Log(gameObject.name + " - end animation sequence");
				gameController.gameState = GameController.stateSearch;
			}
			CallNextEvents();
		}
	}

	public void OnDrawGizmosSelected () {
		Gizmos.color = Color.gray;
		for (int x=-8; x<8; x++) {
			Gizmos.DrawRay(
				transform.position + new Vector3(x * GameController.gameScale, -8, 0),
				Vector3.up * 8 * GameController.gameScale);
		}
		for (int y=-8; y<8; y++) {
			Gizmos.DrawRay(
				transform.position + new Vector3(-8, y * GameController.gameScale, 0),
				Vector3.right * 8 * GameController.gameScale);
		}

		Gizmos.color = Color.red;
		for (int i=0; i<positions.Length; i++) {
			Gizmos.DrawWireSphere((Vector2)transform.position + positions[i] * GameController.gameScale, 0.2f);
		}
	}
}
