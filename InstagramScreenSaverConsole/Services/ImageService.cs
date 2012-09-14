using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using InstagramScreenSaver.Helpers;
using InstagramScreenSaver.Services;

namespace InstagramScreenSaver
{
	public static class ImageService
	{
		internal static List<System.Drawing.Bitmap> GetImages()
		{
			var items = InstagramService.GetItems();
			List<Bitmap> images = new List<Bitmap>();

			foreach (var item in items)
			{
				images.Add(ImageHelper.GetPic(item.Thumbnail));
			}

			return images;
		}


		internal static System.Drawing.Image CreateBackground()
		{
			var images = GetImages();

			Bitmap background = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

			int y = Screen.PrimaryScreen.WorkingArea.Height - 155;
			int x = 0;

			Point p;
			using (Graphics g = Graphics.FromImage(background))
			{
				foreach (var item in images)
				{
					if (x <= Screen.PrimaryScreen.WorkingArea.Width - item.Width)
					{
						p = new Point(x, y);
						x = x + item.Width + 10;
					}
					else
					{
						y = y - item.Height - 10;
						x = 0;
						p = new Point(x, y);
					}

					g.DrawImage(item, p);
				}
			}

			return background;
		}
	}
}
