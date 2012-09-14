using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;

namespace InstagramScreenSaver.Helpers
{
	public static class ImageHelper
	{
		public static Bitmap GetPic(string url)
		{
			Bitmap pic = null;
			try
			{
				ImageConverter ic = new ImageConverter();

				using (WebClient Client = new WebClient())
				{
					byte[] data = Client.DownloadData(url);
					pic = (Bitmap)ic.ConvertFrom(data);
				}
			}
			catch (Exception)
			{

			}

			return pic;
		}

		public static string SaveAsJpeg(this Image Img, string path, Int64 Quality)
		{
			string filename = string.Format("{0}.jpg", DateTime.Now.ToBinary());

			ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
			Encoder QualityEncoder = Encoder.Quality;

			using (EncoderParameters EP = new EncoderParameters(1))
			{
				using (EncoderParameter QualityEncoderParameter = new EncoderParameter(QualityEncoder, Quality))
				{
					EP.Param[0] = QualityEncoderParameter;
					Img.Save(path + filename, jgpEncoder, EP);
				}
			}

			return filename;
		}

		static private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
		}
	}
}
