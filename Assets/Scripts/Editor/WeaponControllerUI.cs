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
		DrawDefaultInspector();

		weaponController.weaponControllerSize = EditorGUILayout.IntField("Size", weaponController.weaponControllerSize);
		if (GUILayout.Button("Resize WeaponController", GUILayout.Height(40))) {
			weaponController.ResizeWeaponController();
		}
//		weaponController.hitLayers = EditorGUILayout.MaskField("Hit Layers", weaponController.hitLayers);

		EditorGUILayout.Space();

		for (int i=0; i<weaponController.names.Length; i++) {
			EditorGUILayout.Space();

			GUILayout.Label("Weapon Index " + i);

			weaponController.names[i] = EditorGUILayout.TextField("Name", weaponController.names[i]);
			weaponController.weaponAnimationIndexes[i] = EditorGUILayout.IntField("Animation Index", weaponController.weaponAnimationIndexes[i]);
			weaponController.weaponInventoryIndexes[i] = EditorGUILayout.IntField("Inventory Index", weaponController.weaponInventoryIndexes[i]);
			weaponController.chipInventoryIndexes[i] = EditorGUILayout.IntField("Chip Inventory Index", weaponController.chipInventoryIndexes[i]);
			weaponController.damages[i] = EditorGUILayout.FloatField("Damage", weaponController.damages[i]);
			weaponController.forces[i] = EditorGUILayout.FloatField("Force", weaponController.forces[i]);
			weaponController.recoilForces[i] = EditorGUILayout.FloatField("Recoil Force", weaponController.recoilForces[i]);
			weaponController.destances[i] = EditorGUILayout.FloatField("Destance", weaponController.destances[i]);
			weaponController.intervals[i] = EditorGUILayout.FloatField("Interval", weaponController.intervals[i]);
			weaponController.bullets[i] = (GameObject)EditorGUILayout.ObjectField("Bullet", weaponController.bullets[i], typeof(GameObject), true);

			EditorGUILayout.Space();
		}
	}
}
