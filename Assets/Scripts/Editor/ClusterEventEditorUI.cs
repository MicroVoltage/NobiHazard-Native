using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ClusterEventEditor))]
public class ClusterEventEditorUI : Editor {
	// Reference to the component
	ClusterEventEditor clusterEventEditor;

	
	void OnEnable () {
		clusterEventEditor = (ClusterEventEditor)target;
	}

	public override void OnInspectorGUI () {
		clusterEventEditor.eventSize = EditorGUILayout.IntField(clusterEventEditor.eventSize);
		if (GUILayout.Button("Resize Cluster Event", GUILayout.Height(20))) {
			clusterEventEditor.ResizeClusterEvent();
		}

		for (int i=0; i<clusterEventEditor.eventSize; i++) {
			EditorGUILayout.BeginHorizontal();
			
			clusterEventEditor.eventNames[i] = EditorGUILayout.TextField(clusterEventEditor.eventNames[i], GUILayout.MinWidth(100));
			
			clusterEventEditor.sameTime[i] = GUILayout.Toggle(
				clusterEventEditor.sameTime[i],
				"ST");

			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button("New Events", GUILayout.Height(40))) {
			GameObject preEvent = clusterEventEditor.gameObject;
			int sEventIndex = 0;
			int mEventIndex = 0;

			for (int eventIndex=0; eventIndex<clusterEventEditor.eventSize; eventIndex++) {
				Undo.IncrementCurrentGroup();
				// Instantiate a gameobject with the selected sprite and selected grid location and as a children of main layer 
				string eventName = clusterEventEditor.name + "-" + mEventIndex.ToString() + "-" + clusterEventEditor.eventNames[eventIndex];
				if (clusterEventEditor.transform.FindChild(eventName)) {
					continue;
				}
				GameObject newEvent = new GameObject(eventName);
				newEvent.transform.position = clusterEventEditor.transform.position;
				newEvent.transform.rotation = clusterEventEditor.transform.rotation;
				newEvent.transform.parent = clusterEventEditor.transform;

				newEvent.AddComponent<GenericEvent>();

				if (!clusterEventEditor.sameTime[eventIndex] || eventIndex == 0) {
					preEvent = newEvent;
					sEventIndex = 0;
					mEventIndex ++;
				} else {
					if (!clusterEventEditor.sameTime[eventIndex - 1]) {
						preEvent.GetComponent<GenericEvent>().NewNextEvent(newEvent);
					} else {
						preEvent.GetComponent<GenericEvent>().AddNextEvent(newEvent);
					}
					newEvent.transform.parent = preEvent.transform;
					newEvent.name += "-" + sEventIndex.ToString();
					sEventIndex ++;
				}

				Undo.RegisterCreatedObjectUndo(newEvent, "Create Event");
			}
		}
	}
}
