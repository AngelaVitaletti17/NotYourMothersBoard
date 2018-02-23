using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectGlow : MonoBehaviour {

	public bool zoomedIn;
	private Color renderColor;

	void Start(){
		renderColor = transform.GetComponent<MeshRenderer> ().material.color;
	}
	void OnMouseEnter(){
		if (!zoomedIn)
			transform.GetComponent<MeshRenderer> ().material.color = Color.gray;
	}
	void OnMouseExit(){
		transform.GetComponent<MeshRenderer> ().material.color = renderColor;
	}
}
