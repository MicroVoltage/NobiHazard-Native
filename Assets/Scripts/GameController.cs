using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameController = null;
	public static InputController inputController = null;
	public static MessageController messageController = null;

	public static float gameScale = 2.0f;

	public Transform cameraFocus;


	void Awake () {
		if (GameController.gameController == null) {
			GameController.gameController = this;
		} else if (GameController.gameController != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		cameraFocus = GameObject.FindGameObjectWithTag("Player").transform;
	}
}
