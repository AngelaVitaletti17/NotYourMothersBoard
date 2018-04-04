using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TutorialText : MonoBehaviour {

	public GameObject[] tutText;
	public GameObject sc;
	private int index = 0;

	void Start () {
		for (int i = 0; i < tutText.Length; i++)
			tutText [i].SetActive (false);
		tutText [0].SetActive (true);
		index++;
	}
	
	void Update(){
		if (Input.GetKeyDown ("space") && index < tutText.Length) {
			tutText [index - 1].SetActive (false);
			tutText [index].SetActive (true);
			if (index == 2) {
				StartCoroutine (sc.GetComponent<tutorialUI> ().mainCam.GetComponent<cameraLook> ().zoomIn (sc.GetComponent<tutorialUI> ().breadboard));
				sc.GetComponent<tutorialUI> ().breadboard.GetComponent<selectGlow> ().zoomedIn = true;
			}
			if (index == 5) {
				sc.GetComponent<tutorialUI> ().openPartsCatalogue ();
			}
			if (index == 6) {
				sc.GetComponent<tutorialUI> ().closePartsCatalogue ();
			}
			index++;
		} else if (Input.GetKeyDown (KeyCode.Y) && index == tutText.Length) { //Reset the tutorial
			StartCoroutine (sc.GetComponent<tutorialUI> ().mainCam.GetComponent<cameraLook> ().zoomOut (sc.GetComponent<tutorialUI> ().breadboard));
			sc.GetComponent<tutorialUI> ().breadboard.GetComponent<selectGlow> ().zoomedIn = false;
			tutText [tutText.Length - 1].SetActive (false);
			tutText [0].SetActive (true);
			index = 1;
		}
		else if (Input.GetKeyDown(KeyCode.N) && index == tutText.Length){ //Let them play!
			StartCoroutine (sc.GetComponent<tutorialUI> ().mainCam.GetComponent<cameraLook> ().zoomOut (sc.GetComponent<tutorialUI> ().breadboard));
			sc.GetComponent<tutorialUI> ().breadboard.GetComponent<selectGlow> ().zoomedIn = false;
			tutText [tutText.Length - 1].SetActive (false);
		}
	}

	void nextScreen(int idx){
		tutText [idx-1].SetActive (false);
		tutText [idx].SetActive (true);
		index++;
	}
}
