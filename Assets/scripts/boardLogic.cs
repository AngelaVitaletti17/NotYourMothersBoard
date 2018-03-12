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
		while (currentNode.nextNode != null && currentNode != endNode)
		{
			if (currentNode.parentComponent.doComponentLogic())
			{
				currentNode = currentNode.nextNode[0];
			}
			else return false;
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

	//For each node's parent component between startNode and endNode, find their Resistance values and return the sum of them all
	public double sumResistance(componentNode startNode, componentNode endNode)
	{
		double resistance = 0.0;
		componentNode currentNode = startNode;

		while (currentNode.nextNode != null && currentNode != endNode)
		{
			//Find value of current node's parent's resistance. Add it to resistance running total
			//resistance += currentNode.getParentComponent.componentResistance;

			currentNode = currentNode.nextNode[0];
		}
		return resistance;
	}

	//For each node between startNode and endNode, find their Resistance values and return the sum of them all
	public double sumVoltage(componentNode startNode, componentNode endNode)
	{
		double voltage = 0.0;
		componentNode currentNode = startNode;

		while (currentNode.nextNode != null && currentNode != endNode)
		{
			//Find value of current node's parent's voltage drop. Subtract it from the voltage running total
			//voltage += currentNode.getParentComponent.componentVoltage;
			currentNode = currentNode.nextNode[0];
		}

		//Find power source's voltage and subtract the running total from it
		if (traceBack(startNode, 99))
		{
			if (getOriginalVoltage(startNode) - voltage < 0.0)
			{
				return 0.0;
			}
			else return getOriginalVoltage(startNode) - voltage;
		}
		else return -1.0;
	}

	public bool traceBack(componentNode referenceNode, int componentType)
	{
		//If this node's parent is a battery, return true
		if (referenceNode.parentComponent.componentType == 99)
		{
			return true;
		}
		//If not, go back a node and check again
		else if (referenceNode.previousNode != null)
		{
			referenceNode = referenceNode.previousNode[0];
		}
		//If there's no battery, return false
		else return false;
	}

	public double getOriginalVoltage(componentNode referenceNode)
	{
		//Make sure a battery is attached
		if (traceBack(referenceNode, 99))
		{
			//If this node's parent is a battery, return its voltage
			if (referenceNode.parentComponent.componentType == 99)
			{
				return referenceNode.parentComponent.componentVoltage;
			}
			//If not, go back a node and try again
			else if (referenceNode.previousNode != null)
			{
				referenceNode = referenceNode.previousNode[0];
			}
		}
		//If there's no battery, return a negative value for voltage
		else return -1.0;
	}
}