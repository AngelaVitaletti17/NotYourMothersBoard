using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resistor : circuitComponent
{
    public int ohms;

    //Empty constructor
    public resistor()
    {
        ohms = 0;
    }

    //Full constructor
    public resistor(int initOhms, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.ohms = initOhms;
    }

    //Accessor methods
    public int getOhms()
    {
        return ohms;
    }


    //Mutator methods
    public void setohms(int newOhms)
    {
        this.ohms = newOhms;
    }

    //method to perform component function
    public new bool doComponentLogic()
    {
        return false;
    }
}
