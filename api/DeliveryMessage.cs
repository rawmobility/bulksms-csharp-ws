using System;
using System.Xml.Serialization;

namespace blender
{
	[XmlRoot("deliverymessage")]
	public class DeliveryMessage
	{
		[XmlElement("id")]
		public String id;
		[XmlElement("created")]
		public DateTime created;
		[XmlElement("originator")]
		public String originator;
		[XmlElement("recipient")]
		public String recipient;
		[XmlElement("body")]
		public String body;
		[XmlElement("logicalMessageId")]
		public String batchId;
		[XmlElement("inReplyTo")]
		public String inReplyTo;

		public DeliveryMessage ()
		{
			
		}
	}
}

