using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace RegionEditor
{
	/// <summary>
	/// Summary description for RegionPanel.
	/// </summary>
	public class RegionPanel : System.Windows.Forms.UserControl
	{
		private int m_MapWidth;
		private int m_MapHeight;

		private bool m_Updating = false;

		private System.Windows.Forms.NumericUpDown NumX;
		private System.Windows.Forms.NumericUpDown NumY;
		private System.Windows.Forms.NumericUpDown NumZ;
		private System.Windows.Forms.TextBox TextBoxSubregionName;
		private System.Windows.Forms.Button ButtonAddSubregion;
		private System.Windows.Forms.Button ButtonDelRegion;
		private System.Windows.Forms.Button ButtonClearRegion;
		private System.Windows.Forms.TextBox TextBoxRegionName;
		private System.Windows.Forms.CheckBox CheckBoxSet;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown NumPriority;
		private System.Windows.Forms.Label label2;
        private TextBox TextBoxTypeName;
        private Label label3;
        private Label label4;
        private TextBox TextBoxMusicName;
        private Label label5;
        private TextBox TextBoxRuneName;
        private Label label6;
        private NumericUpDown NumMaxZ;
        private Label label8;
        private CheckBox checkBoxSmartNoHousing;
        private CheckBox checkBoxLogoutDelay;
        private CheckBox checkBoxGuardsDisabled;
        private Label label7;
        private NumericUpDown NumMinZ;
        private Button ButtonResetGoLoc;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RegionPanel( int mapWidth, int mapHeight, MapRegion region )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_Updating = true;

			m_MapWidth = mapWidth;
			m_MapHeight = mapHeight;

			NumX.Maximum = mapWidth;
			NumY.Maximum = mapHeight;

			TextBoxRegionName.Text = region.Name;
            TextBoxTypeName.Text = region.TypeName;
            TextBoxRuneName.Text = region.RuneName;
            TextBoxMusicName.Text = region.MusicName;

            checkBoxSmartNoHousing.Visible = false;
            checkBoxGuardsDisabled.Visible = false;

            if (region.LogoutDelayActive == XmlBool.False)
                checkBoxLogoutDelay.Checked = false;
            else
                checkBoxLogoutDelay.Checked = true;

            if (region.TypeName == "NoHousingRegion")
            {
                checkBoxSmartNoHousing.Visible = true;
                if (region.SmartNoHousing == XmlBool.True)
                    checkBoxSmartNoHousing.Checked = true;
                else
                    checkBoxSmartNoHousing.Checked = false;
            }
            else if (region.TypeName == "GuardedRegion" || region.TypeName == "TownRegion")
            {
                checkBoxGuardsDisabled.Visible = true;
                if (region.GuardsDisabled == XmlBool.True)
                    checkBoxGuardsDisabled.Checked = true;
                else
                    checkBoxGuardsDisabled.Checked = false;
            }

			try
			{
				NumX.Value = region.GoLocation.X;
			}
			catch
			{
				NumX.Value = NumX.Minimum;
			}
			try
			{
                NumY.Value = region.GoLocation.Y;
			}
			catch
			{
				NumY.Value = NumY.Minimum;
			}
			try
			{
                NumZ.Value = region.GoLocation.Z;
			}
            catch
            {
                NumY.Value = 0;
            }

            try
            {
                NumMinZ.Value = region.MinZ;
            }
            catch
            {
                NumMinZ.Value = MapRegion.DefaultMinZ;
            }
            try
            {
                NumMaxZ.Value = region.MaxZ;
            }
            catch
            {
                NumMaxZ.Value = MapRegion.DefaultMaxZ;
            }
			

            try
            {
                NumPriority.Value = region.Priority;
            }
            catch
            {
                NumPriority.Value = 50;
            }

			m_Updating = false;
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
            this.NumX = new System.Windows.Forms.NumericUpDown();
            this.NumY = new System.Windows.Forms.NumericUpDown();
            this.NumZ = new System.Windows.Forms.NumericUpDown();
            this.TextBoxSubregionName = new System.Windows.Forms.TextBox();
            this.ButtonAddSubregion = new System.Windows.Forms.Button();
            this.ButtonDelRegion = new System.Windows.Forms.Button();
            this.ButtonClearRegion = new System.Windows.Forms.Button();
            this.TextBoxRegionName = new System.Windows.Forms.TextBox();
            this.CheckBoxSet = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NumPriority = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxTypeName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxMusicName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxRuneName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.NumMaxZ = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxSmartNoHousing = new System.Windows.Forms.CheckBox();
            this.checkBoxLogoutDelay = new System.Windows.Forms.CheckBox();
            this.checkBoxGuardsDisabled = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NumMinZ = new System.Windows.Forms.NumericUpDown();
            this.ButtonResetGoLoc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NumX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMaxZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMinZ)).BeginInit();
            this.SuspendLayout();
            // 
            // NumX
            // 
            this.NumX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumX.Location = new System.Drawing.Point(309, 7);
            this.NumX.Name = "NumX";
            this.NumX.Size = new System.Drawing.Size(48, 20);
            this.NumX.TabIndex = 1;
            this.NumX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumX.ValueChanged += new System.EventHandler(this.NumX_ValueChanged);
            // 
            // NumY
            // 
            this.NumY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumY.Location = new System.Drawing.Point(363, 7);
            this.NumY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NumY.Name = "NumY";
            this.NumY.Size = new System.Drawing.Size(48, 20);
            this.NumY.TabIndex = 2;
            this.NumY.Value = new decimal(new int[] {
            5699,
            0,
            0,
            0});
            this.NumY.ValueChanged += new System.EventHandler(this.NumY_ValueChanged);
            // 
            // NumZ
            // 
            this.NumZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumZ.Location = new System.Drawing.Point(417, 7);
            this.NumZ.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NumZ.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.NumZ.Name = "NumZ";
            this.NumZ.Size = new System.Drawing.Size(48, 20);
            this.NumZ.TabIndex = 3;
            this.NumZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumZ.ValueChanged += new System.EventHandler(this.NumZ_ValueChanged);
            // 
            // TextBoxSubregionName
            // 
            this.TextBoxSubregionName.Location = new System.Drawing.Point(97, 105);
            this.TextBoxSubregionName.Name = "TextBoxSubregionName";
            this.TextBoxSubregionName.Size = new System.Drawing.Size(160, 20);
            this.TextBoxSubregionName.TabIndex = 4;
            // 
            // ButtonAddSubregion
            // 
            this.ButtonAddSubregion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonAddSubregion.Location = new System.Drawing.Point(3, 102);
            this.ButtonAddSubregion.Name = "ButtonAddSubregion";
            this.ButtonAddSubregion.Size = new System.Drawing.Size(88, 23);
            this.ButtonAddSubregion.TabIndex = 6;
            this.ButtonAddSubregion.Text = "Add Subregion";
            this.ButtonAddSubregion.Click += new System.EventHandler(this.ButtonAddSubsection_Click);
            // 
            // ButtonDelRegion
            // 
            this.ButtonDelRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonDelRegion.Location = new System.Drawing.Point(263, 102);
            this.ButtonDelRegion.Name = "ButtonDelRegion";
            this.ButtonDelRegion.Size = new System.Drawing.Size(94, 23);
            this.ButtonDelRegion.TabIndex = 7;
            this.ButtonDelRegion.Text = "Delete Region";
            this.ButtonDelRegion.Click += new System.EventHandler(this.ButtonDelRegion_Click);
            // 
            // ButtonClearRegion
            // 
            this.ButtonClearRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonClearRegion.Location = new System.Drawing.Point(362, 102);
            this.ButtonClearRegion.Name = "ButtonClearRegion";
            this.ButtonClearRegion.Size = new System.Drawing.Size(86, 23);
            this.ButtonClearRegion.TabIndex = 8;
            this.ButtonClearRegion.Text = "Clear Region";
            this.ButtonClearRegion.Click += new System.EventHandler(this.ButtonClearRegion_Click);
            // 
            // TextBoxRegionName
            // 
            this.TextBoxRegionName.Location = new System.Drawing.Point(97, 7);
            this.TextBoxRegionName.Name = "TextBoxRegionName";
            this.TextBoxRegionName.Size = new System.Drawing.Size(160, 20);
            this.TextBoxRegionName.TabIndex = 9;
            this.TextBoxRegionName.TextChanged += new System.EventHandler(this.TextBoxRegionName_TextChanged);
            // 
            // CheckBoxSet
            // 
            this.CheckBoxSet.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBoxSet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CheckBoxSet.Location = new System.Drawing.Point(471, 7);
            this.CheckBoxSet.Name = "CheckBoxSet";
            this.CheckBoxSet.Size = new System.Drawing.Size(48, 24);
            this.CheckBoxSet.TabIndex = 11;
            this.CheckBoxSet.Text = "Set";
            this.CheckBoxSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBoxSet.Click += new System.EventHandler(this.CheckBoxSet_Click);
            this.CheckBoxSet.CheckedChanged += new System.EventHandler(this.CheckBoxSet_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Region";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumPriority
            // 
            this.NumPriority.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumPriority.Location = new System.Drawing.Point(309, 76);
            this.NumPriority.Name = "NumPriority";
            this.NumPriority.Size = new System.Drawing.Size(48, 20);
            this.NumPriority.TabIndex = 13;
            this.NumPriority.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumPriority.ValueChanged += new System.EventHandler(this.NumPriority_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Priority";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxTypeName
            // 
            this.TextBoxTypeName.Location = new System.Drawing.Point(97, 30);
            this.TextBoxTypeName.Name = "TextBoxTypeName";
            this.TextBoxTypeName.Size = new System.Drawing.Size(160, 20);
            this.TextBoxTypeName.TabIndex = 15;
            this.TextBoxTypeName.TextChanged += new System.EventHandler(this.TextBoxTypeName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Music";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxMusicName
            // 
            this.TextBoxMusicName.Location = new System.Drawing.Point(97, 76);
            this.TextBoxMusicName.Name = "TextBoxMusicName";
            this.TextBoxMusicName.Size = new System.Drawing.Size(160, 20);
            this.TextBoxMusicName.TabIndex = 17;
            this.TextBoxMusicName.TextChanged += new System.EventHandler(this.TextBoxMusicName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Rune Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxRuneName
            // 
            this.TextBoxRuneName.Location = new System.Drawing.Point(97, 53);
            this.TextBoxRuneName.Name = "TextBoxRuneName";
            this.TextBoxRuneName.Size = new System.Drawing.Size(160, 20);
            this.TextBoxRuneName.TabIndex = 19;
            this.TextBoxRuneName.TextChanged += new System.EventHandler(this.TextBoxRuneName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(263, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Go Loc";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumMaxZ
            // 
            this.NumMaxZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumMaxZ.Location = new System.Drawing.Point(309, 53);
            this.NumMaxZ.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NumMaxZ.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.NumMaxZ.Name = "NumMaxZ";
            this.NumMaxZ.Size = new System.Drawing.Size(48, 20);
            this.NumMaxZ.TabIndex = 22;
            this.NumMaxZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumMaxZ.ValueChanged += new System.EventHandler(this.NumMaxZ_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(273, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "MaxZ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxSmartNoHousing
            // 
            this.checkBoxSmartNoHousing.AutoSize = true;
            this.checkBoxSmartNoHousing.Location = new System.Drawing.Point(363, 55);
            this.checkBoxSmartNoHousing.Name = "checkBoxSmartNoHousing";
            this.checkBoxSmartNoHousing.Size = new System.Drawing.Size(106, 17);
            this.checkBoxSmartNoHousing.TabIndex = 30;
            this.checkBoxSmartNoHousing.Text = "SmartNoHousing";
            this.checkBoxSmartNoHousing.UseVisualStyleBackColor = true;
            this.checkBoxSmartNoHousing.CheckedChanged += new System.EventHandler(this.checkBoxSmartNoHousing_CheckedChanged);
            // 
            // checkBoxLogoutDelay
            // 
            this.checkBoxLogoutDelay.AutoSize = true;
            this.checkBoxLogoutDelay.Location = new System.Drawing.Point(363, 33);
            this.checkBoxLogoutDelay.Name = "checkBoxLogoutDelay";
            this.checkBoxLogoutDelay.Size = new System.Drawing.Size(86, 17);
            this.checkBoxLogoutDelay.TabIndex = 31;
            this.checkBoxLogoutDelay.Text = "LogoutDelay";
            this.checkBoxLogoutDelay.UseVisualStyleBackColor = true;
            this.checkBoxLogoutDelay.CheckedChanged += new System.EventHandler(this.checkBoxLogoutDelay_CheckedChanged);
            // 
            // checkBoxGuardsDisabled
            // 
            this.checkBoxGuardsDisabled.AutoSize = true;
            this.checkBoxGuardsDisabled.Location = new System.Drawing.Point(363, 78);
            this.checkBoxGuardsDisabled.Name = "checkBoxGuardsDisabled";
            this.checkBoxGuardsDisabled.Size = new System.Drawing.Size(101, 17);
            this.checkBoxGuardsDisabled.TabIndex = 32;
            this.checkBoxGuardsDisabled.Text = "GuardsDisabled";
            this.checkBoxGuardsDisabled.UseVisualStyleBackColor = true;
            this.checkBoxGuardsDisabled.CheckedChanged += new System.EventHandler(this.checkBoxGuardsDisabled_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(276, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "MinZ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumMinZ
            // 
            this.NumMinZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumMinZ.Location = new System.Drawing.Point(309, 30);
            this.NumMinZ.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NumMinZ.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.NumMinZ.Name = "NumMinZ";
            this.NumMinZ.Size = new System.Drawing.Size(48, 20);
            this.NumMinZ.TabIndex = 33;
            this.NumMinZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumMinZ.ValueChanged += new System.EventHandler(this.NumMinZ_ValueChanged);
            // 
            // ButtonResetGoLoc
            // 
            this.ButtonResetGoLoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonResetGoLoc.Location = new System.Drawing.Point(454, 102);
            this.ButtonResetGoLoc.Name = "ButtonResetGoLoc";
            this.ButtonResetGoLoc.Size = new System.Drawing.Size(86, 23);
            this.ButtonResetGoLoc.TabIndex = 35;
            this.ButtonResetGoLoc.Text = "Reset Go Loc";
            this.ButtonResetGoLoc.Click += new System.EventHandler(this.ButtonResetGoLoc_Click);
            // 
            // RegionPanel
            // 
            this.Controls.Add(this.ButtonResetGoLoc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.NumMinZ);
            this.Controls.Add(this.checkBoxGuardsDisabled);
            this.Controls.Add(this.checkBoxLogoutDelay);
            this.Controls.Add(this.checkBoxSmartNoHousing);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.NumMaxZ);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TextBoxRuneName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TextBoxMusicName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBoxTypeName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NumPriority);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBoxSet);
            this.Controls.Add(this.TextBoxRegionName);
            this.Controls.Add(this.ButtonClearRegion);
            this.Controls.Add(this.ButtonDelRegion);
            this.Controls.Add(this.ButtonAddSubregion);
            this.Controls.Add(this.TextBoxSubregionName);
            this.Controls.Add(this.NumZ);
            this.Controls.Add(this.NumY);
            this.Controls.Add(this.NumX);
            this.Name = "RegionPanel";
            this.Size = new System.Drawing.Size(560, 128);
            ((System.ComponentModel.ISupportInitialize)(this.NumX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMaxZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMinZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public new event RegionChangedEventHandler RegionChanged;

		protected virtual void OnRegionChanged( RegionEventArgs e )
		{
			RegionChanged( this, e );
		}

		private void CheckBoxSet_Click(object sender, System.EventArgs e)
		{
			OnRegionChanged( RegionEventArgs.SetGo() );
		}

		private void TextBoxRegionName_TextChanged(object sender, System.EventArgs e)
		{
			if ( TextBoxRegionName.Text.Length == 0 )
				return;

			if ( !m_Updating )
				OnRegionChanged( RegionEventArgs.ChangeName( TextBoxRegionName.Text ) );
		}

		private void NumPriority_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int) NumPriority.Value;

			if ( val > 100 )
				val = 100;
			if ( val < 0 )
				val = 0;

			if ( !m_Updating )
				OnRegionChanged( RegionEventArgs.PriorityChange( val ) );
		}

		private void CheckBoxSet_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( CheckBoxSet.Checked )
			{
				if ( !m_Updating )
					OnRegionChanged( RegionEventArgs.SetGo() );
			}
		}

		private void ButtonDelRegion_Click(object sender, System.EventArgs e)
		{
			if ( MessageBox.Show( 
				this,
				"This will delete the current region from its facet. Are you sure?",
				"Confirm region deletion",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == DialogResult.Yes )
				OnRegionChanged( RegionEventArgs.Delete() );
		}

		private void ButtonClearRegion_Click(object sender, System.EventArgs e)
		{
			if ( MessageBox.Show( 
				this,
				"This will remove all the rectangles and subregions from the current region. Are you sure?",
				"Confirm region clear",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == DialogResult.Yes )
				OnRegionChanged( RegionEventArgs.Clear() );
		}

		private void ButtonAddSubsection_Click(object sender, System.EventArgs e)
		{
			if ( TextBoxSubregionName.Text.Length == 0 )
			{
				MessageBox.Show( "Please enter a name for the subsection" );
				return;
			}

			OnRegionChanged( RegionEventArgs.NewSubregion( TextBoxSubregionName.Text ) );
		}

		private void ChangePos()
		{
			int x = (int) NumX.Value;
			int y = (int) NumY.Value;
			int z = (int) NumZ.Value;

			if ( x < 0 )
				x = 0;
			if ( y < 0 )
				y = 0;
			if ( z < -128 )
				z = -128;

			if ( x >= m_MapWidth )
				x = m_MapWidth - 1;
			if ( y >= m_MapHeight )
				y = m_MapHeight - 1;
			if ( z > 127 )
				z = 127;

			if ( !m_Updating )
				OnRegionChanged( RegionEventArgs.ChangeGo( x, y, z ) );
		}

		private void NumX_ValueChanged(object sender, System.EventArgs e)
		{
			ChangePos();
		}

		private void NumY_ValueChanged(object sender, System.EventArgs e)
		{
			ChangePos();
		}

		private void NumZ_ValueChanged(object sender, System.EventArgs e)
		{
			ChangePos();
		}

		public void UpdateGoLocation( int x, int y, int z )
		{
			m_Updating = true;

			NumX.Value = x;
			NumY.Value = y;
			NumZ.Value = z;

			CheckBoxSet.Checked = false;

			m_Updating = false;		
		}

        private void TextBoxTypeName_TextChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeType(TextBoxTypeName.Text));
        }

        private void TextBoxRuneName_TextChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeRuneName(TextBoxRuneName.Text));
        }

        private void TextBoxMusicName_TextChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeMusic(TextBoxMusicName.Text));
        }

        private void NumMinZ_ValueChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeZ((int)NumMinZ.Value, (int)NumMaxZ.Value));
        }

        private void NumMaxZ_ValueChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeZ((int)NumMinZ.Value, (int)NumMaxZ.Value));
        }

        private void checkBoxLogoutDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeLogoutDelay(checkBoxLogoutDelay.Checked));
        }

        private void checkBoxSmartNoHousing_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeSmartNoHousing(checkBoxSmartNoHousing.Checked));
        }

        private void checkBoxGuardsDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_Updating)
                OnRegionChanged(RegionEventArgs.ChangeGuardsDisabled(checkBoxGuardsDisabled.Checked));
        }

        private void ButtonResetGoLoc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                this,
                "This will reset the go location to it's default location. Are you sure?",
                "Confirm reset go location",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                OnRegionChanged(RegionEventArgs.ClearGoLoc());
        }
	}

	public enum RegionActions
	{
		SetGo,
		ChangeGo,
		RenameRegion,
		DeleteRegion,
		ClearRegion,
        AddSubregion,
		ChangePriority,
        ChangeTypeName,
        ChangeZ,
        ChangeMusicName,
        ChangeRuneName,
        ChangeLogoutDelay,
        ChangeGuardsDisabled,
        ChangeSmartNoHousing,
        ResetGoLoc
	}

	public delegate void RegionChangedEventHandler( object sender, RegionEventArgs e );

	public class RegionEventArgs : EventArgs
	{
		private RegionActions m_Action;
		private int m_X;
		private int m_Y;
		private int m_Z;
		private string m_Name;
        private string m_TypeName;
        private string m_MusicName;
        private string m_RuneName;
        private int m_MinZ;
        private int m_MaxZ;
        private bool m_LogoutDelay;
        private bool m_GuardsDisabled;
        private bool m_SmartNoHousing;

		public RegionActions Action { get { return m_Action; } }

		public object Data
		{
			get
			{
				switch ( m_Action )
				{
					case RegionActions.RenameRegion:
                    case RegionActions.AddSubregion:
						return m_Name;
					case RegionActions.ChangeGo:
						return new int[] { m_X, m_Y, m_Z };
					case RegionActions.ChangePriority:
						return m_X;
                    case RegionActions.ChangeTypeName:
                        return m_TypeName;
                    case RegionActions.ChangeZ:
                        return new int[] { m_MinZ, m_MaxZ };
                    case RegionActions.ChangeRuneName:
                        return m_RuneName;
                    case RegionActions.ChangeMusicName:
                        return m_MusicName;
                    case RegionActions.ChangeLogoutDelay:
                        return m_LogoutDelay;
                    case RegionActions.ChangeGuardsDisabled:
                        return m_GuardsDisabled;
                    case RegionActions.ChangeSmartNoHousing:
                        return m_SmartNoHousing;
					default:
						return null;
				}
			}
		}

		private RegionEventArgs()
		{
		}

		public static RegionEventArgs ChangeName( string newName )
		{
			RegionEventArgs e = new RegionEventArgs();
			e.m_Action = RegionActions.RenameRegion;
			e.m_Name = newName;
			return e;
		}

		public static RegionEventArgs NewSubregion( string subregionName )
		{
			RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.AddSubregion;
			e.m_Name = subregionName;
			return e;
		}

        public static RegionEventArgs ChangeType( string newType)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeTypeName;
            e.m_TypeName = newType;
            return e;
        }

        public static RegionEventArgs ChangeMusic(string newMusic)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeMusicName;
            e.m_MusicName = newMusic;
            return e;
        }

        public static RegionEventArgs ChangeRuneName(string newRuneName)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeRuneName;
            e.m_RuneName = newRuneName;
            return e;
        }

		public static RegionEventArgs ChangeGo(int x, int y, int z)
		{
			RegionEventArgs e = new RegionEventArgs();
			e.m_Action = RegionActions.ChangeGo;
			e.m_X = x;
			e.m_Y = y;
			e.m_Z = z;
			return e;
		}

        public static RegionEventArgs ChangeZ(int minZ, int maxZ)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeZ;
            e.m_MinZ = minZ;
            e.m_MaxZ = maxZ;
            return e;
        }

        public static RegionEventArgs ChangeLogoutDelay(bool state)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeLogoutDelay;
            e.m_LogoutDelay = state;
            return e;
        }

        public static RegionEventArgs ChangeGuardsDisabled(bool state)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeGuardsDisabled;
            e.m_GuardsDisabled = state;
            return e;
        }

        public static RegionEventArgs ChangeSmartNoHousing(bool state)
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ChangeSmartNoHousing;
            e.m_SmartNoHousing = state;
            return e;
        }

		public static RegionEventArgs SetGo()
		{
			RegionEventArgs e = new RegionEventArgs();
			e.m_Action = RegionActions.SetGo;
			return e;
		}

		public static RegionEventArgs Delete()
		{
			RegionEventArgs e = new RegionEventArgs();
			e.m_Action = RegionActions.DeleteRegion;
			return e;
		}

		public static RegionEventArgs Clear()
		{
			RegionEventArgs e = new RegionEventArgs();
			e.m_Action = RegionActions.ClearRegion;
			return e;
		}

        public static RegionEventArgs ClearGoLoc()
        {
            RegionEventArgs e = new RegionEventArgs();
            e.m_Action = RegionActions.ResetGoLoc;
            return e;
        }

		public static RegionEventArgs PriorityChange( int newPriority )
		{
			RegionEventArgs e = new RegionEventArgs();
			e.m_X = newPriority;
			e.m_Action = RegionActions.ChangePriority;
			return e;
		}
	}
}