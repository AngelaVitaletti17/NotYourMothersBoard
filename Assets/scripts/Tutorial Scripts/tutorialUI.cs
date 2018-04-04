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
	public linkedList global_LL = new linkedList();

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
                float leftNx;
                float leftNz;
                float rightNx;
                float rightNz;
                             
               
                if (newItem.name.Contains("battery_spawner"))
                {

                    //global_LL = newItem.AddComponent(typeof(linkedList)) as linkedList;

                    inputNode = new componentNode(newItem.GetComponent<battery>(), breadboard.GetComponent<gridLayout>().positionHolder[414].x, breadboard.GetComponent<gridLayout>().positionHolder[414].z, null, null);
                    outputNode = new componentNode(newItem.GetComponent<battery>(), breadboard.GetComponent<gridLayout>().positionHolder[415].x, breadboard.GetComponent<gridLayout>().positionHolder[415].z, null, null);

                    breadboard.GetComponent<gridLayout>().gridPositions[breadboard.GetComponent<gridLayout>().positionHolder[414]] = true;
                    breadboard.GetComponent<gridLayout>().gridPositions[breadboard.GetComponent<gridLayout>().positionHolder[415]] = true;

                    inputNode.nextNode = null;
                    outputNode.previousNode = null;
                    //linkedList global_LL = new linkedList(inputNode, outputNode);
                    global_LL.head = inputNode;
                    global_LL.tail = outputNode;

                    
                    //test logic boardlogic.
                }
                else
                {
                    // gets cordinates for left and right componentNodes of newItem
                    int sc = newItem.GetComponent<gridPlacement>().spaceCount;
                    Vector3[] os = breadboard.GetComponent<gridLayout>().oldSpots;
                    if (sc % 2 == 0)
                    {
                        leftN = os[(sc / 2) - 1];
                        rightN = os[sc - 1];
                    }
                    else
                    {
                        leftN = os[(sc / 2)];
                        rightN = os[sc - 1];
                    }

                    leftNx = leftN.x;
                    leftNz = leftN.z;
                    rightNx = rightN.x;
                    rightNz = rightN.z;
                    float Y_constant = 1.957507f; // height constant from in game

                    //create vector3 of input and output cordinates
                    Vector3 leftNVector = new Vector3(leftNx, Y_constant, leftNz);
                    Vector3 rightNVector = new Vector3(rightNx, Y_constant, rightNz);


                    //**series logic**//   **LOGIC FOR MIDTERM PRESENTATION**                    
                    //componentNode c = pseudoTail of global_LL    //pseudoTail is last component placed into linkedList. Tail = other battery input
                    //if c = null   // comepleted circuit
                    //  if inSameCol(leftNVector, C.vector3) || inSameCol(rightNVector,c.vector3)
                    //    create componentNodes and add after battery        
                    //if inSameRow(leftNVector, C.vector3) || inSameRow(rightNVector,c.vector3)
                    //    create componentNodes and add after pseudoTail 

                    componentNode pseudoTail = global_LL.getPseudoTail(); //pseudoTail is last component placed into linkedList. Tail = other battery input

                    if (pseudoTail == null)// circuit is complete
                    {
                        print("User has placed component after circuit was completed.");

                    }
                    else
                    {
                        if (pseudoTail == global_LL.head) // Linked list only head and tail, (only battery) // checking power rails(columns) for matches
                        {
                            //check if newItem's Nodes in same column as battery nodes

                            componentNode head = global_LL.head;
                            float headz = head.getYPos(); // really z axis in unity not y

                            if (headz == leftNz)//check if left node in the power rail
                            {
                                print("LEFT NODE IN ");
                            }
                            if (headz == rightNz)//check if right node in the power rail
                            {
                                print("RIGHT NODE IN ");
                            }

                        }
                        else //not a battery, checking if newItem's nodes in same row as pseudoTail
                        {
                            // check if newItem's Nodes in same row as pseudoTail's nodes
                            //TODO

                        }

                    }

                    if (newItem.name.Contains("chip_spawner"))
                    {

                    }
                    else if (newItem.name.Contains("diode_spawner"))
                    {

                    }
                    else if (newItem.name.Contains("elec_cap_spawner"))
                    {

                    }
                    else if (newItem.name.Contains("resistor_spawning"))
                    {

                        // sets componentNodes of newItem
                        var nextNode_Array = new componentNode[] { outputNode };
                        var previousNode_Array = new componentNode[] { pseudoTail };

                        inputNode = new componentNode(newItem.GetComponent<resistor>(), leftNx, leftNz, previousNode_Array, nextNode_Array);

                        previousNode_Array = new componentNode[] { inputNode };
                        nextNode_Array = new componentNode[] { };
                        outputNode = new componentNode(newItem.GetComponent<resistor>(), rightNx, rightNz, previousNode_Array, nextNode_Array);

                    }
                    else if (newItem.name.Contains("LED_spawner"))
                    {

                    }
                    else if (newItem.name.Contains("switch_spawner"))
                    {

                    }
                    else if (newItem.name.Contains("wire_spawner"))
                    {

                    }
                    else if (newItem.name.Contains("transistor_spawner"))
                    {

                    }
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
