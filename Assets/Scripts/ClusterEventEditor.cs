using UnityEngine;
using System.Collections;

public class ClusterEventEditor : MonoBehaviour {
	public int eventSize;
	
	public bool[] sameTime;
	public string[] eventNames;
	
	public void ResizeClusterEvent () {
		string[] namesX = eventNames;
		eventNames = new string[eventSize];
		System.Array.Copy (namesX, eventNames, Mathf.Min (namesX.Length, eventNames.Length));

		bool[] sameTimeX = sameTime;
		sameTime = new bool[eventSize];
		System.Array.Copy (sameTimeX, sameTime, Mathf.Min (sameTimeX.Length, sameTime.Length));
	}
}
