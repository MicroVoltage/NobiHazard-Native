using UnityEngine;
using System.Collections;

public class StateController : MonoBehaviour {
	//public string[] stateBoolNames;
	//public bool[] stateBools;

	public string[] stateIntNames;
	public int[] stateInts;

	//public string[] stateFloatNames;
	//public float[] stateFloats;

	//public string[] stateStringNames;
	//public string[] stateStrings;

	void Awake () {
		if (GameController.stateController == null) {
			GameController.stateController = this;
		} else if (GameController.stateController != this) {
			Destroy(gameObject);
		}
	}

	public void LoadAll () {
		LoadInts();
	}
	public void SaveAll () {
		SaveInts();
	}
	/*
	public int GetBoolIndex (string boolName) {
		for (int i=0; i<boolName.Length; i++) {
			if (stateBoolNames[i] == boolName) {
				return i;
			}
		}
		
		Debug.LogError(boolName + " - bool not exist");
		return -1;
	}
	public bool GetBool (string boolName) {
		return stateBools[GetBoolIndex(boolName)];
	}
	public void SetBool (string boolName, bool newBool) {
		stateBools[GetBoolIndex(boolName)] = newBool;
	}
	public void LoadBools () {
		if (!PlayerPrefs.HasKey(stateBoolNames[0])) {
			return;
		}

		for (int i=0; i<stateBools.Length; i++) {
			if (PlayerPrefs.GetString(stateBoolNames[i]) == "true") {
				stateBools[i] = true;
			} else {
				stateBools[i] = false;
			}
		}
	}
	public void SaveBools () {
		for (int i=0; i<stateBools.Length; i++) {
			if (stateBools[i]) {
				PlayerPrefs.SetString(stateBoolNames[i], "true");
			} else {
				PlayerPrefs.SetString(stateBoolNames[i], "false");
			}
		}
	}
	*/

	public int GetIntIndex (string intName) {
		for (int i=0; i<intName.Length; i++) {
			if (stateIntNames[i] == intName) {
				return i;
			}
		}
		
		Debug.LogError(intName + " - int not exist");
		return -1;
	}
	public int GetInt (string intName) {
		return stateInts[GetIntIndex(intName)];
	}
	public void SetInt (string intName, int newInt) {
		stateInts[GetIntIndex(intName)] = newInt;
	}
	public bool MatchInt (string intName, int matchInt) {
		if (stateInts[GetIntIndex(intName)] == matchInt) {
			return true;
		}
		return false;
	}
	public bool GetIntBool (string intBoolName) {
		if (stateInts[GetIntIndex(intBoolName)] > 0) {
			return true;
		} else {
			return false;
		}
	}
	public void SetIntBool (string intBoolName, bool newIntBool) {
		if (newIntBool) {
			stateInts[GetIntIndex(intBoolName)] = 1;
		} else {
			stateInts[GetIntIndex(intBoolName)] = 0;
		}
	}
	public void LoadInts () {
		if (!PlayerPrefs.HasKey(stateIntNames[0])) {
			return;
		}
		
		for (int i=0; i<stateInts.Length; i++) {
			stateInts[i] = PlayerPrefs.GetInt(stateIntNames[i]);
		}
	}
	public void SaveInts () {
		for (int i=0; i<stateInts.Length; i++) {
			PlayerPrefs.SetInt(stateIntNames[i], stateInts[i]);
		}
	}
}
