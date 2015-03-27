using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class TeleportEvent : MonoBehaviour {
	public int targetSceneIndex;
	public Vector2 targetPosition;

	public enum Orientation {Back, Right, Front, Left};
	public Orientation newOrientation;

	void Start () {
		gameObject.name = gameObject.name + "-teleport";
	}

	public void OnEvent () {
		Debug.Log(gameObject.name + " - get teleport event");

		SceneController targetSceneController = GameController.gameController.scenes[targetSceneIndex].GetComponent<SceneController>();
		GameObject heroInstance = GameController.heroController.heroInstance;

		heroInstance.transform.parent = targetSceneController.eventLayer;
		heroInstance.transform.position = (Vector3)targetPosition;

		GameController.inputController.SetOrientationIndex((int)newOrientation);
		GameController.gameController.CloseScenes();
		GameController.gameController.OpenScene(targetSceneIndex);
	}

	public void OnDrawGizmosSelected () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, targetPosition);
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(targetPosition, 1);
	}
}
