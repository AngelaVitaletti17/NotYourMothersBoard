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
    public Button ClearOut; //for wiping the board
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
	public boardLogic boardlogic = new boardLogic();

	//For dragging/placing
	public bool canBePlaced = true;
	public bool isSpawned, batteryPlaced = false;
	public Vector3 batteryLocation;
	private Vector3 mousePosition, itemPosition;
	private GameObject newItem;

	//For Text Information
	public Text info; //Text element for displaying information
	public int playerStep = 0;

	//For nodes
	public componentNode cn;

	//For clearing the board
	private List<GameObject> placed;
	private int placedIndex = 0;

	//For soldering pen
	public Vector3 sPenOrigin, sAngleOrig, sAngleNew;

	//For multimeter
	public GameObject meter, currentObject;
	public bool meterMode = false;
	public GameObject pen1, pen2;
	public Text mmActive;
	private Vector3 p1p, p2p, p1r, p2r;

	//Particle Effects
	public ParticleSystem sparks;

	//For the tutorial help boxes
	public GameObject walkthrough;

	//For help
	public GameObject help;
	public Button helpB;

	//Repair
	public GameObject[] parts;
	public GameObject bad;
	public GameObject sucRep; //Success Box

	void Awake(){
		cam = mainCam.GetComponent<cameraLook> ();
		grid = FindObjectOfType<gridLayout> ();
	}

	void Start () {
		sparks.Stop ();

		global_LL = breadboard.AddComponent(typeof(linkedList)) as linkedList;
		boardlogic = breadboard.AddComponent (typeof(boardLogic)) as boardLogic;
		placed = new List<GameObject> ();

		GameObject pen = GameObject.FindGameObjectWithTag ("pen");
		sPenOrigin = pen.transform.position;
		sAngleOrig = pen.transform.eulerAngles;
		sAngleNew = new Vector3(pen.transform.eulerAngles.x, 270f, pen.transform.eulerAngles.z);

		p1p = new Vector3(pen1.transform.position.x, pen1.transform.position.y + 0.075f, pen1.transform.position.z);
		p2p = new Vector3(pen2.transform.position.x, pen2.transform.position.y + 0.075f, pen2.transform.position.z);
		p1r = pen1.transform.eulerAngles;
		p2r = pen2.transform.eulerAngles;

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

		/*if (SceneManager.GetActiveScene().buildIndex == 3)
			batteryLocation = new Vector3(-0.233f, 1.651f, 3.01f);
		else if (SceneManager.GetActiveScene().buildIndex == 2)
			batteryLocation = new Vector3(-3.381f, 1.802f, 4.073f);*/

        //Clearing the board
        ClearOut.onClick.AddListener(clearBoard);

		//Help
		helpB.onClick.AddListener(toggleHelp);
		help.SetActive (false);
		walkthrough.SetActive (false);

		//MeterMode
		mmActive.gameObject.SetActive(false);

		if (SceneManager.GetActiveScene ().buildIndex == 3) { //repair level, manually set the component nodes
			sucRep.SetActive(false);

			grid.posAndSpots[grid.positionHolder[90]] = parts[0];
			grid.posAndSpots[grid.positionHolder[95]] = parts[0];

			grid.posAndSpots[grid.positionHolder[96]] = parts[1];
			grid.posAndSpots[grid.positionHolder[114]] = parts[1];

			grid.posAndSpots[grid.positionHolder[115]] = parts[2];
			grid.posAndSpots[grid.positionHolder[187]] = parts[2];

			grid.posAndSpots[grid.positionHolder[188]] = parts[3];
			grid.posAndSpots[grid.positionHolder[206]] = parts[3];

			grid.posAndSpots[grid.positionHolder[199]] = parts[4];
			grid.posAndSpots[grid.positionHolder[203]] = parts[4];

			bad = parts [2];
		}
	}

	// Update is called once per frame
	void Update () {

		if (SceneManager.GetActiveScene ().buildIndex == 1 && mainCam.GetComponent<TutorialText>().doneText){ //On Tutorial 
			walkthrough.SetActive(true);
			switch (playerStep) {
			case 0:
				walkthrough.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "Start by placing a battery.";
				print ("Start by placing the battery.");
				break;
			case 1:
				walkthrough.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "Now place a switch (Hint: You might have to place a wire first).";
				print ("Now place a switch");		
				break;
			case 2:
				walkthrough.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "Then, a resistor";
				print ("Then, a resistor");		
				break;
			case 3:
				walkthrough.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "Now an LED";
				print ("Now an LED");		
				break;
			case 4:
				walkthrough.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "Test to see if the circuit is complete. (Don't forget, a wire might have to lead back to the column the battery's red wire is in)";
				print ("Test the circuit if it's complete");		
				break;
			case 5:
				walkthrough.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "Tutorial Complete! Congratulations!";
				print ("Tutorial complete");		
				break;
			}
		}
		if (GameObject.Find ("battery_spawner(Clone)")) { //The battery already exists
			buttonArray [0].GetComponent<Button> ().interactable = false;
		} else
			buttonArray [0].GetComponent<Button> ().interactable = true;
		if (Input.GetMouseButtonDown (0) && !isSpawned && !meterMode) { //If the left button is click and the item is not spawned, and not in meter mode
			//Get the raycast data
			RaycastHit hit; 
			Ray ray = mainCam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) { //If we hit something, let's see what we hit
				if (hit.transform.name == "BreadBoard") { //If we hit the breadboard, zoom into it
					hit.transform.gameObject.GetComponent<selectGlow> ().zoomedIn = true;
					hit.transform.gameObject.GetComponent<Collider> ().enabled = false; //Maybe don't do this
					StartCoroutine (cam.zoomIn (hit.transform.gameObject)); //Start the coroutine to zoom in
				} else if ((hit.transform.gameObject.tag == "battery" && !breadboard.GetComponent<selectGlow> ().zoomedIn) || (hit.transform.gameObject.tag == "pen" && breadboard.GetComponent<selectGlow> ().zoomedIn)) { //If we selected the battery, let's drag in around
					isSpawned = true;
					newItem = hit.transform.gameObject;
					if (hit.transform.gameObject.tag == "battery")
						//Destroy Node Visuals
						destroyNodeVisuals(newItem);
					if (hit.transform.gameObject.tag == "pen")
						hit.transform.eulerAngles = sAngleNew;
				} else if (hit.transform.gameObject.tag == "component" && breadboard.GetComponent<selectGlow> ().zoomedIn) { //If we selected a component, let's drag it around
					isSpawned = true;
					newItem = hit.transform.gameObject;
					if (newItem.transform.childCount > 0) //There are most likely leads on this object
						newItem.transform.GetChild (0).localScale = newItem.GetComponent<gridPlacement> ().oScale;
					else
						newItem.transform.localScale = newItem.GetComponent<gridPlacement> ().oScale;

					//Destroy Node Visuals
					destroyNodeVisuals(newItem);
				} else if (hit.transform.name == "button") {

					//Next line should check for null on global_LL.head 1==1
					if (GameObject.Find ("battery_spawner(Clone)")) 
						{
							if (global_LL.head.nextNode.Length != 0) 
							{
								if (boardlogic.doCircuitLogicSeries (global_LL.head, global_LL.tail)) 
								{
									print ("Circuit Working");
								//Check if correct components were used
								//&& boardlogic.traceForward(global_LL.head,6)
								//add above to if statement if switch is fixed to size 2x1
									if (boardlogic.traceForward (global_LL.head, 2) && boardlogic.traceForward (global_LL.head, 3)) 
									{
										print ("CORRECT COMPONENTS USED");
									boardlogic.lightUp (global_LL.tail);
									//Next tutorial step
									if (playerStep == 4)
										playerStep++;
									
									} else {
										print ("Correct components not used");
									}

								}
								else 
								{
									print ("Circuit not functioning correctly or broken circuit?");
									sparks.Play ();
								}
							} else 
							{
								print ("Just a battery.");
							}
						} else
							print ("Battery not spawned");
					

				}
				if (SceneManager.GetActiveScene ().buildIndex == 3) { //repair level
					//Check if bad component
				}
				//Check to see if newItem is in the placed list
				//if (placed.Contains (newItem)) {
				//	placed.Remove (newItem);
				//}
			}
		} else if (isSpawned && !meterMode) { //If we are currently dragging the item and not in meterMode
			closePartsCatalogue (); //Keep the parts catalogue closed to avoid spawning multiple items
			//Have the component follow where the mouse moves
			itemPosition = Input.mousePosition;
			itemPosition.z = 0.4f;
			newItem.transform.position = Camera.main.ScreenToWorldPoint (itemPosition);
			if (newItem.tag != "battery") {
				newItem.GetComponent<gridPlacement> ().enabled = true; //Make sure this is enabled to get the highlights
				canBePlaced = newItem.GetComponent<gridPlacement> ().getComponentPlacementStatus (); //check to see if the item can be placed (is the spot valid?)
			}
			if (Input.GetMouseButtonDown (0) && newItem.tag == "pen") { //We gonna place some solder
				GameObject solder = GameObject.CreatePrimitive (PrimitiveType.Cube);
				solder.GetComponent<Collider> ().enabled = false;
				solder.transform.localScale = solder.transform.localScale * 0.017f;
				solder.tag = "solder";
				PlaceItem (Camera.main.ScreenToWorldPoint (itemPosition), solder);
			}
			if (canBePlaced && Input.GetMouseButton (1)) { //The item is placed on the board if it is in a valid spot
				//Item has been placed, keep track of it
				placed.Add (newItem); //Add the item
				isSpawned = false;
				if (newItem.tag == "component") { //If we are dragging the component, place it in the nearest spot on the grid
					PlaceItem (Camera.main.ScreenToWorldPoint (itemPosition), newItem);
					grid.scale_component (newItem);
				} else if (newItem.tag == "battery") {
					//Put item in preset spot
					newItem.transform.position = batteryLocation;
					if (SceneManager.GetActiveScene ().buildIndex == 1) //Tutorial Level
						newItem.transform.eulerAngles = new Vector3 (-90f, 0f, 270f);
					/*else if (SceneManager.GetActiveScene ().buildIndex == 2) //Creation Level
						newItem.transform.eulerAngles = new Vector3 (-90f, 0f, 0f);
					else if (SceneManager.GetActiveScene ().buildIndex == 3) //Repair Level
						newItem.transform.eulerAngles = new Vector3 (-90f, 0f, 180f);*/

					//Next tutorial step
					playerStep++;

					//Make these positions unable to be taken, set the dictionary 
				} else if (newItem.tag == "pen") {
					newItem.transform.position = sPenOrigin;
					newItem.transform.eulerAngles = sAngleOrig;

				}
				if (newItem.tag != "pen")
					grid.set_spots (newItem);

				//Henry and Kevin

				componentNode inputNode = newItem.AddComponent (typeof(componentNode)) as componentNode;
				componentNode outputNode = newItem.AddComponent (typeof(componentNode)) as componentNode;
				Vector3 leftN; 
				Vector3 rightN;
				float leftNx;
				float leftNz;
				float rightNx;
				float rightNz;
				float inX = -3f;
				float inZ = -3f;
				float outX = -3f;
				float outZ = -3f;
				bool leftNodeIsConnected = false;
				bool rightNodeIsConnected = false;
				//float Y_constant = 1.957507f; // height constant from in game
				float Y_constant = newItem.transform.position.y;
				var nullNode_Array = new componentNode[] { };


				//start: a component is placed on the bread board
				if (newItem.name.Contains ("battery_spawner")) { //if a battery was placed
					int bnode1 = 0;
					int bnode2 = 0;


					bnode1 = 414;
					bnode2 = 415;

					//creates input and output nodes of battery
					inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<battery> (), breadboard.GetComponent<gridLayout> ().positionHolder [bnode1].x, breadboard.GetComponent<gridLayout> ().positionHolder [bnode1].z, nullNode_Array, nullNode_Array);
					outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<battery> (), breadboard.GetComponent<gridLayout> ().positionHolder [bnode2].x, breadboard.GetComponent<gridLayout> ().positionHolder [bnode2].z, nullNode_Array, nullNode_Array);
					//sets positions as taken
					breadboard.GetComponent<gridLayout> ().gridPositions [breadboard.GetComponent<gridLayout> ().positionHolder [bnode1]] = true;
					breadboard.GetComponent<gridLayout> ().gridPositions [breadboard.GetComponent<gridLayout> ().positionHolder [bnode2]] = true;
					//sets head and tail of linked list
					global_LL.head = inputNode;
					global_LL.tail = outputNode;

					createNodeVisuals (bnode1, bnode2, newItem); 
				} else {//if another component was placed

					// gets cordinates for left and right componentNodes of newItem
					int sc = newItem.GetComponent<gridPlacement> ().spaceCount;
					Vector3[] os = breadboard.GetComponent<gridLayout> ().oldSpots;
					if (sc % 2 == 0) {
						leftN = os [(sc / 2) - 1];
						rightN = os [sc - 1];
						createNodeVisuals (System.Array.IndexOf(grid.positionHolder, os[(sc / 2) - 1]), System.Array.IndexOf(grid.positionHolder, os[sc - 1]), newItem);
					} else {
						leftN = os [(sc / 2)];
						rightN = os [sc - 1];
						createNodeVisuals (System.Array.IndexOf(grid.positionHolder, os[(sc / 2)]), System.Array.IndexOf(grid.positionHolder, os[sc - 1]), newItem);
					}


					//saves x and z component of left and right side of component
					leftNx = leftN.x;
					leftNz = leftN.z;
					rightNx = rightN.x;
					rightNz = rightN.z;

					//create vector3 of input and output cordinates
					Vector3 leftNVector = new Vector3 (leftNx, Y_constant, leftNz);
					Vector3 rightNVector = new Vector3 (rightNx, Y_constant, rightNz);

					//pseudoTail is last component placed into linkedList. Tail =  battery input
					componentNode pseudoTail = global_LL.getPseudoTail (); 


					//if (boardlogic.isCompleteCircuitSeries(global_LL.head))
					// FIX. Does not account for empty next/previousNODE array.Check length of the array before getting value.
					//errors out at boardLogic.traceback line 143, called by iscomplecircuitseries line 49

					if (false) {// circuit is complete ^^ see above
						print ("User has placed component after circuit was completed.");
					} else {

						if (pseudoTail.getXZ () == global_LL.head.getXZ ()) {
							// Linked list only head and tail, (only battery in list) 
							// checking power rails(columns) for matches

							print ("pseduoTail is head");
							//check if newItem's Nodes in same column as battery nodes


							componentNode head = global_LL.head;
							float headx = head.getXPos (); //gets column cordinate

							if (headx == leftNx) {//check if left node in the power rail
								print ("(POWER) LEFT NODE CONNECTED ");
								leftNodeIsConnected = true;
								//set cordinates of inputNode and outputNode
								inX = leftNx;
								inZ = leftNz;
								outX = rightNx;
								outZ = rightNz;
							}
							if (headx == rightNx) {//check if right node in the power rail
								print ("(POWER) RIGHT NODE CONNECTED ");
								rightNodeIsConnected = true;
								//set cordinates of inputNode and outputNode
								inX = rightNx;
								inZ = rightNz;
								outX = leftNx;
								outZ = leftNz;
							}

						}

						if (pseudoTail.getXZ () == global_LL.tail.getXZ ()) {
							// Linked list looping back to tail?
							// checking power rails(columns) for matches

							print ("pseduoTail is tail");
							//check if newItem's Nodes in same column as battery nodes

							componentNode tail = global_LL.tail;
							float tailx = tail.getXPos (); //gets column cordinate

							if (tailx == leftNx) {//check if left node in the power rail
								print ("(POWER) LEFT NODE CONNECTED ");
								leftNodeIsConnected = true;
								//set cordinates of inputNode and outputNode
								inX = leftNx;
								inZ = leftNz;
								outX = rightNx;
								outZ = rightNz;
							}
							if (tailx == rightNx) {//check if right node in the power rail
								print ("(POWER) RIGHT NODE CONNECTED ");
								rightNodeIsConnected = true;
								//set cordinates of inputNode and outputNode
								inX = rightNx;
								inZ = rightNz;
								outX = leftNx;
								outZ = leftNz;
							}

						} else { //List is not empty, checking if newItem's nodes in same row as pseudoTail outputNode

							// check if newItem's Nodes in same row as pseudoTail's nodes
							componentNode lastNode = global_LL.getPseudoTail ();
							float lastNodez = lastNode.getYPos (); // really z in unity terms 
							float lastNodex = lastNode.getXPos ();                            
							Vector3 lastNodeVector = new Vector3 (lastNodex, Y_constant, lastNodez);

							//get breadboard index for left/right node of newItem and outputNode of pseudoTail
							int lastNodeIndex = System.Array.IndexOf (breadboard.GetComponent<gridLayout> ().positionHolder, lastNodeVector);
							int leftNodeIndex = System.Array.IndexOf (breadboard.GetComponent<gridLayout> ().positionHolder, leftNVector);
							int rigthNodeIndex = System.Array.IndexOf (breadboard.GetComponent<gridLayout> ().positionHolder, rightNVector);

							//get remainder so see what column index is in. 
							//index = (1-18)
							//(1-2) (17-18) = battery terminals
							//(3-9) = left grid
							//(10-16) = right grid
							int lastNodeIndexCol = (lastNodeIndex % 18) + 1;
							int leftNodeIndexCol = (leftNodeIndex % 18) + 1;
							int rigthNodeIndexCol = (rigthNodeIndex % 18) + 1;

							//print("HERE IS WHERE PROBLEM IS");
							//print("Z position of pseudoTail: "+ lastNodez);
							//print("Z position of leftNode: "+leftNz);
							//print("Z position of rigthNode: "+rightNz);

							//if leftNode is in the same row as pseudoTail Node
							if (lastNodez == leftNz) {
								print ("LEFT NODE IN SAME ROW AS NODE");
								//checks if Both nodes are in left grid
								if (((lastNodeIndexCol <= 9) && (lastNodeIndexCol >= 2)) && ((leftNodeIndexCol <= 9) && (leftNodeIndexCol >= 2))) { // both in same row and col range

									print ("H LEFT NODE CONNECTED ");
									leftNodeIsConnected = true;
									//set cordinates of inputNode and outputNode
									inX = leftNx;
									inZ = leftNz;
									outX = rightNx;
									outZ = rightNz;
								}
								//checks if Both nodes are in right grid
								if (((lastNodeIndexCol <= 16) && (lastNodeIndexCol >= 10)) && ((leftNodeIndexCol <= 16) && (leftNodeIndexCol >= 10))) {// both in same row and col range 
									print ("H LEFT NODE CONNECTED ");
									leftNodeIsConnected = true;
									//set cordinates of inputNode and outputNode
									inX = leftNx;
									inZ = leftNz;
									outX = rightNx;
									outZ = rightNz;
								}


							}
							//if rightNode is in the same row as pseudoTail Node
							if (lastNodez == rightNz) {
								print ("RIGHT NODE IN SAME ROW AS NODE");
								//checks if Both nodes are in left grid
								if (((lastNodeIndexCol <= 9) && (lastNodeIndexCol >= 2)) && ((rigthNodeIndexCol <= 9) && (rigthNodeIndexCol >= 2))) { // both in same row and col range
									print ("H RIGHT NODE CONNECTED ");
									rightNodeIsConnected = true;
									inX = rightNx;
									inZ = rightNz;
									outX = leftNx;
									outZ = leftNz;
								}
								//checks if Both nodes are in right grid
								if (((lastNodeIndexCol <= 16) && (lastNodeIndexCol >= 10)) && ((rigthNodeIndexCol <= 16) && (rigthNodeIndexCol >= 10))) {// both in same row and col range 
									print ("H RIGHT NODE CONNECTED ");
									rightNodeIsConnected = true;
									inX = rightNx;
									inZ = rightNz;
									outX = leftNx;
									outZ = leftNz;
								}
							}



							//print("lastNodeIndex: "+lastNodeIndex);
							//print("leftNodeIndex: "+leftNodeIndex);
							//print("rigthNodeInde: "+rigthNodeIndex);


						}

					}

					if (newItem.name.Contains ("chip_spawner")) {

					} else if (newItem.name.Contains ("diode_spawner")) {
						print ("spawning diode ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<diode> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<diode> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
						}

					} else if (newItem.name.Contains ("elec_cap_spawner")) {
						print ("spawning capacitor ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<capacitor> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<capacitor> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
						}

					} else if (newItem.name.Contains ("resistor_spawner")) {
						print ("spawning resistor ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<resistor> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<resistor> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
							//Next tutorial step
							if (playerStep == 2)
								playerStep++;
						}
					} else if (newItem.name.Contains ("LED_spawner")) {
						print ("spawning led ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<led> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<led> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
							//Next tutorial step
							if (playerStep == 3)
								playerStep++;
						}

					} else if (newItem.name.Contains ("switch_spawner")) {
						print ("spawning switch ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<circuitSwitch> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<circuitSwitch> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
							//Next tutorial step
							if (playerStep == 1) {
								playerStep++;
							}
						}

					} else if (newItem.name.Contains ("wire_spawner")) {
						print ("spawning wire ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<wire> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<wire> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
						}

					} else if (newItem.name.Contains ("transistor_spawner")) {
						print ("spawning transitor ");
						// sets componentNodes of newItem
						inputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<resistor> (), inX, inZ, nullNode_Array, nullNode_Array);
						outputNode = new componentNode (newItem.GetInstanceID (), newItem.GetComponent<resistor> (), outX, outZ, nullNode_Array, nullNode_Array);

						if (leftNodeIsConnected || rightNodeIsConnected) {
							var nextNode_Array = new componentNode[] { };
							var previousNode_Array = new componentNode[] { };

							//sets next/previous nodes for inputNode
							nextNode_Array = new componentNode[] { outputNode };
							previousNode_Array = new componentNode[] { pseudoTail };
							inputNode.setNextNode (nextNode_Array);
							inputNode.setPreviousNode (previousNode_Array);

							previousNode_Array = new componentNode[] { inputNode };
							pseudoTail.setNextNode (previousNode_Array);

							outputNode.setNextNode (nullNode_Array);
							outputNode.setPreviousNode (previousNode_Array);

							if (outputNode.getXPos () == global_LL.tail.getXPos ()) {
								previousNode_Array = new componentNode[] { global_LL.tail };
								outputNode.setNextNode (previousNode_Array);
								global_LL.tail.setPreviousNode (nextNode_Array);
							}
						}

					}
				}
				global_LL.printList ();
			} 
			if (Input.GetKeyDown (KeyCode.R)) {
				newItem.transform.eulerAngles = new Vector3 (newItem.transform.eulerAngles.x, newItem.transform.eulerAngles.y + 90f, newItem.transform.eulerAngles.z);
			}

		} else if (!isSpawned && !meterMode) {
			if (newItem != null) {
				if (newItem.tag != "battery")
					newItem.GetComponent<gridPlacement> ().enabled = false;
			}
			if (Input.GetKeyDown (KeyCode.M) && !meterMode) { //Enter meterMode
				meterMode = true;
				mmActive.gameObject.SetActive(true);

			} 
		} else if (Input.GetKeyDown (KeyCode.M) && meterMode) { //We also need to snap the positions of the pens back
			meterMode = false;
			mmActive.gameObject.SetActive(false);
			pen1.transform.position = p1p;
			pen2.transform.position = p2p;
			pen1.transform.eulerAngles = p1r;
			pen2.transform.eulerAngles = p2r;
			if (meter.GetComponent<multimeter> ().rIndex == 0) { //It's off
				meter.GetComponent<multimeter> ().updateReading ("");
			} else
				meter.GetComponent<multimeter> ().updateReading ("-----");

		} else if (meterMode) { //We are currently in meter mode, now we can snap shit
			closePartsCatalogue ();
			circuitComponent cc = null;
			if (Input.GetMouseButtonDown (0)) { //We've clicked something, let us see what it is OR something needs to be updated cuz we touched the dial
				RaycastHit hit; 
				Ray ray = mainCam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.tag == "component") { //We are hitting a component, let's snap some pens
						Vector3 snappedAlready = Vector3.zero;
						for (int i = 0; i < grid.positionHolder.Length; i++) {
							if (snappedAlready != Vector3.zero && grid.posAndSpots [grid.positionHolder [i]] == hit.transform.gameObject) {
								pen2.transform.position = grid.positionHolder [i];
								if (Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 270f || Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 90f)
									pen2.transform.eulerAngles = new Vector3 (0f, 0f, 20f);
								else if (Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 180f || Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 0f)
									pen2.transform.eulerAngles = new Vector3 (20f, 90f, 0f);
								break;
							} else if (grid.posAndSpots [grid.positionHolder [i]] == hit.transform.gameObject) {
								snappedAlready = grid.positionHolder [i];
								pen1.transform.position = grid.positionHolder [i];
								if (Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 270f || Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 90f)
									pen1.transform.eulerAngles = new Vector3 (0f, 0f, 20f);
								else if (Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 180f || Mathf.Round (hit.transform.gameObject.transform.eulerAngles.y) == 0f)
									pen1.transform.eulerAngles = new Vector3 (20f, 90f, 0f);
							}
						}
						cc = hit.transform.gameObject.GetComponent<circuitComponent> ();
						currentObject = hit.transform.gameObject;
					} if (meter.GetComponent<multimeter>().update) {
						cc = currentObject.transform.gameObject.GetComponent<circuitComponent> ();
					}
					//Make sure cc is not null
					if (cc != null) {
						if (cc.componentType == 0) { //NULL

						} else if (cc.componentType == 1) { //Battery
							double b = cc.GetComponent<battery> ().voltage;
							if (meter.GetComponent<multimeter> ().checkState () == 1) //We're in the battery state
							meter.GetComponent<multimeter> ().updateReading (b.ToString ());
						} else if (cc.componentType == 2) { //Resistor
							double cur = cc.componentCurrent;
							double v = cc.componentVoltage;
							double res = cc.GetComponent<resistor> ().getOhms ();
							if (meter.GetComponent<multimeter> ().checkState () == 1) //DC Voltage
							meter.GetComponent<multimeter> ().updateReading (v.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 3) //Smol amps
							meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "p");
							else if (meter.GetComponent<multimeter> ().checkState () == 4) //milli amps
							meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "m");
							else if (meter.GetComponent<multimeter> ().checkState () == 5) // amps
							meter.GetComponent<multimeter> ().updateReading (cur.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 7) //ohms
							meter.GetComponent<multimeter> ().updateReading (res.ToString ());
						} else if (cc.componentType == 3) { //LED
							double cur = cc.componentCurrent;
							double v = cc.componentVoltage;
							if (meter.GetComponent<multimeter> ().checkState () == 1) //DC Voltage
								meter.GetComponent<multimeter> ().updateReading (v.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 3) //Smol amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "p");
							else if (meter.GetComponent<multimeter> ().checkState () == 4) //milli amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "m");
							else if (meter.GetComponent<multimeter> ().checkState () == 5) // amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString ());
						} else if (cc.componentType == 4) { //Capacitor
							double cur = cc.componentCurrent;
							double v = cc.componentVoltage;
							double cap = cc.GetComponent<capacitor> ().getFarads();
							if (meter.GetComponent<multimeter> ().checkState () == 1) //DC Voltage
								meter.GetComponent<multimeter> ().updateReading (v.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 3) //Smol amps
								meter.GetComponent<multimeter> ().updateReading (cap.ToString () + "p");
							else if (meter.GetComponent<multimeter> ().checkState () == 4) //milli amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "m");
							else if (meter.GetComponent<multimeter> ().checkState () == 5) // amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString ());
						} else if (cc.componentType == 5) { //Diode
							double cur = cc.componentCurrent;
							double v = cc.componentVoltage;
							if (meter.GetComponent<multimeter> ().checkState () == 1) //DC Voltage
								meter.GetComponent<multimeter> ().updateReading (v.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 3) //Smol amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "p");
							else if (meter.GetComponent<multimeter> ().checkState () == 4) //milli amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "m");
							else if (meter.GetComponent<multimeter> ().checkState () == 5) // amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString ());
						} else if (cc.componentType == 6) { //Switch
							double cur = cc.componentCurrent;
							double v = cc.componentVoltage;
							if (meter.GetComponent<multimeter> ().checkState () == 1) //DC Voltage
								meter.GetComponent<multimeter> ().updateReading (v.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 3) //Smol amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "p");
							else if (meter.GetComponent<multimeter> ().checkState () == 4) //milli amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "m");
							else if (meter.GetComponent<multimeter> ().checkState () == 5) // amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString ());
						} else if (cc.componentType == 7) { //Potentiometer

						} else if (cc.componentType == 99) { //Wire
							double cur = cc.componentCurrent;
							double v = cc.componentVoltage;
							if (meter.GetComponent<multimeter> ().checkState () == 1) //DC Voltage
								meter.GetComponent<multimeter> ().updateReading (v.ToString ());
							else if (meter.GetComponent<multimeter> ().checkState () == 3) //Smol amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "p");
							else if (meter.GetComponent<multimeter> ().checkState () == 4) //milli amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString () + "m");
							else if (meter.GetComponent<multimeter> ().checkState () == 5) // amps
								meter.GetComponent<multimeter> ().updateReading (cur.ToString ());
						}
					}
				} 
					meter.GetComponent<multimeter> ().update = false;
			}
		}
	}

	public void openPartsCatalogue(){
		closeInventory.gameObject.SetActive (true);
		openInventory.gameObject.SetActive (false);
		inventory.SetActive (true);
	}

	public void closePartsCatalogue(){
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

	void PlaceItem(Vector3 clickPoint, GameObject hit){ //For placing components
		Vector3 final;
		if (hit.tag == "solder") {
			final = grid.GetGridPoint (clickPoint, 1);
			hit.transform.position = final;
		}
		else {
			final = grid.GetGridPoint (clickPoint, hit.gameObject.GetComponent<gridPlacement> ().spaceCount); //For placing components
			if (hit.gameObject.GetComponent<gridPlacement> ().spaceCount % 2 != 0)
				hit.transform.position = final;
			else
				hit.transform.position = (breadboard.GetComponent<gridLayout> ().oldSpots [0] + breadboard.GetComponent<gridLayout> ().oldSpots [1]) / 2f;
		}
	}

	public void clearBoard(){
		foreach (GameObject item in placed) {
			destroyNodeVisuals (item);
			Destroy	(item);
		}
		//Reset if tutorial
		playerStep = 0;
	}

	public void createNodeVisuals(int leftIndex, int rightIndex, GameObject newItem){
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Plane); //Create a plane to represent the highlight
		cube.GetComponent<Renderer> ().material.color = Color.blue;
		cube.GetComponent<Collider> ().enabled = false;
		cube.transform.localScale = cube.transform.localScale * 0.002f;
		cube.transform.position = new Vector3(grid.positionHolder [leftIndex].x, grid.positionHolder[leftIndex].y - 0.01f, grid.positionHolder[leftIndex].z);

		//Second Node
		GameObject cube2 = GameObject.CreatePrimitive (PrimitiveType.Plane);
		cube2.GetComponent<Renderer> ().material.color = Color.blue;
		cube2.GetComponent<Collider> ().enabled = false;
		cube2.transform.localScale = cube2.transform.localScale * 0.002f;
		cube2.transform.position = new Vector3(grid.positionHolder [rightIndex].x, grid.positionHolder[rightIndex].y - 0.01f, grid.positionHolder[rightIndex].z);
	
		newItem.GetComponent<gridPlacement> ().visualNodes [0] = cube;
		newItem.GetComponent<gridPlacement> ().visualNodes [1] = cube2;
	}

	public void destroyNodeVisuals (GameObject newItem){

		//Get rid of node visuals
		for (int i = 0; i < newItem.GetComponent<gridPlacement>().visualNodes.Length; i++)
			Destroy (newItem.GetComponent<gridPlacement>().visualNodes [i]);
	}

	public void toggleHelp(){
		if (help.activeSelf)
			help.SetActive (false);
		else
			help.SetActive (true);
	}
}
