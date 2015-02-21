using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AnimationEditor))]
public class AnimationEditorUI : Editor {
	AnimationEditor animationEditor;


	void OnEnable () {
		animationEditor = (AnimationEditor)target;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector();

		if (GUILayout.Button("Generate Frames", GUILayout.Height(50))) {
			animationEditor.GenerateFrames();
		}

		if (GUILayout.Button("Auto Names", GUILayout.Height(50))) {
			animationEditor.AutoName();
		}
		if (GUILayout.Button("Clean Names", GUILayout.Height(25))) {
			string autoName = animationEditor.autoName;
			animationEditor.autoName = "";
			animationEditor.AutoName();
			animationEditor.autoName = autoName;
		}
		if (GUILayout.Button("Toggle ONE", GUILayout.Height(25))) {
			for (int i=0; i<animationEditor.isOneFrame.Length; i++) {
				animationEditor.isOneFrame[i] = !animationEditor.isOneFrame[i];
			}
		}
		if (GUILayout.Button("Generate Animations", GUILayout.Height(50))) {
			animationEditor.GenerateAnimations();
		}
		if (GUILayout.Button("Save Animations", GUILayout.Height(50))) {
			for (int i=0; i<animationEditor.animationClips.Length; i++) {
				AssetDatabase.CreateAsset(
					animationEditor.animationClips[i],
					"Assets/Animations/" + animationEditor.animationClips[i].name + ".anim");
				AssetDatabase.SaveAssets();
			}
		}

		if (animationEditor.frameTextures.Length == 0) {
			return;
		}

		for (int i=0; i<animationEditor.sortedFrames.Length / 3; i++) {
			if (animationEditor.frameTextures[i * 3]) {
				if (i % 4 == 0) {
					EditorGUILayout.Space();
				}

				EditorGUILayout.BeginHorizontal();

				animationEditor.animationNames[i] = GUILayout.TextField(animationEditor.animationNames[i], GUILayout.MinWidth(100));
				
				animationEditor.isOneFrame[i] = GUILayout.Toggle(
					animationEditor.isOneFrame[i],
					"ONE");

				for (int j=0; j<3; j++) {
					GUILayout.Button(animationEditor.frameTextures[i * 3 + j], animationEditor.guiSkin.GetStyle("Button"));
				}

				EditorGUILayout.EndHorizontal();
			}
		}
	}
}
