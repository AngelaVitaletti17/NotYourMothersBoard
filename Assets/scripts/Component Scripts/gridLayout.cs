using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class gridLayout : MonoBehaviour
{
	//For the grid layout physical representation
	public int columnCount, rowCount; //The column and the row size

	//For the math behind placing components on the grid
	public Dictionary<Vector3, bool> gridPositions; //Used to represent the grid positions, and whether or not they are filled
	private List<Vector3> keys; //Used to represent a LIST of the positions from gridPositions. Needed to convert into the dictionary into an array for indices
	private Vector3[] positionHolder, oldSpots; //An array of the positions, and the previous spots on the board that are highlighted (to show where a component will be placed)
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
	public Vector3 GetGridPoint(Vector3 position, int size)
	{
		Vector3 result = Vector3.zero; //Initialize the resulting point to zero
		Vector3 result2 = Vector3.zero; //Iniialize a second vector 
		//Compare
		for (int i = 0; i < positionHolder.Length; i++) {
			if (i == 0) { //If we're on the first index, we have nothing to compare to. The result will be that position
				result = positionHolder [i];
			}
			else if (Vector3.Distance (position, positionHolder[i]) < Vector3.Distance (position, result)) { //Compare the distance between the position to the current grid spot to the distance between the position and the current result
				result2 = result;
				result = positionHolder[i];
			}
		}

		if (size % 2 == 0) {
			return (result + result2) / 2f;
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
		Vector3 componentLocation = GetGridPoint (position, size); //The current location of the component
		int index = System.Array.IndexOf (positionHolder, componentLocation); //the index of the current location (the component position), the midpoint
		int componentCol = index % columnCount; //Zero-based column index 
		int componentRow = index / columnCount;
		int halfSplitter = size / 2;
		int newSize = size - halfSplitter - 1;
		int sIndex = 1;
		if (oldHighlight != null && oldHighlight[0] != null) {
			for (int i = 0; i < oldHighlight.Length; i++) {
				Destroy (oldHighlight [i]);
			}
		}
		if (size % 2 != 0)
			spots [0] = componentLocation; //the first spot will be the location of where the grid spot is
		//If the component is horizontal
		if (component.transform.rotation.eulerAngles.y == 270f || Mathf.Round(component.transform.rotation.eulerAngles.y) == 90f){
			//Generate the spots to be highlighted
			for (int i = 0; i < halfSplitter; i++) {
				if (index - i - 1 < 0 || (index - i - 1) / columnCount != componentRow) { //The left side, which should be the first half of the spots
					spots [sIndex] = nullValue;
					sIndex++;
				} else {
					spots [sIndex] = positionHolder [index - i - 1];
					sIndex++;
				}
			}
			for (int i = 0; i < newSize; i++){
				if ((index + i + 1) > positionHolder.Length - 1 || (index + i + 1) / columnCount != componentRow) {
					spots [sIndex] = nullValue;
					sIndex++;
				}
				else {
					spots [sIndex] = positionHolder [index + i + 1];
					sIndex++;
				}
			}

		} else if (Mathf.Round(component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f) { //if the component is vertical
			for (int i = 0; i < halfSplitter; i++) {
				if ((index - ((i + 1) * columnCount) < 0) || (index - ((i + 1) * columnCount)) / columnCount != componentRow - (i + 1)) { //The left side, which should be the first half of the spots
					spots [sIndex] = nullValue;
					sIndex++;
				} else {
					spots [sIndex] = positionHolder [index - ((i + 1) * columnCount)];
					sIndex++;
				}
			}
			for (int i = 0; i < newSize; i++){
				if ((index + ((i + 1) * columnCount) > positionHolder.Length - 1) || (index + ((i + 1) * columnCount)) / columnCount != componentRow + (i + 1)) {
					spots [sIndex] = nullValue;
					sIndex++;
				}
				else {
					spots [sIndex] = positionHolder [index + ((i + 1) * columnCount)];
					sIndex++;
				}
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

	public void scale_component(GameObject c){
		int half = c.GetComponent<gridPlacement> ().spaceCount / 2;
		GameObject ch;
		Vector3 distance, currentScale;
		float scaleTo, current;
		if (c.transform.rotation.eulerAngles.y == 270f || Mathf.Round (c.transform.rotation.eulerAngles.y) == 90f) {
			if (oldSpots [oldSpots.Length - 1].x > oldSpots [half].x)
				distance = oldSpots [oldSpots.Length - 1] - oldSpots [half];
			else
				distance = oldSpots [half] - oldSpots [oldSpots.Length - 1];

			scaleTo = distance.x;
			if (c.transform.childCount > 0) {
				ch = c.transform.GetChild (0).gameObject;
				current = ch.GetComponent<Renderer> ().bounds.size.x;
				currentScale = ch.transform.localScale;
				currentScale.z = scaleTo * currentScale.z / current;
				ch.transform.localScale = currentScale;

			} else {
				current = c.GetComponent<Renderer> ().bounds.size.x;
				currentScale = c.transform.localScale;
				currentScale.z = scaleTo * currentScale.z / current;
				c.transform.localScale = currentScale;
			}
		} else if (Mathf.Round (c.transform.rotation.eulerAngles.y) == 0f || c.transform.rotation.eulerAngles.y == 180f) {
			if (oldSpots [oldSpots.Length - 1].z > oldSpots [half].z)
				distance = oldSpots [oldSpots.Length - 1] - oldSpots [half];
			else
				distance = oldSpots [half] - oldSpots [oldSpots.Length - 1];

			scaleTo = distance.z;
			if (c.transform.childCount > 0) {
				ch = c.transform.GetChild (0).gameObject;
				current = ch.GetComponent<Renderer> ().bounds.size.z;
				currentScale = ch.transform.localScale;
				currentScale.z = scaleTo * currentScale.z / current;
				ch.transform.localScale = currentScale;

			} else {
				current = c.GetComponent<Renderer> ().bounds.size.z;
				currentScale = c.transform.localScale;
				currentScale.z = scaleTo * currentScale.z / current;
				c.transform.localScale = currentScale;
			}
		}
	}

	//Represent the grid in a physical space (in the scene editor)
	private void OnDrawGizmos()
	{
		BoxCollider b = GetComponent<BoxCollider> (); //represents the collider of the breadboard
		Gizmos.color = Color.cyan; //the color of the spheres that will represent the grid spot
		Vector3 start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.019f, -b.size.y - 0.006f, -b.size.z - 0.005f) * 0.5f); //Get the bottom left corner location of the breadboard
		Vector3 newPos = start; //newPos will represent each consecutive position on the grid
		float colCountTemp = columnCount; //the temporary count of how many items are in each row. When a new row is reached, it will be set to 0.
		int k = 0;

		for (float x = 0; x < rowCount; x ++)
		{
			for (float z = 0; z < columnCount; z ++)
			{
				if (colCountTemp == columnCount) { //If we've reaached a new row
					newPos = new Vector3 (start.x, newPos.y, newPos.z + 0.027f);
					colCountTemp = 0; //Reset the row count
				} else if (z == 0 || z == 1 || z == columnCount - 1)
					newPos = new Vector3 (newPos.x + 0.027f, newPos.y, newPos.z);
				else if (z == 2 || z == columnCount - 2)
					newPos = new Vector3 (newPos.x + 0.031f, newPos.y, newPos.z);
				else if (z == (columnCount / 2))
					newPos = new Vector3 (newPos.x + 0.041f, newPos.y, newPos.z);
				else { //We've not yet reached a new row 
					newPos = new Vector3 (newPos.x + 0.027f, newPos.y, newPos.z);
				}
				if (positionHolder.Length > 0) {
					positionHolder [k] = newPos;
					k++;
				}
				Gizmos.color = Color.green;
				colCountTemp++; //Incremement the temporary row count
				Gizmos.DrawSphere(newPos, 0.007f); //Draw a sphere to represent a spot in the board
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
