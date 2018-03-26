using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridPlacement : MonoBehaviour {

	public int spaceCount; //Representation of how many slots a component will take up, assigned in the editor 
	public gridLayout grid; //A link to a gridLayout script attached to a GameObject to reference variables and functions
	public tutorialUI tUI; //A link to a tutorialUI script attached to a GameObject to reference varaibles and functions
	public Vector3[] highlightedSpots, getRidOfSpots; //Vector3 arrays containing the spots to be highlighted, and the spots to free up when a component is picked up, respectively
	public GameObject[] highlights; //An array of the actual highlight GameObject that appear on screen
	public GameObject breadboard, sceneController; //A GameObject representing the breadboard and scene controller, respectively
	private bool useRed, placeable = false; //Used to determine if a red color will be used as the highlight to represent incorrect board placement

	void Start () {
		highlightedSpots = new Vector3[spaceCount]; //Initialize the highlighted spots locations array (change to size of spaceCount in the future, AV)
		highlights = new GameObject[spaceCount]; //Initialize the highlights game object array (change to size of spaceCount in the future, AV)
		tUI = sceneController.GetComponent<tutorialUI> (); //A link to the tutorial UI script on the scene controller
		grid = breadboard.GetComponent<gridLayout> (); //A link to the grid layout script on the breadboard

	}
	
	// Update is called once per frame
	void Update () {
		if (tUI.isSpawned) { //If the object is currently being dragged and is not yet placed
			highlightedSpots = grid.GetNearestPoints (this.transform.position, spaceCount, this.gameObject, highlights); //Get the locations of the spots to be highlighted
			useRed = false; //Don't use red until we know placement is incorrect
			//Determine if any of the spots are invalid, if so, we use a red color
			for (int i = 0; i < highlightedSpots.Length; i++) {
				if (highlightedSpots [i] == grid.nullValue || (grid.gridPositions.ContainsKey (highlightedSpots [i]) && grid.gridPositions [highlightedSpots [i]] == true)) { //spot is invalid or taken
					useRed = true;
					placeable = false;
				}
			}
			//Check some other conditions
			for (int i = 0; i < highlightedSpots.Length; i++) {
				if (highlightedSpots [i] != grid.nullValue) { //As long as we have a valid spot on the board
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Plane); //Create a plane to represent the highlight
					if (useRed)
						cube.GetComponent<Renderer> ().material.color = Color.red;
					else if (!useRed && (grid.gridPositions.ContainsKey (highlightedSpots [i]) && grid.gridPositions [highlightedSpots [i]] == false)) {
						cube.GetComponent<Renderer> ().material.color = Color.green;
						placeable = true;
					}
					cube.GetComponent<Collider> ().enabled = false;
					cube.transform.localScale = cube.transform.localScale * 0.002f;
					cube.transform.position = highlightedSpots [i];
					highlights [i] = cube;
				}
			}

			if (getRidOfSpots != null && getRidOfSpots.Length > 0){
				for (int i = 0; i < getRidOfSpots.Length; i++)
					grid.gridPositions [getRidOfSpots [i]] = false;
			}
		} else if (!tUI.isSpawned && highlights.Length != 0) {
			for (int i = 0; i < highlights.Length; i++) {
				Destroy (highlights [i]);
			}
			useRed = false;
			getRidOfSpots = grid.GetNearestPoints (this.transform.position, spaceCount, this.gameObject, null);
		}
	}
	public bool getComponentPlacementStatus(){
		return placeable;
	}

}
