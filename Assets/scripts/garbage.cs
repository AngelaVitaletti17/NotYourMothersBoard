using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class garbage : MonoBehaviour {

	public GameObject sc;
	private GameObject[] hl;

	void Start(){
		
	}

	void OnTriggerEnter(Collider other){
		hl = GameObject.FindGameObjectsWithTag ("highlight");
		//For repair
		if (SceneManager.GetActiveScene().buildIndex == 3 && other.gameObject == sc.GetComponent<tutorialUI>().bad){
			sc.GetComponent<tutorialUI> ().sucRep.SetActive (true);
		}
		for (int i = 0; i < hl.Length; i++)
			Destroy (hl [i]);
		Destroy (other.gameObject);
		sc.GetComponent<tutorialUI> ().isSpawned = false;
		sc.GetComponent<tutorialUI> ().canBePlaced = true;
	}
}
