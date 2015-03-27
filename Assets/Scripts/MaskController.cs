using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskController : MonoBehaviour {
	public string[] colorNames;
	public Color[] colors;

	public Image mask;


	void Awake () {
		if (GameController.maskController == null) {
			GameController.maskController = this;
		} else if (GameController.maskController != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {

	}
	
	public void ShowMask (string colorName, float deltaTime) {

	}
	
	public void HideMask (float deltaTime) {

	}

	int GetColorIndex (string colorName) {
		for (int i=0; i<colorNames.Length; i++) {
			if (colorNames[i] == colorName) {
				return i;
			}
		}

		Debug.LogError(colorName + " - color not exist");
		return -1;
	}
}
