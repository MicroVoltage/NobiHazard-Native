using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AnimatorManager), typeof(WeaponController))]
public class PlayerController : MonoBehaviour {
	public float walkForce = 300.0f;

	public int weaponIndex = 0;
	public bool weaponDrawn = false;

	public bool isMoving = false;
	public bool isFiring = false;
	public bool isDead = false;

	private GameController gameController;
	private InputController inputController;
	private AnimatorManager animatorManager;
	private Inventory inventory;
	private WeaponController weaponController;

	private int gameState;


	void Start () {
		gameController = GameController.gameController;
		inputController = GameController.inputController;
		animatorManager = GetComponent<AnimatorManager>();
		inventory = GameController.inventory;
		weaponController = GetComponent<WeaponController>();
	}

	void Update () {
		if (isDead) {
			return;
		}

		gameState = gameController.gameState;

		RefreshPlayerState();

		RefreshPlayerAnimation();
	}

	void RefreshPlayerState () {
		if (inputController.shift) {
			weaponDrawn = !weaponDrawn;
		}

		isMoving = false;
		isFiring = false;
		switch (gameState) {
		case GameController.stateSearch:
			if (weaponDrawn) {
				gameController.gameState = GameController.stateFight;
				break;
			}

			isMoving = inputController.direction.sqrMagnitude > 0;
			rigidbody2D.AddForce(inputController.direction.normalized * walkForce);
			break;
		case GameController.stateFight:
			if (!weaponDrawn) {
				gameController.gameState = GameController.stateSearch;
				break;
			}

			isMoving = inputController.direction.sqrMagnitude > 0;
			rigidbody2D.AddForce(inputController.direction.normalized * walkForce);

			if (inputController.fire) {
				if (weaponController.Fire(weaponIndex) == 0) {
					isFiring = true;
				}
			}
			break;
		case GameController.stateMenu:

			break;
		case GameController.stateMessage:
			
			break;
		}


		if (!inventory.HasHealth()) {
			isDead = true;
			RefreshPlayerAnimation();
		}
	}

	void RefreshPlayerAnimation () {
		if (isDead) {
			animatorManager.PlayAnimation(inputController.orientationIndex, AnimatorManager.stateDead, 0);
			return;
		}

		switch (gameState) {
		case GameController.stateSearch:
			if (isMoving) {
				animatorManager.PlayAnimation(inputController.orientationIndex, AnimatorManager.stateWalk, 0);
			} else {
				animatorManager.PlayAnimation(inputController.orientationIndex, AnimatorManager.stateIdle, 0);
			}

			break;
		case GameController.stateFight:
			if (inputController.fire) {
				animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, AnimatorManager.stateFire, 0);
			} else {
				if (isMoving) {
					animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, AnimatorManager.stateWalk, 0);
				} else {
					animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, AnimatorManager.stateIdle, 0);
				}
			}

			break;
		case GameController.stateMenu:
			animatorManager.PlayAnimation(inputController.orientationIndex, AnimatorManager.stateIdle, 0);

			break;
		case GameController.stateMessage:
			animatorManager.PlayAnimation(inputController.orientationIndex, AnimatorManager.stateIdle, 0);

			break;
		}
	}

	void ApplyDamage (float damage) {
		inventory.SubHealth(damage);
	}
}
