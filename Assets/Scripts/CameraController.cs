using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform focus;
	public Vector2 safeRect;

	private Vector3 wantedPosition;

	void FixedUpdate () {
		RefreshFocus();

		wantedPosition = transform.position;
		if (focus.position.x > transform.position.x + safeRect.x) {
			wantedPosition.x += focus.position.x - transform.position.x - safeRect.x;
		} else if (focus.position.x < transform.position.x - safeRect.x) {
			wantedPosition.x += focus.position.x - transform.position.x + safeRect.x;
		}
		if (focus.position.y > transform.position.y + safeRect.y) {
			wantedPosition.y += focus.position.y - transform.position.y - safeRect.y;
		} else if (focus.position.y < transform.position.y - safeRect.y) {
			wantedPosition.y += focus.position.y - transform.position.y + safeRect.y;
		}
		transform.position = wantedPosition;
	}

	void RefreshFocus () {
		focus = GameController.gameController.cameraFocus;
	}
}
