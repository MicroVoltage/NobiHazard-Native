using UnityEngine;
using System.Collections;

public class GenericAnimationControllerEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool arriveStart;
	public bool examStart;
	
	public enum Comparation {Equal, Less, More};
	public string[] requiredIntNames;
	public Comparation[] requiredIntComparations;
	public int[] requiredInts;
	
	public string[] requiredIntBoolNames;
	public string[] requiredInversedIntBoolNames;
	
	public string[] requiredItemNames;
	public bool deleteRequiredItems;
	
	public AudioClip sound;
	
	public GameObject[] nextEvents;
	
	public void OnSceneEnter () {
		if (autoStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnApproach () {
		if (approachStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnArrive () {
		if (arriveStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnExam () {
		if (examStart) {
			if (!MeetRequirements()) {
				return;
			}
			OnEvent();
		}
	}
	
	public void OnChainEnter () {
		if (!MeetRequirements()) {
			return;
		}
		OnEvent();
	}
	
	public bool MeetRequirements () {
		for (int i=0; i<requiredIntNames.Length; i++) {
			switch (requiredIntComparations[i]) {
			case Comparation.Equal:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) == requiredInts[i])) {
					return false;
				}
				break;
			case Comparation.Less:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) < requiredInts[i])) {
					return false;
				}
				break;
			case Comparation.More:
				if (!(GameController.stateController.GetInt(requiredIntNames[i]) > requiredInts[i])) {
					return false;
				}
				break;
			}
		}
		
		for (int i=0; i<requiredIntBoolNames.Length; i++) {
			if (!GameController.stateController.GetIntBool(requiredIntBoolNames[i])) {
				return false;
			}
		}
		
		for (int i=0; i<requiredInversedIntBoolNames.Length; i++) {
			if (GameController.stateController.GetIntBool(requiredInversedIntBoolNames[i])) {
				return false;
			}
		}
		
		for (int i=0; i<requiredItemNames.Length; i++) {
			if (!GameController.inventoryController.HasItem(GameController.inventoryController.GetItemIndex(requiredItemNames[i]))) {
				return false;
			}
			if (deleteRequiredItems) {
				GameController.inventoryController.SubItem(GameController.inventoryController.GetItemIndex(requiredItemNames[i]));
			}
		}
		
		AudioSource.PlayClipAtPoint(sound, transform.position);
		
		return true;
	}
	
	public void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}
	
	/******************************* Event Alike *******************************/


	public GenericAnimationEvent targetGenericAnimationEvent;

	public string[] newNames;
	public Vector2[] newPositions;

	private bool sentAnimation;

	void Start () {
		gameObject.name = gameObject.name + "-genericAnimationController";
	}

	public void OnEvent () {
		targetGenericAnimationEvent.names = newNames;
		targetGenericAnimationEvent.positions = newPositions;

		targetGenericAnimationEvent.OnEvent();
		sentAnimation = true;
	}

	void Update () {
		if (sentAnimation && !targetGenericAnimationEvent.isPlaying) {
			CallNextEvents();
			sentAnimation = false;
		}
	}

	public void OnDrawGizmosSelected () {
		Gizmos.color = Color.gray;
		for (int x=-8; x<8; x++) {
			Gizmos.DrawRay(
				transform.position + new Vector3(x * GameController.gameScale, -8, 0),
				Vector3.up * 8 * GameController.gameScale);
		}
		for (int y=-8; y<8; y++) {
			Gizmos.DrawRay(
				transform.position + new Vector3(-8, y * GameController.gameScale, 0),
				Vector3.right * 8 * GameController.gameScale);
		}
		
		Gizmos.color = Color.red;
		Vector2 position = Vector2.zero;
		for (int i=0; i<newPositions.Length; i++) {
			position += newPositions[i];
			Gizmos.DrawWireSphere((Vector2)transform.position + position * GameController.gameScale, 0.2f);
		}
	}
}
