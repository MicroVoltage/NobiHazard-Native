using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class DeleteEvent : MonoBehaviour {
	public GameObject[] targets;

	public void OnEvent () {
		foreach (GameObject target in targets) {
			Destroy(target);
		}

		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
