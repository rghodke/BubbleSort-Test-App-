/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 7/21/2015
 * Time: 6:56 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace TestApp
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		ArrayList points = new ArrayList();
		int curloc = 0;
		int curmax = 9;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			dataGridView1.ColumnCount = 2;
			dataGridView1.Columns[0].Name = "Index";
			dataGridView1.Columns[1].Name = "Value";
			
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			progressBar1.Maximum = 450;
			Shuffle();
			HighlightRows();
			UpdateFields();
			
			//SQL STUFF
			SqlConnection tempConnection = new SqlConnection("user id=AdminTest;" +
			                                                 "password=AdminTest; server=RON\\SQLEXPRESS;" +
			                                                 "Trusted_Connection=yes;" +
			                                                 "database=AdminTest; " +
			                                                 "connection timeout=5");
			try
			{
				tempConnection.Open();
			}
			catch(Exception e)
			{
				
			}
			
			SqlCommand  myCommand = new SqlCommand("Command String", tempConnection);
			SqlCommand myCommand2 = new SqlCommand("INSERT INTO table1 (column1, column2) " +
			                                       "Values ('string', 1)", tempConnection);
			myCommand2.ExecuteNonQuery();
			
			try
			{
				SqlDataReader myReader = null;
				SqlCommand    myCommand3 = new SqlCommand("select * from table1",
				                                          tempConnection);
				myReader = myCommand3.ExecuteReader();
				while(myReader.Read())
				{

				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			SqlCommand myCommand4 = new SqlCommand(
				"SELECT * FROM table1 WHERE Column = " + " ", tempConnection);
			
			SqlParameter myParam = new SqlParameter("@Param1", SqlDbType.VarChar, 11);
			myParam.Value = "Garden Hose";
			
			SqlParameter myParam2 = new SqlParameter("@Param2", SqlDbType.Int, 4);
			myParam2.Value = 42;

			SqlParameter myParam3 = new SqlParameter("@Param3", SqlDbType.Text);
			myParam.Value = "Note that I am not specifying size. " +
				"If I did that it would trunicate the text.";
			
			SqlCommand myCommand5 = new SqlCommand(
				"SELECT * FROM table WHERE Column = @Param2", tempConnection);
			myCommand.Parameters.Add(myParam2);
			
			try
			{
				tempConnection.Close();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			
			
			//END SQL STUFF
			
		}
		
		void Shuffle()
		{
			timer.Enabled = false;
			StepButton.Enabled = true;
			PlayButton.Enabled = true;
			textBox4.Text = "";
			textBox5.Text = "";
			textBox6.Text = "";
			points = new ArrayList();
			dataGridView1.Rows.Clear();
			Random random = new Random();
			
			for(int i = 0; i<10; i++)
			{
				points.Add(random.Next(0, 100));
				dataGridView1.Rows.Add(i.ToString(), points[i]) ;
			}
			curloc = 0;
			curmax = 9;
			progressBar1.Value = 0;
			UpdateFields();
			HighlightRows();

			StepButton.Enabled = true;
			
			
		}
		
		void UpdateProgress()
		{
			dataGridView1.Rows.Clear();
			for(int i = 0; i<10; i++)
			{
				dataGridView1.Rows.Add(i.ToString(), points[i]) ;
			}
			progressBar1.Step = 10;
			progressBar1.PerformStep();
			
			
		}
		
		void ShuffleButtonClick(object sender, EventArgs e)
		{
			Shuffle();

		}

		

		void ProgressSort()
		{
			if ( curloc < curmax )
			{
				string temp1 = (points[curloc + 1]).ToString();
				string temp2 = (points[curloc]).ToString();
				if(Int32.Parse(temp1) > Int32.Parse(temp2))
				{
					textBox4.Text = points[curloc].ToString();
					textBox5.Text = "true";
					textBox6.Text = temp2 + " has swapped with " + temp1;
					points.Reverse(curloc, 2);
				}
				else
				{
					textBox4.Text = "";
					textBox5.Text = "false";
					textBox6.Text = temp2 + " did not swap with " + temp1;
				}
				curloc++;
			}
			UpdateProgress();
			if ( curloc == curmax )
			{
				curloc = 0;
				curmax--;
			}
			if(curmax == 0)
			{
				StepButton.Enabled = false;
				PlayButton.Enabled = false;
				ShuffleButton.Enabled = true;
			}
			
			UpdateFields();
			
		}
		

		void StepButtonClick(object sender, EventArgs e)
		{
			ProgressSort();
			HighlightRows();

		}
		
		void HighlightRows()
		{
			DataGridViewRow tempr0 = dataGridView1.Rows[curloc];
			DataGridViewRow tempr1 = dataGridView1.Rows[curloc+1];
			tempr0.DefaultCellStyle.BackColor = Color.Tomato;
			tempr1.DefaultCellStyle.BackColor = Color.Tomato;
		}
		
		void UpdateFields()
		{
			textBox1.Text = curloc.ToString();
			textBox2.Text = points[curloc].ToString();
			textBox3.Text = points[curloc+1].ToString();
		}

		Timer timer = new Timer();
		
		void PlayButtonClick(object sender, EventArgs e)
		{
			timer = new Timer();
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = (1000) * (1);
			timer.Enabled = true;
			timer.Start();
			PlayButton.Enabled = false;
			ShuffleButton.Enabled = false;
			
			
		}
		
		void timer_Tick(object sender, EventArgs e)
		{
			ProgressSort();
			HighlightRows();

		}
		
		


	}
}
