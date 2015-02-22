using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AnimatorManager))]
public class PlayerController : MonoBehaviour {

	public float walkForce = 300.0f;
	public float maxHealth = 100.0f;
	public string[] weaponNames;
	public float[] weaponFireInterval;
	public float decapDamage = 200.0f;

	public bool isWalking;
	public bool isArmed;
	public bool canFire = true;

	public bool isDecaped = false;
	public bool isDead = false;

	public float health = 100.0f;
	public int weaponIndex;
	
	private float lastFireTime;

	private InputController inputController;
	private AnimatorManager animatorManager;


	void Start () {
		inputController = GameController.inputController;
		animatorManager = GetComponent<AnimatorManager>();
	}

	void Update () {
		RefreshPlayerState();

		RefreshPlayerAnimation();



		if (isDead) {
			if (isDecaped) {
				animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 3, 3);
			} else {
				animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 3, 0);
			}
			return;
		} else if (isDecaped) {
			animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 3, 1);
			health -= decapDamage * Time.deltaTime;
			if (health <= 0) {
				isDead = true;
			}
			return;
		}

		canFire = Time.time - lastFireTime >= weaponFireInterval[weaponIndex];

		switch (GameController.gameController.gameState) {
		case 0:
			isWalking = inputController.direction.sqrMagnitude > 0;
			if (canFire && inputController.A) {
				animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 2, 0);
				canFire = false;
				lastFireTime = Time.time;
			} else if (isWalking) {
					rigidbody2D.AddForce(inputController.direction * walkForce);
					animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 1, 0);
			} else {
					animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 0, 0);
			}
			break;
		case 1:
			isWalking = false;
			animatorManager.PlayAnimation(inputController.orientationIndex, weaponIndex, 0, 0);
			break;
		}
	}

	void RefreshPlayerState () {

	}

	void RefreshPlayerAnimation () {

	}
}
