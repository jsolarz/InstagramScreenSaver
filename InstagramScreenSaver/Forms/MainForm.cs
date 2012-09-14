using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using InstagramScreenSaver.Services;

namespace InstagramScreenSaver.Forms
{
	public partial class MainForm : Form
	{
		string _text = "LOADING...";
		bool IsPreviewMode = false;

		#region Preview API's

		[DllImport("user32.dll")]
		static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		#endregion

		#region Constructors

		public MainForm()
		{
			InitializeComponent();
		}

		//This constructor is passed the bounds this form is to show in
		//It is used when in normal mode
		public MainForm(Rectangle Bounds)
		{
			InitializeComponent();
			this.Bounds = Bounds;
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			//hide the cursor
			Cursor.Hide();
		}

		//This constructor is the handle to the select screensaver dialog preview window
		//It is used when in preview mode (/p)
		public MainForm(IntPtr PreviewHandle)
		{
			InitializeComponent();

			//set the preview window as the parent of this window
			SetParent(this.Handle, PreviewHandle);

			//make this a child window, so when the select screensaver dialog closes, this will also close
			SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

			//set our window's size to the size of our window's new parent
			Rectangle ParentRect;
			GetClientRect(PreviewHandle, out ParentRect);
			this.Size = ParentRect.Size;

			//set our location at (0, 0)
			this.Location = new Point(0, 0);

			IsPreviewMode = true;
		}

		#endregion

		#region GUI

		//sets up the fake BSOD
		private void MainForm_Shown(object sender, EventArgs e)
		{
			if (!IsPreviewMode) //we don't want all those effects for just a preview
			{
				this.Refresh();
				//keep the screen black for one second to simulate the changing of screen resolution
				System.Threading.Thread.Sleep(1000);
			}

			DrawString();
			this.BackgroundImage = ImageService.CreateBackground();

			TimeSpan span = new TimeSpan(0, 10, 0);
			var newBackground = new System.Timers.Timer(span.TotalMilliseconds) { AutoReset = true, Enabled = true };
			newBackground.Elapsed += (caller, args) => this.BackgroundImage = ImageService.CreateBackground();
			newBackground.Start();
		}

		private void DrawString()
		{
			var g = this.CreateGraphics();

			using (Font f = new Font(FontsService.Fonts.Families[0], 15))
			{
				SizeF size;
				using (Font f2 = FontsService.AppropriateFont(g, this.Bounds.Size, _text, f, out size))
				{
					PointF p = new PointF(
						(Screen.PrimaryScreen.WorkingArea.Width - size.Width) / 2,
						(Screen.PrimaryScreen.WorkingArea.Height - size.Height) / 2);
					g.DrawString(_text, f2, Brushes.White, p);
				}
			}
		}

		#endregion

		#region User Input

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (!IsPreviewMode) //disable exit functions for preview
			{
				Application.Exit();
			}
		}

		private void MainForm_Click(object sender, EventArgs e)
		{
			if (!IsPreviewMode) //disable exit functions for preview
			{
				Application.Exit();
			}
		}

		//start off OriginalLoction with an X and Y of int.MaxValue, because
		//it is impossible for the cursor to be at that position. That way, we
		//know if this variable has been set yet.
		Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

		private void MainForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (!IsPreviewMode) //disable exit functions for preview
			{
				//see if originallocat5ion has been set
				if (OriginalLocation.X == int.MaxValue & OriginalLocation.Y == int.MaxValue)
				{
					OriginalLocation = e.Location;
				}
				//see if the mouse has moved more than 20 pixels in any direction. If it has, close the application.
				if (Math.Abs(e.X - OriginalLocation.X) > 20 | Math.Abs(e.Y - OriginalLocation.Y) > 20)
				{
					Application.Exit();
				}
			}
		}

		#endregion
	}
}
