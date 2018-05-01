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
		if (SceneManager.GetActiveScene ().buildIndex == 1) { //The tutorial
			tutText [0].SetActive (true);
			index++;
		}
	}
	
	void Update(){
		if (SceneManager.GetActiveScene().buildIndex == 1){
			if (Input.GetKeyDown ("space") && index < tutText.Length) {
				tutText [index - 1].SetActive (false);
				tutText [index].SetActive (true);
				if (index == 2) {
					StartCoroutine (sc.GetComponent<tutorialUI> ().mainCam.GetComponent<cameraLook> ().zoomIn (sc.GetComponent<tutorialUI> ().breadboard));
					sc.GetComponent<tutorialUI> ().breadboard.GetComponent<selectGlow> ().zoomedIn = true;
				}
				if (index == 7) {
					sc.GetComponent<tutorialUI> ().openPartsCatalogue ();
				}
				if (index == 8) {
					sc.GetComponent<tutorialUI> ().closePartsCatalogue ();
				}
				if (index == 10) {
					sc.GetComponent<tutorialUI> ().meter.GetComponent<multimeter> ().m_zoom ();
				}
				if (index == 13) {
					StartCoroutine(Camera.main.GetComponent<cameraLook> ().zoomIn (sc.GetComponent<tutorialUI> ().breadboard));
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
	}

	void nextScreen(int idx){
		tutText [idx-1].SetActive (false);
		tutText [idx].SetActive (true);
		index++;
	}
}
