using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class EventActivator : MonoBehaviour {
	private InputController inputController;
	private BoxCollider2D eventDetector;
	
	void Start () {
		inputController = GameController.gameController.GetComponent<InputController>();
		eventDetector = GetComponent<BoxCollider2D>();
		
		eventDetector.size = new Vector2(GameController.gameScale, GameController.gameScale);
	}
	
	void Update () {
		transform.localPosition = inputController.orientation * GameController.gameScale;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (!collider.CompareTag("Event")) {
			return;
		}

		collider.gameObject.SendMessage("OnArrive", SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (!collider.CompareTag("Event")) {
			return;
		}
		if (inputController.A) {
			Debug.Log("OnExam - even fired to " + collider.gameObject.name);
			collider.gameObject.SendMessage("OnExam", SendMessageOptions.DontRequireReceiver);
		}
	}
}
