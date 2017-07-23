using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace RegionEditor
{
	/// <summary>
	/// Summary description for FacetEditor.
	/// </summary>
	public class FacetPanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button ButtonAddRegion;
		private System.Windows.Forms.Button ButtonDelFacet;
		private System.Windows.Forms.CheckBox RegionFocus;
		private System.Windows.Forms.TextBox FacetName;
		private System.Windows.Forms.TextBox RegionName;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FacetPanel(string name)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			FacetName.Text = name;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.FacetName = new System.Windows.Forms.TextBox();
			this.ButtonAddRegion = new System.Windows.Forms.Button();
			this.RegionName = new System.Windows.Forms.TextBox();
			this.ButtonDelFacet = new System.Windows.Forms.Button();
			this.RegionFocus = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// FacetName
			// 
			this.FacetName.Location = new System.Drawing.Point(8, 8);
			this.FacetName.Name = "FacetName";
			this.FacetName.Size = new System.Drawing.Size(160, 20);
			this.FacetName.TabIndex = 0;
			this.FacetName.Text = "";
			this.FacetName.TextChanged += new System.EventHandler(this.FacetName_TextChanged);
			// 
			// ButtonAddRegion
			// 
			this.ButtonAddRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonAddRegion.Location = new System.Drawing.Point(240, 8);
			this.ButtonAddRegion.Name = "ButtonAddRegion";
			this.ButtonAddRegion.Size = new System.Drawing.Size(96, 23);
			this.ButtonAddRegion.TabIndex = 1;
			this.ButtonAddRegion.Text = "Add Region";
			this.ButtonAddRegion.Click += new System.EventHandler(this.ButtonAddRegion_Click);
			// 
			// RegionName
			// 
			this.RegionName.Location = new System.Drawing.Point(344, 8);
			this.RegionName.Name = "RegionName";
			this.RegionName.Size = new System.Drawing.Size(160, 20);
			this.RegionName.TabIndex = 2;
			this.RegionName.Text = "";
			// 
			// ButtonDelFacet
			// 
			this.ButtonDelFacet.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonDelFacet.Location = new System.Drawing.Point(8, 40);
			this.ButtonDelFacet.Name = "ButtonDelFacet";
			this.ButtonDelFacet.Size = new System.Drawing.Size(160, 23);
			this.ButtonDelFacet.TabIndex = 3;
			this.ButtonDelFacet.Text = "Delete Facet";
			this.ButtonDelFacet.Click += new System.EventHandler(this.ButtonDelFacet_Click);
			// 
			// RegionFocus
			// 
			this.RegionFocus.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.RegionFocus.Location = new System.Drawing.Point(344, 40);
			this.RegionFocus.Name = "RegionFocus";
			this.RegionFocus.Size = new System.Drawing.Size(160, 24);
			this.RegionFocus.TabIndex = 4;
			this.RegionFocus.Text = "Focus on new region";
			// 
			// FacetPanel
			// 
			this.Controls.Add(this.RegionFocus);
			this.Controls.Add(this.ButtonDelFacet);
			this.Controls.Add(this.RegionName);
			this.Controls.Add(this.ButtonAddRegion);
			this.Controls.Add(this.FacetName);
			this.Name = "FacetPanel";
			this.Size = new System.Drawing.Size(512, 72);
			this.ResumeLayout(false);

		}
		#endregion

		#region Events

		public event DeleteFacetEventHandler DeleteFacet;
		public event RenameFacetEventHandler RenameFacet;
		public event NewRegionEventHandler NewRegion;

		#endregion

		#region Event Handlers

		protected virtual void OnDeleteFacet( FacetEventArgs e )
		{
			DeleteFacet( this, e );
		}

		protected virtual void OnRenameFacet( FacetEventArgs e )
		{
			if ( Created )
				RenameFacet( this, e );
		}

		protected virtual void OnNewRegion( FacetEventArgs e )
		{
			NewRegion( this, e );
		}

		#endregion

		private void ButtonAddRegion_Click(object sender, System.EventArgs e)
		{
			if ( RegionName.Text.Length == 0 )
			{
				MessageBox.Show( "Please enter a name for the new region. The region name can't be empty" );
				return;
			}

			FacetEventArgs args = new FacetEventArgs( FacetActions.AddRegion, RegionName.Text, RegionFocus.Checked );

			OnNewRegion( args );
		}

		private void ButtonDelFacet_Click(object sender, System.EventArgs e)
		{
			if ( 
				MessageBox.Show(
				this,
				"This action will delete the current facet and all its regions, and it can't be undone. Are you sure you want to delete it?",
				"Confirm facet deletion",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question )
				== DialogResult.Yes )
			{
				FacetEventArgs args = new FacetEventArgs( FacetActions.DeleteFacet, "", false );

				OnDeleteFacet( args );
			}
		}

		private void FacetName_TextChanged(object sender, System.EventArgs e)
		{
			if ( FacetName.Text.Length == 0 )
				return;

			FacetEventArgs args = new FacetEventArgs( FacetActions.RenameFacet, FacetName.Text, false );

			OnRenameFacet( args );
		}
	}

	#region Delegates

	public delegate void DeleteFacetEventHandler( object sender, FacetEventArgs e );
	public delegate void RenameFacetEventHandler( object sender, FacetEventArgs e );
	public delegate void NewRegionEventHandler( object sender, FacetEventArgs e );

	#endregion

	#region FacetActions

	/// <summary>
	/// The action referenced by the FacetEventArgs
	/// </summary>
	public enum FacetActions
	{
		DeleteFacet,
		RenameFacet,
		AddRegion
	}

	#endregion

	#region FacetEventArgs

	public class FacetEventArgs : System.EventArgs
	{
        private string m_Name;

		/// <summary>
		/// This is the new name of the facet for the Rename action, or the name of the new region for the add region action
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
		}

		private FacetActions m_Action;

		/// <summary>
		/// Gets the facet action for this event args
		/// </summary>
		public FacetActions Action
		{
			get
			{
				return m_Action;
			}
		}

		private bool m_FocusRegion = false;

		/// <summary>
		/// Gets a value stating whether the program focus should go to the new region added
		/// </summary>
		public bool FocusRegion
		{
			get
			{
				return m_FocusRegion;
			}
		}

		/// <summary>
		/// Creates a new FacetEventArgs object
		/// </summary>
		/// <param name="action">The FacetActions value for this args</param>
		/// <param name="name">The name of the new region, or the new name of the facet, depending on the action</param>
		/// <param name="focus">States whether the program should focus on the new region if the action adds a new region</param>
		public FacetEventArgs( FacetActions action, string name, bool focus )
		{
			m_Action = action;
			m_Name = name;
			m_FocusRegion = focus;
		}
	}

	#endregion
}
