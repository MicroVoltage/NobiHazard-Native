﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class GenericAnimationEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	public GameObject[] nextEvents;
	
	void OnSceneEnter () {
		if (autoStart) {
			OnGenericAnimationEvent();
		}
	}
	
	void OnApproach () {
		if (approachStart) {
			OnGenericAnimationEvent();
		}
	}
	
	void OnExam () {
		if (examStart) {
			OnGenericAnimationEvent();
		}
	}
	
	void OnChainEnter () {
		OnGenericAnimationEvent();
	}
	
	void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}

	public bool startAnimationSequence;
	public bool endAnimationSequence;

	public float speed = 8.0f;
	public float arriveRadius = 0.1f;

	public bool hasOrientation;
	public string[] names;
	public Vector2[] positions;

	public bool isPlaying;

	private GameController gameController;
	private Animator animator;

	private int animationIndex;
	private Vector2 wantedPosition;
	private Vector2 deltaPosition;
	public int orientationIndex;
	private string lastAnimationName;

	void Start () {
		gameObject.name = gameObject.name + "-genericAnimation";
		 
		gameController = GameController.gameController;
		animator = GetComponent<Animator>();

		for (int i=0; i<names.Length; i++) {
			if (names[i] == "") {
				names[i] = names[i-1];
			}
		}
	}

	void Update () {
		if (!isPlaying) {
			return;
		} else if (deltaPosition.magnitude < arriveRadius) {
			if (animationIndex < positions.Length - 1) {
				animationIndex ++;

				wantedPosition = (Vector2)transform.position + positions[animationIndex] * GameController.gameScale;
				deltaPosition = wantedPosition - (Vector2)transform.position;
				GetOrientationIndex();
			} else {
				isPlaying = false;
				animationIndex = 0;
				if (endAnimationSequence) {
					Debug.Log(gameObject.name + " - end animation sequence");
					gameController.gameState = GameController.stateSearch;
				}
				CallNextEvents();
				return;
			}
		}

		deltaPosition = wantedPosition - (Vector2)transform.position;

		// Play animation
		string animationName = names[animationIndex];
		if (hasOrientation) {
			animationName += AnimatorManager.s + AnimatorManager.orientationNames[orientationIndex];
		}
		PlayAnimation(animationName);

		transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * speed);
	}

	void OnGenericAnimationEvent () {
		Debug.Log(gameObject.name + " - get generic animation event");
		if (startAnimationSequence) {
			Debug.Log(gameObject.name + " - start generic animation sequence");
			gameController.gameState = GameController.stateAnimation;
		}

		isPlaying = true;
		animationIndex = 0;

		wantedPosition = (Vector2)transform.position + positions[animationIndex] * GameController.gameScale;
		deltaPosition = wantedPosition - (Vector2)transform.position;
		GetOrientationIndex();
	}

	public int GetOrientationIndex () {
		if (deltaPosition.magnitude < arriveRadius) {
			return orientationIndex;
		}

		if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y)) {
			if (deltaPosition.x > 0) {
				orientationIndex = AnimatorManager.orientRight;
			} else {
				orientationIndex = AnimatorManager.orientLeft;
			}
		} else if (Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y)) {
			if (deltaPosition.y > 0) {
				orientationIndex = AnimatorManager.orientBack;
			} else {
				orientationIndex = AnimatorManager.orientFront;
			}
		}


		return orientationIndex;
	}


	public void PlayAnimation (string animationName) {
		if (animationName == lastAnimationName) {
			return;
		}
		
		lastAnimationName = animationName;
		
		AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (!animatorStateInfo.IsName(animationName)) {
			animator.Play(animationName, 0);
		}
	}
}