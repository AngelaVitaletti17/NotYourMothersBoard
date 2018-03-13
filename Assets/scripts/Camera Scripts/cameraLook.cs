using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLook : MonoBehaviour {

	public float clampViewX;
	public float clampViewY;
	public Vector3 initalPosition, newPosition, initialRotation, newRotation;

	private Vector3 currentRotation;
	private bool left, right = false;

	void Start () {
		currentRotation = Vector3.zero;
	}

	void Update(){
		
	}

	void FixedUpdate () {
		currentRotation.x = Input.GetAxis ("Vertical");
		currentRotation.y = Input.GetAxis ("Horizontal");

		if (Input.GetKey (KeyCode.A)) {
				transform.Rotate (currentRotation * 75f * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D) ) {
			currentRotation.x = -1 * currentRotation.x;
			transform.Rotate (new Vector3 (0.25f, transform.rotation.y, transform.rotation.z));
		}
	}

	public IEnumerator zoomIn(){
		while (transform.position != newPosition) {
			transform.position = Vector3.MoveTowards (transform.position, newPosition, 10f * Time.deltaTime);
			if (transform.eulerAngles != newRotation) {
				transform.eulerAngles = Vector3.RotateTowards (transform.eulerAngles, newRotation, 10f, 250f * Time.deltaTime);
				yield return null;
			}
			yield return null;
		}
		GetComponent<Camera> ().orthographic = true;
		GetComponent<Camera> ().orthographicSize = 0.4f;
	}
}
