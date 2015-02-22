using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Inventory))]
public class InventoryUI : Editor {
	Inventory inventory;

	void OnEnable () {
		inventory = (Inventory)target;
	}

	public override void OnInspectorGUI () {
		if (GUILayout.Button("Resize Inventory", GUILayout.Height(40))) {
			inventory.ResizeInventory();
		}

		for (int i=0; i<inventory.itemNames.Length; i++) {
			GUILayout.Label("Item " + i);
			inventory.itemCounts[i] = EditorGUILayout.IntField("Count", inventory.itemCounts[i]);
			inventory.itemNames[i] = EditorGUILayout.TextField("Name", inventory.itemNames[i]);
			inventory.itemDescriptions[i] = EditorGUILayout.TextField("Description", inventory.itemDescriptions[i]);
			inventory.itemInts[i] = EditorGUILayout.IntField("Int", inventory.itemInts[i]);
			inventory.itemFloats[i] = EditorGUILayout.FloatField("Float", inventory.itemFloats[i]);
			EditorGUILayout.Space();
		}
	}
}
