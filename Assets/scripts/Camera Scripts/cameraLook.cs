using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLook : MonoBehaviour {

	public float clampViewX;
	public float clampViewY;
	public Vector3 initalPosition, newPosition, initialRotation, newRotation;

	private Vector3 currentRotation;

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

	public IEnumerator zoomIn(GameObject bb){
		Vector3 newPos = new Vector3 (bb.transform.position.x, bb.transform.position.y + 0.4f, bb.transform.position.z);
		while (transform.eulerAngles != newRotation) {
			transform.position = Vector3.MoveTowards (transform.position, newPos, 10f * Time.deltaTime);
			transform.eulerAngles = Vector3.RotateTowards (transform.eulerAngles, newRotation, 10f, 260f * Time.deltaTime);
			yield return null;
		}
		GetComponent<Camera> ().orthographic = true;
		GetComponent<Camera> ().orthographicSize = 0.4f;
	}

	public IEnumerator zoomOut(GameObject bb){
		GetComponent<Camera> ().orthographic = false;

		while (transform.eulerAngles != initialRotation) {
			transform.eulerAngles = Vector3.RotateTowards (transform.eulerAngles, initialRotation, 10f, 260 * Time.deltaTime);
			transform.position = Vector3.MoveTowards (transform.position, initalPosition, 50 * Time.deltaTime);
			yield return null;
		}
		//transform.eulerAngles = initialRotation;
		bb.GetComponent<selectGlow> ().zoomedIn = false;
		bb.GetComponent<BoxCollider> ().enabled = true;
	}
}
