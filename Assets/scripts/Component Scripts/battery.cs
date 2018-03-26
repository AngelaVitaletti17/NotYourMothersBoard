using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery : circuitComponent
{

    public int voltage;

    //Website to Unity Inheritance 
    //https://unity3d.com/learn/tutorials/topics/scripting/inheritance

    //Empty constructor
    public battery()
    {
        voltage = 0;
        componentType = 1;
    }

    //Full constructor    
    public battery( int initVoltage,  componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.voltage = initVoltage;
        componentType = 1;
    }


    //Accessor methods
    public int getVoltage()
    {
        return voltage;
    }


    //Mutator methods
    public void setVoltage(int newVoltage)
    {
        this.voltage = newVoltage;
    }

    //method to perform component function
    public new bool doComponentLogic(double circuitVoltage, double circuitCurrent)
    {
        //default part works fine
        return true;
    }

    public new bool doComponentLogic()
    {
        //default part works fine
        return true;
    }
}
