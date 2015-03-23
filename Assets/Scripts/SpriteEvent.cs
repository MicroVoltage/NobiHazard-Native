using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent), typeof(SpriteRenderer))]
public class SpriteEvent : MonoBehaviour {
	public Sprite sprite;
	
	void Start () {
		gameObject.name = gameObject.name + "-genericSprite";
	}

	public void OnEvent () {
		GetComponent<SpriteRenderer>().sprite = sprite;

		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
