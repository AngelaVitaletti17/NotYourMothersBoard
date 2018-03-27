using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectGlow : MonoBehaviour {

	public bool zoomedIn;

	void Start(){
	}
	void OnMouseEnter(){
		if (!zoomedIn)
			transform.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.white);
	}
	void OnMouseExit(){
		transform.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
	}
}
