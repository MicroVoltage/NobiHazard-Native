using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericEvent))]
public class FrameEvent : MonoBehaviour {
	public bool endFrameSequence;
	
	public string[] frameNames;
	public string backgroundName;
	public float[] frameTimes;

	FrameController frameController;

	bool showingFrame;
	int frameIndex = 1;
	float lastFrameTime;
	

	void Start () {
		frameController = GameController.frameController;

		gameObject.name = gameObject.name + "-frame";

		for (int i=0; i<frameNames.Length; i++) {
			if (frameNames[i] == "") {
				frameNames[i] = frameNames[i-1];
			}
		}
	}

	void OnEvent () {
		Debug.Log(gameObject.name + " - get frame event");

		showingFrame = true;
		frameController.ShowFrame(frameNames[0], backgroundName);
		lastFrameTime = Time.time;
	}

	void Update () {
		if (!showingFrame || Time.time < lastFrameTime + frameTimes[frameIndex - 1]) {
			return;
		}

		if (frameIndex < frameNames.Length) {
			frameController.ShowFrame(frameNames[frameIndex], backgroundName);
			lastFrameTime = Time.time;
			frameIndex++;
		} else {
			if (endFrameSequence) {
				frameController.HideFrame();
			}
			showingFrame = false;
			frameIndex = 1;
			
			gameObject.SendMessage("EventCallBack", SendMessageOptions.RequireReceiver);
		}
	}
}
