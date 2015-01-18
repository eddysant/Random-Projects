using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RandomWeather
{	
	[ServiceContract]
	public interface IWeather
	{
		[WebGet()]
		[OperationContract]
		WeatherObj GetWeather();
	}

	[DataContract]
	public class WeatherObj
	{
		[DataMember]
		public int Temperature { get; set; }
		[DataMember]
		public string Condition { get; set; }
	}
}
