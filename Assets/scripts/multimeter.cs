using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class multimeter : MonoBehaviour {

	public Vector3[] rotationAngles; //An array of positions for each rotation
	public Vector3 initPos, zoomPos, initRotation, zoomRotation; //Represents the initial position and the zoomed in position
	public GameObject breadboard, cam; //The breadboard
	public bool update = false;
	public GameObject dial, mReading; //Access to the dial and the text for the multimeter reading
	private bool pickedUp = false;
	public int rIndex = 0;
	private Vector3 lastPos, lastRot;
	private bool lastView = false;

	void Start () {
		dial = this.transform.GetChild (0).gameObject;
		mReading = this.transform.GetChild (1).gameObject;

		int scene = SceneManager.GetActiveScene ().buildIndex;
		zoomPos = new Vector3 (dial.transform.position.x, dial.transform.position.y + 0.35f, dial.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F) && !pickedUp) { //The multimeter has been called upon, and it's not picked up yet
			cam.GetComponent<Camera> ().orthographic = false;
			cam.transform.position = zoomPos;
			cam.transform.eulerAngles = zoomRotation;
			pickedUp = true;
		} else if (Input.GetKeyDown (KeyCode.F) && pickedUp) { //The multimeter needs to be put down
			cam.GetComponent<Camera> ().orthographic = lastView;
			cam.transform.position = lastPos;
			cam.transform.eulerAngles = lastRot;
			pickedUp = false;
		} else if (!pickedUp) {
			lastPos = cam.transform.position;
			lastRot = cam.transform.eulerAngles;
			lastView = cam.GetComponent<Camera> ().orthographic;
		} else if (pickedUp) {
			//Detect if the player is clicking the dial
			if (Input.GetMouseButtonDown (0)) {
				print (rIndex);
				if (rIndex == rotationAngles.Length - 1)
					updateReading ("");
				update = true;
				rIndex++;
				if (rIndex == rotationAngles.Length)
					rIndex = 0;
				Vector3 a = new Vector3 (0f, rotationAngles [rIndex].y, 0f);
				dial.transform.localEulerAngles = a;
			}
		}
	}

	//For changing the reading
	public void updateReading(string value){
		mReading.GetComponent<TextMesh> ().text = value;
	}

	//For checking if the right option is selected to read the value on the multimeter
	public int checkState(){
		return rIndex;
	}
}
