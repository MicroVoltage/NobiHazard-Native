using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AnimatorManager))]
public class AnimationController : MonoBehaviour {
	public float walkSpeed = 1.0f;
	public float arriveRadius =0.1f;

	public bool isDead;
	public bool weaponDrawn;
	public int weaponIndex;
	public int orientationIndex;

	public bool playing;
	public Vector2 wantedPosition;

	private AnimatorManager animatorManager;

	void Start () {
		animatorManager = GetComponent<AnimatorManager>();

		MoveTo(Vector2.one);
	}

	void Update () {
		if (isDead) {
			animatorManager.PlayAnimation(orientationIndex, weaponIndex, AnimatorManager.stateDead, 0);
			playing = false;
			return;
		}

		if (!playing) {
			if (weaponDrawn) {
				animatorManager.PlayAnimation(orientationIndex, weaponIndex, AnimatorManager.stateIdle, 0);
			} else {
				animatorManager.PlayAnimation(orientationIndex, AnimatorManager.stateIdle, 0);
			}
			return;
		} else if (Vector2.Distance(transform.position, wantedPosition) < arriveRadius) {
			playing = false;
		}

		if (weaponDrawn) {
			animatorManager.PlayAnimation(GetOrientationIndex(), weaponIndex, AnimatorManager.stateWalk, 0);
		} else {
			animatorManager.PlayAnimation(GetOrientationIndex(), AnimatorManager.stateWalk, 0);
		}

		transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * walkSpeed);
	}

	public int GetOrientationIndex () {
		Vector2 deltaPosition = wantedPosition - (Vector2)transform.position;
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

	public void ChangeWeaponState (bool newWeaponDrawn, int newWeaponIndex) {
		weaponDrawn = newWeaponDrawn;
		weaponIndex = newWeaponIndex;
	}

	public void MoveTo (Vector2 position, float speed) {
		wantedPosition = position;
		walkSpeed = speed;
		playing = true;
	}
	public void MoveTo (Vector2 position) {
		MoveTo(position, walkSpeed);
	}
}
