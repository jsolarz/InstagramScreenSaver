using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using InstagramScreenSaver.Entities;
using Newtonsoft.Json;
using System.Drawing;
using InstagramScreenSaver.Helpers;

namespace InstagramScreenSaver.Services
{
	public static class InstagramService
	{
		public static List<InstaImage> GetItems()
		{
			List<InstaImage> images = new List<InstaImage>();

			try
			{
				RestClient client = new RestSharp.RestClient();
				client.BaseUrl = "https://api.instagram.com";
				IRestRequest request = new RestSharp.RestRequest();

				request.Resource = string.Format("v1/media/popular?client_id={0}", Config.InstagramClientId);
				var response = client.Execute(request);

				if (response != null && !string.IsNullOrEmpty(response.Content))
				{
					dynamic product = JsonConvert.DeserializeObject(response.Content);

					foreach (var item in product.data)
					{
						images.Add(new InstaImage
						{
							LowResolution = item.images.low_resolution.url,
							StandardResolution = item.images.standard_resolution.url,
							Thumbnail = item.images.thumbnail.url
						});
					}
				}
			}
			catch (Exception)
			{
			}

			return images;
		}


	}
}
