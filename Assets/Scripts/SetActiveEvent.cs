using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class SetActiveEvent : MonoBehaviour {
	void Start () {
		gameObject.name = gameObject.name + "-setActive";
	}

	public void OnEvent () {
		for (int i=0; i<transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
		}
		for (int i=0; i<transform.childCount; i++) {
			transform.GetChild(i).parent = transform.parent;
		}

		SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
