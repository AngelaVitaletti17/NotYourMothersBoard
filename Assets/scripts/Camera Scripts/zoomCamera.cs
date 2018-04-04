using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomCamera : MonoBehaviour {

	public Camera sCam;
	private Vector3 sCamOrigin;

	void Start () {
		sCamOrigin = sCam.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver(){
		sCam.gameObject.SetActive (true);
		sCam.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseExit(){
		sCam.transform.position = sCamOrigin;
		sCam.gameObject.SetActive (false);
	}
}
