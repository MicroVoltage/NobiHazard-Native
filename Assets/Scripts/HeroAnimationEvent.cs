using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class HeroAnimationEvent : MonoBehaviour {
	public bool endAnimationSequence;

	public bool relative;
	public Vector2[] positions;
	
	HeroController heroController;
	PlayerAnimationController animationController;
	GameController gameController;

	GameObject hero;

	bool playingAnimation;
	int animationIndex;
	
	void Start () {
		gameObject.name = gameObject.name + "-animation";
		
		heroController = GameController.heroController;
		gameController = GameController.gameController;
	}
	
	public void OnEvent () {
		Debug.Log(gameObject.name + " - get animation event");
		gameController.gameState = GameController.stateAnimation;

		hero = heroController.heroInstance;
		if (hero == null) {
			Debug.LogError(heroController.heroIndex + " - hero not instantiated");
		}

		hero.GetComponent<PlayerController>().enabled = false;
		hero.GetComponent<PlayerAnimationController>().enabled = true;

		PlayAnimation(0);
		playingAnimation = true;
		animationIndex = 1;
	}
	
	void PlayAnimation (int animationIndex) {
		animationController = hero.GetComponent<PlayerAnimationController>();
		if (relative) {
			animationController.MoveTo(
				(Vector2)hero.transform.position + positions[animationIndex] * GameController.gameScale);
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
			hero.GetComponent<PlayerAnimationController>().enabled = false;

			GameController.inputController.SetOrientationIndex(animationController.orientationIndex);

			playingAnimation = false;
			animationIndex = 1;
			
			if (endAnimationSequence) {
				Debug.Log(gameObject.name + " - end animation sequence");
				gameController.gameState = GameController.stateSearch;
			}

			gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
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
