using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class EventActivator : MonoBehaviour {
	private InputController inputController;
	private BoxCollider2D eventDetector;
	private GameController gameController;
	
	void Start () {
		inputController = GameController.inputController;
		eventDetector = GetComponent<BoxCollider2D>();
		gameController = GameController.gameController;
		
		eventDetector.size = new Vector2(GameController.gameScale / 2, GameController.gameScale / 2);
	}
	
	void Update () {
		transform.localPosition = inputController.orientation * GameController.gameScale;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (!collider.CompareTag("Event")) {
			return;
		}
		if (gameController.gameState == GameController.stateSearch || gameController.gameState == GameController.stateFight) {
			Debug.Log("OnApproach - even fired to " + collider.gameObject.name);
			collider.gameObject.SendMessage("OnApproach", SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (!collider.CompareTag("Event")) {
			return;
		}

		if (inputController.exam) {
			Debug.Log("OnExam - even fired to " + collider.gameObject.name);
			collider.gameObject.SendMessage("OnExam", SendMessageOptions.DontRequireReceiver);
		}
	}
}
