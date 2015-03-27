using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform targetTransform;
	public Vector2 targetPosition;

	public Rect sceneRect;
	public Rect viewRect;

	public AudioClip[] musics;

	public Vector3 newPosition;

	void Awake () {
		if (GameController.cameraController == null) {
			GameController.cameraController = this;
		} else if (GameController.cameraController != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		viewRect.height = Camera.main.orthographicSize * 2;
		viewRect.width = viewRect.height * Camera.main.aspect;
		viewRect.center = targetPosition;

		newPosition.z = transform.position.z;
	}

	public void LockCamera () {
		targetTransform = null;
	}
	public void LockCamera (Vector2 newPosition) {
		targetTransform = null;
		targetPosition = newPosition;
	}

	public void UnlockCamera (Transform newTransform) {
		targetTransform = newTransform;
		targetPosition = newTransform.position;
	}

	void FixedUpdate () {
		viewRect.width = viewRect.height * Camera.main.aspect;
		if (targetTransform) {
			targetPosition = targetTransform.position;
		}
		viewRect.center = targetPosition;

		// x test
		if (viewRect.xMin < sceneRect.xMin) {
			if (viewRect.xMax > sceneRect.xMax) {
				newPosition.x = sceneRect.center.x;
			} else {
				newPosition.x = sceneRect.xMin + viewRect.width/2;
			}
		} else {
			if (viewRect.xMax > sceneRect.xMax) {
				newPosition.x = sceneRect.xMax - viewRect.width/2;
			} else {
				newPosition.x = targetPosition.x;
			}
		}
		// x special case test
		if (viewRect.width >= sceneRect.width) {
			newPosition.x = sceneRect.center.x;
		}

		// y test
		if (viewRect.yMin < sceneRect.yMin) {
			if (viewRect.yMax > sceneRect.yMax) {
				newPosition.y = sceneRect.center.y;
			} else {
				newPosition.y = sceneRect.yMin + viewRect.height/2;
			}
		} else {
			if (viewRect.yMax > sceneRect.yMax) {
				newPosition.y = sceneRect.yMax - viewRect.height/2;
			} else {
				newPosition.y = targetPosition.y;
			}
		}
		// x special case test
		if (viewRect.height >= sceneRect.height) {
			newPosition.y = sceneRect.center.y;
		}

		transform.position = newPosition;
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

	public void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube((Vector3)sceneRect.center, (Vector3)sceneRect.size);

		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube((Vector3)viewRect.center, (Vector3)viewRect.size);
	}
}
