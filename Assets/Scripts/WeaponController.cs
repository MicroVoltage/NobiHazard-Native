using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public int weaponControllerSize;
	// Name has to be corresponding to the AnimatorManager's weaponNames
	public string[] names;
	public int[] weaponAnimationIndexes;
	public int[] weaponInventoryIndexes;
	public int[] chipInventoryIndexes;
	// Generic weapon parameters
	public float[] damages;
//	public bool[] isShotgun;
	public float[] forces;
	public float[] recoilForces;
	public float[] destances;
	public float[] intervals;
	public LayerMask hitLayers;
	// If bullet, don't use generic ApplyDamage(float damage)
	public GameObject[] bullets;
	// Bullets left for each weapon
	public int[] bulletCounts;

	InputController inputController;
	InventoryController inventoryController;

	[HideInInspector]
	public float lastFireTime;


	public void ResizeWeaponController () {
		string[] namesX = names;
		int[] weaponIndexesX = weaponInventoryIndexes;
		int[] chipIndexesX = chipInventoryIndexes;
		float[] damagesX = damages;
		float[] forcesX = forces;
		float[] recoilForcesX = recoilForces;
		float[] destancesX = destances;
		float[] intervalsX = intervals;
		GameObject[] bulletsX = bullets;
		
		names = new string[weaponControllerSize];
		weaponInventoryIndexes = new int[weaponControllerSize];
		chipInventoryIndexes = new int[weaponControllerSize];
		damages = new float[weaponControllerSize];
		forces = new float[weaponControllerSize];
		recoilForces = new float[weaponControllerSize];
		destances = new float[weaponControllerSize];
		intervals = new float[weaponControllerSize];
		bullets = new GameObject[weaponControllerSize];
		
		System.Array.Copy(namesX, names, Mathf.Min(namesX.Length, names.Length));
		System.Array.Copy(weaponIndexesX, weaponInventoryIndexes, Mathf.Min(weaponIndexesX.Length, weaponInventoryIndexes.Length));
		System.Array.Copy(chipIndexesX, chipInventoryIndexes, Mathf.Min(chipIndexesX.Length, chipInventoryIndexes.Length));
		System.Array.Copy(damagesX, damages, Mathf.Min(damagesX.Length, damages.Length));
		System.Array.Copy(forcesX, forces, Mathf.Min(forcesX.Length, forces.Length));
		System.Array.Copy(recoilForcesX, recoilForces, Mathf.Min(recoilForcesX.Length, recoilForces.Length));
		System.Array.Copy(destancesX, destances, Mathf.Min(destancesX.Length, destances.Length));
		System.Array.Copy(intervalsX, intervals, Mathf.Min(intervalsX.Length, intervals.Length));
		System.Array.Copy(bulletsX, bullets, Mathf.Min(bulletsX.Length, bullets.Length));

		int[] weaponAnimationIndexesX = weaponAnimationIndexes;
		weaponAnimationIndexes = new int[weaponControllerSize];
		System.Array.Copy (weaponAnimationIndexesX, weaponAnimationIndexes, Mathf.Min (weaponAnimationIndexesX.Length, weaponAnimationIndexes.Length));
	}

	void Start () {
		inputController = GameController.inputController;
		inventoryController = GameController.inventoryController;

		bulletCounts = new int[names.Length];
	}

	public bool HasWeapon (int weaponIndex) {
		return inventoryController.HasItem(weaponInventoryIndexes[weaponIndex]);
	}
	
	/** Fire
	 * @ return
	 * 0: successful
	 * 1: unsuccessful - isn't cooled down
	 * 2: unsuccessful - no weapon
	 * 3: unsuccessful - no bullet
	 **/
	public int Fire (int weaponIndex) {
		if (!inventoryController.HasItem(weaponInventoryIndexes[weaponIndex])) {
			return 2;
		}

		if (Time.time - lastFireTime < intervals[weaponIndex]) {
			return 1;
		}
		//Renew bullet count
		if (weaponInventoryIndexes[weaponIndex] != chipInventoryIndexes[weaponIndex]) {
			if (bulletCounts[weaponIndex] <= 0) {
				if (inventoryController.HasItem(chipInventoryIndexes[weaponIndex])) {
					inventoryController.SubItem(chipInventoryIndexes[weaponIndex]);
					bulletCounts[weaponIndex] = inventoryController.GetItemInt(chipInventoryIndexes[weaponIndex]);
				} else {
					return 3;
				}
			} else {
				bulletCounts[weaponIndex]--;
			}
		}

		if (bullets[weaponIndex]) {
			Instantiate(bullets[weaponIndex], GetBulletPosition(), GetBulletRotation());
		} else {
			FireGeneric(weaponIndex, destances[weaponIndex]);
		}

		lastFireTime = Time.time;
		return 0;
	}

	void FireGeneric (int weaponIndex, float destance) {
		rigidbody2D.AddForce(-inputController.orientation * recoilForces[weaponIndex]);

		Debug.DrawLine(transform.position, transform.position + (Vector3)inputController.orientation * destance, Color.red);

		RaycastHit2D[] raycastHit = Physics2D.LinecastAll(
			transform.position,
			(Vector2)transform.position + inputController.orientation * destance, hitLayers.value);
		for (int i=0; i<raycastHit.Length; i++) {
			Debug.Log(raycastHit[i].collider.gameObject.name);

			if (raycastHit[i].rigidbody) {
				raycastHit[i].rigidbody.AddForce(inputController.orientation * forces[weaponIndex]);
			}
			raycastHit[i].collider.SendMessage("ApplyDamage", damages[weaponIndex]);
		}
	}

	Vector3 GetBulletPosition () {
		Vector3 localBulletPosition = inputController.orientation * GameController.gameScale;
		return transform.TransformPoint(localBulletPosition);
	}

	Quaternion GetBulletRotation () {
		return Quaternion.LookRotation(inputController.orientation);
	}
}
