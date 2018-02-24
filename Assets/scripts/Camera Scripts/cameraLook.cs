using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLook : MonoBehaviour {

	public float clampViewX;
	public float clampViewY;
	public Vector3 initalPosition, newPosition, initialRotation, newRotation;

	private Vector3 currentRotation;
	private bool left, right = false;

	private gridLayout grid;

	void Start () {
		grid = FindObjectOfType<gridLayout> ();
		currentRotation = Vector3.zero;
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = gameObject.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.name == "BreadBoard") {
					hit.transform.gameObject.GetComponent<selectGlow> ().zoomedIn = true;
					hit.transform.gameObject.GetComponent<Collider> ().enabled = false;
					StartCoroutine (zoomIn ());
				} else {
					PlaceCubeNear (hit.point);
				}
			}
		}
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

	IEnumerator zoomIn(){
		while (transform.position != newPosition) {
			transform.position = Vector3.MoveTowards (transform.position, newPosition, 10f * Time.deltaTime);
			if (transform.eulerAngles != newRotation)
				transform.eulerAngles = Vector3.RotateTowards (transform.eulerAngles, newRotation, 10f, 10f);
			yield return null;
		}
		print (transform.position == newPosition);
	}
	void PlaceCubeNear(Vector3 clickPoint){
		Vector3 final = grid.GetNearestPointOnGrid (clickPoint);
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.localScale = cube.transform.localScale * 0.05f;
		cube.transform.position = final;
	}
}
