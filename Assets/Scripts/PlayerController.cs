using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerAnimatorController), typeof(WeaponController))]
public class PlayerController : MonoBehaviour {
	public float walkForce = 300.0f;
	public float fireTime = 0.2f;
	public float recoilTime = 0.2f;

	public int weaponIndex;
	public bool weaponDrawn;

	public bool isMoving;
	/* 0 idle, 1 fire, 2 recoil */
	public int fireState;
	public bool isDead;

	GameController gameController;
	InputController inputController;
	PlayerAnimatorController animatorManager;
	InventoryController inventory;
	WeaponController weaponController;

	int gameState;


	void Start () {
		gameController = GameController.gameController;
		inputController = GameController.inputController;
		animatorManager = GetComponent<PlayerAnimatorController>();
		inventory = GameController.inventoryController;
		weaponController = GetComponent<WeaponController>();
	}

	void FixedUpdate () {
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
			if (weaponDrawn) {
				weaponDrawn = weaponController.HasWeapon(weaponIndex);
			}
		}

		isMoving = false;
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
					fireState = 1;
				}
			}
			break;
		case GameController.stateMenu:

			break;
		case GameController.stateMessage:
			
			break;
		}

		switch (fireState) {
		case 1:
			if (Time.time > weaponController.lastFireTime + fireTime) {
				fireState = 2;
			}
			break;
		case 2:
			if (Time.time > weaponController.lastFireTime + fireTime + recoilTime) {
				fireState = 0;
			}
			break;
		}

		if (!inventory.HasHealth()) {
			isDead = true;
			RefreshPlayerAnimation();
		}
	}

	void RefreshPlayerAnimation () {
		if (isDead) {
			animatorManager.PlayAnimation(inputController.orientationIndex, PlayerAnimatorController.stateDead, 0);
			return;
		}

		switch (gameState) {
		case GameController.stateSearch:
			if (isMoving) {
				animatorManager.PlayAnimation(inputController.orientationIndex, PlayerAnimatorController.stateWalk, 0);
			} else {
				animatorManager.PlayAnimation(inputController.orientationIndex, PlayerAnimatorController.stateIdle, 0);
			}

			break;
		case GameController.stateFight:
			switch (fireState) {
			case 0:
				if (isMoving) {
					animatorManager.PlayAnimation(inputController.orientationIndex, weaponController.weapons[weaponIndex].animationIndex, PlayerAnimatorController.stateWalk, 0);
				} else {
					animatorManager.PlayAnimation(inputController.orientationIndex, weaponController.weapons[weaponIndex].animationIndex, PlayerAnimatorController.stateIdle, 0);
				}
				break;
			case 1:
				animatorManager.PlayAnimation(inputController.orientationIndex, weaponController.weapons[weaponIndex].animationIndex, PlayerAnimatorController.stateFire, 0);

				break;
			case 2:
				animatorManager.PlayAnimation(inputController.orientationIndex, weaponController.weapons[weaponIndex].animationIndex, PlayerAnimatorController.stateFire, 1);

				break;
			}

			break;
		case GameController.stateMenu:
			animatorManager.PlayAnimation(inputController.orientationIndex, PlayerAnimatorController.stateIdle, 0);

			break;
		case GameController.stateMessage:
			animatorManager.PlayAnimation(inputController.orientationIndex, PlayerAnimatorController.stateIdle, 0);

			break;
		}
	}

	void ApplyDamage (float damage) {
		inventory.SubHealth(damage);
	}
}
