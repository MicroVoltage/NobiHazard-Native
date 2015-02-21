using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform focus;
	public Vector2 safeRect;

	private Vector3 wantPosition;

	void FixedUpdate () {
		RefreshFocus();

		wantPosition = transform.position;
		if (focus.position.x > transform.position.x + safeRect.x) {
			wantPosition.x += focus.position.x - transform.position.x - safeRect.x;
		} else if (focus.position.x < transform.position.x - safeRect.x) {
			wantPosition.x += focus.position.x - transform.position.x + safeRect.x;
		}
		if (focus.position.y > transform.position.y + safeRect.y) {
			wantPosition.y += focus.position.y - transform.position.y - safeRect.y;
		} else if (focus.position.y < transform.position.y - safeRect.y) {
			wantPosition.y += focus.position.y - transform.position.y + safeRect.y;
		}
		transform.position = wantPosition;
	}

	void RefreshFocus () {
		focus = GameController.gameController.cameraFocus;
	}
}
