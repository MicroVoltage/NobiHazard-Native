using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class StateEvent : MonoBehaviour {


//	StateController stateController;

	void Start () {
		gameObject.name = gameObject.name + "-state";

//		stateController = GameController.stateController;
	}

	public void OnEvent() {


		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}
}
