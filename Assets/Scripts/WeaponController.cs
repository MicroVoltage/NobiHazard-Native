using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	// Name has to be corresponding to the AnimatorManager's weaponNames
	public string[] names;
	public int[] weaponIndexes;
	public int[] chipIndexes;
	// Generic weapon parameters
	public float[] damages;
	public bool[] isShotgun;
	public float[] force;
	public float[] recoilForce;
	public float[] fireIntervals;
	public LayerMask hitLayers;
	// If bullet, don't use generic ApplyDamage(float damage)
	public GameObject[] bullets;
	// Bullets left for each weapon
	[HideInInspector]
	public int[] bulletCounts;

	private InputController inputController;
	private Inventory inventory;

	private float lastFireTime;

	void Start () {
		inputController = GameController.inputController;
		inventory = GetComponent<Inventory>();

		bulletCounts = new int[names.Length];
	}

	/** Fire
	 * @ return
	 * 0: successful
	 * 1: unsuccessful - isn't cooled down
	 * 2: unsuccessful - no weapon
	 * 3: unsuccessful - no bullet
	 **/
	public int Fire (int weaponIndex) {
		if (!inventory.HasItem(weaponIndexes[weaponIndex])) {
			return 2;
		}

		if (Time.time - lastFireTime < fireIntervals[weaponIndex]) {
			return 1;
		}
		//Renew bullet count
		if (bulletCounts[weaponIndex] <= 0) {
			if (inventory.HasItem(chipIndexes[weaponIndex])) {
				inventory.SubItem(chipIndexes[weaponIndex]);
				bulletCounts[weaponIndex] = inventory.GetItemInt(chipIndexes[weaponIndex]);
			} else {
				return 3;
			}
		} else {
			bulletCounts[weaponIndex]--;
		}

		if (bullets[weaponIndex]) {
			Instantiate(bullets[weaponIndex], GetBulletPosition(), GetBulletRotation());
		} else {
			FireGeneric(weaponIndex);
		}
		lastFireTime = Time.time;
		return 0;
	}

	void FireGeneric (int weaponIndex) {
		rigidbody2D.AddForce(-inputController.orientation * recoilForce[weaponIndex]);
		RaycastHit2D[] raycastHit = Physics2D.LinecastAll(
			transform.position,
			(Vector2)transform.position + inputController.orientation * 1000.0f,
			hitLayers);
		for (int i=0; i<raycastHit.Length; i++) {
			if (raycastHit[i].rigidbody) {
				raycastHit[i].rigidbody.AddForce(inputController.orientation * force[weaponIndex]);
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
