using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RegionEditor
{
	/// <summary>
	/// Summary description for NewFacet.
	/// </summary>
	public class NewFacet : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton Map0;
		private System.Windows.Forms.RadioButton Map1;
		private System.Windows.Forms.RadioButton Map2;
		private System.Windows.Forms.RadioButton Map3;
		private System.Windows.Forms.TextBox m_FacetName;
		private System.Windows.Forms.RadioButton Map4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewFacet()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.m_FacetName = new System.Windows.Forms.TextBox();
			this.Map0 = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.Map1 = new System.Windows.Forms.RadioButton();
			this.Map2 = new System.Windows.Forms.RadioButton();
			this.Map3 = new System.Windows.Forms.RadioButton();
			this.Map4 = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(232, 128);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(152, 128);
			this.button2.Name = "button2";
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			// 
			// m_FacetName
			// 
			this.m_FacetName.Location = new System.Drawing.Point(88, 16);
			this.m_FacetName.Name = "m_FacetName";
			this.m_FacetName.Size = new System.Drawing.Size(208, 20);
			this.m_FacetName.TabIndex = 2;
			this.m_FacetName.Text = "";
			// 
			// Map0
			// 
			this.Map0.Checked = true;
			this.Map0.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Map0.Location = new System.Drawing.Point(16, 80);
			this.Map0.Name = "Map0";
			this.Map0.Size = new System.Drawing.Size(40, 24);
			this.Map0.TabIndex = 3;
			this.Map0.TabStop = true;
			this.Map0.Text = "0";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "Name:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(232, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "Select the default map file for this facet:";
			// 
			// Map1
			// 
			this.Map1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Map1.Location = new System.Drawing.Point(72, 80);
			this.Map1.Name = "Map1";
			this.Map1.Size = new System.Drawing.Size(40, 24);
			this.Map1.TabIndex = 9;
			this.Map1.Text = "1";
			// 
			// Map2
			// 
			this.Map2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Map2.Location = new System.Drawing.Point(128, 80);
			this.Map2.Name = "Map2";
			this.Map2.Size = new System.Drawing.Size(40, 24);
			this.Map2.TabIndex = 10;
			this.Map2.Text = "2";
			// 
			// Map3
			// 
			this.Map3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Map3.Location = new System.Drawing.Point(184, 80);
			this.Map3.Name = "Map3";
			this.Map3.Size = new System.Drawing.Size(40, 24);
			this.Map3.TabIndex = 11;
			this.Map3.Text = "3";
			// 
			// Map4
			// 
			this.Map4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Map4.Location = new System.Drawing.Point(240, 80);
			this.Map4.Name = "Map4";
			this.Map4.Size = new System.Drawing.Size(40, 24);
			this.Map4.TabIndex = 12;
			this.Map4.Text = "4";
			// 
			// NewFacet
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(314, 160);
			this.ControlBox = false;
			this.Controls.Add(this.Map4);
			this.Controls.Add(this.Map3);
			this.Controls.Add(this.Map2);
			this.Controls.Add(this.Map1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Map0);
			this.Controls.Add(this.m_FacetName);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewFacet";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create new facet";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			if ( m_FacetName.Text.Length == 0 )
			{
				MessageBox.Show( "Please enter a name for the new facet" );
				return;
			}

			DialogResult = DialogResult.OK;
			this.Close();
		}

		public string FacetName
		{
			get
			{
				return m_FacetName.Text;
			}
		}

		public int Mapfile
		{
			get
			{
				if ( Map1.Checked )
					return 1;
				if ( Map2.Checked )
					return 2;
				if ( Map3.Checked )
					return 3;
				if ( Map4.Checked )
					return 4;
				return 0;
			}
		}
	}
}
