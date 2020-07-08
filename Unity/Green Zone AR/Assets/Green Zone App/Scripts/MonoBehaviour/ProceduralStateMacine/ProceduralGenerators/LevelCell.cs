using GameLogic;
using UnityEngine;

public class LevelCell : MonoBehaviour {

	public IntVector2 coordinates;
    public UnitType tileState;
	public LevelRoom room;
	private LevelCellEdge[] edges = new LevelCellEdge[LevelDirections.Count];

	private int initializedEdgeCount;

    public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == LevelDirections.Count;
		}
	}

	public LevelDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range(0, LevelDirections.Count - initializedEdgeCount);
			for (int i = 0; i < LevelDirections.Count; i++) {
				if (edges[i] == null) {
					if (skips == 0) {
						return (LevelDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("LevelCell has no uninitialized directions left.");
		}
	}

	public void Initialize (LevelRoom room) {
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	public LevelCellEdge GetEdge (LevelDirection direction) {
		return edges[(int)direction];
	}

	public void SetEdge (LevelDirection direction, LevelCellEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}
}