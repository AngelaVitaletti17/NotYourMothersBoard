using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loading : MonoBehaviour {
	// Update is called once per frame
	void FixedUpdate () {
		this.GetComponent<RectTransform> ().Rotate (0f, 0f, 350 * Time.deltaTime);
	}
}
