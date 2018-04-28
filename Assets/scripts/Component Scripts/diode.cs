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
        componentType = 5;
    }

    //Full constructor
    public diode(int initMinVoltage, int initMaxVoltage, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.voltageDrop = initMinVoltage;
        this.maxVoltage = initMaxVoltage;
        this.componentType = 5;
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
    public override bool doComponentLogic(double circuitVoltage, double circuitCurrent)
    {
        // Check acceptable inputs                
        if (this.componentVoltage > this.maxVoltage)
        {
            return false;
        }
        return true;
    }
    
    public override bool doComponentLogic()
    {
        //default part works fine
        return true;
    }
}
