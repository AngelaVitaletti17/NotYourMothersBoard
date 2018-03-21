using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridPlacement : MonoBehaviour {

	public int spaceCount; //Representation of how many slots a component will take up, assigned in the editor 
	public gridLayout grid; //A link to a gridLayout script attached to a GameObject to reference variables and functions
	public tutorialUI tUI; //A link to a tutorialUI script attached to a GameObject to reference varaibles and functions
	public Vector3[] highlightedSpots;
	public GameObject[] highlights;
	public GameObject breadboard, sceneController;
	private Vector3 originalRotation;
	private int index;
	void Start () {
		highlightedSpots = new Vector3[3];
		highlights = new GameObject[3];
		index = 0;
		tUI = sceneController.GetComponent<tutorialUI> ();
		grid = breadboard.GetComponent<gridLayout> ();
		originalRotation = transform.rotation.eulerAngles;

	}
	
	// Update is called once per frame
	void Update () {
		if (tUI.isSpawned) { //If the object is currently being dragged and is not yet placed
			highlightedSpots = grid.GetNearestPoints (this.transform.position, 3, this.gameObject, highlights, originalRotation);
			index = 0;
			for (int i = 0; i < highlightedSpots.Length; i++) {
				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Plane);
				if (highlightedSpots [i] == null) //red zone, cannot be placed here
					cube.GetComponent<Renderer> ().material.color = Color.red;
				else
					cube.GetComponent<Renderer> ().material.color = Color.green;
				cube.GetComponent<Collider> ().enabled = false;
				cube.transform.localScale = cube.transform.localScale * 0.005f;
				cube.transform.position = highlightedSpots [i];
				highlights [index] = cube;
				index++;
			}
		} else if (!tUI.isSpawned && highlights.Length != 0) {
			for (int i = 0; i < highlights.Length; i++) {
				Destroy (highlights [i]);
			}
			index = 0;
		}
	}

}
