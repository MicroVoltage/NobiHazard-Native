using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class CameraEvent : MonoBehaviour {
	public bool endCameraSequence;

	public float speed = 10.0f;
	public float arriveRadius = 0.1f;

	public bool relative;
	public Vector2[] positions;

	[HideInInspector]
	public bool isPlaying;

	CameraController cameraController;

	Transform originalCameraFocus;
	int animationIndex;
	Vector2 wantedPosition;
	Vector2 deltaPosition;

	
	void Start () {
		cameraController = GameController.cameraController;
		
		gameObject.name = gameObject.name + "-camera";
	}

	void Update () {
		if (!isPlaying) {
			return;
		} else if (deltaPosition.magnitude < arriveRadius) {
			if (animationIndex < positions.Length - 1) {
				animationIndex ++;
				
				wantedPosition = (Vector2)transform.position + positions[animationIndex] * GameController.gameScale;
				deltaPosition = wantedPosition - (Vector2)transform.position;
			} else {
				if (endCameraSequence) {
					cameraController.targetTransform = cameraController.originalTargetTransform;
					cameraController.originalTargetTransform = null;
				}

				isPlaying = false;
				animationIndex = 0;

				gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
				return;
			}
		}
		
		deltaPosition = wantedPosition - (Vector2)transform.position;
		
		transform.position = Vector3.MoveTowards(
			transform.position,
			new Vector3(wantedPosition.x, wantedPosition.y, transform.position.z),
			Time.deltaTime * speed);
	}

	public void OnEvent () {
		if (!cameraController.originalTargetTransform) {
			cameraController.originalTargetTransform = cameraController.targetTransform;
		}
		if (relative) {
			transform.position = cameraController.transform.position;
		}
		cameraController.targetTransform = transform;

		isPlaying = true;
		animationIndex = 0;
		
		wantedPosition = (Vector2)transform.position + positions[animationIndex] * GameController.gameScale;
		deltaPosition = wantedPosition - (Vector2)transform.position;
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.gray;
		for (int x=-8; x<8; x++) {
			Gizmos.DrawRay(
				transform.position + new Vector3(x * GameController.gameScale, -8, 0),
				Vector3.up * 8 * GameController.gameScale);
		}
		for (int y=-8; y<8; y++) {
			Gizmos.DrawRay(
				transform.position + new Vector3(-8, y * GameController.gameScale, 0),
				Vector3.right * 8 * GameController.gameScale);
		}
		
		Gizmos.color = Color.red;
		Vector2 position = Vector2.zero;
		if (relative) {
			Gizmos.color = Color.magenta;
		}
		for (int i=0; i<positions.Length; i++) {
			position += positions[i];
			Gizmos.DrawLine(
				(Vector2)transform.position + (position - positions[i]) * GameController.gameScale,
				(Vector2)transform.position + position * GameController.gameScale);
			Gizmos.DrawWireSphere((Vector2)transform.position + position * GameController.gameScale, 0.2f);
		}
	}
}
