using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	public Button play, quit, tut, cre, rep;
	public GameObject selection;
	public Image load;
	void Start () {
		//Hide the loading thingy
		load.gameObject.SetActive(false);

		//Initialize the play button
		play.onClick.AddListener (delegate {
			selection.SetActive (true);
		});

		//Initialize the quit button
		quit.onClick.AddListener(Application.Quit); //Exit the game

		//Initalize the tutorial level button
		tut.onClick.AddListener(delegate {ShowLoadAndLoad(1);});

		//Creation
		cre.onClick.AddListener(delegate {ShowLoadAndLoad(2);});

		//Repair
		rep.onClick.AddListener(delegate {ShowLoadAndLoad(3);});
	}

	void ShowLoadAndLoad(int scene){
		load.gameObject.SetActive(true);
		SceneManager.LoadScene(scene);
	}
}
