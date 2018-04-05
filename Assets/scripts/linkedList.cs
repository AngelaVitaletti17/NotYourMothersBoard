﻿using System.Collections;
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
       
        if (pseudoTail == null)
        {
            print("Calling empty list");
            return null;
        }
        while(pseudoTail.nextNode[0] != null)
        {
            pseudoTail = pseudoTail.nextNode[0];
        }
        if (pseudoTail == this.tail)// circuit is complete, from head to tail
        {
            return null;
        }
        return pseudoTail;
    }
	public bool combineLists()
	{
		return true;
	}
}
