using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class HeroEvent : MonoBehaviour {
	public enum CharaterEventType {AddHero, DestroyHero, HideHero, ShowHero};
	public CharaterEventType eventType;
	public int heroIndex;
	public int orientationIndex;
	
	private HeroController heroController;
	private InputController inputController;

	void Start () {
		gameObject.name = gameObject.name + "-hero";

		heroController = GameController.heroController;
		inputController = GameController.inputController;
	}

	public void OnEvent () {
		Debug.Log(gameObject.name + " - get animation event");

		switch (eventType) {
		case CharaterEventType.AddHero:
			heroController.NewHero(heroIndex, transform.position);
			inputController.orientationIndex = orientationIndex;

			break;
		case CharaterEventType.DestroyHero:
			Debug.LogError("Why are you trying to do that?!");

			break;
		case CharaterEventType.HideHero:
			heroController.heroInstance.SetActive(false);

			break;
		case CharaterEventType.ShowHero:
			heroController.heroInstance.SetActive(true);

			break;
		}

		gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
	}


}
