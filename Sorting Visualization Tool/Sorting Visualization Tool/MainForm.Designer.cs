/*
 * Created by SharpDevelop.
 * User: Nick
 * Date: 7/1/2021
 * Time: 1:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Sorting_Visualization_Tool
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button Start;
		private System.Windows.Forms.ComboBox SortingAlgorithm;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ArraySizeTextBox;
		private System.Windows.Forms.Button GenerateArray;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label ArraySizeError;
		private System.Windows.Forms.PictureBox pictureBox;
		
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.Start = new System.Windows.Forms.Button();
			this.SortingAlgorithm = new System.Windows.Forms.ComboBox();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ArraySizeTextBox = new System.Windows.Forms.TextBox();
			this.GenerateArray = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.ArraySizeError = new System.Windows.Forms.Label();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Purple;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(984, 75);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// Start
			// 
			this.Start.Enabled = false;
			this.Start.Location = new System.Drawing.Point(12, 12);
			this.Start.Name = "Start";
			this.Start.Size = new System.Drawing.Size(75, 23);
			this.Start.TabIndex = 1;
			this.Start.Text = "Start";
			this.Start.UseVisualStyleBackColor = true;
			this.Start.Click += new System.EventHandler(this.StartClick);
			// 
			// SortingAlgorithm
			// 
			this.SortingAlgorithm.FormattingEnabled = true;
			this.SortingAlgorithm.Items.AddRange(new object[] {
			"Bubble Sort",
			"Quick Sort",
			"Merge Sort",
			"Radix Sort"});
			this.SortingAlgorithm.Location = new System.Drawing.Point(93, 14);
			this.SortingAlgorithm.Name = "SortingAlgorithm";
			this.SortingAlgorithm.Size = new System.Drawing.Size(121, 21);
			this.SortingAlgorithm.TabIndex = 2;
			this.SortingAlgorithm.Text = "Bubble Sort";
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(701, 5);
			this.trackBar1.Maximum = 100;
			this.trackBar1.Minimum = 1;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(271, 45);
			this.trackBar1.TabIndex = 3;
			this.trackBar1.TickFrequency = 5;
			this.trackBar1.Value = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(595, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Speed Slider";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(287, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(109, 23);
			this.label2.TabIndex = 5;
			this.label2.Text = "Array Size (10 - 100)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ArraySizeTextBox
			// 
			this.ArraySizeTextBox.Location = new System.Drawing.Point(402, 15);
			this.ArraySizeTextBox.Name = "ArraySizeTextBox";
			this.ArraySizeTextBox.Size = new System.Drawing.Size(177, 20);
			this.ArraySizeTextBox.TabIndex = 6;
			this.ArraySizeTextBox.Text = "10";
			this.ArraySizeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// GenerateArray
			// 
			this.GenerateArray.Location = new System.Drawing.Point(12, 41);
			this.GenerateArray.Name = "GenerateArray";
			this.GenerateArray.Size = new System.Drawing.Size(202, 23);
			this.GenerateArray.TabIndex = 7;
			this.GenerateArray.Text = "Generate Array";
			this.GenerateArray.UseVisualStyleBackColor = true;
			this.GenerateArray.Click += new System.EventHandler(this.GenerateArrayClick);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Purple;
			this.label3.Location = new System.Drawing.Point(0, 500);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(984, 67);
			this.label3.TabIndex = 8;
			// 
			// ArraySizeError
			// 
			this.ArraySizeError.BackColor = System.Drawing.Color.Purple;
			this.ArraySizeError.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ArraySizeError.ForeColor = System.Drawing.Color.Red;
			this.ArraySizeError.Location = new System.Drawing.Point(277, 41);
			this.ArraySizeError.Name = "ArraySizeError";
			this.ArraySizeError.Size = new System.Drawing.Size(318, 23);
			this.ArraySizeError.TabIndex = 9;
			this.ArraySizeError.Text = "Array Size Error: Using Default of 10";
			this.ArraySizeError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ArraySizeError.Visible = false;
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(0, 100);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(984, 400);
			this.pictureBox.TabIndex = 10;
			this.pictureBox.TabStop = false;
			this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gray;
			this.ClientSize = new System.Drawing.Size(984, 561);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.ArraySizeError);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.GenerateArray);
			this.Controls.Add(this.ArraySizeTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.SortingAlgorithm);
			this.Controls.Add(this.Start);
			this.Controls.Add(this.pictureBox1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "Sorting Visualization Tool";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
