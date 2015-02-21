using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileEditor))]
public class TileEditorUI : Editor {
	// Reference to the component
	TileEditor tileEditor;
	
	// Scrollbar position
	Vector2 scrollbarPosition;
	// Column and row values on which the mouse is on
	Vector3 mousePositionOnMap;
	
	void OnEnable () {
		tileEditor = (TileEditor)target;
	}
	
	void OnSceneGUI () {
		Event guiEvent = Event.current;
		
		if (guiEvent.isMouse) {
			Tools.current = Tool.View;
			Tools.viewTool = ViewTool.FPS;
		} else {
			return;
		}
		
		tileEditor.mousePositionOnMap = GetMousePositionOnMap(guiEvent);
		SceneView.RepaintAll();
		
		if (guiEvent.type == EventType.MouseMove) {
			return;
		}
		
		if (guiEvent.button == 0) {
			Draw((int)mousePositionOnMap.x, (int)mousePositionOnMap.y);
			guiEvent.Use();
		} else if (guiEvent.button == 1) {
			Delete((int)mousePositionOnMap.x, (int)mousePositionOnMap.y);
			guiEvent.Use();
		}
	}
	
	public override void OnInspectorGUI () {
		DrawDefaultInspector();

		tileEditor.generateTiles = GUILayout.Toggle(
			tileEditor.generateTiles,
			"Generate Tiles",
			GUILayout.Height(50));
		if (tileEditor.generateTiles) {
			tileEditor.GenerateTiles();
		}

		//Show scroll bar For next layout
		//scrollbarPosition = GUILayout.BeginScrollView(scrollbarPosition);
		tileEditor.tileTextureID = GUILayout.SelectionGrid(
			tileEditor.tileTextureID,
			tileEditor.tileTextures,
			6,
			tileEditor.guiSkin.GetStyle("Button"));
		//GUILayout.EndScrollView();
	}
	
	Vector3 GetMousePositionOnMap (Event guiEvent) {
		// Get mouse position in World
		Ray guiRay = HandleUtility.GUIPointToWorldRay(
			new Vector2(guiEvent.mousePosition.x, guiEvent.mousePosition.y)
			);
		Vector3 mousePositionInScene = guiRay.origin;
		Vector3 anchor = tileEditor.transform.position;
		
		// Convert to map local position
		Vector2 mousePositionToAnchor = mousePositionInScene - new Vector3(anchor.x, anchor.y, 0);
		// Column and row values on which the mouse is on
		mousePositionOnMap = new Vector3(
			(int)(mousePositionToAnchor.x / tileEditor.tileSize),
			(int)(mousePositionToAnchor.y / tileEditor.tileSize),
			0);

		if (mousePositionOnMap.x < 0) {
			mousePositionOnMap.x -= 0;
		}
		if (mousePositionOnMap.y < 0) {
			mousePositionOnMap.y -= 0;
		}
		return mousePositionOnMap;
	}
	
	void Draw (int x, int y) {
		string tileName = tileEditor.mapName + y.ToString() + "." + x.ToString();
		//Checks if a game object has been already created on that place
		if (!tileEditor.transform.Find(tileName)) {
			//lets you undo editor changes
			Undo.IncrementCurrentGroup();
			
			// Instantiate a gameobject with the selected sprite and selected grid location and as a children of main layer 
			GameObject tile = new GameObject(tileName);
			SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
			renderer.sprite = tileEditor.tiles[tileEditor.tileTextureIDtoTileNo[tileEditor.tileTextureID]];
			renderer.sortingLayerID = tileEditor.sortingLayerID;

			tile.transform.position = tileEditor.realMousePositionOnMap;
			tile.transform.parent = tileEditor.transform;
			AddAttributes(tile);

			Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
		} else if (tileEditor.transform.Find(tileName)) {
			Delete(x, y);
			Draw(x, y);
		}
	}

	void AddAttributes (GameObject tile) {
		if (tileEditor.hasCollider) {
			tile.AddComponent<BoxCollider2D>();
		}
	}
	
	void Delete (int x, int y) {
		string tileName = tileEditor.mapName + y.ToString() + "." + x.ToString();
		if (tileEditor.transform.Find(tileName)) {
			Undo.IncrementCurrentGroup();
			Undo.DestroyObjectImmediate(tileEditor.transform.Find(tileName).gameObject);
		}
	}
}
