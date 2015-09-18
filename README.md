# bulksms-csharp-ws

This is a C# project demonstrating the use of Bulk SMS WebService.

## WSDL

WSDL is located at http://apps.rawmobility.com/gateway/ws/sendsms?wsdl

### Send to single recipient

```c#
String url = "http://apps.rawmobility.com/gateway/ws/sendsms";
String routeId = "routeid-routeid-routeid";
String username = "USER_NAME";
String password = "PASS_WORD";
SendSMSService sendSms = new SendSMSService (url);
batchMessageMultiBody sendResult = sendSms.sendSms (username, password, "Test", "61000000000", "Hello this is a test", routeId);
String remoteId = sendResult.recipients [0].id;
Console.WriteLine ("Singlel SMS Sent, ID: " + remoteId);
```

### Send to multiple recipients
```c#

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
	foreach(batchRecipientMultiBody recipientResult in batchResult.recipients) {					
		Console.WriteLine ("SMS To: " + recipientResult.recipient + ", ID: " + recipientResult.id);
	}

} catch (Exception e) {
	Console.WriteLine ("Error: " + e.Message);
}
```

### Receive Incoming Message
```C#
[System.Web.Services.WebMethod()]
public String IncomingMessage(String xml)
{
	blender.DeliveryMessage message = blender.SMSApi.ReceiveMO(xml);
	
	// TODO: Save message, process asynchronously
	
	return "ok";
}
```


### Receive Delivery Receipt
```C#
[System.Web.Services.WebMethod()]
public String DeliveryReceipt(String xml)
{
	blender.DeliveryReceipt receipt = blender.SMSApi.ReceiveDR(xml);
	
	// TODO: Save receipt, process asynchronously
	
	return "ok";
}
```
