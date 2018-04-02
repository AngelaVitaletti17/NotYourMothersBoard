using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	public Button play, quit, tut, cre, rep;
	public GameObject selection;
	void Start () {
		//Initialize the play button
		play.onClick.AddListener (delegate {
			selection.SetActive (true);
		});
		//Initialize the quit button
		quit.onClick.AddListener(Application.Quit); //Exit the game

		//Initalize the tutorial level button
		tut.onClick.AddListener(delegate {SceneManager.LoadScene(1);});
	}
}
