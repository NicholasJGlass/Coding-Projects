/*
 * Created by SharpDevelop.
 * User: Nick
 * Date: 7/9/2021
 * Time: 2:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Path_Finding_Visualization_Tool
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	/// 
	
		struct Location
		{
			public int X;
			public int Y;
		}
		
		struct Box
		{
			public bool isWall;
			public bool visited;
			public bool isStack;
			public bool start;
			public bool end;
			public bool path;
			public bool isQueued;
			public int predX;
			public int predY;
			public int distance; //sets current distance cell is found out: prevents researching a cell if it can be reached in less distance
			public double DtoEnd; //distance of cell to end location
		}

			
	
	public partial class MainForm : Form
	{
		
	
		Box[,] grid = new Box[120,63];
		Location location = new Location();
		Location startLocation = new Location();
		Location endLocation = new Location();
		Location tempLocation = new Location();
		Location currentLocation = new Location();
		Location above = new Location();
		Location below = new Location();
		Location left = new Location();
		Location right = new Location();
		public bool pathFind = false;
		public bool locked = false;
		public string clickFlag = "";
	    public int minDistance;
	    public int currentDistance = 0;
	    public bool[] check = new bool[4];
		Queue<Location> Q = new Queue<Location>();
		Stack<Location> S = new Stack<Location>();
		Stack<Location> TempS = new Stack<Location>();
		PriorityQueueMin<Location> PQ = new PriorityQueueMin<Location>();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			canvas.Invalidate();
			timer.Interval = trackBar.Value;
			grid[0,0].start = true;
			startLocation.X = 0;
			startLocation.Y = 0;
			grid[119,62].end = true;
			endLocation.X = 119;
			endLocation.Y = 62;
			minDistance = 2000;

			
			Reset();
		}
		
		public void start_Click(object sender, EventArgs e)
		{
			if (clickFlag == "start")
			{
				errorLabel.Visible = true;
				errorLabel.Text = "No Start Location";
				return;
			}
			else if (clickFlag == "end")
			{
				errorLabel.Visible = true;
				errorLabel.Text = "No End Location";
				return;
			}
			
			errorLabel.Visible = false;
			start.Enabled = false;
			pathAlgorithms.Enabled = false;
			locked = true;
			timer.Start();
		}
		
		//controls timer for animation of path finding algorithm
		public void timer_Tick(object sender, EventArgs e)
		{
			timer.Interval = trackBar.Value;
			switch(pathAlgorithms.SelectedItem.ToString())
			{
				case "Breadth First Search":
					BFS();
					break;
				
				case "Depth First Search":
					DFS();
					break;
					
				case "Depth First Search (Greedy)":
					DFSgreedy();
					break;
					
				case "A*":
					Astar();
					break;
			}
		}
		
		//move start and end posistions
		public void CanvasMouseDown(object sender, MouseEventArgs e)
		{
			if (locked == true || switchB.Text == "Enable Start and End Movement") 
			{
				//if locked, then algorithm is working: stop wall interactions while algorithm is working
				//if switchB is set to "Enable Wall Creation": start and end positions can not be changed
				return;
			}
			
			if ( (int)((e.X - 50) / 10) >= 120 || (int)((e.Y - 20) / 10) >= 63 || (int)((e.X - 50)) / 10 < 0 || (int)((e.Y - 20) / 10) < 0)
			{
				//outside of grid
				return;
			}
			
			
			//create code that will remove the start or end position and then set a flag for which one has been removed
			if(grid[(int)((e.X - 50) / 10), (int)((e.Y - 20) / 10)].start == true && clickFlag == "")
			{
				grid[(int)((e.X - 50) / 10), (int)((e.Y - 20) / 10)].start = false;
				grid[(int)((e.X - 50) / 10), (int)((e.Y - 20) / 10)].isStack = false;
				clickFlag = "start";
				return;
			}
			else if(grid[(int)((e.X - 50) / 10), (int)((e.Y - 20) / 10)].end == true && clickFlag == "")
			{
				grid[(int)((e.X - 50) / 10), (int)((e.Y - 20) / 10)].end = false;
				clickFlag = "end";
				return;
			}
				
				
			//replace the start or end at place of click
			
			if(clickFlag == "start" && grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].end == false )
			{
				grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].start = true;
				grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].isWall = false;
				grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].isStack = true;
				startLocation.X = (e.X - 50) / 10;
				startLocation.Y = (e.Y - 20) / 10;
				clickFlag = "";
			}
			else if(clickFlag == "end" && grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].start == false )
			{
				grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].end = true;
				grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].isWall = false;
				endLocation.X = (e.X - 50) / 10;
				endLocation.Y = (e.Y - 20) / 10;
				clickFlag = "";
			}	
				
			Reset();
		}
		
		
		//create and remove walls
		public void CanvasMouseMove(object sender, MouseEventArgs e)
		{
						
			//gets mouse location for drawing
			location.X = e.Location.X;
			location.Y = e.Location.Y;
						
			canvas.Invalidate();
			
			if (locked == true || switchB.Text == "Enable Wall Creation") 
			{
				//if locked, then algorithm is working: stop wall interactions while algorithm is working
				//if switchb is set to "Enable Start and End Movement": walls and not be placed or removed
				return;
			}

			
			if (e.Button == MouseButtons.Left)
			{
				//if left mouse button is down, draw wall
				if (e.Y >= 20 && e.Y < 650 && e.X >= 50 && e.X < 1250)
				{
					//if mouse is within the grid: make grid cell at mouse location a wall
					if (grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].start == false && grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].end == false)
					{
						//only make grid cell a wall if it is not the start position or the end position
						grid[(int)(e.X - 50) / 10, (int)(e.Y - 20) / 10].isWall = true;
					}
				}
			}
			
			if (e.Button == MouseButtons.Right)
			{
				//if right mouse button is down, remove wall

				if (e.Y >= 20 && e.Y < 650 && e.X >= 50 && e.X < 1250)
				{
					//if mouse is within the grid: move grid cell at mouse location not a wall
					
					//move a 5x5 eraser
					for (int i = -3; i < 4; i++)
					{
						for(int j = -3; j < 4; j++)
						{
							if ((int)(e.X - 50 + (i * 10)) / 10 >= 0 && (int)(e.X - 50 + (i * 10)) / 10 < 120 && (int)(e.Y - 20 + (j * 10)) / 10 >= 0 && (int)(e.Y - 20 + (j * 10)) / 10 < 63)
							{
								//test if in range of array
									grid[(int)(e.X - 50 + (i * 10)) / 10, (int)(e.Y - 20 + (j * 10)) / 10].isWall = false;
								
							}
						}
					}
				}
			}
		}
		
		public void SwitchBClick(object sender, EventArgs e)
		{
			if(switchB.Text=="Enable Wall Creation")
			{
				switchB.Text="Enable Start and End Movement";
			}
			else if (switchB.Text=="Enable Start and End Movement")
			{
				switchB.Text="Enable Wall Creation";
			}
		}
		
		public void Reset()
		{
			timer.Stop();	
			start.Enabled = true;
			pathAlgorithms.Enabled = true;
			locked = false;
			canvas.Invalidate();
			pathFind = false;
			currentLocation.X = -1;
			currentLocation.Y = -1;
			currentDistance = 0;
			minDistance = 2000;
			
			Q = new Queue<Location>();
			S = new Stack<Location>();
			PQ = new PriorityQueueMin<Location>();
			
			//reset predecesors
			for (int x = 0; x < 120; x++)
			{
				for (int y = 0; y < 63; y++)
				{
					grid[x,y].predX = -1;
					grid[x,y].predY = -1;
					grid[x,y].path = false;
					grid[x,y].visited = false;
					grid[x,y].isStack = false;
					grid[x,y].distance = 2000;
					grid[x,y].isQueued = false;
					grid[x,y].DtoEnd = Math.Sqrt(Math.Pow(x - endLocation.X,2) + Math.Pow(y - endLocation.Y,2)); //distance formula between two points: grid cell and the end location
				}
			}
			
			//reset check array
			for (int i = 0; i < 4; i++)
			{
				check[i] = false;
			}
			
			//add start location to queue
			Q.Enqueue(startLocation);
			S.Push(startLocation);
			PQ.Enqueue(0,startLocation);
			grid[startLocation.X,startLocation.Y].isStack = true;
		}
		
		public void RestartClick(object sender, EventArgs e)
		{
			Reset();
		}
		
		public void canvas_Paint(object sender, PaintEventArgs e)
		{
			Pen blackPen = new Pen(Color.Black);
			Pen bluePen = new Pen(Color.Blue);
			SolidBrush blackBrush = new SolidBrush(Color.Black);
			SolidBrush pinkBrush = new SolidBrush(Color.Pink);
			SolidBrush blueBrush = new SolidBrush(Color.Blue);
			SolidBrush greenBrush = new SolidBrush(Color.LawnGreen);
			SolidBrush redBrush = new SolidBrush(Color.Red);
			SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
			SolidBrush purpleBrush = new SolidBrush(Color.Purple);
			Graphics drawIt = e.Graphics;
			
			//draw the grid array
			for (int y = 0; y < 63; y++)
			{
				for (int x = 0; x < 120;x++)
				{
					if (grid[x,y].start == true) 
					{
						drawIt.FillRectangle(greenBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
					else if (grid[x,y].end == true)
					{
						drawIt.FillRectangle(redBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
					else if (grid[x,y].isWall == true)
					{
						drawIt.FillRectangle(blackBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
					else if (grid[x,y].isStack == true)
					{
						drawIt.FillRectangle(yellowBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
					else if (grid[x,y].path == true)
					{
						drawIt.FillRectangle(yellowBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
					else if (grid[x,y].isQueued == true)
					{
						drawIt.FillRectangle(purpleBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
					else if (grid[x,y].visited == true)
					{
						drawIt.FillRectangle(blueBrush,(x * 10) + 50,(y * 10) + 20 ,10,10);
					}
				}
			}
			
			//draws grid lines
			for (int i = 20; i <= 650; i+=10)
			{
				drawIt.DrawLine(blackPen,50,i,1250,i);
			}
			
			for (int i = 50; i <= 1250; i+=10)
			{
				drawIt.DrawLine(blackPen,i,20,i,650);
			}
			
			//draw start or finish position as it follows mouse
			if(clickFlag == "start")
			{
				drawIt.FillRectangle(greenBrush,(location.X),(location.Y),10,10);
			}
			else if (clickFlag == "end")
			{
				drawIt.FillRectangle(redBrush,(location.X),(location.Y ),10,10);
			}

			
			
		}
		
		public void BFS()
		{
			canvas.Invalidate();
			int tempx = 0;
			
			if (currentLocation.X != endLocation.X || currentLocation.Y != endLocation.Y)
			{
				//if current location has not reached end, then continue the search
				if (Q.Count == 0)
				{
					//queue is empty, no legal path to end
					errorLabel.Text = "No Legal Path To End";
					errorLabel.Visible = true;
					timer.Stop();
					return;
				}
				currentLocation = Q.Dequeue();
			}
			
			if (currentLocation.X == endLocation.X && currentLocation.Y == endLocation.Y)
			{								
				//current location has been reached: searching is finished
				
				if (pathFind == false)
				{
					//if path retracing has just started
					//set initial position for tempLocation to be current location
					tempLocation.X = currentLocation.X;
					tempLocation.Y = currentLocation.Y;	
					pathFind = true;
				}

				//use predecessors to draw shortest path starting with end until start is reached
				tempx = tempLocation.X;
				tempLocation.X = grid[tempx,tempLocation.Y].predX;
				tempLocation.Y = grid[tempx,tempLocation.Y].predY;
				
				//set new cell as part of path to be drawn
				grid[tempLocation.X,tempLocation.Y].path = true;
				
				if (tempLocation.X == startLocation.X && tempLocation.Y == startLocation.Y)
				{
					//entire path has been connected from end to start
					timer.Stop();
				}
				
				return;
			}
			
			//load adjacent unvisited non-wall grid cells to the queue
			//check adjacent cells (one above, one below, one left, and one right (if within the grid))
			
			//cell above
			tempLocation.X = currentLocation.X;
			tempLocation.Y = currentLocation.Y - 1;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63) 
			{
				//check to make sure grid cell looked for is in the grid
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].visited == false)
				{
					//if cell is not a wall, and has not yet been visited
					//add tempLocation to queue,set to visited, and 
					Q.Enqueue(tempLocation);
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;

					
				}
			}
			
			
			//cell below
			tempLocation.X = currentLocation.X;
			tempLocation.Y = currentLocation.Y + 1;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63) 
			{
				//check to make sure grid cell looked for is in the grid
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].visited == false)
				{
					//if cell is not a wall, and has not yet been visited
					//add tempLocation to queue,set to visited, and 
					Q.Enqueue(tempLocation);
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;

					
				}
			}
			
			//cell left
			tempLocation.X = currentLocation.X - 1;
			tempLocation.Y = currentLocation.Y;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63) 
			{
				//check to make sure grid cell looked for is in the grid
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].visited == false)
				{
					//if cell is not a wall, and has not yet been visited
					//add tempLocation to queue,set to visited, and 
					Q.Enqueue(tempLocation);
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;

					
				}
			}
			
			//cell right
			tempLocation.X = currentLocation.X + 1;
			tempLocation.Y = currentLocation.Y;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63) 
			{
				//check to make sure grid cell looked for is in the grid
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].visited == false)
				{
					//if cell is not a wall, and has not yet been visited
					//add tempLocation to queue,set to visited, and 
					Q.Enqueue(tempLocation);
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;

					
				}
			}
			
		}
		
		public void DFS()
		{		
			int tempx = 0;
			canvas.Invalidate();
			
			if (S.Count == 0 )
			{
				//stack is empty: finished exploring grid cells
				//path finding is complete
				//start drawing path from end location using predeccesors
				
				if (pathFind == false) 
				{
					//if path finding is just beginning: set temp location to the end location
					tempLocation.X = endLocation.X;
					tempLocation.Y = endLocation.Y;
					pathFind = true;
				}
				
				//use predecessors to draw shortest path starting with end until start is reached
				tempx = tempLocation.X;
				tempLocation.X = grid[tempx,tempLocation.Y].predX;
				tempLocation.Y = grid[tempx,tempLocation.Y].predY;
				
				//set new cell as part of path to be drawn
				if (tempLocation.X == -1 && tempLocation.Y == -1)
				{
					//no legal path to end
					errorLabel.Text = "No Legal Path To End";
					errorLabel.Visible = true;
					timer.Stop();
					return;
				}
				grid[tempLocation.X,tempLocation.Y].path = true;
				
				if (tempLocation.X == startLocation.X && tempLocation.Y == startLocation.Y)
				{
					//entire path has been connected from end to start
					timer.Stop();
				}
				
				return;
			}
			
			//get next location
			currentLocation = S.Peek();
			
			if (currentLocation.X == endLocation.X && currentLocation.Y == endLocation.Y)
			{
				//we have found the end point
				//check if new minimum distance
				
				if (minDistance > S.Count + 1)
				{
					//new minimum distance is found
					//set new minimum path based on predeccesors
					minDistance = S.Count + 1; //S contains the entire current path except for +1 for the end location
					
					while (S.Peek().X != startLocation.X && S.Peek().Y != startLocation.Y)
					{
						//iterate through stack to create predeccesor path until start location is reached
						tempLocation = S.Pop();
						grid[tempLocation.X,tempLocation.Y].predX = S.Peek().X;
						grid[tempLocation.X,tempLocation.Y].predY = S.Peek().Y;
						TempS.Push(tempLocation); //stores stack in reverse order
					}
					
					//replace the stack back in the right order
					while (TempS.Count != 0)
					{
						S.Push(TempS.Pop());
					}
				}
			}
			
			//if end location is not found: add an adjacent cell to stack	
			
			//cell above
			tempLocation.X = currentLocation.X;
			tempLocation.Y = currentLocation.Y - 1;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63 && S.Count + 1 < minDistance)
			{
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
				{
					S.Push(tempLocation);
					grid[tempLocation.X,tempLocation.Y].distance = S.Count;
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].isStack = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
					return;
				}
			}
			
			//cell below
			tempLocation.X = currentLocation.X;
			tempLocation.Y = currentLocation.Y + 1;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63 && S.Count + 1 < minDistance)
			{
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
				{
					S.Push(tempLocation);
					grid[tempLocation.X,tempLocation.Y].distance = S.Count;
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].isStack = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
					return;
				}
			}
			
			//cell right
			tempLocation.X = currentLocation.X + 1;
			tempLocation.Y = currentLocation.Y;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63 && S.Count + 1 < minDistance)
			{
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
				{
					S.Push(tempLocation);
					grid[tempLocation.X,tempLocation.Y].distance = S.Count;
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].isStack = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
					return;
				}
			}
			
			//cell left
			tempLocation.X = currentLocation.X - 1;
			tempLocation.Y = currentLocation.Y;
			
			if (tempLocation.X >= 0 && tempLocation.X < 120 && tempLocation.Y >=0 && tempLocation.Y < 63 && S.Count + 1 < minDistance)
			{
				if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
				{
					S.Push(tempLocation);
					grid[tempLocation.X,tempLocation.Y].distance = S.Count;
					grid[tempLocation.X,tempLocation.Y].visited = true;
					grid[tempLocation.X,tempLocation.Y].isStack = true;
					grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
					grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
					return;
				}
			}
			
			//no valid adjacent cells to add: remove current cell
			S.Pop();
			grid[currentLocation.X,currentLocation.Y].isStack = false;

		}
		
		//exact same as DFS except for how adjacent grid cells are added to the stack/path
		//the adjacent cell closet to the position of the end location will be added (if available), thereby pushing the DFS towards the direction of the end location
		public void DFSgreedy()
		{
			int tempx = 0;
			canvas.Invalidate();
						
			//reset check array
			for (int i = 0; i < 4; i++)
			{
				check[i] = true;
			}

			
			if (S.Count == 0 )
			{
				//stack is empty: finished exploring grid cells
				//path finding is complete
				//start drawing path from end location using predeccesors
				
				if (pathFind == false) 
				{
					//if path finding is just beginning: set temp location to the end location
					tempLocation.X = endLocation.X;
					tempLocation.Y = endLocation.Y;
					pathFind = true;
				}
				
				//use predecessors to draw shortest path starting with end until start is reached
				tempx = tempLocation.X;
				tempLocation.X = grid[tempx,tempLocation.Y].predX;
				tempLocation.Y = grid[tempx,tempLocation.Y].predY;
				
				//set new cell as part of path to be drawn
				
				if (tempLocation.X == -1 && tempLocation.Y == -1)
				{
					//no legal path to end
					errorLabel.Text = "No Legal Path To End";
					errorLabel.Visible = true;
					timer.Stop();
					return;
				}
				grid[tempLocation.X,tempLocation.Y].path = true;
				
				if (tempLocation.X == startLocation.X && tempLocation.Y == startLocation.Y)
				{
					//entire path has been connected from end to start
					timer.Stop();
				}
				
				return;
			}
			
			//get next location
			currentLocation = S.Peek();
			
			if (currentLocation.X == endLocation.X && currentLocation.Y == endLocation.Y)
			{
				//we have found the end point
				//check if new minimum distance
				
				if (minDistance > S.Count + 1)
				{
					//new minimum distance is found
					//set new minimum path based on predeccesors
					minDistance = S.Count + 1; //S contains the entire current path except for +1 for the end location
					
					while (S.Peek().X != startLocation.X && S.Peek().Y != startLocation.Y)
					{
						//iterate through stack to create predeccesor path until start location is reached
						tempLocation = S.Pop();
						grid[tempLocation.X,tempLocation.Y].predX = S.Peek().X;
						grid[tempLocation.X,tempLocation.Y].predY = S.Peek().Y;
						TempS.Push(tempLocation); //stores stack in reverse order
					}
					
					//replace the stack back in the right order
					while (TempS.Count != 0)
					{
						S.Push(TempS.Pop());
					}
				}
			}
			
			//if end location is not found: add an adjacent cell to stack	
			//add adjacent cell that is closer to the end location		
			
			//check array:
			//0 = above
			//1 = below
			//2 = left
			//3 = right
			
			//check is adjacent cells are in the grid
			if ( 0 <= currentLocation.X && currentLocation.X < 120 && 0 <= currentLocation.Y - 1 && currentLocation.Y -1 < 63)
			{
				above.X = currentLocation.X;
				above.Y = currentLocation.Y - 1;
			}
			else
			{
				above.X = currentLocation.X;
				above.Y = currentLocation.Y;
				check[0] = false;
			}
		
			if ( 0 <= currentLocation.X && currentLocation.X < 120 && 0 <= currentLocation.Y + 1 && currentLocation.Y + 1 < 63)
			{
				below.X = currentLocation.X;
				below.Y = currentLocation.Y + 1;
			}
			else
			{
				below.X = currentLocation.X;
				below.Y = currentLocation.Y;
				check[1] = false;
			}
			
			if ( 0 <= currentLocation.X - 1 && currentLocation.X - 1 < 120 && 0 <= currentLocation.Y && currentLocation.Y < 63)
			{
				left.X = currentLocation.X - 1;
				left.Y = currentLocation.Y;
			}
			else
			{
				left.X = currentLocation.X;
				left.Y = currentLocation.Y;
				check[2] = false;
			}
			
			if ( 0 <= currentLocation.X + 1 && currentLocation.X + 1 < 120 && 0 <= currentLocation.Y && currentLocation.Y < 63)
			{
				right.X = currentLocation.X + 1;
				right.Y = currentLocation.Y;
			}
			else
			{
				right.X = currentLocation.X;
				right.Y = currentLocation.Y;
				check[3] = false;
			}
			
			for (int i = 0; i < 4; i++)
			{
				//check four times to make sure that all cells are checked. Without for loop, if the last cell is the closet to the end location but unavailable, the other cells have already been passed over
				

					//if (distance of cell above to end location             <=          distance of cell below to end location        or already checked) && if distance of cell above to end location             <=          distance of cell left to end location       or already check)     && if distance of cell above to end location             <=          distance of cell right to end location && above needs to be checked && path is not longer than distance it currently takes to reach end location
					if((grid[above.X,above.Y].DtoEnd <= grid[below.X,below.Y].DtoEnd || check[1] == false) && (grid[above.X,above.Y].DtoEnd <= grid[left.X,left.Y].DtoEnd || check[2] == false) && (grid[above.X,above.Y].DtoEnd < grid[right.X,right.Y].DtoEnd || check[3] == false) && check[0] == true && S.Count + 1 < minDistance)
					{
			   				//cell above is closest of the not already checked cells
			   		
						tempLocation.X = currentLocation.X;
						tempLocation.Y = currentLocation.Y - 1;
	
							if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
							{
								S.Push(tempLocation);
								grid[tempLocation.X,tempLocation.Y].distance = S.Count;
								grid[tempLocation.X,tempLocation.Y].visited = true;
								grid[tempLocation.X,tempLocation.Y].isStack = true;
								grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
								grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
								return;
							}
						//cell not added: set check to false
						check[0] = false;
					}
				

								

					//if (distance of cell below to end location             <=          distance of cell above to end location        or already checked) && if distance of cell below to end location             <=          distance of cell left to end location       or already check)     && if distance of cell below to end location             <=          distance of cell right to end location && below needs to be checked && path is not longer than distance it currently takes to reach end location
					if((grid[below.X,below.Y].DtoEnd <= grid[above.X,above.Y].DtoEnd || check[0] == false) && (grid[below.X,below.Y].DtoEnd <= grid[left.X,left.Y].DtoEnd || check[2] == false) && (grid[below.X,below.Y].DtoEnd < grid[right.X,right.Y].DtoEnd || check[3] == false) && check[1] == true && S.Count + 1 < minDistance)
					{
			   			//cell below is closest of the not already checked cells
			   			
					tempLocation.X = currentLocation.X;
					tempLocation.Y = currentLocation.Y + 1;

						if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
						{
							S.Push(tempLocation);
							grid[tempLocation.X,tempLocation.Y].distance = S.Count;
							grid[tempLocation.X,tempLocation.Y].visited = true;
							grid[tempLocation.X,tempLocation.Y].isStack = true;
							grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
							grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
							return;
						}
			   			
			   			//cell not added: set check to false
						check[1] = false;
					}
				
				

					//if (distance of cell left to end location              <=          distance of cell above to end location        or already checked) && if distance of cell left to end location              <=          distance of cell below to end location       or already check)     && if distance of cell left to end location             <=          distance of cell right to end location && left needs to be checked && path is not longer than distance it currently takes to reach end location
					if((grid[left.X,left.Y].DtoEnd <= grid[above.X,above.Y].DtoEnd || check[0] == false) && (grid[left.X,left.Y].DtoEnd <= grid[below.X,below.Y].DtoEnd || check[1] == false) && (grid[left.X,left.Y].DtoEnd < grid[right.X,right.Y].DtoEnd || check[3] == false) && check[2] == true && S.Count + 1 < minDistance)
					{
				   		//cell left is closest of the not already checked cells
				   		tempLocation.X = currentLocation.X - 1;
					tempLocation.Y = currentLocation.Y;
		
						if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
						{
							S.Push(tempLocation);
							grid[tempLocation.X,tempLocation.Y].distance = S.Count;
							grid[tempLocation.X,tempLocation.Y].visited = true;
								grid[tempLocation.X,tempLocation.Y].isStack = true;
							grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
							grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
							return;
						}
				   		
				   		//cell not added: set check to false
						check[2] = false;
					}
				

					//cell right is in grid and be found at a shorter distance
				
					//if (distance of cell right to end location             <=          distance of cell above to end location        or already checked) && if distance of cell right to end location             <=          distance of cell below to end location       or already check)     && if distance of cell right to end location            <=          distance of cell left to end location && right needs to be checked && path is not longer than distance it currently takes to reach end location
					if((grid[right.X,right.Y].DtoEnd <= grid[above.X,above.Y].DtoEnd || check[0] == false) && (grid[right.X,right.Y].DtoEnd <= grid[below.X,below.Y].DtoEnd || check[1] == false) && (grid[right.X,right.Y].DtoEnd < grid[left.X,left.Y].DtoEnd || check[2] == false) && check [3] == true && S.Count + 1 < minDistance)
					{
				   		//cell right is closest of the not already checked cells
				   		tempLocation.X = currentLocation.X + 1;
					tempLocation.Y = currentLocation.Y;
					
		
						if (grid[tempLocation.X,tempLocation.Y].isWall == false && grid[tempLocation.X,tempLocation.Y].isStack == false && grid[tempLocation.X,tempLocation.Y].distance > S.Count + 1)
						{
							S.Push(tempLocation);
							grid[tempLocation.X,tempLocation.Y].distance = S.Count;
							grid[tempLocation.X,tempLocation.Y].visited = true;
							grid[tempLocation.X,tempLocation.Y].isStack = true;
							grid[tempLocation.X,tempLocation.Y].predX = currentLocation.X;
							grid[tempLocation.X,tempLocation.Y].predY = currentLocation.Y;
							return;
						}
				   		
				   		//cell not added: set check to false
						check[3] = false;
					
				}
			}
			

		
			//no valid adjacent cells to add: remove current cell
			S.Pop();
			grid[currentLocation.X,currentLocation.Y].isStack = false;

		}
		
		//chooses to explore adjacent nodes based on G + H values
		//G is the distance from the starting location
		//H is the distance formula between current location and end location
		public void Astar()
		{
			canvas.Invalidate();
			int tempx;
			if (currentLocation.X != endLocation.X || currentLocation.Y != endLocation.Y)
			{
				if (PQ.Count == 0)
				{
					//priority queue empty, no legal path
					errorLabel.Text = "No Legal Path to End";
					errorLabel.Visible = true;
					timer.Stop();
					return;
				}
				currentLocation = PQ.Dequeue();
				grid[currentLocation.X, currentLocation.Y ].isQueued = false;
				currentDistance++;
			}


			
			if (currentLocation.X == endLocation.X && currentLocation.Y == endLocation.Y)
			{
				//end location has been found
				if (pathFind == false) 
				{
					//if path finding is just beginning: set temp location to the end location
					tempLocation.X = endLocation.X;
					tempLocation.Y = endLocation.Y;
					pathFind = true;
				}
				
				//use predecessors to draw shortest path starting with end until start is reached
				tempx = tempLocation.X;
				tempLocation.X = grid[tempx,tempLocation.Y].predX;
				tempLocation.Y = grid[tempx,tempLocation.Y].predY;
				
				//set new cell as part of path to be drawn
				grid[tempLocation.X,tempLocation.Y].path = true;
				
				if (tempLocation.X == startLocation.X && tempLocation.Y == startLocation.Y)
				{
					//entire path has been connected from end to start
					timer.Stop();
				}
				
				return;
			}
				
			
			//enqueue adjacent nodes that have not be visited yet
			
			//above
			if(currentLocation.Y - 1 >= 0 && grid[currentLocation.X, currentLocation.Y - 1].isWall == false && grid[currentLocation.X, currentLocation.Y - 1].visited == false)
			{
				tempLocation.X = currentLocation.X;
				tempLocation.Y = currentLocation.Y - 1;
				grid[currentLocation.X, currentLocation.Y - 1].visited = true;
				grid[currentLocation.X, currentLocation.Y - 1].isQueued = true;
				grid[tempLocation.X, tempLocation.Y].predX = currentLocation.X;
				grid[tempLocation.X, tempLocation.Y].predY = currentLocation.Y;
				PQ.Enqueue((int)grid[tempLocation.X, tempLocation.Y].DtoEnd, tempLocation);
			}
			
			//below
			if(currentLocation.Y + 1 < 63 && grid[currentLocation.X, currentLocation.Y + 1].isWall == false && grid[currentLocation.X, currentLocation.Y + 1].visited == false)
			{
				tempLocation.X = currentLocation.X;
				tempLocation.Y = currentLocation.Y + 1;
				grid[currentLocation.X, currentLocation.Y + 1].visited = true;
				grid[currentLocation.X, currentLocation.Y + 1].isQueued = true;
				grid[tempLocation.X, tempLocation.Y].predX = currentLocation.X;
				grid[tempLocation.X, tempLocation.Y].predY = currentLocation.Y;
				PQ.Enqueue((int)grid[tempLocation.X, tempLocation.Y].DtoEnd, tempLocation);
			}
			
			//left
			if(currentLocation.X - 1 >= 0 && grid[currentLocation.X - 1, currentLocation.Y].isWall == false && grid[currentLocation.X - 1, currentLocation.Y].visited == false)
			{
				tempLocation.X = currentLocation.X - 1;
				tempLocation.Y = currentLocation.Y;
				grid[currentLocation.X - 1, currentLocation.Y].visited = true;
				grid[currentLocation.X - 1, currentLocation.Y].isQueued = true;
				grid[tempLocation.X, tempLocation.Y].predX = currentLocation.X;
				grid[tempLocation.X, tempLocation.Y].predY = currentLocation.Y;
				PQ.Enqueue((int)grid[tempLocation.X, tempLocation.Y].DtoEnd, tempLocation);
			}
			
			//right
			if(currentLocation.X + 1 >= 0 && grid[currentLocation.X + 1, currentLocation.Y].isWall == false && grid[currentLocation.X + 1, currentLocation.Y].visited == false)
			{
				tempLocation.X = currentLocation.X + 1;
				tempLocation.Y = currentLocation.Y;
				grid[currentLocation.X + 1, currentLocation.Y].visited = true;
				grid[currentLocation.X + 1, currentLocation.Y].isQueued = true;
				grid[tempLocation.X, tempLocation.Y].predX = currentLocation.X;
				grid[tempLocation.X, tempLocation.Y].predY = currentLocation.Y;
				PQ.Enqueue((int)grid[tempLocation.X, tempLocation.Y].DtoEnd, tempLocation);
			}
		}
		
		
	}
}
