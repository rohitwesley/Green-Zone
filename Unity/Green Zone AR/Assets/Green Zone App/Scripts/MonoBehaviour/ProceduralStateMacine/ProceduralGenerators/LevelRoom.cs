using UnityEngine;
using System.Collections.Generic;

public class LevelRoom : ScriptableObject {

	public int settingsIndex;

	public LevelRoomSettings settings;
	
	private List<LevelCell> cells = new List<LevelCell>();
	
	public void Add (LevelCell cell) {
		cell.room = this;
		cells.Add(cell);
	}

	public void Assimilate (LevelRoom room) {
		for (int i = 0; i < room.cells.Count; i++) {
			Add(room.cells[i]);
		}
	}
}