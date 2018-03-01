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
    }

    //Full constructor
    //Sets Voltage, and then usees Constructor from Circuit Component to set rest of varaibles
    public battery( int initVoltage, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.voltage = initVoltage;
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
    public new bool doComponentLogic()
    {
        return false;
    }
}
