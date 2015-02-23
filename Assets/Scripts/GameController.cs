using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameController = null;
	public static InputController inputController = null;
	public static MessageController messageController = null;
	public static Inventory inventory = null;
	public static CharacterManager characterManager = null;

	public const float gameScale = 2.0f;

	public Transform cameraFocus;

	public GameObject[] scenes;
	public int sceneIndex;

	/** gameState
	 * 0: free search state
	 * 1: free fight state
	 * 2: menu state
	 * 3: message event state
	 **/
	public int gameState = 0;
	public const int stateSearch = 0;
	public const int stateFight = 1;
	public const int stateMenu = 2;
	public const int stateMessage = 3;


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

	public void AddScene (GameObject scene, int sceneIndex) {
		if (scenes.Length < sceneIndex + 1) {
			GameObject[] scenesX = scenes;
			scenes = new GameObject[sceneIndex + 1];
			System.Array.Copy(scenesX, scenes, scenesX.Length);
		}

		if (scenes[sceneIndex]) {
			Debug.LogError(sceneIndex + " - repeated sceneIndex");
		}
		scenes[sceneIndex] = scene;
	}
}
