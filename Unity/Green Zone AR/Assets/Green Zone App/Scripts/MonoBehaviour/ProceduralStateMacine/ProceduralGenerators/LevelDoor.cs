using UnityEngine;

public class LevelDoor : LevelPath {

	public Transform hinge;

	private LevelDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as LevelDoor;
		}
	}
	
	public override void Initialize (LevelCell primary, LevelCell other, LevelDirection direction) {
		base.Initialize(primary, other, direction);
		if (OtherSideOfDoor != null) {
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge) {
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}
	}
}