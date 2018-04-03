using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TutorialText : MonoBehaviour {

	public GameObject[] tutText;
	private int index = 0;

	void Start () {
		for (int i = 0; i < tutText.Length; i++)
			tutText [i].SetActive (false);
		tutText [0].SetActive (true);
		index++;
	}
	
	void Update(){
		if (Input.GetKey (KeyCode.Space)) {
			tutText [index - 1].SetActive (false);
			tutText [index].SetActive (true);
			index++;
		}
	}
}
