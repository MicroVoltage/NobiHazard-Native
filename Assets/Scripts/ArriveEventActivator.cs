using UnityEngine;
using System.Collections;

public class ArriveEventActivator : MonoBehaviour {
	void OnTriggerEnter (Collider other) {
		Debug.Log(other.gameObject.name);

		foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, GameController.gameScale, Vector2.one)) {
			Debug.Log(hit.collider.gameObject.name);
			Collider2D collider = hit.collider;
			if (!collider.CompareTag("Event")) {
				return;
			}

			Debug.Log("OnArrive - even fired to " + collider.gameObject.name);
			collider.gameObject.SendMessage("OnArrive", SendMessageOptions.DontRequireReceiver);
		}
	}
}
