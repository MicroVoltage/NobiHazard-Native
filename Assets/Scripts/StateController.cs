using UnityEngine;
using System.Collections;

public class StateController : MonoBehaviour {
	// switch is set manually
	public string[] switchNames;
	public bool[] switches;
	// eventSwitch is set by event automatically
	public string[] eventSwitchNames;

	public string[] counterNames;
	public int[] counters;

	public const int timerSize = 3;
	public float[] timers = new float[timerSize];
	public bool[] timerStates = new bool[timerSize];


	void Awake () {
		if (GameController.stateController == null) {
			GameController.stateController = this;
		} else if (GameController.stateController != this) {
			Destroy(gameObject);
		}
	}

	public int GetIndex (string[] names, string name) {
		for (int i=0; i<names.Length; i++) {
			if (names[i] == name) {
				return i;
			}
		}
		
		Debug.LogError(name + " - name not exist");
		return -1;
	}


	public bool GetSwitch (string switchName) {
		return switches[GetIndex(switchNames, switchName)];
	}

	public void SetSwitch (string switchName, bool newSwitch) {
		switches[GetIndex(switchNames, switchName)] = newSwitch;
	}


	public bool GetEventSwitch (string eventSwitchName) {
		int eventSwitchIndex = GetIndex(eventSwitchNames, eventSwitchName);
		if (eventSwitchIndex < 0) {
			return false;
		} else {
			return true;
		}
	}

	public void SetEventSwitch (string eventSwitchName) {
		if (GetEventSwitch(eventSwitchName)) {
			return;
		}

		string[] eventSwitchNamesX = eventSwitchNames;
		eventSwitchNames = new string[eventSwitchNamesX.Length + 1];
		System.Array.Copy(eventSwitchNamesX, eventSwitchNames, Mathf.Min(eventSwitchNamesX.Length, eventSwitchNames.Length));

		eventSwitchNames[eventSwitchNames.Length - 1] = eventSwitchName;
	}


	public int GetCounter (string counterName) {
		return counters[GetIndex(counterNames, counterName)];
	}

	public void SetCounter (string counterName, int value) {
		int counterIndex = GetIndex(counterNames, counterName);

		if (counterIndex < 0) {
			string[] counterNamesX = counterNames;
			counterNames = new string[counterNamesX.Length + 1];
			System.Array.Copy(counterNamesX, counterNames, Mathf.Min(counterNamesX.Length, counterNames.Length));

			int[] countersX = counters;
			counters = new int[countersX.Length + 1];
			System.Array.Copy(countersX, counters, Mathf.Min(countersX.Length, counters.Length));

			counterNames[counterNames.Length - 1] = counterName;
			counters[counters.Length - 1] = value;
		} else {
			counters[counterIndex] = value;
		}
	}

	public void IncrementCounter (string counterName, int value) {
		counters[GetIndex(counterNames, counterName)] += value;
	}


	public float GetTimer (int timer) {
		return timers[timer];
	}

	public void SetTimer (int timer, bool newState) {
		timerStates[timer] = newState;
	}

	void Update () {
		for (int i=0; i<timers.Length; i++) {
			if (timerStates[i]) {
				timers[i] += Time.deltaTime;
			}
		}
	}
}
