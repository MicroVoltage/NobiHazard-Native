using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameController = null;
	public static InputController inputController = null;
	public static MessageController messageController = null;
	public static Inventory inventory = null;

	public static float gameScale = 2.0f;

	public Transform cameraFocus;

	/** gameState
	 * 0: free search state
	 * 1: free fight state
	 * 2: menu state
	 * 3: message event state
	 **/
	public int gameState = 0;


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

		gameState = 0;
	}
}
