using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameController = null;
	public static InputController inputController = null;
	public static MessageController messageController = null;
	public static Inventory inventory = null;
	public static CharacterManager characterManager = null;
	public static CameraController cameraController = null;

	public const float gameScale = 2.0f;

	public Transform cameraFocus;

	public GameObject[] scenes;
	public SceneController[] sceneControllers;
	public int sceneIndex;

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
		sceneIndex = 0;
		messageController.HideMessage();
		OpenScene(0, Vector2.zero);
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

		characterManager.DestroyCharacters();

		for (int i=0; i<scenes.Length; i++) {
			scenes[i].SetActive(false);
		}
	}
	public void CloseScene (int sceneIndex) {
		cameraController.LockCameraPosition(cameraController.transform.position);
		
		characterManager.DestroyCharacters();

		scenes[sceneIndex].SetActive(false);
	}
	public void CloseScene () {
		cameraController.LockCameraPosition(cameraController.transform.position);
		
		characterManager.DestroyCharacters();
		
		scenes[sceneIndex].SetActive(false);
	}

	public void OpenScene (int newSceneIndex, Vector2 startPosition) {
		scenes[newSceneIndex].SetActive(true);
		sceneControllers[newSceneIndex].eventLayer.BroadcastMessage("OnSceneEnter", sceneIndex);

		sceneIndex = newSceneIndex;
		cameraController.LockCameraPosition(startPosition);
	}
}
