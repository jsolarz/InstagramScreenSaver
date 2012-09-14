using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;

namespace InstagramScreenSaver.Services
{
	public static class FontsService
	{
		public static PrivateFontCollection Fonts { get; set; }

		internal static void LoadFont()
		{
			Fonts = new PrivateFontCollection();

			// specify embedded resource name
			Stream fontStream = new MemoryStream(Properties.Resources.futura_extrabold);

			// create an unsafe memory block for the font data
			System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);

			// create a buffer to read in to
			byte[] fontdata = new byte[fontStream.Length];

			//byte[] fontdata = InstagramScreenSaver.Properties.Resources.futura_extrabold;

			// read the font data from the resource
			fontStream.Read(fontdata, 0, (int)fontStream.Length);

			// copy the bytes to the unsafe memory block
			Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);

			// pass the font to the font collection
			Fonts.AddMemoryFont(data, (int)fontStream.Length);

			// close the resource stream
			fontStream.Close();

			// free up the unsafe memory
			Marshal.FreeCoTaskMem(data);
		}

		public static Font AppropriateFont(Graphics g, Size layoutSize, string s, Font f, out SizeF extent)
		{
			//if (maxFontSize == minFontSize)
			//    f = new Font(f.FontFamily, minFontSize, f.Style);

			extent = g.MeasureString(s, f);

			//if (maxFontSize <= minFontSize)
			//    return f;

			float hRatio = layoutSize.Height / extent.Height;
			float wRatio = layoutSize.Width / extent.Width;
			float ratio = (hRatio < wRatio) ? hRatio : wRatio;

			float newSize = f.Size * ratio;

			//if (newSize < minFontSize)
			//    newSize = minFontSize;
			//else if (newSize > maxFontSize)
			//    newSize = maxFontSize;

			f = new Font(f.FontFamily, newSize, f.Style);
			extent = g.MeasureString(s, f);

			return f;
		}

	}
}
