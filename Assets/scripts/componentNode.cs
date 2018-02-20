using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	
 	The componentNode class represents a circuit board component's connection point.
	These nodes, when linked together, form a chain of connections. The chains, in 
	turn, form a logical representation of the physical circuit found on the board.
*/

public class componentNode : MonoBehaviour
{

	//Identifies the circuit board component that the componentNode is a part of
	public circuitComponent parentComponent;
	//Location of the componentNode on the board
	public float xPos;
	public float yPos;

	//Identifiers that define the componentNode(s) directly before and after this componentNode
	public componentNode[] previousNode;
	public componentNode[] nextNode;

	//Empty constructor
	public componentNode()
	{
		parentComponent = null;
		xPos = null;
		yPos = null;
		previousNode = null;
		nextNode = null;
	}

	//Full constructor
	public componentNode(circuitComponent initParentComponent, float initXPos, float initYPos, componentNode[] initPreviousNode, componentNode[] initNextNode)
	{
		this.parentComponent = initParentComponent;
		this.xPos = initXPos;
		this.yPos = initYPos;
		this.previousNode = initPreviousNode;
		this.nextNode = initNextNode;
	}

	//Accessor methods
	public string getParentComponent()
	{
		return parentComponent.transform.name;
	}

	public int getXPos()
	{
		return xPos;
	}

	public int getYPos()
	{
		return yPos;
	}

	public Vector2 getXY()
	{
		Vector2 pos = new Vector2 (xPos, yPos);
		return pos;
	}

	public componentNode[] getPreviousNode()
	{
		return previousNode;
	}

	public componentNode[] getNextNode()
	{
		return nextNode;
	}

	//Mutator methods
	public void setXY(float newXPos, float newYPos)
	{
		this.xPos = newXPos;
		this.yPos = newYPos;
	}
	public void setXY(Vector2 newXY)
	{
		this.xPos = newXY.x;
		this.yPos = newXY.y;
	}
	public void setPreviousNode(componentNode[] newPreviousNode)
	{
		this.previousNode = newPreviousNode;
	}
	public void setNextNode(componentNode[] newNextNode)
	{
		this.nextNode = newNextNode;
	}
}
