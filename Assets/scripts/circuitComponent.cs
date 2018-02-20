using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuitComponent : MonoBehaviour
{
	public int componentType;
	public componentNode[] inputNode;
	public componentNode[] outputNode;
	public double componentCurrent;
	public double componentVoltage;
	public bool isLocked;

	//Empty constructor
	circuitComponent()
	{
		componentType = 0;
		inputNode = null;
		outputNode = null;
		componentCurrent = 0.00;
		componentVoltage = 0.00;
		isLocked = true;
	}

	//Full constructor
	circuitComponent(int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked)
	{
		componentType = initComponentType;
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
