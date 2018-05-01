using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class gridLayout : MonoBehaviour
{
	//For the grid layout physical representation
	public int columnCount, rowCount; //The column and the row size

	//For the math behind placing components on the grid
	public Dictionary<Vector3, bool> gridPositions, OldGP; //Used to represent the grid positions, and whether or not they are filled
	public Dictionary<Vector3, GameObject> posAndSpots, OldPS; //Used to keep track of input and output node locations
	private List<Vector3> keys; //Used to represent a LIST of the positions from gridPositions. Needed to convert into the dictionary into an array for indices
	public Vector3[] positionHolder, oldSpots, OldP; //An array of the positions, and the previous spots on the board that are highlighted (to show where a component will be placed)
	public Vector3 nullValue; //Used to represent that a value is out of bounds
	private bool alreadyInit = false;

	//Run when the program starts
	void Awake(){
		positionHolder = new Vector3[columnCount * rowCount]; //Initialize the position array to be a size of the total holes in the board
		SetGridSpots();
		nullValue = new Vector3 (-1f, -1f, -1f); //Initialize the null value vector
		OldP = positionHolder;
		OldGP = gridPositions;
		OldPS = posAndSpots;

	}

	void Start(){
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
		int width = component.GetComponent<gridPlacement> ().rows;
		Vector3[] spots = new Vector3[width * size]; //New Vector3 array containing a certain amount of spots depending on component size
		Vector3 componentLocation = GetGridPoint (position, size); //The current location of the component
		int index = System.Array.IndexOf (positionHolder, componentLocation); //the index of the current location (the component position), the midpoint

		int prevCC = columnCount;
		int prevRC = rowCount;
		if (this.transform.rotation.eulerAngles.y == 270f || Mathf.Round (this.transform.rotation.eulerAngles.y) == 90f) {
			prevCC = rowCount;
			prevRC = columnCount;
		}

		int componentCol = index % prevCC; //Zero-based column index 
		int componentRow = index / prevCC;
		int halfSplitter = size / 2;
		int newSize = size - halfSplitter - 1;
		int sIndex = 1;
		if (oldHighlight != null && oldHighlight[0] != null) {
			for (int i = 0; i < oldHighlight.Length; i++) {
				Destroy (oldHighlight [i]);
			}
		}

		componentCol = index % prevCC; //Zero-based column index 
		componentRow = index / prevCC; //Recalculate

		if (size % 2 != 0) {
			spots [0] = componentLocation; //the first spot will be the location of where the grid spot is
		} else {
			componentCol = index % prevCC; //Zero-based column index 
			componentRow = index / prevCC;
			newSize = size - halfSplitter;
			halfSplitter = halfSplitter - 1;
			spots [0] = positionHolder [index];
		}
		int indexN = index;
		int colN = componentCol;
		int rowN = componentRow;
		int tracker = 0;
		//print(System.Array.IndexOf(positionHolder, spots[0]));
		for (int k = 0; k < width; k++) {
			if (width != 1 && k != 0) {
				if (k < halfSplitter) {
					if (component.transform.rotation.eulerAngles.y == 270f || Mathf.Round (component.transform.rotation.eulerAngles.y) == 90f)
						index = (index - ((tracker + 1) * prevCC));
					else if (Mathf.Round (component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f)
						index = index - tracker - 1;
					componentRow = index / prevCC;
					componentCol = index % prevCC;
				}
				else if (k == halfSplitter && halfSplitter != 0) {
					index = indexN;
					tracker = 0;
					componentRow = rowN;
					componentCol = colN;
				}
				if (k >= newSize) {
					if (component.transform.rotation.eulerAngles.y == 270f || Mathf.Round (component.transform.rotation.eulerAngles.y) == 90f)
						index = (index + ((tracker + 1) * prevCC));
					else if (Mathf.Round (component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f)
						index = index + tracker + 1;
					componentRow = index / prevCC;
					componentCol = index % prevCC;
				}
				if (index != indexN){
					if (index > 0 && index < positionHolder.Length - 1) { // Check if in bounds
						if (Mathf.Round (component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f) {
							if (componentCol == (indexN % prevCC) - (tracker + 1) || componentCol == (indexN % prevCC) + (tracker + 1))
								spots [sIndex] = positionHolder [index];
							else
								spots [sIndex] = nullValue;
						} else {
							if (componentCol == (indexN % prevCC)) {
								spots [sIndex] = positionHolder [index];
							} else
								spots [sIndex] = nullValue;
						}
					}
					else {
						spots [sIndex] = nullValue;
					}
					sIndex++;
					tracker++;
				}
			}
			if (component.transform.rotation.eulerAngles.y == 270f || Mathf.Round (component.transform.rotation.eulerAngles.y) == 90f) {
				//Generate the spots to be highlighted
				for (int i = 0; i < halfSplitter; i++) {
					if (index - i - 1 < 0 || index < 0 || index > positionHolder.Length - 1 || (index - i - 1) / prevCC != componentRow) { //The left side, which should be the first half of the spots
						spots [sIndex] = nullValue;
						sIndex++;
					} else {
						spots [sIndex] = positionHolder [index - i - 1];
						sIndex++;
					}
				}
				if (size % 2 != 0) {
				}
				for (int i = 0; i < newSize; i++) {
					if ((index + i + 1) > positionHolder.Length - 1 || index < 0 || index > positionHolder.Length - 1 || (index + i + 1) / prevCC != componentRow) {
						spots [sIndex] = nullValue;
						sIndex++;
					} else {
						spots [sIndex] = positionHolder [index + i + 1];
						sIndex++;
					}
				}

			} else if (Mathf.Round (component.transform.rotation.eulerAngles.y) == 0f || component.transform.rotation.eulerAngles.y == 180f) { //if the component is vertical
				for (int i = 0; i < halfSplitter; i++) {
					if ((index - ((i + 1) * prevCC) < 0) || index > positionHolder.Length - 1 || index < 0 || (index - ((i + 1) * prevCC)) / prevCC != componentRow - (i + 1)) { //The left side, which should be the first half of the spots
						spots [sIndex] = nullValue;
						sIndex++;
					} else {
						spots [sIndex] = positionHolder [index - ((i + 1) * prevCC)];
						sIndex++;
					}
				}
				for (int i = 0; i < newSize; i++) {
					if ((index + ((i + 1) * prevCC) > positionHolder.Length - 1) || index < 0 || index > positionHolder.Length - 1 || (index + ((i + 1) * prevCC)) / prevCC != componentRow + (i + 1)) {
						spots [sIndex] = nullValue;
						sIndex++;
					} else {
						spots [sIndex] = positionHolder [index + ((i + 1) * prevCC)];
						sIndex++;
					}
				}
			}
		}
		oldSpots = spots; //Set old spots to the current spots in order to be destroyed later
		return spots; //Return the spots to be highlighted
	}
	public void set_spots(GameObject component){
		for (int i = 0; i < oldSpots.Length; i++) {
			if (oldSpots[i] != nullValue)
				gridPositions [oldSpots [i]] = true;
		}
		//Also let's keep track of the gameObject's input and output nodes
		int sc = 0;
		if (component.tag == "battery")
			sc = 2;
		else
			sc = component.GetComponent<gridPlacement> ().spaceCount;
		if (component.tag != "battery") {
			if (sc % 2 == 0) {
				posAndSpots [oldSpots [(sc / 2) - 1]] = component;
				posAndSpots [oldSpots [sc - 1]] = component;
			} else {
				posAndSpots [oldSpots [(sc / 2)]] = component;
				posAndSpots [oldSpots [sc - 1]] = component;
			}
		}
	}

	public void scale_component(GameObject c){
		int half = c.GetComponent<gridPlacement> ().spaceCount / 2;
		GameObject ch;
		Vector3 distance, currentScale, max, min;
		float scaleTo, current;
		max = min = oldSpots [0];
		if (c.transform.rotation.eulerAngles.y == 270f || Mathf.Round (c.transform.rotation.eulerAngles.y) == 90f) {
			//Find the max and min x values in oldSpots
			for (int i = 0; i < oldSpots.Length; i++) {
				if (oldSpots [i].x > max.x)
					max = oldSpots[i];
				else if (oldSpots [i].x < min.x)
					min = oldSpots[i];
			}

			distance = max - min;

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
			for (int i = 0; i < oldSpots.Length; i++) {
				if (oldSpots [i].z > max.z)
					max = oldSpots[i];
				else if (oldSpots [i].z < min.z)
					min = oldSpots[i];
			}

			distance = max - min;

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

	private void SetGridSpots()
	{
		BoxCollider b = GetComponent<BoxCollider> (); //represents the collider of the breadboard
		//Gizmos.color = Color.cyan; //the color of the spheres that will represent the grid spot
		Vector3 start = Vector3.zero;
		float colCountTemp = 0; //the temporary count of how many items are in each row. When a new row is reached, it will be set to 0.
		float rowCountTemp = 0;
		float sudoCol = columnCount;
		float sudoRow = rowCount;
		if (this.transform.rotation.eulerAngles.y == 270f || Mathf.Round (this.transform.rotation.eulerAngles.y) == 90f) {
			start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.019f, -b.size.y - 0.006f, -b.size.z - 0.005f) * 0.5f); //Get the bottom left corner location of the breadboard
			sudoCol = rowCount;
			//rowCountTemp = columnCount;
			sudoRow = columnCount;
		} else if (Mathf.Round (this.transform.rotation.eulerAngles.y) == 0f || this.transform.rotation.eulerAngles.y == 180f) {
			start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.019f, -b.size.y - 0.006f, -b.size.z - 0.005f) * 0.5f); //Get the bottom left corner location of the breadboard
		}
		Vector3 newPos = start; //newPos will represent each consecutive position on the grid

		int k = 0;

		for (float x = 0; x < sudoRow; x ++)
		{
			for (float z = 0; z < sudoCol; z ++)
			{
					if (Mathf.Round (this.transform.rotation.eulerAngles.y) == 90f) {
						if ((z == 0 && x != 0)) { //If we've reaached a new row
							if (x == 2 || x == rowCount - 2)
								newPos = new Vector3 (start.x + 0.027f, newPos.y, newPos.z - 0.031f);
							else if (x == (columnCount / 2))
								newPos = new Vector3 (start.x + 0.027f, newPos.y, newPos.z - 0.041f);
							else
								newPos = new Vector3 (start.x + 0.027f, newPos.y, newPos.z - 0.027f);
							rowCountTemp = 0; //Reset the row count
						}
						else { //We've not yet reached a new row 
							newPos = new Vector3 (newPos.x + 0.027f, newPos.y, newPos.z);
						}
					} 
					else if (this.transform.rotation.eulerAngles.y == 270f) {
						if ((z == 0 && x != 0)) { //If we've reaached a new row
							if (x == 2 || x == rowCount - 2)
								newPos = new Vector3 (start.x - 0.027f, newPos.y, newPos.z + 0.031f);
							else if (x == (columnCount / 2))
								newPos = new Vector3 (start.x - 0.027f, newPos.y, newPos.z + 0.041f);
							else
								newPos = new Vector3 (start.x - 0.027f, newPos.y, newPos.z + 0.027f);
							rowCountTemp = 0; //Reset the row count
						}
						else { //We've not yet reached a new row 
							newPos = new Vector3 (newPos.x - 0.027f, newPos.y, newPos.z);
						}
					}else if (Mathf.Round (this.transform.rotation.eulerAngles.y) == 0f || this.transform.rotation.eulerAngles.y == 180f) {
						if (colCountTemp == columnCount || z == 0) { //If we've reaached a new row
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
					}
				if (positionHolder.Length > 0) {
					positionHolder [k] = newPos;
					k++;
				}
				//Gizmos.color = Color.green;
				colCountTemp++; //Incremement the temporary row count
				//Gizmos.DrawSphere(newPos, 0.007f); //Draw a sphere to represent a spot in the board
			}
		}
		set_dict ();
	}
	public void set_dict(){
		if (!alreadyInit) {
			gridPositions = new Dictionary<Vector3, bool> (columnCount * rowCount);
			for (int i = 0; i < positionHolder.Length; i++) {
				gridPositions.Add (positionHolder [i], false);
			}
			posAndSpots = new Dictionary<Vector3, GameObject> (columnCount * rowCount);
			for (int i = 0; i < positionHolder.Length; i++) {
				posAndSpots.Add (positionHolder [i], null);
			}
		}
		alreadyInit = true;
	}

	public void resetGrid(){
		gridPositions = new Dictionary<Vector3, bool> (columnCount * rowCount);
		for (int i = 0; i < positionHolder.Length; i++) {
			gridPositions.Add (positionHolder [i], false);
		}
		posAndSpots = new Dictionary<Vector3, GameObject> (columnCount * rowCount);
		for (int i = 0; i < positionHolder.Length; i++) {
			posAndSpots.Add (positionHolder [i], null);
		}
	}
	/*void OnDrawGizmos(){
		BoxCollider b = GetComponent<BoxCollider> (); //represents the collider of the breadboard
		//Gizmos.color = Color.cyan; //the color of the spheres that will represent the grid spot
		Vector3 start = Vector3.zero;
		float colCountTemp = 0; //the temporary count of how many items are in each row. When a new row is reached, it will be set to 0.
		float rowCountTemp = 0;
		float sudoCol = columnCount;
		float sudoRow = rowCount;
		if (this.transform.rotation.eulerAngles.y == 270f || Mathf.Round (this.transform.rotation.eulerAngles.y) == 90f) {
			start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.019f, -b.size.y - 0.006f, -b.size.z - 0.005f) * 0.5f); //Get the bottom left corner location of the breadboard
			sudoCol = rowCount;
			//rowCountTemp = columnCount;
			sudoRow = columnCount;
		} else if (Mathf.Round (this.transform.rotation.eulerAngles.y) == 0f || this.transform.rotation.eulerAngles.y == 180f) {
			start = transform.TransformPoint (b.center - new Vector3 (b.size.x - 0.019f, -b.size.y - 0.006f, -b.size.z - 0.005f) * 0.5f); //Get the bottom left corner location of the breadboard
		}
		Vector3 newPos = start; //newPos will represent each consecutive position on the grid

		int k = 0;

		for (float x = 0; x < sudoRow; x ++)
		{
			for (float z = 0; z < sudoCol; z ++)
			{
				if (Mathf.Round (this.transform.rotation.eulerAngles.y) == 90f) {
					if ((z == 0 && x != 0)) { //If we've reaached a new row
						if (x == 2 || x == rowCount - 2)
							newPos = new Vector3 (start.x + 0.027f, newPos.y, newPos.z - 0.031f);
						else if (x == (columnCount / 2))
							newPos = new Vector3 (start.x + 0.027f, newPos.y, newPos.z - 0.041f);
						else
							newPos = new Vector3 (start.x + 0.027f, newPos.y, newPos.z - 0.027f);
						rowCountTemp = 0; //Reset the row count
					}
					else { //We've not yet reached a new row 
						newPos = new Vector3 (newPos.x + 0.027f, newPos.y, newPos.z);
					}
				} 
				else if (this.transform.rotation.eulerAngles.y == 270f) {
					if ((z == 0 && x != 0)) { //If we've reaached a new row
						if (x == 2 || x == rowCount - 2)
							newPos = new Vector3 (start.x - 0.027f, newPos.y, newPos.z + 0.031f);
						else if (x == (columnCount / 2))
							newPos = new Vector3 (start.x - 0.027f, newPos.y, newPos.z + 0.041f);
						else
							newPos = new Vector3 (start.x - 0.027f, newPos.y, newPos.z + 0.027f);
						rowCountTemp = 0; //Reset the row count
					}
					else { //We've not yet reached a new row 
						newPos = new Vector3 (newPos.x - 0.027f, newPos.y, newPos.z);
					}
				}else if (Mathf.Round (this.transform.rotation.eulerAngles.y) == 0f || this.transform.rotation.eulerAngles.y == 180f) {
					if (colCountTemp == columnCount || z == 0) { //If we've reaached a new row
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
				}
				Gizmos.color = Color.red;
				colCountTemp++; //Incremement the temporary row count
				rowCountTemp++;
				Gizmos.DrawSphere(newPos, 0.007f); //Draw a sphere to represent a spot in the board
			}
		}
	}*/
}
