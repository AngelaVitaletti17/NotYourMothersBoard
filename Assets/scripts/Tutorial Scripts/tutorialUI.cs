using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorialUI : MonoBehaviour {

	//For inventory open-close buttons
	public Button openInventory; //The button that opens the Parts Catalogue
	public Button closeInventory; //The button the closes the Parts Catalogue
	public Button zoomOut; //The button for zooming out once on the bread board
	public GameObject inventory; //The gameobject in the scene that holds the Parts Catalogue (UI Panel)

	//For items in inventory
	public Button[] buttonArray; //Holds the buttons for spawning component gameobjects
	public GameObject[] instantiateItem; // Holds the GameObjects that should be spawned from the button

	//Connect to cameraLook
	public Camera mainCam;
	private cameraLook cam;

	//For breadboard and breadboard grid
	public GameObject breadboard;
	private gridLayout grid;
	linkedList l;

	//For dragging/placing
	public bool canBePlaced = true;
	public bool isSpawned = false;
	public Vector3 batteryLocation;
	private Vector3 mousePosition, itemPosition;
	private GameObject newItem;

	//For Text Information
	public Text info; //Text element for displaying information

	//For nodes
	public componentNode cn;

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
	//Back button
		zoomOut.onClick.AddListener(delegate {StartCoroutine(cam.zoomOut(breadboard));});
	
	//Placing the battery
		batteryLocation = new Vector3(-2.221f, 2.017f, -9.197f);
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
					StartCoroutine (cam.zoomIn (hit.transform.gameObject)); //Start the coroutine to zoom in
				} else if (hit.transform.gameObject.tag == "battery" && !breadboard.GetComponent<selectGlow>().zoomedIn) { //If we selected the battery, let's drag in around
					isSpawned = true;
					newItem = hit.transform.gameObject;
				} else if (hit.transform.gameObject.tag == "component" && breadboard.GetComponent<selectGlow>().zoomedIn) { //If we selected a component, let's drag it around
					isSpawned = true;
					newItem = hit.transform.gameObject;
					if (newItem.transform.childCount > 0) //There are most likely leads on this object
						newItem.transform.GetChild(0).localScale = newItem.GetComponent<gridPlacement>().oScale;
					else
						newItem.transform.localScale = newItem.GetComponent<gridPlacement> ().oScale;
				}
			}
		} else if (isSpawned) { //If we are currently dragging the item
			closePartsCatalogue (); //Keep the parts catalogue closed to avoid spawning multiple items
			//Have the component follow where the mouse moves
			itemPosition = Input.mousePosition; 
			itemPosition.z = 0.4f;
			newItem.transform.position = Camera.main.ScreenToWorldPoint (itemPosition);
			if (newItem.tag != "battery"){
				newItem.GetComponent<gridPlacement> ().enabled = true; //Make sure this is enabled to get the highlights
				canBePlaced = newItem.GetComponent<gridPlacement> ().getComponentPlacementStatus (); //check to see if the item can be placed (is the spot valid?)
			}
			if (canBePlaced && Input.GetMouseButton (1)) { //The item is placed on the board if it is in a valid spot
				isSpawned = false;
				if (newItem.tag == "component") { //If we are dragging the component, place it in the nearest spot on the grid
					PlaceItem (Camera.main.ScreenToWorldPoint (itemPosition), newItem);
					grid.scale_component (newItem);
				} else if (newItem.tag == "battery") {
					//Put item in preset spot
					newItem.transform.position = batteryLocation;
					//Make these positions unable to be taken, set the dictionary 
				}
				grid.set_spots ();

				//Henry and Kevin

				componentNode inputNode = newItem.AddComponent (typeof(componentNode)) as componentNode;
				componentNode outputNode = newItem.AddComponent (typeof(componentNode)) as componentNode;
				Vector3 leftN; 
				Vector3 rightN;


				if (newItem.name.Contains("battery_spawner")) {
					l = newItem.AddComponent (typeof(linkedList)) as linkedList;

					inputNode = new componentNode (newItem.GetComponent<battery> (), breadboard.GetComponent<gridLayout>().positionHolder[414].x,breadboard.GetComponent<gridLayout>().positionHolder[414].y , null , null ); //oldSpots for vector
					outputNode = new componentNode (newItem.GetComponent<battery> (), breadboard.GetComponent<gridLayout>().positionHolder[415].x, breadboard.GetComponent<gridLayout>().positionHolder[415].y, null, null);

					breadboard.GetComponent<gridLayout>().gridPositions[breadboard.GetComponent<gridLayout>().positionHolder[414]] = true;
					breadboard.GetComponent<gridLayout>().gridPositions[breadboard.GetComponent<gridLayout>().positionHolder[415]] = true;

					inputNode.nextNode = null;
					outputNode.previousNode = null; 
					l = new linkedList (inputNode, outputNode);

				
					//test logic boardlogic.
				} else if (newItem.name.Contains( "chip_spawner")) {
					
				} else if (newItem.name.Contains( "diode_spawner")) {

				} else if (newItem.name.Contains("elec_cap_spawner")) {

				} else if (newItem.name.Contains("resistor_spawning")) {

					int sc = newItem.GetComponent<gridPlacement> ().spaceCount; 
					Vector3[] os = breadboard.GetComponent<gridLayout> ().oldSpots;
					if (sc % 2 == 0) {
						leftN = os[(sc/2)-1];
						rightN = os[sc-1];
					} else {
						leftN = os[(sc/2)];
						rightN = os[sc-1];
					}

					inputNode = new componentNode (newItem.GetComponent<battery> (), leftN.x,leftN.z , null , null ); //oldSpots for vector
					outputNode = new componentNode (newItem.GetComponent<battery> (), rightN.x, rightN.z, null, null);


					// set next and previous nodes


					//breadboard.GetComponent<gridLayout>().gridPositions[breadboard.GetComponent<gridLayout>().positionHolder[leftN]] = true;
					//breadboard.GetComponent<gridLayout>().gridPositions[breadboard.GetComponent<gridLayout>().positionHolder[rightN]] = true;



				} else if (newItem.name.Contains( "LED_spawner")) {

				} else if (newItem.name.Contains( "switch_spawner")) {

				} else if (newItem.name.Contains( "wire_spawner")) {

				} else if (newItem.name.Contains( "transistor_spawner")) {

				}
			} 
			if (Input.GetKeyDown (KeyCode.R)) {
				newItem.transform.eulerAngles = new Vector3 (newItem.transform.eulerAngles.x, newItem.transform.eulerAngles.y + 90f, newItem.transform.eulerAngles.z);
			}

		} else if (!isSpawned && newItem != null){
			if (newItem.tag != "battery")
				newItem.GetComponent<gridPlacement> ().enabled = false;
		}
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

	void PlaceItem(Vector3 clickPoint, GameObject hit){ //For placing components WILL NOT WORK WITH BATTERY YET NOTE
		Vector3 final;
		final = grid.GetGridPoint (clickPoint, hit.gameObject.GetComponent<gridPlacement>().spaceCount); //For placing components
		if (hit.gameObject.GetComponent<gridPlacement> ().spaceCount % 2 != 0)
			hit.transform.position = final;
		else
			hit.transform.position = (breadboard.GetComponent<gridLayout> ().oldSpots [0] + breadboard.GetComponent<gridLayout> ().oldSpots [1]) / 2f;
	}
}
