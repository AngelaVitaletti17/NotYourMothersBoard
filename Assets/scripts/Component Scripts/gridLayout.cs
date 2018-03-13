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

		Color ballColor = new Color (135f/255f, 0f/255f, 255f/255f, 255/255);
		Gizmos.color = ballColor;
		Vector3 start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.017f, -b.size.y - 0.01f, -b.size.z) * 0.5f);
		Vector3 newPos = start;
		float rowCountTemp = rowCount;
		int k = 0;
		gridPositions = new Vector3[holeCount * rowCount];

		for (float x = 0; x < holeCount; x += size)
		{
			for (float z = 0; z < rowCount; z += size)
			{
				if (rowCountTemp == 8) {
					newPos = new Vector3 (start.x, newPos.y, newPos.z + 0.085f);
					rowCountTemp = 0;
				} else {
					newPos = new Vector3 (newPos.x + 0.07f, newPos.y, newPos.z);
				}
				gridPositions [k] = newPos;
				k++;
				rowCountTemp++;
				Gizmos.DrawSphere(newPos, 0.02f);
			}
		}
	}

}
