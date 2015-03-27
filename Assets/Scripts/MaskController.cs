using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskController : MonoBehaviour {
	public string[] colorNames;
	public Color[] colors;
	public Color transparent;

	public Image mask;

	Color targetColor;
	float targetTime;
	float targetDeltaTime;


	void Awake () {
		if (GameController.maskController == null) {
			GameController.maskController = this;
		} else if (GameController.maskController != this) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		if (Time.time > targetTime) {
			mask.color = targetColor;
			return;
		}

		mask.color = Color.Lerp(mask.color, targetColor, (targetTime-Time.time)/targetDeltaTime);
	}

	public void ShowMask (string colorName, float deltaTime) {
		targetColor = colors[GetColorIndex(colorName)];

		targetTime = Time.time + deltaTime;
		targetDeltaTime = deltaTime;
	}
	
	public void HideMask (float deltaTime) {
		targetColor = transparent;

		targetTime = Time.time + deltaTime;
		targetDeltaTime = deltaTime;
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
