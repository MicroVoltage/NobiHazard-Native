using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {
	public GameObject[] characters;
	
	public GameObject heroInstance;
	
	public int heroIndex = -1;
	
	private GameController gameController;
	private CameraController cameraController;
	
	void Awake () {
		if (GameController.heroController == null) {
			GameController.heroController = this;
		} else if (GameController.heroController != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		gameController = GameController.gameController;
		cameraController = GameController.cameraController;
	}

	public void NewHero (int newHeroIndex, Vector2 position) {
		if (heroIndex == newHeroIndex) {
			Debug.LogError(newHeroIndex + " - newing exsiting hero");
			return;
		} else {
			heroIndex = newHeroIndex;
		}

		SceneController curSceneController = gameController.sceneControllers[gameController.sceneIndex];
		heroInstance = (GameObject)Instantiate(characters[newHeroIndex], position, transform.rotation);
		heroInstance.transform.parent = curSceneController.eventLayer;

		if (cameraController.focus != heroInstance.transform) {
			cameraController.SetCameraFocus(heroInstance.transform);
			cameraController.SetCameraPosition(heroInstance.transform.position);
		}

		heroInstance.GetComponent<PlayerController>().enabled = true;
		heroInstance.GetComponent<PlayerAnimationController>().enabled = false;
	}

	public void DestroyHero () {
		Destroy(heroInstance);
		heroInstance = null;
		heroIndex = -1;
	}
}
