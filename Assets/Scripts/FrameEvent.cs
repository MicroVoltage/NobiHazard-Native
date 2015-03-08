using UnityEngine;
using System.Collections;

public class FrameEvent : MonoBehaviour {
	public bool autoStart;
	public bool approachStart;
	public bool examStart;
	
	public enum Comparation {Equal, Less, More};
	public string[] requiredIntNames;
	public Comparation[] requiredIntComparations;
	public int[] requiredInts;
	public string[] requiredIntBoolNames;
	
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
		
		return true;
	}
	
	public void CallNextEvents () {
		for (int i=0; i<nextEvents.Length; i++) {
			nextEvents[i].SendMessage("OnChainEnter");
		}
	}
	
	/******************************* Event Alike *******************************/


	public bool hasBackground;
	public string[] frameNames;
	public string[] backgroundNames;
	public float[] frameTimes;

	private FrameController frameController;

	private bool showingFrame;
	private int frameIndex = 1;
	private float lastFrameTime;
	

	void Start () {
		frameController = GameController.frameController;

		gameObject.name = gameObject.name + "-frame";

		for (int i=0; i<frameNames.Length; i++) {
			if (frameNames[i] == "") {
				frameNames[i] = frameNames[i-1];
			}
		}

		if (hasBackground) {
			for (int i=0; i<backgroundNames.Length; i++) {
				backgroundNames[i] = "";
			}
		} else {
			for (int i=0; i<backgroundNames.Length; i++) {
				if (backgroundNames[i] == "") {
					backgroundNames[i] = backgroundNames[i-1];
				}
			}
		}
	}

	void OnEvent () {
		Debug.Log(gameObject.name + " - get frame event");

		showingFrame = true;
		frameController.ShowFrame(frameNames[0], backgroundNames[0]);
		lastFrameTime = Time.time;
	}

	void Update () {
		if (!showingFrame || Time.time < lastFrameTime + frameTimes[frameIndex - 1]) {
			return;
		}

		if (frameIndex < frameNames.Length) {
			frameController.ShowFrame(frameNames[frameIndex], backgroundNames[frameIndex]);
			lastFrameTime = Time.time;
			frameIndex++;
		} else {
			frameController.HideFrame();
			showingFrame = false;
			frameIndex = 1;
			
			CallNextEvents();
		}
	}
}
