using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RandomWeather
{
	public class Weather : IWeather
	{
		public WeatherObj GetWeather()
		{
			var random = new Random();

			int temp = random.Next(-100, 100);
			
			int max = 6;
			if (temp < 10)
				max = 5;

			int weather = random.Next(max);
			string weatherString = string.Empty;

			switch (weather)
			{
				case 0:
					weatherString = "Partly Cloudy";
					break;
				case 1:
					weatherString = "Thunderstorms";
					break;
				case 2:
					weatherString = "Raining";
					break;
				case 3:
					weatherString = "Sunny";
					break;
				case 4:
					weatherString = "Cloudy";
					break;
				case 5:
					weatherString = "Snowing";
					break;
			}
						

			WeatherObj weatherObj = new WeatherObj();
			weatherObj.Temperature = temp;
			weatherObj.Condition = weatherString;
			return weatherObj;
		}
	}
}
