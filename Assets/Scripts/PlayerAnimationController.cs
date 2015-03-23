using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerAnimatorController))]
public class PlayerAnimationController : MonoBehaviour {
	public float walkSpeed = 1.0f;
	public float arriveRadius =0.1f;

	public bool isDead;
	public bool weaponDrawn;
	public int weaponIndex;
	public int orientationIndex;

	public bool playing;
	public Vector2 wantedPosition;

	private PlayerAnimatorController animatorManager;
//	private GameController gameController;

	void Start () {
		animatorManager = GetComponent<PlayerAnimatorController>();
//		gameController = GameController.gameController;
	}

	void Update () {
		if (isDead) {
			animatorManager.PlayAnimation(orientationIndex, weaponIndex, PlayerAnimatorController.stateDead, 0);
			playing = false;
			return;
		}

		if (!playing) {
			if (weaponDrawn) {
				animatorManager.PlayAnimation(orientationIndex, weaponIndex, PlayerAnimatorController.stateIdle, 0);
			} else {
				animatorManager.PlayAnimation(orientationIndex, PlayerAnimatorController.stateIdle, 0);
			}
			return;
		} else if (Vector2.Distance(transform.position, wantedPosition) < arriveRadius) {
			playing = false;
		}

		if (weaponDrawn) {
			animatorManager.PlayAnimation(GetOrientationIndex(), weaponIndex, PlayerAnimatorController.stateWalk, 0);
		} else {
			animatorManager.PlayAnimation(GetOrientationIndex(), PlayerAnimatorController.stateWalk, 0);
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
				orientationIndex = PlayerAnimatorController.orientRight;
			} else {
				orientationIndex = PlayerAnimatorController.orientLeft;
			}
		} else if (Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y)) {
			if (deltaPosition.y > 0) {
				orientationIndex = PlayerAnimatorController.orientBack;
			} else {
				orientationIndex = PlayerAnimatorController.orientFront;
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
