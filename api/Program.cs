using System;
using apps.rawmobility.com;

namespace blender
{
	class MainClass
	{
		public static void Main (string[] args)
		{

			// Live
			String url = "http://apps.rawmobility.com/gateway/ws/sendsms";
			String routeId = "c46f7737-be0b-48e2-a1fd-0f671f5265eb";
			String username = "USER_NAME";
			String password = "PASS_WORD";

			Console.WriteLine ("Hello World!");
			SendSMSService sendSms = new SendSMSService (url);
			batchMessageMultiBody sendResult = sendSms.sendSms (username, password, "Test", "61000000000", "Hello this is a test", routeId);
			Console.WriteLine ("Singlel SMS Sent, ID: " + sendResult.recipients [0].id);

			// Send batch message

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
				foreach(batchRecipientMultiBody recipientResult in batchResult.recipients) {					
					Console.WriteLine ("SMS To: " + recipientResult.recipient + ", ID: " + recipientResult.id);
				}

			} catch (Exception e) {
				Console.WriteLine ("Error: " + e.Message);
			}
		}
	}
}
