using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridLayout : MonoBehaviour
{
	[SerializeField]
	private float size = 1f;
	public Renderer rend;
	public int holeCount, rowCount = 0;
	private Vector3[] gridPositions;

	void Start(){
		
	}

	public Vector3 GetNearestPointOnGrid(Vector3 position)
	{
		Vector3 result = Vector3.zero;
		for (int i = 0; i < gridPositions.Length; i++) {
			if (i == 0) {
				result = gridPositions [i];
			}
			else if (Vector3.Distance (position, gridPositions [i]) < Vector3.Distance (position, result)) {
				result = gridPositions [i];
			}
		}

		return result;
	}

	private void OnDrawGizmos()
	{
		BoxCollider b = GetComponent<BoxCollider> ();

		Gizmos.color = Color.cyan;
		Vector3 start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.0115f, -b.size.y - 0.005f, -b.size.z - 0.0f) * 0.5f);
		Vector3 newPos = start;
		float rowCountTemp = rowCount;
		int k = 0;
		gridPositions = new Vector3[holeCount * rowCount];

		for (float x = 0; x < holeCount; x += size)
		{
			for (float z = 0; z < rowCount; z += size)
			{
				if (rowCountTemp == 16) {
					newPos = new Vector3 (start.x, newPos.y, newPos.z + 0.0415f);
					rowCountTemp = 0;
				} else {
					newPos = new Vector3 (newPos.x + 0.036f, newPos.y, newPos.z);
				}
				gridPositions [k] = newPos;
				k++;
				rowCountTemp++;
				Gizmos.DrawSphere(newPos, 0.01f);
			}
		}
	}

}
