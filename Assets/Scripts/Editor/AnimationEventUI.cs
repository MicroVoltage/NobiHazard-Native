using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AnimationEvent))]
public class AnimationEventUI : Editor {
	AnimationEvent animationEvent;

	void OnEnable () {
		animationEvent = (AnimationEvent)target;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		EditorGUILayout.Space();

//		animationEvent.animationEventSize = EditorGUILayout.IntField("Size", animationEvent.animationEventSize);
		if (GUILayout.Button("Resize WeaponController", GUILayout.Height(40))) {
			animationEvent.ResizeAnimationEvent();
		}

		for (int i=0; i<animationEvent.positions.Length; i++) {
			GUILayout.Label("Animation " + i);
			
			animationEvent.characterIndexes[i] = EditorGUILayout.IntField("Character Index", animationEvent.characterIndexes[i]);
			animationEvent.positions[i] = EditorGUILayout.Vector2Field("Move To", animationEvent.positions[i]);
			
			EditorGUILayout.Space();
		}
	}
}
