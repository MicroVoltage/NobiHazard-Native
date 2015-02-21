using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorManager : MonoBehaviour {
	public string avatarName;
	public static string s = "-";

	public static string[] orientationNames = {"back", "right", "front", "left"};
	public static int back = 0;
	public static int right = 1;
	public static int front = 2;
	public static int left = 3;

	public string[] idleNames;
	public string[] walkNames;
	public string[] fireNames;
	public string[] deadNames;
	public static int idle = 0;
	public static int walk = 1;
	public static int fire = 2;
	public static int dead = 3;

	public string[] weaponNames;

	public bool testAnimation;
	public int orientation;
	public int weapon;
	public int state;
	public int stateParam;

	private Animator animator;


	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update () {
		if (!testAnimation) {
			return;
		}

		PlayAnimation(orientation, weapon, state, stateParam);

	}

	/** PlayAnimation
	 * int orientation : 0 - 3 : back right front left
	 * int state : 0 - 3 : idle walk fire dead
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
		case 3:
			stateName = deadNames[stateParam];
			break;
		default:
			Debug.LogError(state.ToString() + " - wrong state call");
			break;
		}

		string animationName = "";
		if (weapon == 0) {
			animationName = avatarName +s+ stateName +s+ orientationNames[orientation];
		} else {
			animationName = avatarName +s+ weaponNames[weapon - 1] +s+ stateName +s+ orientationNames[orientation];
		}

		AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (!animatorStateInfo.IsName(animationName)) {
			animator.Play(animationName, 0);
		}
	}
}
