using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class AWSSNS
    {
        AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient();
        public AWSSNS()
        {
            
        }

        public int CreateTopic(string strTopicName, string strDisplayName)
        {
            var topic = snsClient.FindTopicAsync(strTopicName).Result;
            if (topic == null)
            {
                var topicRequest = new CreateTopicRequest
                {
                    Name = strTopicName
                };

                var topicResponse = snsClient.CreateTopicAsync(topicRequest);

                var topicAttribute = new SetTopicAttributesRequest
                {
                    TopicArn = topicResponse.Result.TopicArn,
                    AttributeName = "DisplayName",
                    AttributeValue = strDisplayName
                };

                snsClient.SetTopicAttributesAsync(topicAttribute);
                return (int)topicResponse.Result.HttpStatusCode;
            }

            return 0;
        }

        public int CreateMobileSubscription(string strTopicName, string strEndPoint)
        {
            var topicResponse = snsClient.FindTopicAsync(strTopicName).Result;

            var subscribeRequest = new SubscribeRequest();
            subscribeRequest.TopicArn = topicResponse.TopicArn;
            subscribeRequest.Protocol = "SMS";
            subscribeRequest.Endpoint = strEndPoint;

            var response = snsClient.SubscribeAsync(subscribeRequest);
            return (int)response.Result.HttpStatusCode;
        }


        public int CreateEmailSubscription(string strTopicName, string strEndPoint)
        {
            var topicResponse = snsClient.FindTopicAsync(strTopicName).Result;

            var subscribeRequest = new SubscribeRequest();
            subscribeRequest.TopicArn = topicResponse.TopicArn;
            subscribeRequest.Protocol = "Email";
            subscribeRequest.Endpoint = strEndPoint;

            var response = snsClient.SubscribeAsync(subscribeRequest);
            return (int)response.Result.HttpStatusCode;
        }


        public int PublishMobileMessage(string strMessage, string strPhoneNumber)
        {
            PublishRequest publishRequest = new PublishRequest();
            publishRequest.Message = strMessage;
            publishRequest.PhoneNumber = strPhoneNumber;
            publishRequest.MessageAttributes.Add("AWS.SNS.SMS.SMSType", new MessageAttributeValue { StringValue = "Transactional", DataType = "String" });

            PublishResponse publishResponse = snsClient.PublishAsync(publishRequest).Result;

            return (int)publishResponse.HttpStatusCode;
        }

        public int PublishEmailMessage(string strTopicName, string strMessage, string subject)
        {
            var topicResponse = snsClient.FindTopicAsync(strTopicName).Result;

            var publishMessage = new PublishRequest();
            publishMessage.TopicArn = topicResponse.TopicArn;
            publishMessage.Message = strMessage;
            publishMessage.Subject = subject;

            var response = snsClient.PublishAsync(publishMessage).Result;
            return (int)response.HttpStatusCode;
        }
    }
}
}
