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
    }

    //Full constructor
    public potentiometer(int initOhms, int initMaxPower, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.ohms = initOhms;
        this.maxPower = initMaxPower;
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
    public new bool doComponentLogic()
    {
        return false;
    }
}
