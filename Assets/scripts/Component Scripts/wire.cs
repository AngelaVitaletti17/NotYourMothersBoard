using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wire : circuitComponent {


	//Empty constructor
	public wire()
	{
		
		componentType = 99;
	}

	//Full constructor    
	public wire( componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
	{
		componentType = 99;
	}
		

	//method to perform component function
	public override bool doComponentLogic(double circuitVoltage, double circuitCurrent)
	{
		//default part works fine
		return true;
	}

	public override bool doComponentLogic()
	{
		//default part works fine
		return true;
	}
}
