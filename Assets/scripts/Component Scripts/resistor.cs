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
        componentType = 2;
    }

    //Full constructor
    public resistor(int initOhms,  componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.ohms = initOhms;
        this.componentType = 2;
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
