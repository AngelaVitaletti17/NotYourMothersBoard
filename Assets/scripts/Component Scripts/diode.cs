using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diode : circuitComponent
{
    public int voltageDrop;
    public int maxVoltage;
    //public int maxReverseVoltage;

    //Empty constructor
    public diode()
    {
        voltageDrop = 0;
        maxVoltage = 0;
    }

    //Full constructor
    public diode(int initMinVoltage, int initMaxVoltage, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.voltageDrop = initMinVoltage;
        this.maxVoltage = initMaxVoltage;
    }

    //Accessor methods
    
    public int getMinVoltage()
    {
        return voltageDrop;
    }

    public int getVoltage()
    {
        return voltageDrop;
    }


    //Mutator methods
   
    public void setVoltageDrop(int newVoltage)
    {
        this.voltageDrop = newVoltage;
    }

    public void setMaxVoltage(int newVoltage)
    {
        this.maxVoltage = newVoltage;
    }

    //method to perform component function
    public new bool doComponentLogic()
    {
        return false;
    }


}
