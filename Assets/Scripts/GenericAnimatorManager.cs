using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class GenericAnimatorManager : MonoBehaviour {
	public string avatarName;
	public static string s = "-";

	public static string[] orientationNames = {"back", "right", "front", "left"};
	public const int orientBack = 0;
	public const int orientRight = 1;
	public const int orientFront = 2;
	public const int orientLeft = 3;

	public string[] animationNames;

	Animator animator;
	string lastAnimationName;


	void Start () {
		animator = GetComponent<Animator>();
	}

	public void PlayAnimation (int orientation, int animationIndex) {
		string animationName = avatarName +s+ animationNames[animationIndex] +s+ orientationNames[orientation];
		
		PlayAnimation(animationName);
	}
	public void PlayAnimation (int orientation, string animationName) {
		string fullAnimationName = avatarName +s+ animationName +s+ orientationNames[orientation];
		
		PlayAnimation(fullAnimationName);
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
