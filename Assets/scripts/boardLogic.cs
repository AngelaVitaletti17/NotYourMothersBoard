using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	The boardLogic class contains the methods used for modifying or viewing the
	properties of either the board itself or its components on a logical level.

	This includes adding components to the board, updating their values, and
	checking the functionality of circuits.
*/

public class boardLogic : MonoBehaviour
{
	//Update values such as voltage and current throughout a section of the circuit
	//by applying series circuitry logic to the section
	public bool doCircuitLogicSeries(componentNode startNode, componentNode endNode)
	{
		//Make sure the circuit is complete
		if (isCompleteCircuitSeries(startNode))
		{
			//Sum circuit resistance
			double circuitResistance = sumResistance(startNode, endNode);

			//Sum circuit voltage
			double circuitVoltage = sumVoltage(startNode, endNode);

			//Apply Ohm's Law to find circuit current
			double circuitCurrent = getCurrent(circuitVoltage, circuitResistance);

			//Update values based on results
			updateComponentValues (startNode, endNode, circuitVoltage, circuitCurrent);
			return true;
		}
		else return false;
	}

	//Check if this specific component is part of a complete series circuit
	public bool isCompleteCircuitSeries(circuitComponent referenceComponent)
	{
		//placeholder
		return true;
	}

	//Check if this specific node is part of a complete series circuit
	public bool isCompleteCircuitSeries(componentNode referenceNode)
	{
		//placeholder
		return true;
	}

	//Update values such as voltage and current throughout a section of the circuit
	//Return true if successful
	public bool updateComponentValues(componentNode startNode, componentNode endNode, double circuitVoltage, double circuitCurrent)
	{
		componentNode currentNode = startNode;
		while (currentNode.nextNode != null && currentNode != endNode) {
			if (currentNode.parentComponent.doComponentLogic()) {
				//currentNode = currentNode.nextNode;
			} else
				return false;
		}
		return true;
	}

	//Use Ohm's Law to calculate resistance
	public double getResistance(double voltage, double current)
	{
		return voltage / current;
	}

	//Use Ohm's Law to calculate voltage
	public double getVoltage(double current, double resistance)
	{
		return current * resistance;
	}

	//Use Ohm's Law to calculate current
	public double getCurrent(double voltage, double resistance)
	{
		return voltage / resistance;
	}

	public double sumResistance(componentNode startNode, componentNode endNode)
	{
		return 0;
	}

	public double sumVoltage(componentNode startNode, componentNode endNode)
	{
		return 0;
	}
}