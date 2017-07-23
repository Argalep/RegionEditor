using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using TheBox.MapViewer;
using TheBox.MapViewer.DrawObjects;
using System.Collections.Generic;

namespace RegionEditor
{
    /*
     * Data reference
     * 
     * Tree: Each node is a facet
     * The Tag for a facet node is the map number (0,1,2,3)
     * 
     * Each region node has the Region object as Tag.
     * Each subnode of a region node is an item in the m_Subsections array list
     */

    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class RegionEditor : System.Windows.Forms.Form
    {
        private System.Windows.Forms.MainMenu TheMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.TreeView Tree;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.ListBox RectList;
        private System.Windows.Forms.Button ButtonZoomIn;
        private System.Windows.Forms.Button ButtonZoomOut;
        private TheBox.MapViewer.MapViewer Map;
        private System.Windows.Forms.Button ButtonAddFacet;
        private System.Windows.Forms.MenuItem FileNew;
        private System.Windows.Forms.MenuItem FileOpen;
        private System.Windows.Forms.MenuItem FileSaveAs;
        private System.Windows.Forms.MenuItem FileExit;
        private System.Windows.Forms.MenuItem menuAlwaysOnTop;
        private System.Windows.Forms.MenuItem menuMap0;
        private System.Windows.Forms.MenuItem menuMap1;
        private System.Windows.Forms.MenuItem menuMap2;
        private System.Windows.Forms.MenuItem menuMap3;
        private System.Windows.Forms.MenuItem menuChangeMulPath;
        private System.Windows.Forms.MenuItem menuDrawStatics;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.SaveFileDialog SaveFile;
        private System.Windows.Forms.ContextMenu FacetMenu;
        private System.Windows.Forms.MenuItem MenuFacetMapfile;
        private System.Windows.Forms.MenuItem mFacetMap0;
        private System.Windows.Forms.MenuItem mFacetMap1;
        private System.Windows.Forms.MenuItem mFacetMap2;
        private System.Windows.Forms.MenuItem mFacetMap3;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuMap4;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusText;
        private MenuItem menuXRay;
        private MenuItem menuMap5;
        private MenuItem mFacetMap4;
        private MenuItem mFacetMap5;
        private IContainer components;

        public RegionEditor()
        {
            // Read options
            m_Options = RegEdOptions.Load();

            // Load the path options
            SetOptions();

            if (TheBox.MapViewer.MapViewer.MulFileManager.DefaultFolder == null || !TheBox.Common.Utility.ValidateUOFolder(TheBox.MapViewer.MapViewer.MulFileManager.DefaultFolder))
            {
                //we couldn't resolve the default folder so we must use the custom folder, is it set up?
                if (TheBox.MapViewer.MapViewer.MulFileManager.CustomFolder == null || !TheBox.Common.Utility.ValidateUOFolder(TheBox.MapViewer.MapViewer.MulFileManager.CustomFolder))
                {
                    //it isn't, so let's ask the user for the folder...
                    FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                    folderBrowser.Description = "Select the folder containing Ultima's Online map files.";
                    folderBrowser.ShowNewFolderButton = false;
                    if (folderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        TheBox.MapViewer.MapViewer.MulFileManager.CustomFolder = folderBrowser.SelectedPath;
                        m_Options.ClientPath = folderBrowser.SelectedPath;

                        //do we have a valid folder now?
                        if (TheBox.MapViewer.MapViewer.MulFileManager.CustomFolder == null || !TheBox.Common.Utility.ValidateUOFolder(TheBox.MapViewer.MapViewer.MulFileManager.CustomFolder))
                        {
                            MessageBox.Show("Couldn't find Ultima Online's map files.");
                            System.Environment.Exit(0);
                            return;
                        }
                    }
                    else
                    {
                        System.Environment.Exit(0);
                        return;
                    }
                }
            }

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Colors
            RectFill = Color.FromArgb(ColorAlpha, RectBorder);
            SelectedRectFill = Color.FromArgb(ColorAlpha, SelectedRectBorder);
            DrawRectFill = Color.FromArgb(ColorAlpha, DrawRectBorder);

            // Displayed rectangles
            m_DisplayedRects = new ArrayList();

            // Reset options now that we have the map
            SetOptions();
        }

        #region Colors

        private int ColorAlpha = 80;

        private Color RectBorder = Color.LightYellow;
        private Color RectFill;

        private Color SelectedRectBorder = Color.Gold;
        private Color SelectedRectFill;

        private Color DrawRectBorder = Color.Violet;
        private Color DrawRectFill;

        private Color CrossHairColor = Color.Gainsboro;

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegionEditor));
            TheBox.Common.MulManager mulManager1 = new TheBox.Common.MulManager();
            this.Map = new TheBox.MapViewer.MapViewer();
            this.TheMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.FileNew = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.FileOpen = new System.Windows.Forms.MenuItem();
            this.FileSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.FileExit = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuAlwaysOnTop = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuMap0 = new System.Windows.Forms.MenuItem();
            this.menuMap1 = new System.Windows.Forms.MenuItem();
            this.menuMap2 = new System.Windows.Forms.MenuItem();
            this.menuMap3 = new System.Windows.Forms.MenuItem();
            this.menuMap4 = new System.Windows.Forms.MenuItem();
            this.menuMap5 = new System.Windows.Forms.MenuItem();
            this.menuChangeMulPath = new System.Windows.Forms.MenuItem();
            this.menuDrawStatics = new System.Windows.Forms.MenuItem();
            this.menuXRay = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.Tree = new System.Windows.Forms.TreeView();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.RectList = new System.Windows.Forms.ListBox();
            this.ButtonZoomIn = new System.Windows.Forms.Button();
            this.ButtonZoomOut = new System.Windows.Forms.Button();
            this.ButtonAddFacet = new System.Windows.Forms.Button();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.FacetMenu = new System.Windows.Forms.ContextMenu();
            this.MenuFacetMapfile = new System.Windows.Forms.MenuItem();
            this.mFacetMap0 = new System.Windows.Forms.MenuItem();
            this.mFacetMap1 = new System.Windows.Forms.MenuItem();
            this.mFacetMap2 = new System.Windows.Forms.MenuItem();
            this.mFacetMap3 = new System.Windows.Forms.MenuItem();
            this.mFacetMap4 = new System.Windows.Forms.MenuItem();
            this.mFacetMap5 = new System.Windows.Forms.MenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Map
            // 
            this.Map.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Map.Center = new System.Drawing.Point(0, 0);
            this.Map.DisplayErrors = true;
            this.Map.DrawStatics = true;
            this.Map.Location = new System.Drawing.Point(232, 136);
            this.Map.Map = TheBox.MapViewer.Maps.Tokuno;
            mulManager1.CustomFolder = null;
            mulManager1.Table = null;
            this.Map.MulManager = mulManager1;
            this.Map.Name = "Map";
            this.Map.Navigation = TheBox.MapViewer.MapNavigation.None;
            this.Map.RotateView = false;
            this.Map.ShowCross = false;
            this.Map.Size = new System.Drawing.Size(560, 340);
            this.Map.TabIndex = 0;
            this.Map.WheelZoom = true;
            this.Map.XRayView = false;
            this.Map.ZoomLevel = 0;
            this.Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            this.Map.MouseLeave += new System.EventHandler(this.Map_MouseLeave);
            this.Map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_MouseMove);
            this.Map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
            // 
            // TheMenu
            // 
            this.TheMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem9,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileNew,
            this.menuItem6,
            this.FileOpen,
            this.FileSaveAs,
            this.menuItem7,
            this.FileExit});
            this.menuItem1.Text = "File";
            // 
            // FileNew
            // 
            this.FileNew.Index = 0;
            this.FileNew.Text = "New";
            this.FileNew.Click += new System.EventHandler(this.FileNew_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
            // 
            // FileOpen
            // 
            this.FileOpen.Index = 2;
            this.FileOpen.Text = "Open";
            this.FileOpen.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // FileSaveAs
            // 
            this.FileSaveAs.Index = 3;
            this.FileSaveAs.Text = "Save As..";
            this.FileSaveAs.Click += new System.EventHandler(this.FileSaveAs_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "-";
            // 
            // FileExit
            // 
            this.FileExit.Index = 5;
            this.FileExit.Text = "Exit";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 1;
            this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAlwaysOnTop,
            this.menuItem11,
            this.menuChangeMulPath,
            this.menuDrawStatics,
            this.menuXRay});
            this.menuItem9.Text = "Options";
            this.menuItem9.Popup += new System.EventHandler(this.menuItem9_Popup);
            // 
            // menuAlwaysOnTop
            // 
            this.menuAlwaysOnTop.Index = 0;
            this.menuAlwaysOnTop.Text = "Always On Top";
            this.menuAlwaysOnTop.Click += new System.EventHandler(this.menuAlwaysOnTop_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMap0,
            this.menuMap1,
            this.menuMap2,
            this.menuMap3,
            this.menuMap4,
            this.menuMap5});
            this.menuItem11.Text = "Map";
            // 
            // menuMap0
            // 
            this.menuMap0.Index = 0;
            this.menuMap0.Text = "Felucca";
            this.menuMap0.Click += new System.EventHandler(this.menuMap0_Click);
            // 
            // menuMap1
            // 
            this.menuMap1.Index = 1;
            this.menuMap1.Text = "Trammel";
            this.menuMap1.Click += new System.EventHandler(this.menuMap1_Click);
            // 
            // menuMap2
            // 
            this.menuMap2.Index = 2;
            this.menuMap2.Text = "Ilshenar";
            this.menuMap2.Click += new System.EventHandler(this.menuMap2_Click);
            // 
            // menuMap3
            // 
            this.menuMap3.Index = 3;
            this.menuMap3.Text = "Malas";
            this.menuMap3.Click += new System.EventHandler(this.menuMap3_Click);
            // 
            // menuMap4
            // 
            this.menuMap4.Index = 4;
            this.menuMap4.Text = "Tokuno";
            this.menuMap4.Click += new System.EventHandler(this.menuMap4_Click);
            // 
            // menuMap5
            // 
            this.menuMap5.Index = 5;
            this.menuMap5.Text = "TerMur";
            this.menuMap5.Click += new System.EventHandler(this.menuMap5_Click);
            // 
            // menuChangeMulPath
            // 
            this.menuChangeMulPath.Index = 2;
            this.menuChangeMulPath.Text = "Change MUL Path";
            this.menuChangeMulPath.Click += new System.EventHandler(this.menuChangeMulPath_Click);
            // 
            // menuDrawStatics
            // 
            this.menuDrawStatics.Index = 3;
            this.menuDrawStatics.Text = "Draw Statics";
            this.menuDrawStatics.Click += new System.EventHandler(this.menuDrawStatics_Click);
            // 
            // menuXRay
            // 
            this.menuXRay.Index = 4;
            this.menuXRay.Text = "X-Ray Vision";
            this.menuXRay.Click += new System.EventHandler(this.menuXRay_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem4,
            this.menuItem5,
            this.menuItem8});
            this.menuItem2.Text = "Help";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Documentation";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click_1);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "Visit ServUO";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.Text = "-";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 3;
            this.menuItem8.Text = "About...";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // Tree
            // 
            this.Tree.Location = new System.Drawing.Point(8, 8);
            this.Tree.Name = "Tree";
            this.Tree.Size = new System.Drawing.Size(216, 216);
            this.Tree.TabIndex = 1;
            this.Tree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.Tree_BeforeSelect);
            this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Tree_AfterSelect);
            this.Tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tree_MouseDown);
            // 
            // OpenFile
            // 
            this.OpenFile.Filter = "Xml Files (*.xml)|*.xml";
            // 
            // RectList
            // 
            this.RectList.Location = new System.Drawing.Point(8, 264);
            this.RectList.Name = "RectList";
            this.RectList.Size = new System.Drawing.Size(216, 212);
            this.RectList.TabIndex = 2;
            this.RectList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RectList_MouseDown);
            // 
            // ButtonZoomIn
            // 
            this.ButtonZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonZoomIn.Location = new System.Drawing.Point(160, 232);
            this.ButtonZoomIn.Name = "ButtonZoomIn";
            this.ButtonZoomIn.Size = new System.Drawing.Size(64, 23);
            this.ButtonZoomIn.TabIndex = 3;
            this.ButtonZoomIn.Text = "Zoom In";
            this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
            // 
            // ButtonZoomOut
            // 
            this.ButtonZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonZoomOut.Location = new System.Drawing.Point(88, 232);
            this.ButtonZoomOut.Name = "ButtonZoomOut";
            this.ButtonZoomOut.Size = new System.Drawing.Size(64, 23);
            this.ButtonZoomOut.TabIndex = 4;
            this.ButtonZoomOut.Text = "Zoom Out";
            this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
            // 
            // ButtonAddFacet
            // 
            this.ButtonAddFacet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonAddFacet.Location = new System.Drawing.Point(8, 232);
            this.ButtonAddFacet.Name = "ButtonAddFacet";
            this.ButtonAddFacet.Size = new System.Drawing.Size(64, 23);
            this.ButtonAddFacet.TabIndex = 6;
            this.ButtonAddFacet.Text = "Add Facet";
            this.ButtonAddFacet.Click += new System.EventHandler(this.ButtonAddFacet_Click);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.Description = "Select the folder containing Ultima\'s Online map files.";
            this.FolderBrowser.ShowNewFolderButton = false;
            // 
            // SaveFile
            // 
            this.SaveFile.Filter = "Xml Files (*.xml)|*.xml";
            // 
            // FacetMenu
            // 
            this.FacetMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuFacetMapfile});
            this.FacetMenu.Popup += new System.EventHandler(this.FacetMenu_Popup);
            // 
            // MenuFacetMapfile
            // 
            this.MenuFacetMapfile.Index = 0;
            this.MenuFacetMapfile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mFacetMap0,
            this.mFacetMap1,
            this.mFacetMap2,
            this.mFacetMap3,
            this.mFacetMap4,
            this.mFacetMap5});
            this.MenuFacetMapfile.Text = "Set map file";
            // 
            // mFacetMap0
            // 
            this.mFacetMap0.Index = 0;
            this.mFacetMap0.Text = "Felucca";
            this.mFacetMap0.Click += new System.EventHandler(this.mFacetMap0_Click);
            // 
            // mFacetMap1
            // 
            this.mFacetMap1.Index = 1;
            this.mFacetMap1.Text = "Trammel";
            this.mFacetMap1.Click += new System.EventHandler(this.mFacetMap1_Click);
            // 
            // mFacetMap2
            // 
            this.mFacetMap2.Index = 2;
            this.mFacetMap2.Text = "Ilshenar";
            this.mFacetMap2.Click += new System.EventHandler(this.mFacetMap2_Click);
            // 
            // mFacetMap3
            // 
            this.mFacetMap3.Index = 3;
            this.mFacetMap3.Text = "Malas";
            this.mFacetMap3.Click += new System.EventHandler(this.mFacetMap3_Click);
            // 
            // mFacetMap4
            // 
            this.mFacetMap4.Index = 4;
            this.mFacetMap4.Text = "Tokuno";
            this.mFacetMap4.Click += new System.EventHandler(this.mFacetMap4_Click);
            // 
            // mFacetMap5
            // 
            this.mFacetMap5.Index = 5;
            this.mFacetMap5.Text = "Termur";
            this.mFacetMap5.Click += new System.EventHandler(this.mFacetMap5_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusText
            // 
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(0, 17);
            // 
            // RegionEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(800, 497);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ButtonAddFacet);
            this.Controls.Add(this.ButtonZoomOut);
            this.Controls.Add(this.ButtonZoomIn);
            this.Controls.Add(this.RectList);
            this.Controls.Add(this.Tree);
            this.Controls.Add(this.Map);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.TheMenu;
            this.Name = "RegionEditor";
            this.Text = "Region Editor";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.RegionEditor_Closing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Application
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new RegionEditor());
        }
        #endregion

        #region Variables

        /// <summary>
        /// The states possible for the control
        /// </summary>
        private enum States
        {
            None,
            Facet,
            Region,
            Subsection
        }

        /// <summary>
        /// The list of MapRectangles currently displayed
        /// </summary>
        private ArrayList m_DisplayedRects;

        /// <summary>
        /// The MapRectangle3D currently selected
        /// </summary>
        private MapRectangle3D m_SelectedRect = null;

        /// <summary>
        /// States whether a rectangle is currently selected for editing
        /// </summary>
        private bool m_LockSelection = false;

        /// <summary>
        /// The point used as reference for the offset during a dragging operation
        /// </summary>
        private Point m_DragStart;

        /// <summary>
        /// States whether a rectangle is being dragged
        /// </summary>
        private bool m_DraggingRect = false;

        /// <summary>
        /// States whether a rectangle is being drawn
        /// </summary>
        private bool m_DrawingRect = false;

        /// <summary>
        /// The MapRectangle3D being drawn
        /// </summary>
        private MapRectangle3D m_DrawRect = null;

        /// <summary>
        /// The control that displays properties for the different items selected by the user
        /// </summary>
        private UserControl m_Panel = null;

        /// <summary>
        /// Part 1 of the cross hair on the map (Circle)
        /// </summary>
        private TheBox.MapViewer.DrawObjects.MapCircle m_CrossHair1 = null;

        /// <summary>
        /// Part 2 of the cross hair on the map (Cross)
        /// </summary>
        private TheBox.MapViewer.DrawObjects.MapCross m_CrossHair2 = null;

        /// <summary>
        /// States whether we're setting a new go location using the map
        /// </summary>
        private bool m_SetGo = false;

        /// <summary>
        /// The Options provider for this program
        /// </summary>
        private RegEdOptions m_Options;

        private bool m_Modified = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the location for all the property panels
        /// </summary>
        private Point PanelLocation
        {
            get { return new Point(232, 8); }
        }

        /// <summary>
        /// Gets or sets the currently selected rectangle
        /// </summary>
        private MapRectangle3D SelectedRect
        {
            get
            {
                return m_SelectedRect;
            }
            set
            {
                // There's a selection locked - don't change anything
                if (m_LockSelection)
                {
                    return;
                }

                if (m_SelectedRect == value)
                    return;

                // Reset colors for the old rect first
                if (m_SelectedRect != null)
                {
                    m_SelectedRect.Color = RectBorder;
                    m_SelectedRect.FillColor = RectFill;
                }

                m_SelectedRect = value;

                if (m_SelectedRect != null)
                {
                    m_SelectedRect.Color = SelectedRectBorder;
                    m_SelectedRect.FillColor = SelectedRectFill;
                }

                int index = m_DisplayedRects.IndexOf(m_SelectedRect);

                RectList.SelectedIndex = index;

                Map.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the MapRectangle3D being currently drawn
        /// </summary>
        private MapRectangle3D DrawingRect
        {
            get
            {
                return m_DrawRect;
            }
            set
            {
                if (value == m_DrawRect)
                    return;

                if (m_DrawRect == null)
                {
                    // We're starting to draw
                    m_DrawingRect = true;

                    m_DrawRect = value;

                    // Add the draw object
                    Map.AddDrawObject(m_DrawRect);

                    return;
                }
                else
                {
                    // We're updating the current object
                    if (value == null)
                    {
                        // Finalize the current rect

                        // Verify if the rect is big enough
                        if ((m_DrawRect.Rectangle.Width < 1) || (m_DrawRect.Rectangle.Height < 1))
                        {
                            m_DrawRect = null;
                            Map.Refresh();
                            return;
                        }

                        // Change the colors:
                        m_DrawRect.Color = RectBorder;
                        m_DrawRect.FillColor = RectFill;

                        // Add to the officially displayed objects
                        m_DisplayedRects.Add(m_DrawRect);

                        // Add to the list
                        RectList.Items.Add(m_DrawRect.Rectangle.ToString());

                        // Add to the region list
                        if (Tree.SelectedNode != null && Tree.SelectedNode.Tag is MapRegion)
                        {
                            // Region node
                            MapRegion region = Tree.SelectedNode.Tag as MapRegion;

                            region.AddArea(m_DrawRect);
                        }

                        m_DrawRect = null;
                        Map.Refresh();
                        return;
                    }
                    else
                    {
                        // Simply updating the drawrect
                        m_DrawRect = value;
                        Map.Refresh();
                    }
                }
            }
        }

        private ArrayList DisplayedRects
        {
            get
            {
                return m_DisplayedRects;
            }
            set
            {
                // First clear existing rects
                m_DisplayedRects.Clear();

                m_DisplayedRects = value;

                // Now load them into the list
                RectList.BeginUpdate();
                RectList.Items.Clear();

                foreach (MapRectangle3D rect in m_DisplayedRects)
                    RectList.Items.Add(rect.Rectangle.ToString());

                RectList.EndUpdate();

                Map.Refresh();
            }
        }

        #endregion

        #region Xml Managment

        /// <summary>
        /// Reads an Xml file into memory
        /// </summary>
        /// <param name="fileName">The full path to the file</param>
        /// <returns>True if the file has been read, false otherwise</returns>
        private bool ReadXmlFile(string fileName)
        {
            Tree.BeginUpdate();
            Tree.Nodes.Clear();

            if (!File.Exists(fileName))
                return false;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(fileName);
            }
            catch
            {
                // Not a valid XML document
                return false;
            }

            if (dom.DocumentElement.Name != "ServerRegions")
                return false;	// Not a regions document

            // Begin tree
            foreach (XmlNode xFacet in dom.DocumentElement.ChildNodes)
            {
                TreeNode tFacet = new TreeNode(xFacet.Attributes["name"].Value);

                // Verify if this is a known facet
                switch (tFacet.Text.ToLower())
                {
                    case "felucca":
                        tFacet.Tag = 0;
                        break;
                    case "trammel":
                        tFacet.Tag = 1;
                        break;
                    case "ilshenar":                        
                        tFacet.Tag = 2;
                        break;
                    case "ýlshenar":
                        tFacet.Tag = 2;
                        break;
                    case "malas":
                        tFacet.Tag = 3;
                        break;
                    case "tokuno":
                        tFacet.Tag = 4;
                        break;
                    case "termur":
                        tFacet.Tag = 5;
                        break;
                    default:
                        tFacet.Tag = 0; // In case this is an unknown map
                        break;
                }

                foreach (XmlElement xRegion in xFacet.SelectNodes("region"))
                {
                    MapRegion region = MapRegion.LoadRegion(xRegion);

                    tFacet.Nodes.Add(region.GetTreeNode());
                }

                // Add the facet node
                Tree.Nodes.Add(tFacet);
            }

            Tree.EndUpdate();
            return true;
        }

        #endregion

        #region Menu

        private void menuItem8_Click(object sender, System.EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.servuo.com");
        }

        private void menuItem3_Click_1(object sender, System.EventArgs e)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string filename = Path.Combine(path, "help.htm");

            if (File.Exists(filename))
                System.Diagnostics.Process.Start(filename);
            else
                MessageBox.Show("Could not locate the help file, please re-install this software to fix this error");
        }

        private void FacetMenu_Popup(object sender, System.EventArgs e)
        {
            int facet = (int)Tree.SelectedNode.Tag;

            mFacetMap0.Checked = false;
            mFacetMap1.Checked = false;
            mFacetMap2.Checked = false;
            mFacetMap3.Checked = false;
            mFacetMap4.Checked = false;
            mFacetMap5.Checked = false;

            switch (facet)
            {
                case 0:
                    mFacetMap0.Checked = true;
                    break;
                case 1:
                    mFacetMap1.Checked = true;
                    break;
                case 2:
                    mFacetMap2.Checked = true;
                    break;
                case 3:
                    mFacetMap3.Checked = true;
                    break;
                case 4:
                    mFacetMap4.Checked = true;
                    break;
                case 5:
                    mFacetMap5.Checked = true;
                    break;
            }
        }

        private void mFacetMap0_Click(object sender, System.EventArgs e)
        {
            Tree.SelectedNode.Tag = 0;
            Map.Map = Maps.Felucca;
        }

        private void mFacetMap1_Click(object sender, System.EventArgs e)
        {
            Tree.SelectedNode.Tag = 1;
            Map.Map = Maps.Trammel;
        }

        private void mFacetMap2_Click(object sender, System.EventArgs e)
        {
            Tree.SelectedNode.Tag = 2;
            Map.Map = Maps.Ilshenar;
        }

        private void mFacetMap3_Click(object sender, System.EventArgs e)
        {
            Tree.SelectedNode.Tag = 3;
            Map.Map = Maps.Malas;
        }

        private void mFacetMap4_Click(object sender, EventArgs e)
        {
            Tree.SelectedNode.Tag = 4;
            Map.Map = Maps.Tokuno;
        }

        private void mFacetMap5_Click(object sender, EventArgs e)
        {
            Tree.SelectedNode.Tag = 5;
            Map.Map = Maps.Termur;
        }

        private void FileSaveAs_Click(object sender, System.EventArgs e)
        {
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                SaveToFile(SaveFile.FileName);
                m_Modified = false;
            }
        }

        /// <summary>
        /// File -> Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            if (CheckModified())
            {
                ClearEverything();

                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    ReadXmlFile(OpenFile.FileName);
                }

                m_Modified = false;
            }
        }

        private void FileNew_Click(object sender, System.EventArgs e)
        {
            if (CheckModified())
            {
                ClearEverything();
                m_Modified = false;
            }
        }

        private void menuItem9_Popup(object sender, System.EventArgs e)
        {
            menuDrawStatics.Checked = Map.DrawStatics;
            menuXRay.Checked = Map.XRayView;
            menuMap0.Checked = false;
            menuMap1.Checked = false;
            menuMap2.Checked = false;
            menuMap3.Checked = false;
            menuMap4.Checked = false;
            menuMap5.Checked = false;

            switch (Map.Map)
            {
                case Maps.Felucca:
                    menuMap0.Checked = true;
                    break;
                case Maps.Trammel:
                    menuMap1.Checked = true;
                    break;
                case Maps.Ilshenar:
                    menuMap2.Checked = true;
                    break;
                case Maps.Malas:
                    menuMap3.Checked = true;
                    break;
                case Maps.Tokuno:
                    menuMap4.Checked = true;
                    break;
                case Maps.Termur:
                    menuMap5.Checked = true;
                    break;
            }

            menuAlwaysOnTop.Checked = this.TopMost;
        }

        private void menuAlwaysOnTop_Click(object sender, System.EventArgs e)
        {
            TopMost = !TopMost;
        }

        private void menuMap0_Click(object sender, System.EventArgs e)
        {
            Map.Map = Maps.Felucca;
        }

        private void menuMap1_Click(object sender, System.EventArgs e)
        {
            Map.Map = Maps.Trammel;
        }

        private void menuMap2_Click(object sender, System.EventArgs e)
        {
            Map.Map = Maps.Ilshenar;
        }

        private void menuMap3_Click(object sender, System.EventArgs e)
        {
            Map.Map = Maps.Malas;
        }

        private void menuMap4_Click(object sender, EventArgs e)
        {
            Map.Map = Maps.Tokuno;
        }

        private void menuMap5_Click(object sender, EventArgs e)
        {
            Map.Map = Maps.Termur;
        }

        private void menuDrawStatics_Click(object sender, System.EventArgs e)
        {
            Map.DrawStatics = !Map.DrawStatics;
        }

        private void menuXRay_Click(object sender, EventArgs e)
        {
            Map.XRayView = !Map.XRayView;
        }

        private void menuChangeMulPath_Click(object sender, System.EventArgs e)
        {
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                //do we have a valid folder?
                if (TheBox.Common.Utility.ValidateUOFolder(FolderBrowser.SelectedPath))
                {
                    Map.MulManager.CustomFolder = FolderBrowser.SelectedPath;
                    m_Options.ClientPath = FolderBrowser.SelectedPath;
                }
                else
                    MessageBox.Show("Couldn't find Ultima Online's map files.");
            }
        }

        #endregion

        #region Panel Managment

        /// <summary>
        /// Creates a new panel that can be used to modify facet properties
        /// </summary>
        private void AddFacetPanel()
        {
            if (m_Panel != null)
                m_Panel.Dispose();

            m_Panel = new FacetPanel(Tree.SelectedNode.Text);
            m_Panel.Location = PanelLocation;

            // Handle messages
            FacetPanel facetPanel = m_Panel as FacetPanel;

            facetPanel.DeleteFacet += new DeleteFacetEventHandler(facetPanel_DeleteFacet);
            facetPanel.NewRegion += new NewRegionEventHandler(facetPanel_NewRegion);
            facetPanel.RenameFacet += new RenameFacetEventHandler(facetPanel_RenameFacet);

            if (!Controls.Contains(m_Panel))
                Controls.Add(m_Panel);
        }

        /// <summary>
        /// Creates a new panel with region properties
        /// </summary>
        private void AddRegionPanel()
        {
            if (m_Panel != null)
                m_Panel.Dispose();

            m_Panel = new RegionPanel(Map.MapWidth, Map.MapHeight, Tree.SelectedNode.Tag as MapRegion);
            m_Panel.Location = PanelLocation;

            // Handle messages
            RegionPanel regionPanel = m_Panel as RegionPanel;

            regionPanel.RegionChanged += new RegionChangedEventHandler(regionPanel_RegionChanged);

            if (!Controls.Contains(m_Panel))
                Controls.Add(m_Panel);
        }

        private void AddRectanglePanel()
        {
            if (Controls.Contains(m_Panel))
                Controls.Remove(m_Panel);

            m_Panel = new RectPanel(new Size(Map.MapWidth, Map.MapHeight), SelectedRect);
            m_Panel.Location = PanelLocation;

            RectPanel rectPanel = m_Panel as RectPanel;

            rectPanel.RectangleChanged += new RectangleEventHandler(rectPanel_RectangleChanged);

            Controls.Add(m_Panel);
        }

        #endregion

        #region Tree

        private void Tree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Tree.SelectedNode == null)
                    return;

                if (Tree.SelectedNode.Tag is int)
                {
                    FacetMenu.Show((Control)sender, new Point(e.X, e.Y));
                    return;
                }
            }
        }

        /// <summary>
        /// Tree: BEFORE SELECT
        /// </summary>
        private void Tree_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            // Always reset the SetGo flag
            m_SetGo = false;

            // Selection removed
            if (Tree.SelectedNode != null)
            {
                // Revert colors
                Tree.SelectedNode.ForeColor = SystemColors.WindowText;
                Tree.SelectedNode.BackColor = SystemColors.Window;

                // Remove draw objects
                Map.RemoveAllDrawObjects();
                m_CrossHair1 = null;
                m_CrossHair2 = null;
            }
        }

        /// <summary>
        ///  Examines the currently selected tree node and sets the appropriate map
        /// </summary>
        private void EnsureMap()
        {
            TreeNode node = Tree.SelectedNode;
            TreeNode facetNode = null;

            if (node == null)
                return;

            while (!(node.Tag is int))
            {
                node = node.Parent;
                if (node == null)
                    throw new Exception(string.Format("Node {0} has no parent facet.", Tree.SelectedNode.Text));
            }

            facetNode = node;

            // int: switch map
            switch ((int)facetNode.Tag)
            {
                case 0:
                    Map.Map = Maps.Felucca;
                    break;
                case 1:
                    Map.Map = Maps.Trammel;
                    break;
                case 2:
                    Map.Map = Maps.Ilshenar;
                    break;
                case 3:
                    Map.Map = Maps.Malas;
                    break;
                case 4:
                    Map.Map = Maps.Tokuno;
                    break;
                case 5:
                    Map.Map = Maps.Termur;
                    break;
                default:
                    throw new Exception(string.Format("The {0} facet is associated with the wrong mapfile {1}", Tree.SelectedNode.Text, (int)Tree.SelectedNode.Tag));
            }
        }

        private void Tree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            // Fix colors
            Tree.SelectedNode.ForeColor = SystemColors.HighlightText;
            Tree.SelectedNode.BackColor = SystemColors.Highlight;

            // Set the right map file
            EnsureMap();

            // Facet node:
            if (Tree.SelectedNode.Tag is int)
            {
                // Clear draw objects
                m_DisplayedRects.Clear();
                Map.RemoveAllDrawObjects();
            }

            // Region node
            if (Tree.SelectedNode.Tag is MapRegion)
            {
                MapRegion region = Tree.SelectedNode.Tag as MapRegion;

                // When selecting a new region, always display the new region
                MakeRectangles(region.Area);

                // Focus the map on the go location
                if ((region.GoLocation.X != -1) && (region.GoLocation.Y != -1))
                    Map.Center = new Point(region.GoLocation.X, region.GoLocation.Y);
            }

            // Select the correct property panel
            SelectPanel();

            // Display the Go location
            UpdateGoLocation();
        }

        #endregion

        #region Mouse

        //
        // MOUSE DOWN
        //

        private void Map_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Use right button to move around map
            if (e.Button == MouseButtons.Right)
            {
                int x = Map.ControlToMapX(e.X);
                int y = Map.ControlToMapY(e.Y);

                Map.Center = new Point(x, y);
            }

            // Left click
            if (e.Button == MouseButtons.Left)
            {
                // Do nothing it we're selecing a go location
                if (m_SetGo)
                    return;

                // Do nothing is there's no selected or if the selected node is a facet
                if ((Tree.SelectedNode == null) || (Tree.SelectedNode.Tag is int))
                    return;

                // Always set a dragging start location
                m_DragStart = Map.ControlToMap(new Point(e.X, e.Y));

                // Make sure the point is inside the map
                m_DragStart = FitPoint(m_DragStart);

                if (SelectedRect != null)
                {
                    // Lock / Unlock selection
                    LockRectangle(!m_LockSelection);
                }
                else
                {
                    // Start a potential drawing action
                    m_DrawingRect = true;
                }
            }
        }

        private void Map_MouseLeave(object sender, EventArgs e)
        {
            StatusText.Text = "";
        }

        private void Map_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_SetGo)
                return;

            Point cursorPosition = Map.ControlToMap(new Point(e.X, e.Y));
            if (cursorPosition != Point.Empty && cursorPosition.X >= 0 && cursorPosition.Y >= 0)
            {
                int cursorZ = Map.GetMapHeight(cursorPosition);
                StatusText.Text = "Position: " + cursorPosition.X.ToString() + ", " + cursorPosition.Y.ToString() + ", " + cursorZ.ToString();
            }
            else
                StatusText.Text = "";

            if (e.Button == MouseButtons.None)
            {
                // No button pressed, check for draw objects at given location
                SelectedRect = GetRectAt(new Point(e.X, e.Y));

                return;
            }

            // Left button: either dragging or drawing
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedRect != null)
                {
                    Point endDrag = Map.ControlToMap(new Point(e.X, e.Y));

                    // Make sure the rect is actually dragged
                    if ((m_DragStart.X == endDrag.X) && (m_DragStart.Y == endDrag.Y))
                        return;

                    // A rectangle is selected: this is a dragging
                    m_DraggingRect = true;

                    int xOffset = endDrag.X - m_DragStart.X;
                    int yOffset = endDrag.Y - m_DragStart.Y;

                    Rectangle rect = SelectedRect.Rectangle;

                    rect.X += xOffset;
                    rect.Y += yOffset;

                    rect = FitRectangle(rect);

                    SelectedRect.Rectangle = rect;

                    // Update the dragging origin
                    m_DragStart = endDrag;

                    Map.Refresh();

                    return;
                }
                else
                {
                    // Do this only if we're drawing a rectangle
                    if (!m_DrawingRect)
                        return;

                    Point end = Map.ControlToMap(new Point(e.X, e.Y));

                    // Make sure the point is inside the map
                    end = FitPoint(end);

                    Rectangle rect = GetRectangle(m_DragStart, end);

                    if (DrawingRect == null)
                    {
                        int minZ = MapRegion.DefaultMinZ;
                        int maxZ = MapRegion.DefaultMaxZ;

                        MapRegion region = Tree.SelectedNode.Tag as MapRegion;
                        if (region != null)
                        {
                            minZ = region.MinZ;
                            maxZ = region.MaxZ;
                        }

                        // Just starting to draw
                        MapRectangle3D mapRect = new MapRectangle3D(rect, Map.Map, DrawRectBorder, DrawRectFill, minZ, maxZ);

                        DrawingRect = mapRect;
                    }
                    else
                    {
                        DrawingRect.Rectangle = rect;

                        Map.Refresh();
                    }

                    return;
                }
            }
        }

        //
        // MOUSE UP
        //

        private void Map_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Left button
            if (e.Button == MouseButtons.Left)
            {
                if (m_SetGo)
                {
                    // Set a new GO location
                    Point newGo = Map.ControlToMap(new Point(e.X, e.Y));
                    int newZ = Map.GetMapHeight(newGo);

                    MapRegion region = Tree.SelectedNode.Tag as MapRegion;

                    region.GoLocation.X = newGo.X;
                    region.GoLocation.Y = newGo.Y;
                    region.GoLocation.Z = newZ;

                    // Update it in the panel
                    ((RegionPanel)m_Panel).UpdateGoLocation(region.GoLocation.X, region.GoLocation.Y, region.GoLocation.Z);

                    UpdateGoLocation();

                    m_SetGo = false;
                    return;
                }
                if (SelectedRect != null)
                {
                    if (m_DraggingRect)
                    {
                        // End of dragging: Unlock selection
                        m_LockSelection = false;
                        m_DraggingRect = false;

                        // Update information in the region
                        UpdateSelectedRect();

                        SelectPanel();

                        m_Modified = true;
                    }
                    else
                        LockRectangle(m_LockSelection);

                    return;
                }

                if (DrawingRect != null)
                {
                    // End of drawing
                    Point end = Map.ControlToMap(new Point(e.X, e.Y));
                    end = FitPoint(end);

                    Rectangle rect = GetRectangle(m_DragStart, end);

                    DrawingRect.Rectangle = rect;

                    DrawingRect = null;

                    m_DrawingRect = false;

                    m_Modified = true;

                    return;
                }
            }
        }

        //
        // Mouse Down on the rectangles list
        private void RectList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int index = RectList.SelectedIndex;

            if (index >= 0)
            {
                int currentIndex = m_DisplayedRects.IndexOf(SelectedRect);

                bool temp = m_LockSelection;

                m_LockSelection = false;

                SelectedRect = (MapRectangle3D)m_DisplayedRects[index];

                // Focus the map
                Map.Center = new Point(m_SelectedRect.Rectangle.X, m_SelectedRect.Rectangle.Y);

                m_LockSelection = temp;

                if (currentIndex != index)
                {
                    // Lock unlock
                    m_LockSelection = true;

                    AddRectanglePanel();
                }
                else
                {
                    LockRectangle(!m_LockSelection);

                    if (m_LockSelection)
                        AddRectanglePanel();
                }
            }
        }

        private void ButtonZoomIn_Click(object sender, System.EventArgs e)
        {
            Map.ZoomIn();
        }

        private void ButtonZoomOut_Click(object sender, System.EventArgs e)
        {
            Map.ZoomOut();
        }

        private void ButtonAddFacet_Click(object sender, System.EventArgs e)
        {
            NewFacet nf = new NewFacet();

            if (nf.ShowDialog() == DialogResult.OK)
            {
                TreeNode fNode = new TreeNode(nf.FacetName);
                fNode.Tag = nf.Mapfile;

                Tree.Nodes.Add(fNode);

                m_Modified = true;
            }
        }

        #endregion

        #region Events

        private void RegionEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Map != null)
            {
                m_Options.AlwaysOnTop = TopMost;
                m_Options.DrawStatics = Map.DrawStatics;
                m_Options.XRayVision = Map.XRayView;

                // Save options
                RegEdOptions.Save(m_Options);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the options stored in the options object
        /// </summary>
        private void SetOptions()
        {
            this.TopMost = m_Options.AlwaysOnTop;
            if (Map != null)
            {
                if (m_Options.ClientPath != "")
                    Map.MulManager.CustomFolder = m_Options.ClientPath;

                Map.DrawStatics = m_Options.DrawStatics;
                Map.XRayView = m_Options.XRayVision;
            }
            else if (m_Options.ClientPath != "")
                TheBox.MapViewer.MapViewer.MulFileManager.CustomFolder = m_Options.ClientPath;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();

                    if (m_Panel != null)
                        m_Panel.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Retrieves the map rectangle located at the specified location
        /// </summary>
        /// <param name="point">The Point to search for rectangles</param>
        /// <returns>The MapRectangle3D at the specified location, null if no rectangle is defined at the specified location</returns>
        private MapRectangle3D GetRectAt(Point point)
        {
            Point location = Map.ControlToMap(point);

            foreach (MapRectangle3D mapRect in m_DisplayedRects)
            {
                Rectangle rect = mapRect.Rectangle;

                if ((rect.Left < location.X) && (rect.Right > location.X) && (rect.Top < location.Y) && (rect.Bottom > location.Y))
                {
                    // This is the draw object
                    return mapRect;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the information in the treenode tag when the selected rectangle is changed
        /// </summary>
        private void UpdateSelectedRect()
        {
            // Main region tag
            if (Tree.SelectedNode.Tag is MapRegion)
            {
                // Update the rectangle in the region
                MapRegion region = Tree.SelectedNode.Tag as MapRegion;

                // The index is the same in the draw objects list and in the region object
                int index = m_DisplayedRects.IndexOf(SelectedRect);

                if (index >= 0)
                {
                    region.Area[index] = new MapRect(SelectedRect.Rectangle, SelectedRect.MinZ, SelectedRect.MaxZ);

                    // Update the text
                    RectList.Items[index] = SelectedRect.Rectangle.ToString();
                }
            }
        }

        /// <summary>
        /// Gets a valid rectangle given two points
        /// </summary>
        /// <param name="topleft">The first point of the rectangle</param>
        /// <param name="bottomright">The second point of the rectangle</param>
        /// <returns>A valid rectangle defined by the two points</returns>
        private Rectangle GetRectangle(Point p1, Point p2)
        {
            int x1, y1, width, height = 0;

            if (p1.X < p2.X)
            {
                x1 = p1.X;
                width = p2.X - p1.X;
            }
            else
            {
                x1 = p2.X;
                width = p1.X - p2.X;
            }

            if (p1.Y < p2.Y)
            {
                y1 = p1.Y;
                height = p2.Y - p1.Y;
            }
            else
            {
                y1 = p2.Y;
                height = p1.Y - p2.Y;
            }

            return new Rectangle(x1, y1, width, height);
        }

        /// <summary>
        /// Makes sure a point is inside the map bounds, and if needed changes its coordinates to fit
        /// </summary>
        /// <param name="p">The point to examine</param>
        /// <returns>The updated point and ensured inside the map borders</returns>
        private Point FitPoint(Point p)
        {
            if (p.X < 0)
                p.X = 0;

            if (p.Y < 0)
                p.Y = 0;

            if (p.X >= Map.MapWidth)
                p.X = Map.MapWidth - 1;

            if (p.Y >= Map.MapHeight)
                p.Y = Map.MapHeight - 1;

            return p;
        }

        /// <summary>
        /// Makes sure a rectangle is inside the map bounds
        /// </summary>
        /// <param name="rect">The rectangle to examnis</param>
        /// <returns>The rectangle, eventually modified to fit (size won't be affected)</returns>
        private Rectangle FitRectangle(Rectangle rect)
        {
            if (rect.X < 0)
                rect.X = 0;

            if (rect.Y < 0)
                rect.Y = 0;

            if (rect.Right >= Map.MapWidth)
                rect.X -= (rect.Right - Map.MapWidth);

            if (rect.Bottom >= Map.MapHeight)
                rect.Y -= (rect.Bottom - Map.MapHeight);

            return rect;
        }

        /// <summary>
        /// Deletes the rectangle currently selected
        /// </summary>
        private void DeleteSelectedRectangle()
        {
            if (SelectedRect == null)
                return;

            int index = m_DisplayedRects.IndexOf(SelectedRect);

            Map.RemoveDrawObject(SelectedRect);

            // This is only on the display
            ArrayList dispRects = new ArrayList(DisplayedRects);
            dispRects.RemoveAt(index);
            DisplayedRects = dispRects;

            if (Tree.SelectedNode.Tag is MapRegion)
            {
                // Region node
                MapRegion region = Tree.SelectedNode.Tag as MapRegion;

                region.Area.RemoveAt(index);
            }

            m_LockSelection = false;
            SelectedRect = null;
        }

        /// <summary>
        /// Selects the appropriate panel according to the selected tree node
        /// </summary>
        private void SelectPanel()
        {
            if (Tree.SelectedNode == null)
            {
                // Make sure the panel is empty
                if (m_Panel != null)
                {
                    Controls.Remove(m_Panel);
                    m_Panel.Dispose();
                    m_Panel = null;
                }

                return;
            }

            if (Tree.SelectedNode.Tag is int)
                AddFacetPanel();
            else if (Tree.SelectedNode.Tag is MapRegion)
                AddRegionPanel();
        }

        /// <summary>
        /// Locks and unlocks a rectangle
        /// </summary>
        /// <param name="Lock">Specifies whether the rectangle should be locked or released</param>
        private void LockRectangle(bool Lock)
        {
            m_LockSelection = Lock;
            if (Lock)
            {
                // Display the rect panel
                AddRectanglePanel();
            }
            else
                // Remove the rect panel and add the right one
                SelectPanel();
        }

        /// <summary>
        /// Updates the display of the crosshair representing the go location
        /// </summary>
        private void UpdateGoLocation()
        {
            if (Tree.SelectedNode.Tag is MapRegion)
            {
                MapRegion region = Tree.SelectedNode.Tag as MapRegion;

                if ((region.GoLocation.X == -1) && (region.GoLocation.Y == -1))
                {
                    if (m_CrossHair1 != null)
                    {
                        Map.RemoveDrawObject(m_CrossHair1);
                        m_CrossHair1 = null;
                        Map.Refresh();
                    }
                    if (m_CrossHair2 != null)
                    {
                        Map.RemoveDrawObject(m_CrossHair2);
                        m_CrossHair2 = null;
                        Map.Refresh();
                    }
                    return;
                }

                if ((m_CrossHair1 == null) || (m_CrossHair2 == null))
                {
                    // No existing crosshair
                    m_CrossHair1 = new MapCircle(3, new Point(region.GoLocation.X, region.GoLocation.Y), Map.Map, CrossHairColor);
                    m_CrossHair2 = new MapCross(4, CrossHairColor, new Point(region.GoLocation.X, region.GoLocation.Y), Map.Map);

                    Map.AddDrawObject(m_CrossHair1, false);
                    Map.AddDrawObject(m_CrossHair2, true);
                }
                else
                {
                    // There's an existing crosshair, so just update the current information
                    m_CrossHair1.Location = new Point(region.GoLocation.X, region.GoLocation.Y);
                    m_CrossHair2.Location = new Point(region.GoLocation.X, region.GoLocation.Y);

                    Map.Refresh();
                }
            }
            else
            {
                // This isn't a region. Remove cross hair if it exists
                if (m_CrossHair1 != null)
                {
                    Map.RemoveDrawObject(m_CrossHair1);
                    m_CrossHair1 = null;
                }
                if (m_CrossHair2 != null)
                {
                    Map.RemoveDrawObject(m_CrossHair2);
                    m_CrossHair2 = null;
                }
            }
        }

        /// <summary>
        /// Creates and sets the MapRectangle3D objects from a list of given rectangles
        /// </summary>
        /// <param name="list">The list of rectangles to be used as base for the map objects</param>
        private void MakeRectangles(ArrayList list)
        {
            // Initialize the displayed rectanlges
            ArrayList dispRects = new ArrayList();

            // Draw the rectangles
            foreach (MapRect rect in list)
            {
                Rectangle rectangle = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                // Create new rectangle
                MapRectangle3D r = new MapRectangle3D(rectangle, Map.Map, RectBorder, RectFill, rect.MinZ, rect.MaxZ);
                Map.AddDrawObject(r, false);

                dispRects.Add(r);
            }

            DisplayedRects = dispRects;
        }

        /// <summary>
        /// Saves the current document to file
        /// </summary>
        /// <param name="FileName">The filename of the destination file. Will be overwritten if exists</param>
        private void SaveToFile(string FileName)
        {
            // Create the whole document
            XmlDocument dom = new XmlDocument();

            XmlNode serverregions = dom.CreateElement("ServerRegions");

            foreach (TreeNode facet in Tree.Nodes)
            {
                XmlNode fNode = dom.CreateElement("Facet");
                XmlAttribute fName = dom.CreateAttribute("name");
                fName.Value = facet.Text;
                fNode.Attributes.Append(fName);

                foreach (TreeNode region in facet.Nodes)
                {
                    MapRegion r = region.Tag as MapRegion;

                    XmlNode rNode = r.GetXml(dom);

                    fNode.AppendChild(rNode);
                }

                serverregions.AppendChild(fNode);
            }

            XmlNode dec = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(dec);

            dom.AppendChild(serverregions);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.IndentChars = "\t";
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(FileName, settings);
            dom.Save(writer);
            writer.Close();
        }

        /// <summary>
        /// Verifies the modified flag before deleting the current information
        /// </summary>
        /// <returns>True if the deletion can proceed, false if it should stop</returns>
        private bool CheckModified()
        {
            if (!m_Modified)
                return true;

            switch (MessageBox.Show(this, "The current document has been modified. Do you want to save it before proceeding?", "Save your work",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
            {
                case DialogResult.Yes:
                    // Save
                    if (SaveFile.ShowDialog() == DialogResult.OK)
                    {
                        SaveToFile(SaveFile.FileName);
                        return true;
                    }
                    else
                        return false;
                case DialogResult.No:
                    return true;
                case DialogResult.Cancel:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Clears the current document data
        /// </summary>
        private void ClearEverything()
        {
            Tree.BeginUpdate();
            Tree.Nodes.Clear();
            Tree.EndUpdate();

            Map.Map = Maps.Felucca;
            Map.Center = new Point(0, 0);

            RectList.Items.Clear();

            SelectPanel();
        }

        #endregion

        #region Panel Handlers

        private void facetPanel_DeleteFacet(object sender, FacetEventArgs e)
        {
            // Delete the currently selected facet. Just delete the tree node
            Tree.Nodes.Remove(Tree.SelectedNode);

            SelectPanel();

            m_Modified = true;
        }

        private void facetPanel_NewRegion(object sender, FacetEventArgs e)
        {
            TreeNode rNode = new TreeNode(e.Name);

            // Create the new region object and add it as Tag for the tree node
            MapRegion region = new MapRegion(e.Name);
            rNode.Tag = region;

            Tree.SelectedNode.Nodes.Add(rNode);

            if (e.FocusRegion)
                Tree.SelectedNode = rNode;

            m_Modified = true;
        }

        private void facetPanel_RenameFacet(object sender, FacetEventArgs e)
        {
            // Just rename the tree node
            Tree.SelectedNode.Text = e.Name;

            m_Modified = true;
        }

        private void regionPanel_RegionChanged(object sender, RegionEventArgs e)
        {
            MapRegion region = Tree.SelectedNode.Tag as MapRegion;

            m_Modified = true;

            switch (e.Action)
            {
                case RegionActions.DeleteRegion:

                    MapRegion parentRegion = Tree.SelectedNode.Parent.Tag as MapRegion;
                    if (parentRegion != null)
                        parentRegion.SubRegions.Remove(region);

                    // Delete the tree node
                    Tree.Nodes.Remove(Tree.SelectedNode);
                    SelectPanel();
                    break;

                case RegionActions.AddSubregion:

                    // Make a new subregion
                    MapRegion subregion = new MapRegion((string)e.Data);
                    region.SubRegions.Add(subregion);

                    TreeNode subregionTree = new TreeNode((string)e.Data);
                    subregionTree.Tag = subregion;
                    Tree.SelectedNode.Nodes.Add(subregionTree);

                    break;

                case RegionActions.ChangeGo:

                    // Change the GO location
                    int[] newGo = e.Data as int[];
                    region.GoLocation.X = newGo[0];
                    region.GoLocation.Y = newGo[1];
                    region.GoLocation.Z = newGo[2];
                    UpdateGoLocation();
                    break;

                case RegionActions.ChangePriority:

                    int newPriority = (int)e.Data;
                    region.Priority = newPriority;
                    break;

                case RegionActions.ClearRegion:

                    region.Area.Clear();
                    Tree.SelectedNode.Nodes.Clear();
                    Map.RemoveAllDrawObjects();
                    RectList.Items.Clear();
                    DisplayedRects.Clear();
                    if ((region.GoLocation.X != -1) && (region.GoLocation.Y != -1))
                    {
                        Map.AddDrawObject(m_CrossHair1, false);
                        Map.AddDrawObject(m_CrossHair2, true);
                    }

                    if (region != null)
                    {
                        region.Area.Clear();
                        region.SubRegions.Clear();
                    }
                    SelectPanel();
                    break;

                case RegionActions.ResetGoLoc:
                    if (region.Area != null && region.Area.Count > 0)
                    {
                        MapRect rect = region.Area[0] as MapRect;
                        region.GoLocation = new Point3D(rect.Start.X + (rect.End.X - rect.Start.X) / 2, rect.Start.Y + (rect.End.Y - rect.Start.Y) / 2, 0);
                    }
                    else
                    {
                        region.GoLocation.X = -1;
                        region.GoLocation.Y = -1;
                        region.GoLocation.Z = 0;
                    }
                    UpdateGoLocation();
                    AddRegionPanel();
                    break;

                case RegionActions.RenameRegion:

                    region.Name = (string)e.Data;
                    Tree.SelectedNode.Text = (string)e.Data;
                    break;

                case RegionActions.ChangeTypeName:

                    region.TypeName = (string)e.Data;
                    break;

                case RegionActions.ChangeRuneName:

                    region.RuneName = (string)e.Data;
                    break;

                case RegionActions.ChangeMusicName:

                    region.MusicName = (string)e.Data;
                    break;

                case RegionActions.SetGo:
                    m_SetGo = true;
                    break;

                case RegionActions.ChangeZ:

                    int[] newZ = e.Data as int[];

                    region.MinZ = newZ[0];
                    region.MaxZ = newZ[1];

                    //update rectangles so they show their new inherited minZ and maxZ.
                    if (DisplayedRects.Count == region.Area.Count)
                    {
                        for (int index = 0; index < region.Area.Count; index++)
                        {
                            MapRectangle3D rectToUpdate = DisplayedRects[index] as MapRectangle3D;
                            MapRect updatedRect = region.Area[index] as MapRect;

                            if (rectToUpdate != null && updatedRect != null)
                            {
                                rectToUpdate.MinZ = updatedRect.MinZ;
                                rectToUpdate.MaxZ = updatedRect.MaxZ;
                            }
                        }
                    }
                    break;

                case RegionActions.ChangeLogoutDelay:

                    if ((bool)e.Data)
                        region.LogoutDelayActive = XmlBool.True;
                    else
                        region.LogoutDelayActive = XmlBool.False;
                    break;

                case RegionActions.ChangeGuardsDisabled:

                    if ((bool)e.Data)
                        region.GuardsDisabled = XmlBool.True;
                    else
                        region.GuardsDisabled = XmlBool.False;
                    break;

                case RegionActions.ChangeSmartNoHousing:

                    if ((bool)e.Data)
                        region.SmartNoHousing = XmlBool.True;
                    else
                        region.SmartNoHousing = XmlBool.False;
                    break;
            }
        }

        private void rectPanel_RectangleChanged(object sender, RectangleEventArgs e)
        {
            m_Modified = true;

            switch (e.Action)
            {
                case RectangleActions.Delete:
                    DeleteSelectedRectangle();
                    SelectPanel();
                    break;
                case RectangleActions.UpdateRect:
                    SelectedRect.Rectangle = e.Rectangle;
                    UpdateSelectedRect();
                    Map.Refresh();
                    break;
                case RectangleActions.UpdateZ:
                    SelectedRect.MinZ = e.MinZ;
                    SelectedRect.MaxZ = e.MaxZ;
                    UpdateSelectedRect();
                    break;
            }
        }
        #endregion
    }
}