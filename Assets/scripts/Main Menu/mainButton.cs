using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainButton : MonoBehaviour {
	public UnityEngine.UI.Button mm;
	// Use this for initialization
	void Start () {

		//Main
		mm.onClick.AddListener(delegate {SceneManager.LoadScene(0);});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
