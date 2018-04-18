using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linkedList : MonoBehaviour
{
	public componentNode head;
	public componentNode tail;


	public linkedList()
	{
		this.head = null;
		this.tail = null;
	}

	public linkedList(componentNode initHead, componentNode initTail)
	{
		this.head = initHead;
		this.tail = initTail;
	}

	public bool addNodeBefore(componentNode referenceNode)
	{
		componentNode newNode = new componentNode();
		newNode.nextNode[0] = referenceNode;
		newNode.previousNode[0] = referenceNode.previousNode[0];
		referenceNode.previousNode[0] = newNode;

		if (this.head == referenceNode || this.head == null)
		{
			this.head = newNode;
		}
		return true;
	}

	public bool addNodeAfter(componentNode referenceNode)
	{
		componentNode newNode = new componentNode();
		newNode.previousNode[0] = referenceNode;
		newNode.nextNode[0] = referenceNode.nextNode[0];
		referenceNode.nextNode[0] = newNode;

		if (this.tail == referenceNode || this.tail == null)
		{
			this.tail = newNode;
		}
		return true;
	}

	public bool removeNode(componentNode referenceNode)
	{
		componentNode prevNode = null;
		componentNode nexNode = null;

		if (this.head == referenceNode)
		{
			this.head = referenceNode.nextNode[0];
			referenceNode = null;
		}
		else prevNode = referenceNode.previousNode[0];

		if (this.tail == referenceNode)
		{
			this.tail = referenceNode.previousNode[0];
			referenceNode = null;
		}
		else nexNode = referenceNode.nextNode[0];

		//If there's nothing left in the linkedList...
		if (prevNode == null && nexNode == null)
		{
			//Remove the linkedList
			Destroy(this);
		}
		else
		{
			prevNode.nextNode[0] = nexNode;
			nexNode.previousNode[0] = prevNode;
		}
		return true;
	}

	public bool setHead(componentNode newHead)
	{
		componentNode referenceNode = this.head;
		this.head = newHead;
		newHead.nextNode[0] = referenceNode;
		referenceNode.previousNode[0] = newHead;
		return true;
	}

	public bool setTail(componentNode newTail)
	{
		componentNode referenceNode = this.tail;
		this.tail = newTail;
		newTail.previousNode[0] = referenceNode;
		referenceNode.nextNode[0] = newTail;
		return true;
	}

	public componentNode getPseudoTail() //returns the last node if tail is not connected (circuit not complete)
	{
		componentNode pseudoTail = this.head;
		int x = 0;


		if (pseudoTail.getXPos() == -1.0f)
		{
			//print("Calling empty node");
		}

		if(pseudoTail.nextNode.Length == 0 )
		{
			//print("empty array");
		}

		while(pseudoTail.nextNode.Length != 0)
		{
			x++;

			pseudoTail = pseudoTail.nextNode[0];
			//print("Iterating through Linked List! at component#: "+ x);
		}

		if (pseudoTail.getXZ() == head.getXZ())
		{

			//print("THIS IS A HEAD (BATTERY)");
		}

		if (pseudoTail.getXZ() == tail.getXZ())// circuit is complete, from head to tail
		{

			//print("THIS IS THE TAIL");
		}

		return pseudoTail;
	}
	public void addNodeAfterPseudoTail(componentNode node)
	{
		componentNode refrence = this.head; // pointer in linked list

		while (refrence.nextNode.Length != 0)//find last node in list
		{
			refrence = refrence.nextNode[0];
		}

        //set next and previous for nodes
		refrence.nextNode = new componentNode[] { node };
        node.previousNode = new componentNode[] { refrence };      
	}

    public componentNode[] getPositiveEndpoints(componentNode node)
    {
        var result = new componentNode[] { }; // result array
        componentNode refrence = node; // pointer in linked list

        while (refrence.nextNode.Length != 0)//find last node in list
        {
            if (refrence.nextNode.Length > 1)//if fork is found
            {
                for (int x = 0; x < refrence.nextNode.Length - 1; x++)// for every fork
                {
                    refrence = refrence.nextNode[x];//each fork
                    componentNode[] resultAddition = getPositiveEndpoints(refrence);// get endpoints for each fork

                    var list = new List<componentNode>();
                    list.AddRange(result);
                    list.AddRange(resultAddition);

                    result = list.ToArray();
                }
                break;
            }
            refrence = refrence.nextNode[0];// go to next node
        }

        result = new componentNode[] { refrence};
        return result;
    }
    public componentNode[] getNegativeEndpoints(componentNode node)
    {
        var result = new componentNode[] { }; // result array
        componentNode refrence = node; // pointer in linked list

        while (refrence.previousNode.Length != 0)//find last node in list
        {
            if (refrence.previousNode.Length > 1)//if fork is found
            {
                for (int x = 0; x < refrence.previousNode.Length - 1; x++)// for every fork
                {
                    refrence = refrence.previousNode[x];//each fork
                    componentNode[] resultAddition = getNegativeEndpoints(refrence);// get endpoints for each fork

                    var list = new List<componentNode>();
                    list.AddRange(result);
                    list.AddRange(resultAddition);

                    result = list.ToArray();
                }
                break;
            }
            refrence = refrence.previousNode[0];// go to next node
        }

        result = new componentNode[] { refrence };
        return result;
    }

    public bool combineLists()
	{
		return true;
	}
}
