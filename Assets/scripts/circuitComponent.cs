using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuitComponent : MonoBehaviour
{
	public int componentType; // 0-NULL 1-battery 2-resistor 3-led 4-capacitor 5-diode 6-circuitSwitch 7-potentiometer
	public componentNode[] inputNode;
	public componentNode[] outputNode;
	public double componentCurrent;
	public double componentVoltage;
	public double componentResistance;
	public bool isLocked;

	//Empty constructor
    public circuitComponent()
	{
		componentType = 0;
		inputNode = null;
		outputNode = null;
		componentCurrent = 0.00;
		componentVoltage = 0.00;
		isLocked = true;
	}

	//Full constructor
	public circuitComponent(componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked)
	{
		componentType = 0;
		inputNode = initInputNode;
		outputNode = initOutputNode;
		isLocked = initIsLocked;
		componentCurrent = 0.00;
		componentVoltage = 0.00;
	}

	//Inherited method to perform component function. If not overridden by specific component,
	//assume an error and return false
	public bool doComponentLogic()
	{
		return false;
	}
}
