using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weapon {
	// No actual use
	public string name;

	public int animationIndex;
	public int weaponInventoryIndex;
	public int clipInventoryIndex;

	// Generic weapon parameters
	public float hitDamage;
	public float hitDestance;
	public float hitForce;
	public float recoilForce;
	public float fireInterval;

	// Special custom parameters
	public GameObject bullet;
}

public class WeaponController : MonoBehaviour {

	public LayerMask hitLayers;
	public Weapon[] weapons;
	// Bullets left for each weapon
	public int[] bulletCounts;

	InputController inputController;
	InventoryController inventoryController;

	[HideInInspector]
	public float lastFireTime;


	void Start () {
		inputController = GameController.inputController;
		inventoryController = GameController.inventoryController;

		bulletCounts = new int[weapons.Length];
	}

	public bool HasWeapon (int weaponIndex) {
		return inventoryController.HasItem(weapons[weaponIndex].weaponInventoryIndex);
	}
	
	/** Fire
	 * @ return
	 * 0: successful
	 * 1: unsuccessful - isn't cooled down
	 * 2: unsuccessful - no weapon
	 * 3: unsuccessful - no bullet
	 **/
	public int Fire (int weaponIndex) {
		if (!HasWeapon(weaponIndex)) {
			return 2;
		}

		if (Time.time - lastFireTime < weapons[weaponIndex].fireInterval) {
			return 1;
		}
		//Renew bullet count
		if (weapons[weaponIndex].weaponInventoryIndex != weapons[weaponIndex].clipInventoryIndex) {
			if (bulletCounts[weaponIndex] <= 0) {
				if (inventoryController.HasItem(weapons[weaponIndex].clipInventoryIndex)) {
					inventoryController.SubItem(weapons[weaponIndex].clipInventoryIndex);
					bulletCounts[weaponIndex] = inventoryController.GetItemInt(weapons[weaponIndex].clipInventoryIndex);
				} else {
					return 3;
				}
			} else {
				bulletCounts[weaponIndex]--;
			}
		}

		if (weapons[weaponIndex].bullet) {
			Instantiate(weapons[weaponIndex].bullet, GetBulletPosition(), GetBulletRotation());
		} else {
			FireGeneric(weaponIndex, weapons[weaponIndex].hitDestance);
		}

		lastFireTime = Time.time;
		return 0;
	}

	void FireGeneric (int weaponIndex, float destance) {
		rigidbody2D.AddForce(-inputController.orientation * weapons[weaponIndex].recoilForce);

		Debug.DrawLine(transform.position, transform.position + (Vector3)inputController.orientation * destance, Color.red);

		RaycastHit2D[] raycastHit = Physics2D.LinecastAll(
			transform.position,
			(Vector2)transform.position + inputController.orientation * destance, hitLayers.value);
		for (int i=0; i<raycastHit.Length; i++) {
			Debug.Log(raycastHit[i].collider.gameObject.name);

			if (raycastHit[i].rigidbody) {
				raycastHit[i].rigidbody.AddForce(inputController.orientation * weapons[weaponIndex].hitForce);
			}
			raycastHit[i].collider.SendMessage("ApplyDamage", weapons[weaponIndex].hitDamage);
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
