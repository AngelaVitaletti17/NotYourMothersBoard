using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery : circuitComponent
{

	public double voltage;

    //Website to Unity Inheritance 
    //https://unity3d.com/learn/tutorials/topics/scripting/inheritance

    //Empty constructor
    public battery()
    {
        voltage = 0f;
        componentType = 1;
    }

    //Full constructor    
    public battery( int initVoltage,  componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.voltage = initVoltage;
        componentType = 1;
    }


    //Accessor methods
	public double getVoltage()
    {
        return voltage;
    }


    //Mutator methods
	public void setVoltage(double newVoltage)
    {
        this.voltage = newVoltage;
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
