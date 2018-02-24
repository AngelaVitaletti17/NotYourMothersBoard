using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragItem : MonoBehaviour {

	Vector3 itemPosition, mousePosition;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		itemPosition = Camera.main.WorldToScreenPoint (transform.position);
	}

	void OnMouseDrag(){
		Vector3 mouse = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, itemPosition.z);
		mousePosition = Camera.main.ScreenToWorldPoint(mouse);
		transform.position = mousePosition;
	}
}
