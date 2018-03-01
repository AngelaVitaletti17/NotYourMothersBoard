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
	private GameObject newItem;
	private bool isSpawned = false;
	private Vector3 mousePosition, itemPosition;

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
		if (Input.GetMouseButtonDown (0) && !isSpawned) {
			RaycastHit hit;
			Ray ray = mainCam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.name == "BreadBoard") {
					hit.transform.gameObject.GetComponent<selectGlow> ().zoomedIn = true;
					hit.transform.gameObject.GetComponent<Collider> ().enabled = false;
					StartCoroutine (cam.zoomIn ());
				} else if (hit.transform.gameObject.tag == "battery") {
					isSpawned = true;
					newItem = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
				}else if (hit.transform.gameObject.tag == "component"){
					isSpawned = true;
					newItem = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
				}
			}
		} else if (isSpawned) {
			closePartsCatalogue ();
			itemPosition = Input.mousePosition;
			itemPosition.z = 0.4f;
			newItem.transform.position = Camera.main.ScreenToWorldPoint(itemPosition);
			if (Input.GetMouseButton (1)) {
				isSpawned = false;
				newItem.GetComponent<Rigidbody> ().isKinematic = false;
				if (newItem.tag == "component")
					PlaceCubeNear (Camera.main.ScreenToWorldPoint(itemPosition), newItem);
			}
			if (Input.GetKeyDown(KeyCode.R)) {
				newItem.transform.eulerAngles = new Vector3 (newItem.transform.eulerAngles.x, newItem.transform.eulerAngles.y + 90f, newItem.transform.eulerAngles.z);
			}
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
		newItem.GetComponent<Rigidbody> ().isKinematic = true;
		isSpawned = true;
		itemPosition = position;
	}

	void PlaceCubeNear(Vector3 clickPoint, GameObject hit){
		Vector3 final = grid.GetNearestPointOnGrid (clickPoint);
		hit.transform.position = final;
		//GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		//cube.transform.localScale = cube.transform.localScale * 0.05f;
		//cube.transform.position = final;
	}
}
