using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potentiometer : circuitComponent
{
    public int ohms;
    public int maxPower;
    

    //Empty constructor
    public potentiometer()
    {
        ohms = 0;
        maxPower = 0;
        componentType = 7;
    }

    //Full constructor
    public potentiometer(int initOhms, int initMaxPower, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.ohms = initOhms;
        this.maxPower = initMaxPower;
        this.componentType = 7;
    }

    //Accessor methods
    public int getOhms()
    {
        return ohms;
    }

    public int getMaxPower()
    {
        return maxPower;
    }


    //Mutator methods
    public void setOhms(int newOhms)
    {
        this.ohms = newOhms;
    }

    public void setMaxPower(int newMaxPower)
    {
        this.maxPower = newMaxPower;
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
