/*
 * Created by SharpDevelop.
 * User: Nick
 * Date: 7/9/2021
 * Time: 2:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Path_Finding_Visualization_Tool
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox canvas;
		private System.Windows.Forms.Button start;
		private System.Windows.Forms.ComboBox pathAlgorithms;
		private System.Windows.Forms.TrackBar trackBar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Button switchB;
		private System.Windows.Forms.Button Restart;

		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.canvas = new System.Windows.Forms.PictureBox();
			this.start = new System.Windows.Forms.Button();
			this.pathAlgorithms = new System.Windows.Forms.ComboBox();
			this.trackBar = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.errorLabel = new System.Windows.Forms.Label();
			this.switchB = new System.Windows.Forms.Button();
			this.Restart = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.DarkGreen;
			this.label1.ForeColor = System.Drawing.Color.DarkGreen;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(1307, 66);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.DarkGreen;
			this.label2.ForeColor = System.Drawing.Color.DarkGreen;
			this.label2.Location = new System.Drawing.Point(0, 746);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(1307, 66);
			this.label2.TabIndex = 1;
			this.label2.Text = "label2";
			// 
			// canvas
			// 
			this.canvas.Location = new System.Drawing.Point(0, 69);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(1307, 674);
			this.canvas.TabIndex = 2;
			this.canvas.TabStop = false;
			this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
			this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasMouseDown);
			this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CanvasMouseMove);
			// 
			// start
			// 
			this.start.Location = new System.Drawing.Point(12, 12);
			this.start.Name = "start";
			this.start.Size = new System.Drawing.Size(75, 23);
			this.start.TabIndex = 3;
			this.start.Text = "Start";
			this.start.UseVisualStyleBackColor = true;
			this.start.Click += new System.EventHandler(this.start_Click);
			// 
			// pathAlgorithms
			// 
			this.pathAlgorithms.FormattingEnabled = true;
			this.pathAlgorithms.Items.AddRange(new object[] {
			"Breadth First Search",
			"Depth First Search",
			"Depth First Search (Greedy)",
			"A*"});
			this.pathAlgorithms.Location = new System.Drawing.Point(93, 14);
			this.pathAlgorithms.Name = "pathAlgorithms";
			this.pathAlgorithms.Size = new System.Drawing.Size(121, 21);
			this.pathAlgorithms.TabIndex = 4;
			this.pathAlgorithms.Text = "Breadth First Search";
			// 
			// trackBar
			// 
			this.trackBar.Location = new System.Drawing.Point(947, 12);
			this.trackBar.Maximum = 2000;
			this.trackBar.Minimum = 1;
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new System.Drawing.Size(347, 45);
			this.trackBar.TabIndex = 5;
			this.trackBar.TickFrequency = 20;
			this.trackBar.Value = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(841, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
			this.label3.TabIndex = 6;
			this.label3.Text = "Change Speed";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// errorLabel
			// 
			this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.errorLabel.ForeColor = System.Drawing.Color.Red;
			this.errorLabel.Location = new System.Drawing.Point(266, 9);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(517, 48);
			this.errorLabel.TabIndex = 7;
			this.errorLabel.Text = "Error Label";
			this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.errorLabel.Visible = false;
			// 
			// switchB
			// 
			this.switchB.Location = new System.Drawing.Point(12, 40);
			this.switchB.Name = "switchB";
			this.switchB.Size = new System.Drawing.Size(202, 23);
			this.switchB.TabIndex = 8;
			this.switchB.Text = "Enable Start and End Movement";
			this.switchB.UseVisualStyleBackColor = true;
			this.switchB.Click += new System.EventHandler(this.SwitchBClick);
			// 
			// Restart
			// 
			this.Restart.Location = new System.Drawing.Point(841, 40);
			this.Restart.Name = "Restart";
			this.Restart.Size = new System.Drawing.Size(100, 23);
			this.Restart.TabIndex = 9;
			this.Restart.Text = "Reset";
			this.Restart.UseVisualStyleBackColor = true;
			this.Restart.Click += new System.EventHandler(this.RestartClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1306, 810);
			this.Controls.Add(this.Restart);
			this.Controls.Add(this.switchB);
			this.Controls.Add(this.errorLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.trackBar);
			this.Controls.Add(this.pathAlgorithms);
			this.Controls.Add(this.start);
			this.Controls.Add(this.canvas);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "Path Finding Visualization Tool";
			((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
