using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public Vector2 orientation;
	public int orientationIndex;
	public Vector2 direction;

	public bool A;
	public bool B;
	public bool S;

	public int mode = 0;	// normal message menu

	void Awake () {
		GameController.inputController = this;
	}

	void Start () {
		orientation = Vector3.zero;
		direction = Vector3.zero;
		A = false;
		B = false;
		S = false;
	}

	void Update () {
		switch (mode) {
		case 0:
			SetOrientation();
			SetOrientationIndex();

			A = Input.GetButtonDown("A");
			B = Input.GetButtonDown("B");
			S = Input.GetButton("S");
			break;
		case 1:
			direction = Vector3.zero;

			A = false;
			B = Input.GetButtonDown("A");
			B = B || Input.GetButtonDown("B");
			break;

		default:
			Debug.LogError(mode.ToString() + " - mode not exist");
			break;
		}
	}

	void SetOrientation () {
		direction = new Vector2(
			Input.GetAxis("H"),
			Input.GetAxis("V"));

		Vector2 newOrientation = Vector3.zero;
		if (direction.sqrMagnitude > 1.0f) {
			newOrientation = direction - orientation;
		} else if (direction.sqrMagnitude == 1.0f) {
			newOrientation = direction;
		}

		if (newOrientation != orientation && direction.sqrMagnitude == 1.0f) {
			orientation = newOrientation;
		}
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
}
