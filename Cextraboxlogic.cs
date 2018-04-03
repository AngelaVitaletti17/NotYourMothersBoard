using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class textboxlogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject TextArea, S;
    public Button[] menuitems;
    void Start()
    {
        menuitems = S.GetComponent<tutorialUI>().buttonArray;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (this.gameObject.GetComponent<Button>() == menuitems[0])
        {
            TextArea.GetComponentInChildren<Text>().text = "a battery stores and supplies charge to the circuit";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[1])
        {
            TextArea.GetComponentInChildren<Text>().text = "capacitors help regulate and store charge";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[2])
        {
            TextArea.GetComponentInChildren<Text>().text = "resistors help regulate charge in a ciruit";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[3])
        {
            TextArea.GetComponentInChildren<Text>().text = "diodes help limit current to flow into only one direction";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[4])
        {
            TextArea.GetComponentInChildren<Text>().text = "switches allow the current to be restricted or free based on it being on or off";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[5])
        {
            TextArea.GetComponentInChildren<Text>().text = "chips run specific actions for the circuit";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[6])
        {
            TextArea.GetComponentInChildren<Text>().text = "LEDs light up when current runs through them";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[7])
        {
            TextArea.GetComponentInChildren<Text>().text = "transistors help regulate flow and provide mroe directions for current to follow";
            TextArea.SetActive(true);
        }
        else if (this.gameObject.GetComponent<Button>() == menuitems[8])
        {
            TextArea.GetComponentInChildren<Text>().text = "wires simply connect two components";
            TextArea.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TextArea.SetActive(false);
    }
}