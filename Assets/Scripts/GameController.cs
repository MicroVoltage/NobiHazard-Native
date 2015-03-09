using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameController = null;
	public static InputController inputController = null;
	public static MessageController messageController = null;
	public static InventoryController inventoryController = null;
	public static CharacterManager characterManager = null;
	public static CameraController cameraController = null;
	public static FrameController frameController = null;
	public static StateController stateController = null;

	public const float gameScale = 2.0f;
	public static Vector2 playerPhase = Vector2.up * gameScale;

	public Canvas canvas;

	public Transform cameraFocus;

	public GameObject[] scenes;
	public SceneController[] sceneControllers;
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

		stateController.LoadAll();
	}

	void Update () {
		if (sceneIndex == -1) {
			messageController.HideMessage();
			frameController.HideFrame();
			OpenScene(0, Vector2.zero);
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

		if (sceneControllers.Length < sceneIndex + 1) {
			SceneController[] sceneControllersX = sceneControllers;
			sceneControllers = new SceneController[sceneIndex + 1];
			System.Array.Copy(sceneControllersX, sceneControllers, sceneControllersX.Length);
		}
		sceneControllers[sceneIndex] = scene.GetComponent<SceneController>();

		CloseScene(sceneIndex);
	}

	public void CloseScenes () {
		cameraController.LockCameraPosition(cameraController.transform.position);

//		characterManager.DestroyCharacters();

		for (int i=0; i<scenes.Length; i++) {
			scenes[i].SetActive(false);
		}
	}

	public void CloseScene (int sceneIndex) {
		cameraController.LockCameraPosition(cameraController.transform.position);

		scenes[sceneIndex].SetActive(false);
	}

	public void OpenScene (int newSceneIndex, Vector2 startPosition) {
		scenes[newSceneIndex].SetActive(true);
		sceneControllers[newSceneIndex].eventLayer.BroadcastMessage("OnSceneEnter", sceneIndex);

		sceneIndex = newSceneIndex;
		cameraController.LockCameraPosition(startPosition);
		if (characterManager.heroInstance != null) {
			cameraController.SetCameraFocus(characterManager.heroInstance.transform);
		}
	}
}
