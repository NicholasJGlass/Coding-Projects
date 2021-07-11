/*
 * Created by SharpDevelop.
 * User: Nick
 * Date: 7/1/2021
 * Time: 1:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace Sorting_Visualization_Tool
{
	

	
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Timer t = new Timer();
		public  int pointer1 = 0;
		public  int pointer2 = 1;
		public  int pivot = 1;
		public  int maxNumber = 0;
		public  int digits;
		public  int currentDigit = 1;
		public  int[] array = new int[2];
		public  int[] drawArray = new int[2];
		public  int[] countArray = new int[2];
		public  bool didSwap = false;
		public  Stack stack = new Stack();
		public	int[] tempArray;
		public  int tempPointer = 0;
		public  int lowRange = 0;
		public	int partSize = 1;
		public  String status = "MaxNumber";
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			t.Interval = trackBar1.Value * 10;
			t.Tick += new System.EventHandler(t_Tick);

			
		}

		void StartClick(object sender, EventArgs e)
		{
				
					Start.Enabled = false;
					SortingAlgorithm.Enabled = false;
					ArraySizeTextBox.Enabled = false;
					GenerateArray.Enabled = false;
					t.Start();

			
		}
		
		void Reset()
		{
					pictureBox.Invalidate();
					pointer1 = 0;
					pointer2 = 1;
					partSize = 1;
					pivot = array.Length - 1; 
					didSwap = false;
					ArraySizeTextBox.Enabled = true;
					SortingAlgorithm.Enabled = true;
					Start.Enabled = true;
					GenerateArray.Enabled = true;
					stack.Push(0);
					stack.Push(0);
					tempPointer = 0;
					Start.Enabled = false;
					status = "MaxNumber";
					t.Stop();
					
		}
		
			//generate a random array
		void GenerateArrayClick(object sender, EventArgs e)
		{
			
			//reset array size error label
			ArraySizeError.Visible = false;
			
			//make sure stack is empty
			stack = new Stack();
			
			//getting array size
			int arraysize = 10;
			
			if(int.TryParse(ArraySizeTextBox.Text,out arraysize))
			{
				arraysize = int.Parse(ArraySizeTextBox.Text);
			}
			else
			{
				//parse failed - use default of 10
				arraysize = 10;
				ArraySizeTextBox.Text = "10";
				ArraySizeError.Text = "Array Size Error: Using Default of 10";
				ArraySizeError.Visible = true;
			}
			
				//bounds the array size between 10 and 100
			if (arraysize < 10)
			{
				arraysize = 10;
				ArraySizeTextBox.Text = "10";
				ArraySizeError.Text = "Array Size Too Low: Using Min of 10";
				ArraySizeError.Visible = true;
			}
			else if (arraysize > 100)
			{
				arraysize = 100;
				ArraySizeTextBox.Text = "100";
				ArraySizeError.Text = "Array Size Too High: Using Max of 100";
				ArraySizeError.Visible = true;
			}
			
			//initializing array with desired size			
			int[] arr = new int[arraysize];
			
			//generate array with random variables 10 to 100 (can be repeated values)
			
			
			var rand = new Random();
			
			for (int i = 0; i <= arr.Length - 1; i++)
			{
				arr[i] = rand.Next(1,300);
			}
			
		
			array = arr;
			
			drawArray = new int[arr.Length];
			for (int i = 0; i <= arr.Length - 1; i++)
			{
				drawArray[i] = array[i];
			}                    
			                    
			tempArray = new int[arr.Length];
			countArray = new int[10]; //10 numbers a digit can be
			for (int i = 0; i < 10; i++)
			{
				//set count for all number of digit to 0
				countArray[i] = 0;
			}
			pivot = arr.Length - 1; 
			stack.Push(0);
			stack.Push(0);
			Start.Enabled = true;
			pictureBox.Invalidate();
			

			
		}
		
		//uses the clock to repeated call selected sort method untill array is sorted
		private void t_Tick(object sender, EventArgs e)
		{
			t.Interval = trackBar1.Value * 10;
			
			switch (SortingAlgorithm.SelectedItem.ToString())
			{
				case "Bubble Sort":
					BubbleSort();
					break;
					
				case "Quick Sort":
					QuickSort();
					break;
					
				case "Merge Sort":
					MergeSort();
					break;
					
				case "Radix Sort":
					RadixSort();
					break;
					
			}
		}
		
		//draws the array in its current state
		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			int[] arr = drawArray;
			
			Pen redPen = new Pen(Color.Red);
			Pen greenPen = new Pen(Color.Green);
			Pen yellowPen = new Pen(Color.Yellow);
			Graphics drawIt = e.Graphics;
			
			//clears canvas for updated array
			drawIt.Clear(Color.Gray);
			
			//draws the array
			for (int i = 0; i<=arr.Length - 1; i++)
			{
				drawIt.DrawRectangle(redPen, (500 - (9 * arr.Length / 2) + (9 *i)), 300 - (arr[i] * 1), 5, arr[i] * 1); //*3 for more drastic variations in hieght, easier to see
			}
			
			//draws over the rectangles where the pointers are looking at
			drawIt.DrawRectangle(yellowPen, (500 - (9 * arr.Length / 2) + (9 *pivot)), 300 - (arr[pivot] * 1), 5, arr[pivot] * 1); //pivot draw first to get drawn over if no pivot needed
			drawIt.DrawRectangle(greenPen, (500 - (9 * arr.Length / 2) + (9 *pointer1)), 300 - (arr[pointer1] * 1), 5, arr[pointer1] * 1);
			drawIt.DrawRectangle(greenPen, (500 - (9 * arr.Length / 2) + (9 *pointer2)), 300 - (arr[pointer2] * 1), 5, arr[pointer2] * 1);
			
				
			redPen.Dispose();
			greenPen.Dispose();
			yellowPen.Dispose();
		}	
		
		private void BubbleSort()
		{
			int[] arr = array;
			drawArray = array;
			int temp;
			
			//if first element is larger than second than swap
			if (arr[pointer1] > arr[pointer2])
			{
				temp = arr[pointer1];
				arr[pointer1] = arr[pointer2];
				arr[pointer2] = temp;
				
				//set didSwap flag: another pass will be required
				didSwap = true;
			}
			

			pictureBox.Invalidate();
			
			//check if that was the end of a pass
			if (pointer2 == arr.Length - 1)
			{
				//pointer2 at end of array: means end of pass
				if (didSwap == true)
				{
					//another pass is required
					pointer1 = -1; //-1 and 0 for pointers because of the upcoming ++ setting pointers to 0 and 1
					pointer2 = 0;
					didSwap = false; //reset swap flag for next pass
				}
				else
				{
					//no swap on a pass means the array is sorted: end sorting
					Reset();
					return;
				}
			}

			//draw updated array
	
			pointer1++;
			pointer2++;
			pivot = pointer1;
			
			array = arr;

		}
		
		//quick sort pivot will be the right most element
		private void QuickSort()
		{
			int[] arr = array;
			drawArray = array;
			int temp = 0;
			
			pictureBox.Invalidate();
			
			if(pivot == pointer1)
			{
				Reset();
				return;
			}
			
			//check if array/sub array is done being partitioned
			if (pivot == pointer2)
			{
				//check is sorting is finished
				if(stack.Count == 0)
				{
					Reset();
					return;
				}
				
				//swap the pivot with pointer1 a.k.a the middle of the partition
				if(arr[pivot] <= arr[pointer1])
				{
					temp = arr[pivot];
					arr[pivot] = arr[pointer1];
					arr[pointer1] = temp;
				}
				else
				{
					pointer1++; //the only time this occurs is if the pivot is the largest value. If no swap occurs and pointer1 is not advanced by one then the element to the left of
								//of the pivot will not be loading into the stack to be compared and sorted
				}
				
				//push left sub array ranges onto stack (pushing low range first and then the high range)
				stack.Push(lowRange);
				
				if(pointer1 - 1 >= lowRange)
				{
				stack.Push(pointer1 - 1); //high range will be the element to the left of the new pivot location (center of partition)
				}
				else
				{
					stack.Push(lowRange);
				}
				
				//push right sub array rangers onto stack
				
				if (pointer1 + 1 <= pivot)
				{
				stack.Push(pointer1 + 1); //low range will be the element to the right of the new pivot location (center of partition)
				}
				else 
				{
					stack.Push(pivot);
				}
				stack.Push(pivot); //high range will be the element of the old pivot location (right more element in sub array)

				
				//when pointer 2 == pivot, the array/sub array has been partitioned: need to get new sub array
				
				do //repeat pulling off sub arrays until a sub array with size greater than 1 is found
				{
				pivot = (int)stack.Pop();  //high range a.k.a right most element
				pointer1 = (int)stack.Pop(); //low range a.k.a left most element
				
				if(pointer1 < arr.Length - 1) //prevents the issue if pointer1 become the right most element then pointer 2 wont become right most element +1
				{
					pointer2 = pointer1 + 1;    //pointer 2 will be used for comparing and swaping with the low range
				}
				else
				{
					pointer2 = pointer1;
				}
				
				lowRange = pointer1;		//must keep track of the left most element of sub array
				//then check if sorting is completed while popping off sub arrays
				if (stack.Count == 0)
				{
					return;
				}
				
				}while(pivot - pointer1 < 1);
	
				pictureBox.Invalidate();
				return;
			}
			

			//begin partitioning array/sub array
			if(arr[pointer2] < arr[pivot])
			{
				//if pointer2 (comparison pointer) is smaller than pivot(high range) a swap occurs (smaller values than the pivot need to go to left of partition)
				temp = arr[pointer2];
				arr[pointer2] = arr[pointer1];
				arr[pointer1] = temp;
				
				pointer1++;
				pointer2++;
			}
			else if(arr[pivot] >= arr[pointer1])
			{
				
				//if pivot is larger than the low range then the low range needs to be slowly moved up until it finds a value that is larger than the pivot
				pointer1++;
				pointer2++;

			}
			else if (arr[pointer2] >= arr[pivot])
			{
				//if pointer2 is larger than the pivot no swap occurs: advance pointer 2  (values largers than the pivot stay to the right of the partition)
				pointer2++;
			}
			
			array = arr;
			
		}
		
		private void MergeSort()
		{
			pictureBox.Invalidate();
			
			if (Math.Ceiling((double)array.Length / (double)partSize) <=2)
			{
				//only 2 parts left to be merged and then sort is done
				if (array[pointer1] <= array[pointer2])
				{
					tempArray[tempPointer] = array[pointer1];
					tempPointer++;
					pointer1++;
					pivot = pointer1;
				}
				else
				{
					tempArray[tempPointer] = array[pointer2];
					tempPointer++;
					pointer2++;
				}
				
				//check if either part is empty
				if ((pointer1) % partSize == 0)
				{
					//if part 1 is empty, add the rest of part 2
					do
					{
						tempArray[tempPointer] = array[pointer2];
						tempPointer++;
						pointer2++;
					}
					while (pointer2 <= array.Length - 1);
					//array should be sorted
					
					for (int i = 0; i <= array.Length - 1; i++)
					{
						array[i] = tempArray[i];
						drawArray[i] = tempArray[i];
					}
					Reset();
					return;
				}
				
				if (pointer2 > array.Length - 1)
				{
					//if part 2 is empty, add the rest of part 1
					do
					{
						tempArray[tempPointer] = array[pointer1];
						tempPointer++;
						pointer1++;
					}
					while ((pointer1) % partSize != 0);
					//array should be sorted
					
					for (int i = 0; i <= array.Length - 1; i++)
					{
						array[i] = tempArray[i];
						drawArray[i] = tempArray[i];
					}
					Reset();
					return;
				}
				for (int i = 0; i <= array.Length - 1; i++)
				{
					drawArray[i] = tempArray[i];
				}
				return;
			}
				
			

			if (array[pointer1] <= array[pointer2])
			{
				if ((pointer1 + 1) % partSize == 0) 
				{
					//pointer has reach end of part size: add the final element in part size
					tempArray[tempPointer] = array[pointer1];
					tempPointer++;
					pointer1++;
					//then add the rest of the numbers from the other part to the temp array
				
					do
					{
						tempArray[tempPointer] = array[pointer2];
						tempPointer++;
						pointer2++;
					}
					while ((pointer2) % partSize != 0  && pointer2 != array.Length);
				
					//both parts should now be added to the temp array
					//both pointers need to move to the next part
					pointer1 += partSize;
					pointer2 += partSize;
				
					if (pointer2 > array.Length - 1)
					{
						//pointer2 has left bounds of array: there can not be two parts to merge
						if (pointer1 > array.Length - 1)
						{
							//pointer1 has also left bounds of array: there are no more parts
							//reset pointers and increase part size
							partSize = partSize * 2;
							pointer1 = 0;
							pointer2 = partSize;
							tempPointer = 0;
							
							for (int i = 0; i <= array.Length - 1; i++)
							{
								array[i] = tempArray[i];
								drawArray[i] = tempArray[i];
							}
							pivot = pointer1;
							return;
						}
						else
						{
							//pointer 1 is not out of bounds: tack final part onto the end of temp array
							do
							{
								tempArray[tempPointer] = array[pointer1];
								pointer1++;
								tempPointer++;
							}
							while (pointer1 < array.Length - 1);
							
							partSize = partSize * 2;
							pointer1 = 0;
							pointer2 = partSize;
							tempPointer = 0;
							
							for (int i = 0; i <= array.Length - 1; i++)
							{
								array[i] = tempArray[i];
								drawArray[i] = tempArray[i];
							}
							pivot = pointer1;
							return;
						}
						
					}
					pivot = pointer1;
					for (int i = 0; i < tempPointer; i++)
					{
						drawArray[i] = tempArray[i];
					}
					return;
					
				}
				else
				{
					//else, pointer has not reached end and simple add current element to temp array
					tempArray[tempPointer] = array[pointer1];
					tempPointer++;
					pointer1++;
					pivot = pointer1;
					for (int i = 0; i < tempPointer; i++)
					{
								drawArray[i] = tempArray[i];
					}
					return;
				}
				
				
			}
			
			if (array[pointer2] < array[pointer1])
			{
				if ((pointer2 + 1) % partSize == 0 || pointer2 + 1 == array.Length) 
				{
					//pointer has reach end of part size: add the final element in part size
					tempArray[tempPointer] = array[pointer2];
					tempPointer++;
					pointer2++;
					//then add the rest of the numbers from the other part to the temp array
				
					do
					{
						tempArray[tempPointer] = array[pointer1];
						tempPointer++;
						pointer1++;
					}
					while ((pointer1) % partSize != 0);
				
					//both parts should now be merged to the temp array
					//both pointers need to move to the next part
					pointer1 += partSize;
					pointer2 += partSize;
				
					if (pointer2 > array.Length - 1)
					{
						//pointer2 has left bounds of array: there can not be two parts to merge
						if (pointer1 > array.Length - 1)
						{
							//pointer1 has also left bounds of array: there are no more parts
							//reset pointers and increase part size
							partSize = partSize * 2;
							pointer1 = 0;
							pointer2 = partSize;
							tempPointer = 0;
							
							for (int i = 0; i <= array.Length - 1; i++)
							{
								array[i] = tempArray[i];
								drawArray[i] = tempArray[i];
							}
							pivot = pointer1;
							return;
						}
						else
						{
							//pointer 1 is not out of bounds: tack final part onto the end of temp array
							do
							{
								tempArray[tempPointer] = array[pointer1];
								pointer1++;
								tempPointer++;
							}
							while (pointer1 < array.Length - 1);
							
							partSize = partSize * 2;
							pointer1 = 0;
							pointer2 = partSize;
							tempPointer = 0;
							
							for (int i = 0; i <= array.Length - 1; i++)
							{
								array[i] = tempArray[i];
								drawArray[i] = tempArray[i];
							}
							pivot = pointer1;
							return;
						}
					}
					pivot = pointer1;
					for (int i = 0; i < tempPointer; i++)
					{
						drawArray[i] = tempArray[i];
					}
					return;
					
				}
				else
				{
					//else, pointer has not reached end of part size and simply add current element to temp array
					tempArray[tempPointer] = array[pointer2];
					tempPointer++;
					pointer2++;
					pivot = pointer1;
					for (int i = 0; i < tempPointer; i++)
					{
						drawArray[i] = tempArray[i];
					}
					return;
				}
				
				
			}
			
		}
		
		private void RadixSort()
		{
			//Raidx Sort is broken into four componenets
			//the first component gets the largest number to count number of digits needed
			//the second component gets the count array at a specific digit
			//the third componenet is to modify the count array
			//the forth component sorts the array at the specific digit
			
			drawArray = array; //for drawing changes during sort
			
			pictureBox.Invalidate();	
			switch (status) 
			{
				case "MaxNumber":
					MaxNumber();
					break;
					
				case "CountArray":
					CountArray();
					break;
					
				case "ModifyArray":
					ModifyArray();
					break;
					
				case "Sort":
					Sort();
					break;
			}
		}
		
		private void MaxNumber()
		{
			if (array[pointer1] > maxNumber)
			{
				maxNumber = array[pointer1];
			}
			
			pointer1++;
	
			if (pointer1 >= array.Length)
			{
				//array has been fully searched
				//reset pointer1; get numbers of digits; change status
				pointer1 = 0;
				digits = (int)Math.Floor(Math.Log10(maxNumber) + 1);
				status = "CountArray";
			}
			
							
			pointer2 = pointer1;
			pivot = pointer1;
		
		}
		
		private void CountArray()
		{
			//search through array and count the number of times a number shows up in the spefic digit
			// for example:    23, 53, 2, 16, 95, 88
			// 		 index:    0, 1, 2, 3, 4, 5, 6, 7, 8, 9
			// count array:    0, 0, 1, 2, 0, 1, 1, 0, 1, 0
			

			countArray[(int)(array[pointer1] / (Math.Pow(10,(currentDigit - 1)))) % 10]++; // dividing by 10^currentDigit chops off digits before mod 10 returns the one place digit
																				  		  // for example:  42859  if the current digit is 3
																				  		  //               42859 / (10^(3-1) = 42859 / 100 = 428
																				  		  //the mod 10 returns 8...which is the 3 digit

			
			//iterate through the array
			pointer1++;
			
			if (pointer1 >= array.Length)
			{
				//array has been fully searched
				//reset pointer1; change status
				pointer1 = array.Length - 1; //next pass will be in reverse
				status = "ModifyArray";
			}
			
			pointer2 = pointer1;
			pivot = pointer1;
		
		}
		
		private void ModifyArray()
		{
			//modify the array by setting each element to sum its count with the summed count in the previos element
			for (int i = 1; i < countArray.Length; i++) //start at index 1 because 0 has no previous element
			{
				countArray[i] += countArray[i-1];
			}
			
			//array should be fully modified
			//fill tempArray with copy of array to allow array to be drawn while replaced.
			
			for (int i = 0; i < array.Length; i++)
			{
				tempArray[i] = array[i];
			}
			
			//change status
			status = "Sort";
		}
		
		private void Sort()
		{
			//iterate through search temparray and place element in position in the array based on the place in the modified count array
			//                   |--------------------gets specific digit----------------------|
			array[ countArray[   ((int)(tempArray[pointer1] / (Math.Pow(10,(currentDigit - 1)))) % 10)      ] - 1] = tempArray[pointer1];

			
			//decrease count in count array of found number
			countArray[   (int)(tempArray[pointer1] / (Math.Pow(10,(currentDigit - 1)))) % 10  ]--;
			//iterate through array
			pointer1--;
			pointer2 = pointer1;
			pivot = pointer1;
			
			if (pointer1 < 0)
			{
				//sorting at this digit is complete
				if (currentDigit >= digits)
				{
					//all digits have been sorted; sorting is complete
					Reset();
					return;
				}
				
				//reset for next pass
				pointer1 = 0;
				pointer2 = pointer1;
				pivot = pointer1;
				currentDigit++;
				status = "CountArray";
				
				//reset count array to 0's
				for (int i=0; i < countArray.Length; i++)
				{
					countArray[i] = 0;
				}
			}
			
		}

	}
}
