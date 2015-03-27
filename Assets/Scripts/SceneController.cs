using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	public int sceneIndex;
	public string sceneName;

	public Rect sceneRect;

	public Transform eventLayer;
	
	Transform[] eventTransforms = new Transform[0];
	float maxY;
	float minY;
	float intervalY;

	void Start () {
		GameController.gameController.AddScene(gameObject, sceneIndex);

		sceneName = gameObject.name;
		eventLayer = transform.FindChild("event");

		for (int i=0; i<eventLayer.childCount; i++) {
			eventLayer.GetChild(i).tag = "Event";
		}
	}

	void Update () {
//		if (eventTransforms.Length != eventLayer.childCount) {
			eventTransforms = new Transform[eventLayer.childCount];
			for (int i=0; i<eventTransforms.Length; i++) {
				eventTransforms[i] = eventLayer.GetChild(i);
			}
//		}

		maxY = eventTransforms[0].position.y;
		minY = eventTransforms[0].position.y;
		for (int i=0; i<eventTransforms.Length; i++) {
			float curY = eventTransforms[i].position.y;
			if (curY > maxY) {
				maxY = curY;
			}
			if (curY < minY) {
				minY = curY;
			}
		}

		intervalY = maxY - minY;

		for (int i=0; i<eventTransforms.Length; i++) {
			eventTransforms[i].position = new Vector3(
				eventTransforms[i].position.x,
				eventTransforms[i].position.y,
				-(maxY - eventTransforms[i].position.y) / intervalY);
		}
	}

	public void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube((Vector3)sceneRect.center, (Vector3)sceneRect.size);
	}
}
