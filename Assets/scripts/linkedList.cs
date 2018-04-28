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
			print("Calling empty node");
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

			print("THIS IS A HEAD (BATTERY)");
		}

		if (pseudoTail.getXZ() == tail.getXZ())// circuit is complete, from head to tail
		{

			print("THIS IS THE TAIL");
		}

		return pseudoTail;
	}
	public void addNodeAfterPseudoTail(componentNode node)
	{
		componentNode refrence = this.head;
		 while (refrence.nextNode.Length != 0)
		{
			refrence = refrence.nextNode[0];
		}
		refrence.nextNode = new componentNode[] { node };

	}

	public bool combineLists()
	{
		return true;
	}

	public void printList()
	{
		componentNode refrence = this.head;

		do {
			print (refrence.parentComponent);
			if (refrence.getXPos () != this.tail.getXPos ())
			{
				if (refrence.nextNode.Length != 0)
				{
					refrence = refrence.nextNode [0];
				}
				else break;
			}
			else
			{
				if (refrence.nextNode.Length != 0)
				{
					refrence = refrence.nextNode [0];
					print (refrence.parentComponent);
					break;
				}
				else break;
			}
		} while(true);
		//while((refrence.getXPos () != this.tail.getXPos ()) && (refrence.getYPos () != this.tail.getYPos ()));
	}
}
