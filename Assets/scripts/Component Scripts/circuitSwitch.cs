using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuitSwitch2 : circuitComponent
{
    //circuitSwtich namesapce already taken. Change in future
   
    public bool gateOpen;

    //Empty constructor
    public circuitSwitch2()
    {
        gateOpen = false;
    }

    //Full constructor
    public circuitSwitch2(bool initGateOpen, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.gateOpen = initGateOpen;
    }

    //Accessor methods
    public bool getGateStatus()
    {
        return gateOpen;
    }


    //Mutator methods
    public void setGateOpen()
    {
        this.gateOpen = true;
    }

    public void setGateClosed()
    {
        this.gateOpen = false;
    }

    //method to perform component function
    public new bool doComponentLogic()
    {
        return false;
    }
}
