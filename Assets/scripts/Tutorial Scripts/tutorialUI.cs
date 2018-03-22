using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorialUI : MonoBehaviour {

	//For inventory open-close buttons
	public Button openInventory; //The button that opens the Parts Catalogue
	public Button closeInventory; //The button the closes the Parts Catalogue
	public GameObject inventory; //The gameobject in the scene that holds the Parts Catalogue (UI Panel)

	//For items in inventory
	public Button[] buttonArray; //Holds the buttons for spawning component gameobjects
	public GameObject[] instantiateItem; // Holds the GameObjects that should be spawned from the button

	//Connect to cameraLook
	public Camera mainCam;
	private cameraLook cam;

	//For breadboard grid
	private gridLayout grid;

	//For dragging
	public bool canBePlaced = true;
	public bool isSpawned = false;
	private Vector3 mousePosition, itemPosition;
	private GameObject newItem;

	void Awake(){
		cam = mainCam.GetComponent<cameraLook> ();
		grid = FindObjectOfType<gridLayout> ();
	}

	void Start () {
	//*OPEN-CLOSE INVENTORY*
		//Inventory is closed by default
		openInventory.gameObject.SetActive (true);
		closeInventory.gameObject.SetActive (false);
		inventory.SetActive (false);

		//When we click the buttons, do something
		openInventory.onClick.AddListener (openPartsCatalogue);
		closeInventory.onClick.AddListener (closePartsCatalogue);
	//*INVENTORY ITEMS*
		for (int i = 0; i < buttonArray.Length; i++) {
			GameObject item = instantiateItem [i];
			buttonArray [i].onClick.AddListener (() => {spawnItem(item);});
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !isSpawned) { //If the left button is click and the item is not spawned
			//Get the raycast data
			RaycastHit hit; 
			Ray ray = mainCam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) { //If we hit something, let's see what we hit
				if (hit.transform.name == "BreadBoard") { //If we hit the breadboard, zoom into it
					hit.transform.gameObject.GetComponent<selectGlow> ().zoomedIn = true;
					hit.transform.gameObject.GetComponent<Collider> ().enabled = false; //Maybe don't do this
					StartCoroutine (cam.zoomIn ()); //Start the coroutine to zoom in
				} else if (hit.transform.gameObject.tag == "battery") { //If we selected the battery, let's drag in around
					isSpawned = true;
					newItem = hit.transform.gameObject;
				} else if (hit.transform.gameObject.tag == "component") { //If we selected a component, let's drag it around
					isSpawned = true;
					newItem = hit.transform.gameObject;
				}
			}
		} else if (isSpawned) { //If we are currently dragging the item
			closePartsCatalogue (); //Keep the parts catalogue closed to avoid spawning multiple items
			//Have the component follow where the mouse moves
			itemPosition = Input.mousePosition; 
			itemPosition.z = 0.4f;
			newItem.transform.position = Camera.main.ScreenToWorldPoint (itemPosition);
			newItem.GetComponent<gridPlacement> ().enabled = true; //Make sure this is enabled to get the highlights
			canBePlaced = newItem.GetComponent<gridPlacement> ().getComponentPlacementStatus (); //check to see if the item can be placed (is the spot valid?)
			if (canBePlaced && Input.GetMouseButton (1)) { //The item is placed on the board if it is in a valid spot
				isSpawned = false;
				if (newItem.tag == "component") //If we are dragging the component, place it in the nearest spot on the grid
					PlaceItem (Camera.main.ScreenToWorldPoint (itemPosition), newItem);
				grid.set_spots ();
			} 
			if (Input.GetKeyDown (KeyCode.R)) {
				newItem.transform.eulerAngles = new Vector3 (newItem.transform.eulerAngles.x, newItem.transform.eulerAngles.y + 90f, newItem.transform.eulerAngles.z);
			}

		} else if (!isSpawned && newItem != null)
			newItem.GetComponent<gridPlacement> ().enabled = false;
	}

	void openPartsCatalogue(){
		closeInventory.gameObject.SetActive (true);
		openInventory.gameObject.SetActive (false);
		inventory.SetActive (true);
	}

	void closePartsCatalogue(){
		closeInventory.gameObject.SetActive (false);
		openInventory.gameObject.SetActive (true);
		inventory.SetActive (false);
	}

	void spawnItem(GameObject item){
		Vector3 position = Input.mousePosition;
		position.z = 2.0f;
		newItem = Instantiate (item, Camera.main.ScreenToWorldPoint(position), item.transform.rotation);
		isSpawned = true;
		itemPosition = position;
	}

	void PlaceItem(Vector3 clickPoint, GameObject hit){
		Vector3 final = grid.GetGridPoint (clickPoint);
		hit.transform.position = final;
	}
}
