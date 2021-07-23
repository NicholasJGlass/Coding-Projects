/*
 * Created by SharpDevelop.
 * User: Nick
 * Date: 7/20/2021
 * Time: 3:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Path_Finding_Visualization_Tool
{
	/// <summary>
	/// Description of PriorityQueue.
	/// </summary>
	public class PriorityQueueMin<T>
	{
		class Node
		{
			public int Priority {get; set;}
			public T Value {get; set;}
		}

		//queue to hold values given
		List<Node> queue = new List<Node>();
		public int Count {get{return queue.Count;}}
		
		public void Enqueue(int priority, T obj)
		{
			//add new node at end and then call MinHeapifyInsertion
			Node temp = new Node() {Priority = priority, Value = obj};
			queue.Add(temp);
			MinHeapifyInsertion(queue.Count - 1);
		}
		
		public T Dequeue()
		{
			//return the top of the heap
			var value = queue[0].Value;
			
			//swap top of heap with last node in heap then remove last node
			Swap(0,queue.Count - 1);
			queue.RemoveAt(queue.Count - 1);
			
			//call MinHeapifyDeletion to swap down the new top node (which pulls up next min to top of heap)
			MinHeapifyDeletion(0);
			
			
			return value;
		}
		
		//after an insertion, the inserted node is added to the end of the the heap
		//the new node needs to be swapped up until its parent is no longer larger than it
		private void MinHeapifyInsertion(int child)
		{
			int parent = child / 2;
			
			if (parent >= 0 && queue[child].Priority < queue[parent].Priority)
			{
				//if the child is less than the parent, swap and check parent's parent
				Swap(child, parent);
				MinHeapifyInsertion(parent);
			}
		}
		
		//after a deletion, the first node(smallest) is swapped with the last node in the heap
		//the first node is not longer the lowest and needs to be swapped down untill it is smaller than both children
		private void MinHeapifyDeletion (int parent)
		{	
			int leftChild = parent * 2 + 1;
			int rightChild = parent * 2 + 2;
			
			if (leftChild <= queue.Count - 1 && queue[leftChild].Priority < queue[parent].Priority)
			{
				//if the left child is less than the parent then they need to be swaped and left child's children must be checked
			 	Swap(parent, leftChild);
			    MinHeapifyDeletion(leftChild);
			}
			if (rightChild <= queue.Count - 1 && queue[rightChild].Priority < queue[parent].Priority)
			{
				//if the right child is less than the parent then they need to be swaped and right child's children must be checked
				Swap(parent, rightChild);
				MinHeapifyDeletion(rightChild);
			}
		}
		
		
		//swaps two nodes
		private void Swap(int i, int j)
		{
			var temp = queue[i];
			queue[i] = queue[j];
			queue[j] = temp;
		}
		

	}
}
