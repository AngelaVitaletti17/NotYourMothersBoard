using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridLayout : MonoBehaviour
{
	[SerializeField]
	private float size = 1f;
	public Renderer rend;
	public int holeCount, rowCount = 0;
	private Dictionary<Vector3, bool> gridPositions;
	private List<Vector3> keys;
	private Vector3[] positionHolder, oldSpots;
	private Vector3 nullValue, flip;

	void Start(){
		positionHolder = new Vector3[holeCount * rowCount];
		nullValue = new Vector3 (-1f, -1f, -1f);
		flip = new Vector3 (1f, 1/3f, 1f);
	}
	void OnTriggerEnter(Collider component){ //If we place something on the board
		//TODO: add to a public list that can be accessed to test the logic

		
	}

	public Vector3 GetNearestPointOnGrid(Vector3 position)
	{
		Vector3 result = Vector3.zero;
		for (int i = 0; i < positionHolder.Length; i++) {
			if (i == 0) {
				result = positionHolder [i];
			}
			else if (Vector3.Distance (position, positionHolder[i]) < Vector3.Distance (position, result)) {
				result = positionHolder[i];
			}
			gridPositions [positionHolder [i]] = true;
		}

		return result;
	}
	public Vector3[] GetNearestPoints(Vector3 position, int size, GameObject component, GameObject[] oldHighlight, Vector3 origR){
		BoxCollider b = component.GetComponent<BoxCollider> ();
		Vector3[] spots = new Vector3[size];
		Vector3 componentLocation = GetNearestPointOnGrid (position);
		int middleIdx = System.Array.IndexOf (positionHolder, componentLocation);
		if (oldHighlight.Length > 0) {
			Destroy (oldHighlight[0]); Destroy (oldHighlight[1]); Destroy (oldHighlight[2]);
		}
		Debug.Log (component.transform.rotation.eulerAngles);
		spots [0] = componentLocation;
		if (component.transform.rotation.eulerAngles.y == 270f || Mathf.Round(component.transform.rotation.eulerAngles.y) == 90f){			if (middleIdx < 0) {
				spots [1] = nullValue;
			} else
				spots [1] = positionHolder [middleIdx - 1];
			if (middleIdx + 2 > positionHolder.Length) {
				spots [2] = nullValue;
			} else
				spots [2] = positionHolder [middleIdx + 1];
		} else if (Mathf.Round(component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f) {
			if (middleIdx - rowCount - 1 < 0) {
				spots [1] = nullValue;
			} else
				spots [1] = positionHolder [middleIdx - rowCount];
			if (middleIdx + rowCount + 1 > positionHolder.Length) {
				spots [2] = nullValue;
			} else
				spots [2] = positionHolder [middleIdx + rowCount];
		}
		if (component.transform.rotation.eulerAngles.y == 0)
			print ("UES");
		oldSpots = spots;
		return spots;
	}

	private void OnDrawGizmos()
	{
		BoxCollider b = GetComponent<BoxCollider> ();

		Gizmos.color = Color.cyan;
		Vector3 start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.0115f, -b.size.y - 0.005f, -b.size.z - 0.000f) * 0.5f);
		Vector3 newPos = start;
		float rowCountTemp = rowCount;
		int k = 0;

		gridPositions = new Dictionary<Vector3, bool> (holeCount * rowCount);

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
				gridPositions.Add (newPos, false);
				k++;
				rowCountTemp++;
				Gizmos.DrawSphere(newPos, 0.01f);
			}
		}

		//Convert dictionary to list or array because you cannot iterate over a dictionary and edit elements in it simultaneously
		keys = new List<Vector3> (gridPositions.Keys);
		positionHolder = keys.ToArray ();
	}

}
