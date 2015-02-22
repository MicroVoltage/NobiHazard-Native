using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public Vector2 orientation;
	public int orientationIndex;
	public Vector2 direction;
	public Vector2 normalizedDirection;

	public float H;
	public float V;

	public bool A;
	public bool B;
	public bool S;
	public bool Adown;
	public bool Bdown;
	public bool Sdown;

	// Exam button
	public bool exam;
	// Fire button
	public bool fire;
	// Menu button
	public bool menu;
	// Cancel button
	public bool cancel;
	// Switch button
	public bool shift;

	private GameController gameController;

	void Awake () {
		if (GameController.inputController == null) {
			GameController.inputController = this;
		} else if (GameController.inputController != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		gameController = GameController.gameController;

		orientation = Vector3.zero;
		orientationIndex = 2;
	}

	void Update () {
		exam = false;
		fire = false;
		menu = false;
		cancel = false;
		shift = false;
		RefreshInputStates();

		switch (gameController.gameState) {
		case GameController.stateSearch:
			SetOrientation();

			exam = Adown;
			menu = Bdown;
			shift = Sdown;
			break;
		case GameController.stateFight:
			if (!B) {
				SetOrientation();
			}

			fire = A;
			shift = Adown;
			break;
		case GameController.stateMenu:

			break;
		case GameController.stateMessage:
			cancel = Adown || Bdown;
			break;
		default:
			Debug.LogError(gameController.gameState + " - input state not exist");
			break;
		}
	}

	void SetOrientation () {
		direction = new Vector2(H, V);
		normalizedDirection = direction.normalized;

		Vector2 newOrientation = Vector3.zero;
		if (direction.sqrMagnitude > 1.0f) {
			newOrientation = direction - orientation;
		} else if (direction.sqrMagnitude == 1.0f) {
			newOrientation = direction;
		}

		if (newOrientation != orientation && direction.sqrMagnitude == 1.0f) {
			orientation = newOrientation;
		}

		SetOrientationIndex();
	}

	void SetOrientationIndex () {
		if (orientation.y > 0) {
			orientationIndex = 0;
		} else if (orientation.x > 0) {
			orientationIndex = 1;
		} else if (orientation.y < 0) {
			orientationIndex = 2;
		} else if (orientation.x < 0) {
			orientationIndex = 3;
		}
	}

	void RefreshInputStates () {
		H = Input.GetAxis("H");
		V = Input.GetAxis("V");

		A = Input.GetButton("A");
		B = Input.GetButton("B");
		S = Input.GetButton("S");
		Adown = Input.GetButtonDown("A");
		Bdown = Input.GetButtonDown("B");
		Sdown = Input.GetButtonDown("S");
	}
}
