using UnityEngine;

public abstract class LevelCellEdge : MonoBehaviour {

	public LevelCell cell, otherCell;

	public LevelDirection direction;

	public virtual void Initialize (LevelCell cell, LevelCell otherCell, LevelDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}
}