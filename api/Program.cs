using System;
using apps.rawmobility.com;
using System.Xml.Serialization;
using System.IO;

namespace blender
{
	class SMSApi
	{
		public static void Main (string[] args)
		{
			// Live
			String url = "http://apps.rawmobility.com/gateway/ws/sendsms";
			// Contact us to retrieve your route id, username and password
			String routeId = "ROUTE-ID-ROUTE-ID-ROUTE-ID";
			String username = "USER_NAME";
			String password = "PASS_WORD";

			SendSMSService sendSms = new SendSMSService (url);

			/************************ SEND SINGLE MESSAGE ******************************/		
			batchMessageMultiBody sendResult = sendSms.sendSms (username, password, "Test", "61000000000", "Hello this is a test", routeId);
			String remoteId = sendResult.recipients [0].id;
			Console.WriteLine ("Singlel SMS Sent, ID: " + remoteId);


			/************************ SEND BATCH MESSAGE ******************************/
			// Create batch object, set default data
			batchMessageMultiBody batch = new batchMessageMultiBody ();
			// Request detailed response (ID per recipient)
			batch.detailedResponse = true;
			// Default route ids
			batch.routeId = routeId;
			// Default originator. Can be overridden in each recipient
			batch.originator = "Default";
			// Default body. Can be overridden in each recipient
			batch.body = "Defulat Body";
			// Schedule Time
			batch.deliverySchedule = DateTime.Parse ("2015-09-16T13:49:00");
			// Set to true for optional property
			batch.deliveryScheduleSpecified = true;
			// Schedule Timezone
			batch.deliveryTimeZone = "Australia/Melbourne";
			// Spread messages over XX hours
			batch.messageSpread = 0;

			// Create array of two recipients
			batch.recipients = new batchRecipientMultiBody[2];

			// Add first recipient
			batchRecipientMultiBody recipient = new batchRecipientMultiBody ();
			recipient.recipient = "61000000001";
			batch.recipients [0] = recipient;
				
			// Add second recipient
			recipient = new batchRecipientMultiBody ();
			// Override originator
			recipient.originator = "Orig2";
			recipient.recipient = "61000000002";
			// Override body
			recipient.body = "Test Message";
			// Set reference
			recipient.reference = "MyRef123";
			batch.recipients [1] = recipient;

			try {
				// Send message
				batchMessageMultiBody batchResult = sendSms.sendBatchMulti (username, password, batch);			
				// Loop over result recipients and retrieve remote id
				foreach (batchRecipientMultiBody recipientResult in batchResult.recipients) {					
					Console.WriteLine ("SMS To: " + recipientResult.recipient + ", ID: " + recipientResult.id);
				}

			} catch (Exception e) {
				Console.WriteLine ("Error: " + e.Message);
			}

			// Receiving messages and receipts - we POST a single parameter named "xml".

			/************************ Simulate Incoming Message ******************************/
			// This should be handled by a WebMethod or Web API
			String incomingXml = "<deliverymessage><created>2011-12-19T06:29:56+0000</created> <id>0ed7b241-096e-4ca4-abc3-a365f11fae8f</id> <ownerUserId>6f8905b9-1a98-4944-9940-2de5991fa107</ownerUserId> <version>0</version> <body>Test 11:33</body> <inReplyTo>cad1bf26-1294-4076-8b44-1197f2568104</inReplyTo> <logicalMessageId>c8aca89b-84e1-42fe-a32c-5050cd6a0590</logicalMessageId> <originator>447939201990</originator> <recipient>447763686669</recipient> </deliverymessage>";
			DeliveryMessage incoming = ReceiveMO (incomingXml);

			/************************ Simulate Delivery Receipt ******************************/
			// This should be handled by a WebMethod or Web API
			String receiptXml = "<deliveryreceipt><created>2011-12-19T06:29:56+0000</created> <id>aa49b0e4-38f7-4491-a5f4-23a01b03811a</id> <deliveryMessageId>2ae13f9d-f5dc-4478-ab17-ba13ddeffad2</deliveryMessageId> <status>ACCEPTED</status><clientReference>myref</clientReference><part>1</part><parts>1</parts></deliveryreceipt>";
			DeliveryReceipt receipt = ReceiveDR (receiptXml);
		}


		/**
		 *  Mobile Originated (incoming message) Deserialization
		 * 
		 * It is recommended that incoming messages are pushed to a local queue/db and handled asynchronously.
		 **/
		public static DeliveryMessage ReceiveMO (String incomingXml)
		{			
			// Parse the XML
			DeliveryMessage incomingMessage = (DeliveryMessage)UnSerializeObject (incomingXml, typeof(DeliveryMessage));

			Console.WriteLine ("Incoming: " + incomingMessage.originator + " -> " + incomingMessage.recipient + ": " + incomingMessage.body);

			return incomingMessage;
		}

		/**
		 * Delivery Receipt Deserialization
		 * 
		 * It is recommended that delivery receipts are pushed to a local queue/db and handled asynchronously.
		 **/
		public static DeliveryReceipt ReceiveDR (String receiptXml)
		{			

			DeliveryReceipt deliveryReceipt = (DeliveryReceipt)UnSerializeObject (receiptXml, typeof(DeliveryReceipt));

			Console.WriteLine ("Receipt for MessageId: " + deliveryReceipt.messageId + ", My Reference: " + deliveryReceipt.reference + ", Status: " + deliveryReceipt.status);

			return deliveryReceipt;

		}


		/**
		 * Unserialize Objects
		 **/
		public static Object UnSerializeObject (String toSerialize, Type type)
		{
			
			XmlSerializer xmlSerializer = new XmlSerializer (type);

			using (TextReader reader = new StringReader (toSerialize)) {

				return xmlSerializer.Deserialize (reader);
			}
		}

		/**
		 * Serialize Objects
		 **/
		public static string SerializeObject (Object toSerialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer (toSerialize.GetType ());

			using (StringWriter textWriter = new StringWriter ()) {
				xmlSerializer.Serialize (textWriter, toSerialize);
				return textWriter.ToString ();
			}
		}
	}


}
