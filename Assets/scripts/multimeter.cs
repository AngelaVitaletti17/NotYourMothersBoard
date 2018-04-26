using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class multimeter : MonoBehaviour {

	public Vector3[] rotationAngles; //An array of positions for each rotation
	public Vector3 initPos, zoomPos, initRotation, zoomRotation; //Represents the initial position and the zoomed in position
	public GameObject breadboard, cam; //The breadboard
	private GameObject dial, mReading; //Access to the dial and the text for the multimeter reading
	private bool pickedUp = false;
	private int rIndex = 0;
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
			cam.GetComponent<Camera>().orthographic = lastView;
			cam.transform.position = lastPos;
			cam.transform.eulerAngles = lastRot;
			pickedUp = false;
		} else if (Input.GetKeyDown (KeyCode.D) && pickedUp) { //Rotate the dial to the next position
			if (rIndex == rotationAngles.Length) //We're at the end of the array
				rIndex = 0;
			dial.transform.eulerAngles = rotationAngles [rIndex];
			rIndex++;
		} else if (!pickedUp) {
			lastPos = cam.transform.position;
			lastRot = cam.transform.eulerAngles;
			lastView = cam.GetComponent<Camera> ().orthographic;
		}
	}

	//For changing the reading
	public void updateReading(string value){
		mReading.GetComponent<TextMesh> ().text = value;
	}

	//For checking if the right option is selected to read the value on the multimeter
	public string checkState(GameObject component){
		return "hi";
	}
}
