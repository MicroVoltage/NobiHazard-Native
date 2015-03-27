using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent), typeof(SpriteRenderer))]
public class SpriteEvent : MonoBehaviour {
	public SpriteRenderer targetSpriteRenderer;
	
	public Sprite sprite;
	
	void Start () {
		gameObject.name = gameObject.name + "-genericSprite";

		if (!targetSpriteRenderer) {
			targetSpriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

	public void OnEvent () {
		targetSpriteRenderer.sprite = sprite;

		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
