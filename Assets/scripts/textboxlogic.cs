using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class textboxlogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject TextArea, S;
    public Button[] menuitems;
    void Start()
    {
        menuitems = S.GetComponent<tutorialUI>().buttonArray;       
    }
		
    public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.gameObject.GetComponent<Button> () == menuitems [0]) {
			TextArea.GetComponentInChildren<Text> ().text = "A battery stores and supplies charge to the circuit (Multimeter: Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [1]) {
			TextArea.GetComponentInChildren<Text> ().text = "Capacitors help regulate and store charge, they are passive two terminal components and each ahs a certain level of capacitence, or the amount of potential energy they can hold (Multimeter: Microfarads, Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [2]) {
			TextArea.GetComponentInChildren<Text> ().text = "Resistors help regulate charge in a ciruit, maintaing a designated level of resistance to regulate charge as it flows (Multimeter: Resistance, Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [3]) {
			TextArea.GetComponentInChildren<Text> ().text = "Diodes help limit current to flow into only one direction, this is achieved by it having low resistance in one direction, but high resistance in the other (Multimeter: Diode Check, Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [4]) {
			TextArea.GetComponentInChildren<Text> ().text = "Switches allow the current to be restricted or free based on it being on or off, its mechanical component removes or restores the path the current follows (Multimeter: Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [5]) {
			TextArea.GetComponentInChildren<Text> ().text = "Chips are components that can perform a number of actions, such as amplifiers, oscillators, or even computer memory (Multimeter: Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [6]) {
			TextArea.GetComponentInChildren<Text> ().text = "LEDs light up when current runs through them, defined as Light Emitting Diode (Multimeter: Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [7]) {
			TextArea.GetComponentInChildren<Text> ().text = "Transistors are semiconductors umainly used ot amplify or redirect signals and current. IT has three connections, the third mainly being used to connect to extrenal circuits (Multimeter: Voltage)";
			TextArea.SetActive (true);
		} else if (this.gameObject.GetComponent<Button> () == menuitems [8]) {
			TextArea.GetComponentInChildren<Text> ().text = "Wires simply connect two components (Multimeter: Voltage)";
			TextArea.SetActive (true);
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		TextArea.GetComponentInChildren<Text> ().text = "Hover over items for a description";
        //TextArea.SetActive(false);
	}
}