using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform focus;
	public bool isLocked;
	public Vector2 lockedPosition;
	public Vector2 safeRect;

	public AudioClip[] musics;

	private Vector3 wantedPosition;

	void Awake () {
		if (GameController.cameraController == null) {
			GameController.cameraController = this;
		} else if (GameController.cameraController != this) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		if (isLocked) {
			SetCameraPosition(lockedPosition);
			return;
		}

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

	public void SetCameraFocus (Transform newFocus) {
		isLocked = false;
		focus = newFocus;
	}

	public void SetCameraPosition (Vector2 newPosition) {
		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
	}

	public void LockCameraPosition (Vector2 newPosition) {
		isLocked = true;
		lockedPosition = newPosition;
		SetCameraPosition(newPosition);
	}

	public void SetMusic (string musicName) {
		for (int i=0; i<musics.Length; i++) {
			if (musics[i].name == musicName) {
				audio.clip = musics[i];
				return;
			}
		}
		Debug.LogError(musicName + " - music not exist");
		return;
	}
}
