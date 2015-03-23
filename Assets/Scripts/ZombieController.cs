using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericAnimatorController))]
public class ZombieController : MonoBehaviour {

	public float walkForce = 300.0f;
	public float fireForce = 400.0f;
	public float drawTime = 0.2f;
	public float fireTime = 0.2f;
	public float recoilTime = 0.3f;

	public bool wondering;
	public float wonderInterval = 2.0f;
	public float arriveRadius = 0.1f;
	/* 0 idle, 1 draw, 2 fire, 3 recoil */
	public int fireState;
	public bool isDead;

	public LayerMask hitLayers;
	public float destance = 2.0f;
	public float damage = 15.0f;

	GenericAnimatorController genericAnimatorManager;
	HealthController healthController;
	BoxCollider2D playerDetecter;

	Vector2 wantedPosition;
	Vector2 deltaPosition;
	float lastWonderTime;
	int orientationIndex;
	float lastDrawTime;
	Vector2 orientation;

	void Start () {
		genericAnimatorManager = GetComponent<GenericAnimatorController>();
		healthController = GetComponent<HealthController>();
		playerDetecter = GetComponent<BoxCollider2D>();
	}

	void FixedUpdate () {
		if (isDead) {
			return;
		}
		if (!healthController.HasHealth()) {
			isDead = true;
			genericAnimatorManager.PlayAnimation(orientationIndex, "dead");
			return;
		}

		if (wondering && Time.time > lastWonderTime + wonderInterval) {
			wantedPosition = (Vector2)transform.position + new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
			lastWonderTime = Time.time;
		} else if (!wondering) {
			wantedPosition = GameController.heroController.heroInstance.transform.position;
		}

		deltaPosition = wantedPosition - (Vector2)transform.position;
		rigidbody2D.AddForce(deltaPosition.normalized * walkForce);

		SetOrientationIndex(GetOrientationIndex());

		playerDetecter.center = orientation * GameController.gameScale;

		switch (fireState) {
		case 0:
			genericAnimatorManager.PlayAnimation(orientationIndex, "walk");
			break;
		case 1:
			if (Time.time > lastDrawTime + drawTime) {
				Fire();
				fireState = 2;
			}
			genericAnimatorManager.PlayAnimation(orientationIndex, "draw");

			break;
		case 2:
			if (Time.time > lastDrawTime + drawTime + fireTime) {
				fireState = 3;
			}
			genericAnimatorManager.PlayAnimation(orientationIndex, "fire");

			break;
		case 3:
			if (Time.time > lastDrawTime + drawTime + fireTime + recoilTime) {
				fireState = 0;
			}
			genericAnimatorManager.PlayAnimation(orientationIndex, "recoil");

			break;
		}
	}

	void Fire () {
		rigidbody2D.AddForce(orientation * fireForce);
		
		Debug.DrawLine(transform.position, transform.position + (Vector3)orientation * destance, Color.red);
		
		RaycastHit2D[] raycastHit = Physics2D.LinecastAll(
			transform.position,
			(Vector2)transform.position + orientation * destance, hitLayers.value);
		for (int i=0; i<raycastHit.Length; i++) {
			Debug.Log(raycastHit[i].collider.gameObject.name);
			
			if (raycastHit[i].rigidbody) {
				raycastHit[i].rigidbody.AddForce(orientation * fireForce);
			}
			raycastHit[i].collider.SendMessage("ApplyDamage", damage);
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Player") {
			fireState = 1;
			lastDrawTime = Time.time;
		}
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (collider.tag == "Player" && fireState == 0) {
			fireState = 1;
			lastDrawTime = Time.time;
		}
	}

	public int GetOrientationIndex () {
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

	public void SetOrientationIndex (int newOrientationIndex) {
		switch (newOrientationIndex) {
		case 0:
			orientation.x = 0;
			orientation.y = 1;
			break;
		case 1:
			orientation.x = 1;
			orientation.y = 0;
			break;
		case 2:
			orientation.x = 0;
			orientation.y = -1;
			break;
		case 3:
			orientation.x = -1;
			orientation.y = 0;
			break;
		}
		orientationIndex = newOrientationIndex;
	}
}
