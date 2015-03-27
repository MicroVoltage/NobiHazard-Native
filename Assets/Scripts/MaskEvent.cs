using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class MaskEvent : MonoBehaviour {
	public string colorName;
	public float colorDeltaTime;
	
	MaskController maskController;

	void Start () {
		maskController = GameController.maskController;
		
		gameObject.name = gameObject.name + "-mask";
	}

	public void OnEvent () {
		maskController.ShowMask(colorName, colorDeltaTime);

		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
