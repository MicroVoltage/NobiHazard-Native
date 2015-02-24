using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;
	
	void OnSceneEnter () {
		if (autoStart) {
			PlayAnimation();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			PlayAnimation();
		}
	}
	
	void OnExam () {
		if (examStart) {
			PlayAnimation();
		}
	}
	
	void OnChainEnter () {
		PlayAnimation();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	public int animationEventSize;
	[HideInInspector]
	public int[] characterIndexes;
	[HideInInspector]
	public Vector2[] positions;

	private CharacterManager characterManager;
	private AnimationController animationController;

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
		characterManager = GameController.characterManager;
	}

	void PlayAnimation () {
		Debug.Log(gameObject.name + " - get animation event");

		PlayAnimation(0);
		playingAnimation = true;
		animationIndex = 1;
	}

	void PlayAnimation (int animationIndex) {
		if (!characterManager.characterInstances[characterIndexes[animationIndex]]) {
			Debug.LogError(characterIndexes[animationIndex] + " - character not instantiated");
		}
		animationController = characterManager.characterInstances[characterIndexes[animationIndex]].GetComponent<AnimationController>();
		animationController.MoveTo(positions[animationIndex]);
	}

	void Update () {
		if (!playingAnimation || !animationController.playing) {
			return;
		}

		if (animationIndex < positions.Length) {
			PlayAnimation(animationIndex);
			animationIndex++;
		} else {
			playingAnimation = false;
			animationIndex = 1;

			CallNextEvents();
		}
	}
}
