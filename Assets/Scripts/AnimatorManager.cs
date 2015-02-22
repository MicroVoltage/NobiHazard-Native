using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorManager : MonoBehaviour {
	public string avatarName;
	public static string s = "-";

	public static string[] orientationNames = {"back", "right", "front", "left"};

	public string[] idleNames;
	public string[] walkNames;
	public string[] fireNames;
	public string[] deadNames;
	public const int stateIdle = 0;
	public const int stateWalk = 1;
	public const int stateFire = 2;
	public const int stateDead = 3;

	public string[] weaponNames;

	private Animator animator;

	private string lastAnimationName;


	void Start () {
		animator = GetComponent<Animator>();
	}


	/** PlayAnimation
	 * int orientation : 0 - 3 : back right front left
	 * int state : 0 1 2 : idle walk fire
	 **/
	public void PlayAnimation (int orientation, int weapon, int state, int stateParam) {
		string stateName = "";
		switch (state) {
		case 0:
			stateName = idleNames[stateParam];
			break;
		case 1:
			stateName = walkNames[stateParam];
			break;
		case 2:
			stateName = fireNames[stateParam];
			break;
		default:
			Debug.LogError(state.ToString() + " - wrong state call");
			break;
		}

		string animationName = avatarName +s+ weaponNames[weapon] +s+ stateName +s+ orientationNames[orientation];

		PlayAnimation(animationName);
	}

	/** PlayAnimation
	 * int orientation : 0 - 3 : back right front left
	 * int state : 0 1 3 : idle walk dead
	 **/
	public void PlayAnimation (int orientation, int state, int stateParam) {
		string stateName = "";
		switch (state) {
		case 0:
			stateName = idleNames[stateParam];
			break;
		case 1:
			stateName = walkNames[stateParam];
			break;
		case 3:
			stateName = deadNames[stateParam];
			break;
		default:
			Debug.LogError(state.ToString() + " - wrong state call");
			break;
		}
		
		string animationName = avatarName +s+ stateName +s+ orientationNames[orientation];

		PlayAnimation(animationName);
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
