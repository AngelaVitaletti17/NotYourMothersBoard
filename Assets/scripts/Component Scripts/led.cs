using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class led : circuitComponent  // child of diode class?
{
    
    public string color;
    public double minCurrent;
    public double maxCurrent;
    //public int maxReverseVoltage;

    //Empty constructor
    public led()
    {
        color = null;
        minCurrent = 0;
        maxCurrent = 0;
        componentType = 3;

    }

    //Full constructor
    public led(string initColor, int initMinCurrent, int initMaxCurrent, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base( initInputNode, initOutputNode, initIsLocked)
    {
        this.color = initColor;
        this.minCurrent = initMinCurrent;
        this.maxCurrent = initMaxCurrent;
        this.componentType = 3;
    }

    //Accessor methods
    public string getColor()
    {
        return color;
    }

    public double getMinCurrent()
    {
        return minCurrent;
    }

    public double getCurrent()
    {
        return minCurrent;
    }


    //Mutator methods
    public void setColor(string newColor)
    {
        this.color = newColor;
    }

    public void setMinCurrent(double newCurrent)
    {
        this.minCurrent = newCurrent;
    }

    public void setMaxCurrent(double newCurrent)
    {
        this.maxCurrent = newCurrent;
    }

    //method to perform component function
    public new bool doComponentLogic(double circuitVoltage, double circuitCurrent)
    {

        // Check acceptable inputs
        if (this.componentCurrent < this.minCurrent)
        {
            return false;
        }

        if (this.componentCurrent > this.maxCurrent)
        {
            return false;
        }
        return true;
    }
    
    public new bool doComponentLogic()
    {
        //no circuitVoltage or cicuitCurrent to check
        return false;
    }
}
