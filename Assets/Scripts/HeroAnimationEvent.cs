using UnityEngine;
using System.Collections;

public class HeroAnimationEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;
	
	void OnSceneEnter () {
		if (autoStart) {
			OnHeroAnimationEvent();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			OnHeroAnimationEvent();
		}
	}
	
	void OnExam () {
		if (examStart) {
			OnHeroAnimationEvent();
		}
	}
	
	void OnChainEnter () {
		OnHeroAnimationEvent();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}
	
	public bool startAnimationSequence;
	public bool endAnimationSequence;

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
	
	void OnHeroAnimationEvent () {
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
		animationController = characterManager.characterInstances[characterManager.heroIndex].GetComponent<AnimationController>();
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
			hero.GetComponent<PlayerController>().enabled = true;
			hero.GetComponent<AnimationController>().enabled = false;

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
