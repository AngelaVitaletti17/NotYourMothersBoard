using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbage : MonoBehaviour {

	public GameObject sc;
	private GameObject[] hl;

	void Start(){
		
	}

	void OnTriggerEnter(Collider other){
		hl = GameObject.FindGameObjectsWithTag ("highlight");
		for (int i = 0; i < hl.Length; i++)
			Destroy (hl [i]);
		Destroy (other.gameObject);
		sc.GetComponent<tutorialUI> ().isSpawned = false;
		sc.GetComponent<tutorialUI> ().canBePlaced = true;
	}
}
