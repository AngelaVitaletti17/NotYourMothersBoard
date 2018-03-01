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
    }

    //Full constructor
    public capacitor(int initFarads, int initComponentType, componentNode[] initInputNode, componentNode[] initOutputNode, bool initIsLocked) : base(initComponentType, initInputNode, initOutputNode, initIsLocked)
    {
        this.farads = initFarads;
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
    public new bool doComponentLogic()
    {
        return false;
    }
}
