using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameController = null;
	public static InputController inputController = null;
	public static CameraController cameraController = null;
	public static StateController stateController = null;

	public static MessageController messageController = null;
	public static FrameController frameController = null;
	public static MaskController maskController = null;
	public static MenuController menuController = null;

	public static HeroController heroController = null;
	public static InventoryController inventoryController = null;

	public const float gameScale = 2.0f;
	public static Vector2 playerPhase = Vector2.up * gameScale;

	public Canvas canvas;
	
	public GameObject[] scenes;
	public int sceneIndex = -1;

	/** gameState
	 * 0: free search state
	 * 1: free fight state
	 * 2: menu state
	 * 3: message event state
	 * 4: animtiton event state
	 **/
	public int gameState = 0;
	public const int stateSearch = 0;
	public const int stateFight = 1;
	public const int stateMenu = 2;
	public const int stateMessage = 3;
	public const int stateAnimation = 4;
	public const int stateFrame = 5;

	/* To Be Deleted */
	public int testStartLevel = 0;


	void Awake () {
		if (GameController.gameController == null) {
			GameController.gameController = this;
		} else if (GameController.gameController != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		gameState = 0;
		sceneIndex = -1;

		canvas.enabled = true;
	}

	void Update () {
		if (sceneIndex == -1) {
			messageController.HideMessage();
			frameController.HideFrame();
			maskController.HideMask(0);
			menuController.HideMenu();

			OpenScene(testStartLevel);
			heroController.NewHero(1, scenes[testStartLevel].transform.position + Vector3.one * gameScale);
		}
	}

	/** AddScene
	 * will only be call in Start () stage
	 **/
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

		CloseScene(sceneIndex);
	}

	public void CloseScenes () {
		for (int i=0; i<scenes.Length; i++) {
			scenes[i].SetActive(false);
		}
	}
	public void CloseScene (int sceneIndex) {
		scenes[sceneIndex].SetActive(false);
	}

	public void OpenScene (int newSceneIndex) {
		scenes[newSceneIndex].SetActive(true);
		SceneController sceneController = scenes[newSceneIndex].GetComponent<SceneController>();
		sceneController.eventLayer.BroadcastMessage("OnSceneEnter", sceneIndex);

		sceneIndex = newSceneIndex;

		cameraController.sceneRect = sceneController.sceneRect;
	}
}
