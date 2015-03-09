using UnityEngine;
using System.Collections;

public class HeroAnimationEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool arriveStart;
	public bool examStart;
	
	public enum Comparation {Equal, Less, More};
	public string[] requiredIntNames;
	public Comparation[] requiredIntComparations;
	public int[] requiredInts;
	public string[] requiredIntBoolNames;
	public string[] requiredInversedIntBoolNames;
	
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
	
	public void OnArrive () {
		if (arriveStart) {
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
		
		for (int i=0; i<requiredInversedIntBoolNames.Length; i++) {
			if (GameController.stateController.GetIntBool(requiredInversedIntBoolNames[i])) {
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

	public bool relative;
	public Vector2[] positions;
	
	private CharacterManager characterManager;
	private AnimationController animationController;
	private GameController gameController;

	private GameObject hero;

	private bool playingAnimation;
	private int animationIndex;
	
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

		hero = characterManager.characterInstances[characterManager.heroIndex];
		if (!hero) {
			Debug.LogError(characterManager.heroIndex + " - hero not instantiated");
		}
		hero.GetComponent<PlayerController>().enabled = false;
		hero.GetComponent<AnimationController>().enabled = true;

		PlayAnimation(0);
		playingAnimation = true;
		animationIndex = 1;
	}
	
	void PlayAnimation (int animationIndex) {
		animationController = characterManager.heroInstance.GetComponent<AnimationController>();
		if (relative) {
			animationController.MoveTo(
				(Vector2)characterManager.heroInstance.transform.position + positions[animationIndex] * GameController.gameScale);
		} else {
			animationController.MoveTo(
				(Vector2)transform.position + positions[animationIndex] * GameController.gameScale);
		}
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
			hero.GetComponent<PlayerController>().enabled = true;
			hero.GetComponent<AnimationController>().enabled = false;

			GameController.inputController.SetOrientationIndex(animationController.orientationIndex);

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
		Vector2 position = Vector2.zero;
		if (relative) {
			Gizmos.color = Color.magenta;
		}
		for (int i=0; i<positions.Length; i++) {
			position += positions[i];
			Gizmos.DrawLine(
				(Vector2)transform.position + (position - positions[i]) * GameController.gameScale,
				(Vector2)transform.position + position * GameController.gameScale);
			Gizmos.DrawWireSphere((Vector2)transform.position + position * GameController.gameScale, 0.2f);
		}
	}
}
