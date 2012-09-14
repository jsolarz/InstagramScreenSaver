using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstagramScreenSaver.Forms;
using System.Reflection;
using System.IO;

namespace InstagramScreenSaver
{
	class Program
	{
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args.Length > 0)
			{
				switch (args[0].ToLower().Trim().Substring(0, 2))
				{
					case "/s"://show
						//show the screen saver
						ShowScreensaver();
						Application.Run();
						break;
					case "/p"://preview
						//preview the screen saver
						Application.Run(new MainForm(new IntPtr(long.Parse(args[1])))); //args[1] is the handle to the preview window
						break;
					case "/c"://configure
						//configure the screen saver
						//inform the user no options can be set in this screen saver
						MessageBox.Show("This screensaver has no options that you can set",
							"Instagram Screen Saver",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
						break;
					default:
						//show the screen saver anyway
						ShowScreensaver();
						Application.Run();
						break;
				}
			}
			else
			{
				//no arguments were passed (we should probably show the screen saver)
				//show the screen saver anyway				
				ShowScreensaver();
				Application.Run();
			}
		}

		//will show the screen saver
		static void ShowScreensaver()
		{
			InstagramScreenSaver.Services.FontsService.LoadFont();

			//loops through all the computer's screens (monitors)
			foreach (Screen screen in Screen.AllScreens)
			{
				//creates a form just for that screen and passes it the bounds of that screen
				MainForm screensaver = new MainForm(screen.Bounds);
				screensaver.Show();
			}
		}
	}
}
