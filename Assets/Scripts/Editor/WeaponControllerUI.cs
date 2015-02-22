using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WeaponController))]
public class WeaponControllerUI : Editor {
	WeaponController weaponController;
	
	void OnEnable () {
		weaponController = (WeaponController)target;
	}
	
	public override void OnInspectorGUI () {
		weaponController.weaponControllerSize = EditorGUILayout.IntField("Size", weaponController.weaponControllerSize);
		if (GUILayout.Button("Resize WeaponController", GUILayout.Height(40))) {
			weaponController.ResizeWeaponController();
		}
		weaponController.hitLayers = EditorGUILayout.LayerField("Hit Layers", weaponController.hitLayers);
		for (int i=0; i<weaponController.names.Length; i++) {
			GUILayout.Label("Weapon " + i);

			weaponController.names[i] = EditorGUILayout.TextField("Name", weaponController.names[i]);
			weaponController.weaponIndexes[i] = EditorGUILayout.IntField("Weapon Index", weaponController.weaponIndexes[i]);
			weaponController.chipIndexes[i] = EditorGUILayout.IntField("Chip Index", weaponController.chipIndexes[i]);
			weaponController.damages[i] = EditorGUILayout.FloatField("Damage", weaponController.damages[i]);
			weaponController.forces[i] = EditorGUILayout.FloatField("Force", weaponController.forces[i]);
			weaponController.recoilForces[i] = EditorGUILayout.FloatField("Recoil Force", weaponController.recoilForces[i]);
			weaponController.fireIntervals[i] = EditorGUILayout.FloatField("Fire Interval", weaponController.fireIntervals[i]);
			weaponController.bullets[i] = (GameObject)EditorGUILayout.ObjectField("Bullet", weaponController.bullets[i], typeof(GameObject), true);

			EditorGUILayout.Space();
		}
	}
}
