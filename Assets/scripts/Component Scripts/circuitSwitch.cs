using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuitSwitch2 : circuitComponent
{
    //circuitSwtich namesapce already taken. Change in future
   
    public bool isOn;
    

    //Empty constructor
    public circuitSwitch2()
    {
        isOn = true;
        componentType = 6;
    }

    //Full constructor
    public circuitSwitch2(bool initGateOpen, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.isOn = initGateOpen;
        this.componentType = 6;
    }

    //Accessor methods
    public bool getGateStatus()
    {
        return isOn;
    }

    //Mutator methods
    public void switchOn()
    {
        this.isOn = true;
    }

    public void switchOff()
    {
        this.isOn = false;
    }

    //method to perform component function
    public new bool doComponentLogic(double circuitVoltage, double circuitCurrent)
    {
        //check if switch on or off
        if (this.getGateStatus())
            return true;
        else
            return false;
    }
    
    public new bool doComponentLogic()
    {
        //default part works fine
        return true;
    }
}
