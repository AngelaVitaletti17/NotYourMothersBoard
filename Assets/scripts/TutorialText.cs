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
		if (Input.GetKeyDown ("space")) {
			tutText [index - 1].SetActive (false);
			tutText [index].SetActive (true);
			if (index == 2) {
				sc.GetComponent<tutorialUI> ().openPartsCatalogue ();
			}
			if (index == 3) {
				sc.GetComponent<tutorialUI> ().closePartsCatalogue ();
			}
			index++;
		}
	}

	void nextScreen(int idx){
		tutText [idx-1].SetActive (false);
		tutText [idx].SetActive (true);
		index++;
	}
}
