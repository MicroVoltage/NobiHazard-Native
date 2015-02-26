using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	public int sceneIndex;

	public Transform background;
	public Transform foreground;
	public Transform eventLayer;

	private GameController gameController;

	private Transform[] eventTransforms = new Transform[0];
	private float maxY;
	private float minY;
	private float intervalY;

	void Start () {
		gameController = GameController.gameController;

		gameController.AddScene(gameObject, sceneIndex);

		background = transform.FindChild("background");
		foreground = transform.FindChild("foreground");
		eventLayer = transform.FindChild("event");
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
}
