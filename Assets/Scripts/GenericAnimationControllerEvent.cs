using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class GenericAnimationControllerEvent : MonoBehaviour {
	public GenericAnimationEvent targetGenericAnimationEvent;

	public string[] newNames;
	public Vector2[] newPositions;

	private bool sentAnimation;

	void Start () {
		gameObject.name = gameObject.name + "-genericAnimationController";
	}

	public void OnEvent () {
		targetGenericAnimationEvent.names = newNames;
		targetGenericAnimationEvent.positions = newPositions;

		targetGenericAnimationEvent.OnEvent();
		sentAnimation = true;
	}

	void Update () {
		if (sentAnimation && !targetGenericAnimationEvent.isPlaying) {
			gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
			sentAnimation = false;
		}
	}

	public void OnDrawGizmosSelected () {
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
		for (int i=0; i<newPositions.Length; i++) {
			position += newPositions[i];
			Gizmos.DrawWireSphere((Vector2)transform.position + position * GameController.gameScale, 0.2f);
		}
	}
}
