using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capacitor : circuitComponent
{
    public int farads;

    //Empty constructor
    public capacitor()
    {
        farads = 0;
        componentType = 4;
    }

    //Full constructor
    public capacitor(int initFarads, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initInputNode, initOutputNode, initIsLocked)
    {
        this.farads = initFarads;
        this.componentType = 4;
    }

    //Accessor methods
    public int getFarads()
    {
        return farads;
    }

    //Mutator methods
    public void setFarads(int newFarads)
    {
        this.farads = newFarads;
    }

    //method to perform component function
	public override bool doComponentLogic(double circuitVoltage, double circuitCurrent)
    {
        //default part works fine
        return true;
    }

	public override bool doComponentLogic()
    {
        //default part works fine
        return true;
    }
}
