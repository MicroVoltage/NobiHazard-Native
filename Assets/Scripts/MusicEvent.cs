using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class MusicEvent : MonoBehaviour {
	public string musicName;

	void Start () {
		gameObject.name = gameObject.name + "-music";
	}
	
	public void OnEvent () {
		Debug.Log(gameObject.name + " - get music event");

		GameController.cameraController.SetMusic(musicName);

		SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
