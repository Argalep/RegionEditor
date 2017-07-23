using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace RegionEditor
{
	/// <summary>
	/// Summary description for RectPanel.
	/// </summary>
	public class RectPanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.NumericUpDown NumX;
		private System.Windows.Forms.NumericUpDown NumY;
		private System.Windows.Forms.NumericUpDown NumWidth;
		private System.Windows.Forms.NumericUpDown NumHeight;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button ButtonDel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Size m_MapSize;

		private int m_X;
		private int m_Y;
		private int m_Width;
		private int m_Height;
        private int m_MinZ;
        private int m_MaxZ;
        private Label label7;
        private NumericUpDown NumMinZ;
        private Label label8;
        private NumericUpDown NumMaxZ;

		/// <summary>
		///  States whether the control is being programatically updated and shouldn't fire events
		/// </summary>
		private bool m_Updating = false;
		
		/// <summary>
		/// Gets or sets the X coordinate of the topleft corner of the rectangle
		/// </summary>
		public int X
		{
			get
			{
				return m_X;
			}
			set
			{
				if ( value == m_X )
					return;
				if ( ( value + m_Width ) >= m_MapSize.Width )
					m_X = m_MapSize.Width - m_Width - 1;
				else if ( value < 0 )
					m_X = 0;
				else
					m_X = value;
				NumX.Value = m_X;
			}
		}

		/// <summary>
		/// Gets or sets the Y coordinate of the topleft corner of the rectangle
		/// </summary>
		public int Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				if ( value == m_Y )
					return;
				if ( ( value + m_Height ) >= m_MapSize.Height )
					m_Y = m_MapSize.Height - m_Height - 1;
				else if ( value < 0 )
					m_Y = 0;
				else
					m_Y = value;
				NumY.Value = m_Y;
			}
		}

		/// <summary>
		/// Gets or sets the width of the rectangle
		/// </summary>
		public int RectWidth
		{
			get
			{
				return m_Width;
			}
			set
			{
				if ( value == m_Width )
					return;
				if ( m_X + value >= m_MapSize.Width )
					m_Width = m_MapSize.Width - m_X - 1;
				else
					m_Width = value;
				NumWidth.Value = m_Width;
			}
		}

        public int MinZ
        {
            get
            {
                return m_MinZ;
            }
            set
            {
                if (value < -128)
                    value = -128;
                if (value > 127)
                    value = 127;
                if (value == m_MinZ)

                m_MinZ = value;
            }
        }

        public int MaxZ
        {
            get
            {
                return m_MaxZ;
            }
            set
            {
                if (value < -128)
                    value = -128;
                if (value > 127)
                    value = 127;
                if (value == m_MinZ)

                    m_MaxZ = value;
            }
        }

		/// <summary>
		/// Gets or sets the height of the rectangle
		/// </summary>
		public int RectHeight
		{
			get
			{
				return m_Height;
			}
			set
			{
				if ( value == m_Height )
					return;
				if ( m_Y + value >= m_MapSize.Height )
					m_Height = m_MapSize.Height - m_Y - 1;
				else
					m_Height = value;
				NumHeight.Value = m_Height;
			}
		}

		/// <summary>
		/// Gets the rectangle
		/// </summary>
		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle( m_X, m_Y, m_Width, m_Height );
			}
			set
			{
				m_Updating = true;
				m_X = value.X;
				NumX.Value = m_X;
				m_Y = value.Y;
				NumY.Value = m_Y;
				m_Width = value.Width;
				NumWidth.Value = m_Width;
				m_Height = value.Height;
				NumHeight.Value = m_Height;
				m_Updating = false;
			}
		}

		public RectPanel( Size MapSize, MapRectangle3D rect )
		{
			m_Updating = true;

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_MapSize = MapSize;
			NumX.Maximum = MapSize.Width;
			NumY.Maximum = MapSize.Height;
			NumWidth.Maximum = MapSize.Width;
			NumHeight.Maximum = MapSize.Height;
			m_X = rect.Rectangle.X;
            NumX.Value = rect.Rectangle.X;
            m_Y = rect.Rectangle.Y;
            NumY.Value = rect.Rectangle.Y;
            m_Width = rect.Rectangle.Width;
            NumWidth.Value = rect.Rectangle.Width;
            m_Height = rect.Rectangle.Height;
            NumHeight.Value = rect.Rectangle.Height;

            NumMinZ.Value = rect.MinZ;
            m_MinZ = rect.MinZ;
            NumMaxZ.Value = rect.MaxZ;
            m_MaxZ = rect.MaxZ;

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
            this.NumWidth = new System.Windows.Forms.NumericUpDown();
            this.NumHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ButtonDel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.NumMinZ = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.NumMaxZ = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NumX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMinZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMaxZ)).BeginInit();
            this.SuspendLayout();
            // 
            // NumX
            // 
            this.NumX.Location = new System.Drawing.Point(32, 8);
            this.NumX.Name = "NumX";
            this.NumX.Size = new System.Drawing.Size(64, 20);
            this.NumX.TabIndex = 0;
            this.NumX.ValueChanged += new System.EventHandler(this.NumX_ValueChanged);
            // 
            // NumY
            // 
            this.NumY.Location = new System.Drawing.Point(32, 40);
            this.NumY.Name = "NumY";
            this.NumY.Size = new System.Drawing.Size(64, 20);
            this.NumY.TabIndex = 1;
            this.NumY.ValueChanged += new System.EventHandler(this.NumY_ValueChanged);
            // 
            // NumWidth
            // 
            this.NumWidth.Location = new System.Drawing.Point(160, 8);
            this.NumWidth.Name = "NumWidth";
            this.NumWidth.Size = new System.Drawing.Size(64, 20);
            this.NumWidth.TabIndex = 2;
            this.NumWidth.ValueChanged += new System.EventHandler(this.NumWidth_ValueChanged);
            // 
            // NumHeight
            // 
            this.NumHeight.Location = new System.Drawing.Point(160, 40);
            this.NumHeight.Name = "NumHeight";
            this.NumHeight.Size = new System.Drawing.Size(64, 20);
            this.NumHeight.TabIndex = 3;
            this.NumHeight.ValueChanged += new System.EventHandler(this.NumHeight_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(112, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Width";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(112, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Height";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ButtonDel
            // 
            this.ButtonDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonDel.Location = new System.Drawing.Point(384, 8);
            this.ButtonDel.Name = "ButtonDel";
            this.ButtonDel.Size = new System.Drawing.Size(120, 23);
            this.ButtonDel.TabIndex = 8;
            this.ButtonDel.Text = "Delete Rectangle";
            this.ButtonDel.Click += new System.EventHandler(this.ButtonDel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(245, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "MinZ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumMinZ
            // 
            this.NumMinZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumMinZ.Location = new System.Drawing.Point(282, 8);
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
            this.NumMinZ.TabIndex = 37;
            this.NumMinZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumMinZ.ValueChanged += new System.EventHandler(this.NumMinZ_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(242, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "MaxZ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumMaxZ
            // 
            this.NumMaxZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumMaxZ.Location = new System.Drawing.Point(282, 40);
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
            this.NumMaxZ.TabIndex = 35;
            this.NumMaxZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumMaxZ.ValueChanged += new System.EventHandler(this.NumMaxZ_ValueChanged);
            // 
            // RectPanel
            // 
            this.Controls.Add(this.label7);
            this.Controls.Add(this.NumMinZ);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.NumMaxZ);
            this.Controls.Add(this.ButtonDel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumHeight);
            this.Controls.Add(this.NumWidth);
            this.Controls.Add(this.NumY);
            this.Controls.Add(this.NumX);
            this.Name = "RectPanel";
            this.Size = new System.Drawing.Size(512, 108);
            ((System.ComponentModel.ISupportInitialize)(this.NumX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMinZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMaxZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public event RectangleEventHandler RectangleChanged;

		protected void OnRectangleChanged( RectangleEventArgs e )
		{
			RectangleChanged( this, e );
		}

		private void ButtonDel_Click(object sender, System.EventArgs e)
		{
            RectangleEventArgs ev = RectangleEventArgs.DeleteRect();

			OnRectangleChanged( ev );
		}

		private void ChangeRect()
		{
			RectangleEventArgs e = RectangleEventArgs.UpdateRect(this.Rectangle);

			OnRectangleChanged( e );
		}

		private void NumX_ValueChanged(object sender, System.EventArgs e)
		{
			X = (int) NumX.Value;

			if ( !m_Updating )
				ChangeRect();
		}

		private void NumY_ValueChanged(object sender, System.EventArgs e)
		{
			Y = (int) NumY.Value;

			if ( !m_Updating )
				ChangeRect();
		}

		private void NumWidth_ValueChanged(object sender, System.EventArgs e)
		{
			RectWidth = (int) NumWidth.Value;

			if ( !m_Updating )
				ChangeRect();
		}

		private void NumHeight_ValueChanged(object sender, System.EventArgs e)
		{
			RectHeight = (int) NumHeight.Value;

			if ( !m_Updating )
				ChangeRect();
		}

        private void ChangeZ()
        {
            RectangleEventArgs e = RectangleEventArgs.UpdateZ(MinZ,MaxZ);

            OnRectangleChanged(e);
        }

        private void NumMinZ_ValueChanged(object sender, EventArgs e)
        {
            m_MinZ = (int)NumMinZ.Value;

            if (!m_Updating)
                ChangeZ();
        }

        private void NumMaxZ_ValueChanged(object sender, EventArgs e)
        {
            m_MaxZ = (int)NumMaxZ.Value;

            if (!m_Updating)
                ChangeZ();
        }
	}

	public delegate void RectangleEventHandler( object sender, RectangleEventArgs e );

    public enum RectangleActions
    {
        Delete,
        UpdateRect,
        UpdateZ
    }

	public class RectangleEventArgs : EventArgs
	{
        private RectangleActions m_Action;
		private Rectangle m_Rectangle;
        private int m_MinZ;
        private int m_MaxZ;

        public RectangleActions Action
        {
            get { return m_Action; }
        }

		public Rectangle Rectangle
		{
			get { return m_Rectangle; }
		}

        public int MinZ
        {
            get { return m_MinZ; }
        }

        public int MaxZ
        {
            get { return m_MaxZ; }
        }

        public static RectangleEventArgs DeleteRect()
        {
            RectangleEventArgs e = new RectangleEventArgs();
            e.m_Action = RectangleActions.Delete;
            return e;
        }

        public static RectangleEventArgs UpdateRect(Rectangle rect)
        {
            RectangleEventArgs e = new RectangleEventArgs();
            e.m_Action = RectangleActions.UpdateRect;
            e.m_Rectangle = rect;
            return e;
        }

        public static RectangleEventArgs UpdateZ(int minZ, int maxZ)
        {
            RectangleEventArgs e = new RectangleEventArgs();
            e.m_Action = RectangleActions.UpdateZ;
            e.m_MinZ = minZ;
            e.m_MaxZ = maxZ;
            return e;
        }

		public RectangleEventArgs()
		{
		}
	}
}
