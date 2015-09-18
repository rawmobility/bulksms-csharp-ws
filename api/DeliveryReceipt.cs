using System;
using System.Xml.Serialization;

namespace blender
{
	[XmlRoot("deliveryreceipt")]
	public class DeliveryReceipt
	{
		[XmlElement("created")]
		public DateTime created;
		[XmlElement("deliveryMessageId")]
		public String messageId;
		[XmlElement("status")]
		public String status;
		[XmlElement("body")]
		public String body;
		[XmlElement("clientReference")]
		public String reference;
		[XmlElement("part")]
		public int part;
		[XmlElement("parts")]
		public int parts;

		public DeliveryReceipt ()
		{
			
		}

		public Boolean isDelivered() {
			return status.Equals ("DELIVERED");
		}
	}
}

