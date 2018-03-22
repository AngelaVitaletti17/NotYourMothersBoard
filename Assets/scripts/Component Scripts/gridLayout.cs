using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridLayout : MonoBehaviour
{
	[SerializeField] //Allows to be seen in editor
	//For the grid layout physical representation
	public int columnCount, rowCount; //The column and the row size

	//For the math behind placing components on the grid
	public Dictionary<Vector3, bool> gridPositions; //Used to represent the grid positions, and whether or not they are filled
	private List<Vector3> keys; //Used to represent a LIST of the positions from gridPositions. Needed to convert into the dictionary into an array for indices
	public Vector3[] positionHolder, oldSpots; //An array of the positions, and the previous spots on the board that are highlighted (to show where a component will be placed)
	public Vector3 nullValue; //Used to represent that a value is out of bounds
	private bool alreadyInit = false;

	//Run when the program starts
	void Start(){
		positionHolder = new Vector3[columnCount * rowCount]; //Initialize the position array to be a size of the total holes in the board
		nullValue = new Vector3 (-1f, -1f, -1f); //Initialize the null value vector
	}

	//Used to determine what is done when we place the component on the board
	void OnTriggerEnter(Collider component){
		//TODO: add to a public list that can be accessed to test the logic

		
	}

	//---------------------------------------------------------------------------------------
	// Function: GetGridPoint
	// Description: Get's the nearest point on the grid relative to the mouse cursor
	// Parameters: Position - the desire position to be compared to spots on the grid
	// Returns: A Vector3 (position) of the nearest point
	//---------------------------------------------------------------------------------------
	public Vector3 GetGridPoint(Vector3 position)
	{
		Vector3 result = Vector3.zero; //Initialize the resulting point to zero
		//Compare
		for (int i = 0; i < positionHolder.Length; i++) {
			if (i == 0) { //If we're on the first index, we have nothing to compare to. The result will be that position
				result = positionHolder [i];
			}
			else if (Vector3.Distance (position, positionHolder[i]) < Vector3.Distance (position, result)) { //Compare the distance between the position to the current grid spot to the distance between the position and the current result
				result = positionHolder[i];
			}
		}

		return result; //return the closest point
	}

	//---------------------------------------------------------------------------------------
	// Function: GetNearestPoints
	// Description: Get the nearest points to be highlighted on the board to show placement
	// Parameters:
	//		position - current position of the component
	//		size - how many spots the component will take up
	//		component - the component
	//		oldHighlight - an array that holds the old data for highlighting the slots on the grid
	// Returns: A list of new spots to show as highlighted (vector 3 array)
	//---------------------------------------------------------------------------------------
	public Vector3[] GetNearestPoints(Vector3 position, int size, GameObject component, GameObject[] oldHighlight){
		Vector3[] spots = new Vector3[size]; //New Vector3 array containing a certain amount of spots depending on component size
		Vector3 componentLocation = GetGridPoint (position); //The current location of the component
		int index = System.Array.IndexOf (positionHolder, componentLocation); //the index of the current location (the component position)
		int componentCol = index % columnCount; //Zero-based column index 
		int componentRow = index / rowCount;
		if (oldHighlight != null) {
			for (int i = 0; i < oldHighlight.Length; i++) {
				Destroy (oldHighlight [i]);
			}
		}
		spots [0] = componentLocation; //the first spot will be the location of the component, slightly underneath
		print((index - 1) / rowCount + " " + componentRow);
		//If the component is horizontal
		if (component.transform.rotation.eulerAngles.y == 270f || Mathf.Round(component.transform.rotation.eulerAngles.y) == 90f){
			//Make sure it doesn't go out of bounds
			if (index < 0 || (index - 1) / rowCount != componentRow) {
				spots [1] = nullValue;
			} else {
				spots [1] = positionHolder [index - 1];
			}
			if (index + 2 > positionHolder.Length || (index + 1) / rowCount != componentRow) {
				spots [2] = nullValue;
			} else {
				spots [2] = positionHolder [index + 1];
			}
		} else if (Mathf.Round(component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f) { //if the component is vertical
			//Make sure it doesn't go out of bounds
			if (index - rowCount - 1 < 0 || (index - rowCount) % columnCount != componentCol) {
				spots [1] = nullValue;
			} else {
				spots [1] = positionHolder [index - rowCount];
			}
			if (index + rowCount + 1 > positionHolder.Length || (index + rowCount) % columnCount != componentCol) {
				spots [2] = nullValue;
			} else {
				spots [2] = positionHolder [index + rowCount];
			}
		}
		oldSpots = spots; //Set old spots to the current spots in order to be destroyed later
		return spots; //Return the spots to be highlighted
	}
	public void set_spots(){
		for (int i = 0; i < oldSpots.Length; i++) {
			if (oldSpots[i] != nullValue)
				gridPositions [oldSpots [i]] = true;
		}
	}

	//Represent the grid in a physical space (in the scene editor)
	private void OnDrawGizmos()
	{
		BoxCollider b = GetComponent<BoxCollider> (); //represents the collider of the breadboard
		Gizmos.color = Color.cyan; //the color of the spheres that will represent the grid spot
		Vector3 start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.0115f, -b.size.y - 0.005f, -b.size.z - 0.000f) * 0.5f); //Get the bottom left corner location of the breadboard
		Vector3 newPos = start; //newPos will represent each consecutive position on the grid
		float rowCountTemp = rowCount; //the temporary count of how many items are in each row. When a new row is reached, it will be set to 0.
		int k = 0;

		for (float x = 0; x < columnCount; x ++)
		{
			for (float z = 0; z < rowCount; z ++)
			{
				if (rowCountTemp == rowCount) { //If we've reaached a new row
					newPos = new Vector3 (start.x, newPos.y, newPos.z + 0.0415f);
					rowCountTemp = 0; //Reset the row count
				} else { //We've not yet reached a new row 
					newPos = new Vector3 (newPos.x + 0.036f, newPos.y, newPos.z);
				}
				if (positionHolder.Length > 0) {
					positionHolder [k] = newPos;
					k++;
				}
				rowCountTemp++; //Incremement the temporary row count
				Gizmos.DrawSphere(newPos, 0.01f); //Draw a sphere to represent a spot in the board
			}
		}
		set_dict ();
	}
	private void set_dict(){
		if (!alreadyInit) {
			gridPositions = new Dictionary<Vector3, bool> (columnCount * rowCount);
			for (int i = 0; i < positionHolder.Length; i++) {
				gridPositions.Add (positionHolder [i], false);
			}
		}
		alreadyInit = true;
	}
}
