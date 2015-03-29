using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskController : MonoBehaviour {
	public string[] colorNames;
	public Color[] colors;

	public Image mask;

	public Color targetColor;
	public Color startColor;
	float targetTime = 99999f;
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
		mask.color = Color.Lerp(startColor, targetColor, 1-(targetTime-Time.time)/targetDeltaTime);
	}

	public void ShowMask (string colorName, float deltaTime) {
		targetColor = colors[GetColorIndex(colorName)];
		startColor = mask.color;

		targetTime = Time.time + deltaTime;
		targetDeltaTime = deltaTime;
	}
	
	public void HideMask (float deltaTime) {
		targetColor = mask.color;
		targetColor.a = 0f;
		startColor = mask.color;


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
