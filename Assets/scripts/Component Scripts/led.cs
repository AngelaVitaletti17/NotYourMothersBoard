using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class led : circuitComponent  // child of diode class?
{
    
    public string color;
    public int minVoltage;
    public int maxVoltage;
    //public int maxReverseVoltage;

    //Empty constructor
    public led()
    {
        color = null;
        minVoltage = 0;
        maxVoltage = 0;

    }

    //Full constructor
    public led(string initColor, int initMinVoltage, int initMaxVoltage, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.color = initColor;
        this.minVoltage = initMinVoltage;
        this.maxVoltage = initMaxVoltage;
    }

    //Accessor methods
    public string getColor()
    {
        return color;
    }

    public int getMinVoltage()
    {
        return minVoltage;
    }

    public int getVoltage()
    {
        return minVoltage;
    }


    //Mutator methods
    public void setColor(string newColor)
    {
        this.color = newColor;
    }

    public void setMinVoltage(int newVoltage)
    {
        this.minVoltage = newVoltage;
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
