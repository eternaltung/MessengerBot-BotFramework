using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFrameworkSample
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                if (activity.ChannelId == "facebook")
                {
                    Activity reply = activity.CreateReply();
                    reply.ChannelData = GetGenericTemplate();
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private JObject GetGenericTemplate()
        {
            // API reference 
            // https://developers.facebook.com/docs/messenger-platform/send-api-reference/generic-template

            return JObject.FromObject(new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "generic",
                        elements = new object[]
                        {
                            new
                            {
                                title = "title1",
                                image_url = "http://petersapparel.parseapp.com/img/item100-thumb.png",
                                subtitle = "sub1",
                                buttons = new object[] 
                                {
                                    new
                                    {
                                        type = "web_url",
                                        url = "https://petersapparel.parseapp.com/view_item?item_id=100",
                                        title = "website"
                                    },
                                    new
                                    {
                                        type = "postback",
                                        title = "chat",
                                        payload = "payload"
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}