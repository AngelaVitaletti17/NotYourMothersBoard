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

			if (circuitResistance != -1 && circuitVoltage != -1 && circuitCurrent != -1) {
				//Update values based on results
				return updateComponentValues (startNode, endNode, circuitVoltage, circuitCurrent);
			} else
				return false;
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
		return(traceBack(referenceNode, 1) && traceForward(referenceNode, 1));
	}

	//Update values such as voltage and current throughout a section of the circuit
	//Return true if successful
	public bool updateComponentValues(componentNode startNode, componentNode endNode, double circuitVoltage, double circuitCurrent)
	{
		componentNode currentNode = startNode;
		while (currentNode.getXZ() != endNode.getXZ())
		{
			print ("ITERATING THROUGH LL");
			if (currentNode.nextNode.Length != 0) {
				componentNode nexNode = currentNode.nextNode [0];

				print ("UPDATING COMPONENT VALUES with :" + circuitVoltage + "-" + circuitCurrent);
				print (currentNode.parentComponent.GetInstanceID ());
				currentNode.parentComponent.componentCurrent = circuitCurrent;
				currentNode.parentComponent.componentVoltage = circuitVoltage;

				if (currentNode.parentComponent == nexNode.parentComponent) {
					
					bool result = currentNode.parentComponent.doComponentLogic (circuitVoltage, circuitCurrent);
					if (result == false) {
						print ("COMPONENT NOT WORKING");
						print (currentNode.parentComponent);
						return false;
					}
				}

				currentNode = currentNode.nextNode [0];
			} else {
				print ("NO MORE NODES");
				return false;
			}
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

		while (currentNode.getXZ() != endNode.getXZ())
		{
			if(currentNode.nextNode.Length != 0)
			{
				//If the node's parent component is a resistor...
				if(currentNode.parentComponent.componentType == 2)
				{
					componentNode nexNode = currentNode.nextNode [0];
					if (currentNode.parentComponent == nexNode.parentComponent) {
						//Find value of current node's parent's resistance. Add it to resistance running total
						resistance += currentNode.parentComponent.GetComponent<resistor>().ohms;
					}
				}

				//Move to next node
				currentNode = currentNode.nextNode[0];
			}
			else return -1;
		}
		return resistance;
	}

	//For each node between startNode and endNode, find their Voltage values and return the sum of them all
	public double sumVoltage(componentNode startNode, componentNode endNode)
	{
		double voltage = 0.0;
		componentNode currentNode = startNode;

		while (currentNode.getXZ() != endNode.getXZ())
		{
			if (currentNode.nextNode.Length != 0) {
				componentNode nexNode = currentNode.nextNode [0];
				if (currentNode.parentComponent == nexNode.parentComponent) {
					//Find value of current node's parent's voltage drop. Subtract it from the voltage running total
					voltage += currentNode.parentComponent.componentVoltage;
				}
				currentNode = currentNode.nextNode [0];
			} else
				return -1;
		}

		//Find power source's voltage and subtract the running total from it
		if (traceBack(startNode, 1))
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
		print ("start traceback");	
		//If this node's parent is a battery, return true
		if (referenceNode.parentComponent.componentType == componentType)
		{
			print ("traceback success");
			return true;
		}
		//If not, go back a node and check again
		else if (referenceNode.previousNode.Length != 0)
		{
			print ("next back");
			return traceBack(referenceNode.previousNode[0], componentType);
		}
		//If there's no battery, return false
		else return false;
	}

	public bool traceForward(componentNode referenceNode, int componentType)
	{
		print ("start traceforward");
		//If this node's parent is a battery, return true
		if (referenceNode.parentComponent.componentType == componentType)
		{
			print ("traceforward success");
			return true;
		}
		//If not, go forward a node and check again
		else if (referenceNode.nextNode.Length != 0)
		{
			print ("next forward");
			return traceForward(referenceNode.nextNode[0], componentType);
		}
		//If there's no battery, return false
		else return false;
	}

	public double getOriginalVoltage(componentNode referenceNode)
	{
		//Make sure a battery is attached
		if (traceBack(referenceNode, 1))
		{	
			//If this node's parent is a battery, return its voltage
			if (referenceNode.parentComponent.componentType == 1) {
				return referenceNode.parentComponent.GetComponent<battery>().voltage;
			}
			//If not, go back a node and try again
			else if (referenceNode.previousNode.Length != 0) {
				return getOriginalVoltage(referenceNode.previousNode [0]);
			}
			//If there's no battery, return a negative value for voltage
			else return -1.0;
		}
		//If there's no battery, return a negative value for voltage
		else return -1.0;
	}

	public void checkForPreviousNode(componentNode referenceNode)
	{
		componentNode curNode = new componentNode();
		//Search for potential previous nodes
		//for each componentNode inlist of componentNodes 
		//If xPos are same, or whatever...


		//If thepotential previous node traces back to the battery
		if(traceBack(referenceNode, 1))
		{
			//Set found node as previous node
			referenceNode.previousNode[0] = curNode;
		}
		//If more to right...
		else if (curNode.xPos < referenceNode.xPos){
			//Set node as next node to found node
			curNode.nextNode[0] = referenceNode;
		}
		//If further left...
		else if (curNode.xPos > referenceNode.xPos){
			//Set found node as next node to node
			referenceNode.nextNode[0] = curNode;
		}
	}
}