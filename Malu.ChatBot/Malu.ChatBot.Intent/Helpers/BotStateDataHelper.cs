using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Malu.ChatBot.IntentService.Helpers
{
    public class BotStateDataHelper
    {
        public async Task<bool> SetStateData(IActivity activity, string key, string value, bool commit)
        {
            var message = activity as IMessageActivity;
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
            {
                var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                var addressKey = new AddressKey()
                {
                    BotId = message.Recipient.Id,
                    ChannelId = message.ChannelId,
                    UserId = message.From.Id,
                    ConversationId = message.Conversation.Id,
                    ServiceUrl = message.ServiceUrl
                };
                var userData = await botDataStore.LoadAsync(addressKey, BotStoreType.BotUserData, CancellationToken.None);

                userData.SetProperty(key, value);
                await botDataStore.SaveAsync(addressKey, BotStoreType.BotUserData, userData, CancellationToken.None);

                if (commit)
                {
                    await botDataStore.FlushAsync(addressKey, CancellationToken.None);
                }

                return true;
            }
        }

        public async Task<string> GetStateData(IActivity activity, string key)
        {
            var message = activity as IMessageActivity;
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
            {
                var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                var addressKey = new AddressKey()
                {
                    BotId = message.Recipient.Id,
                    ChannelId = message.ChannelId,
                    UserId = message.From.Id,
                    ConversationId = message.Conversation.Id,
                    ServiceUrl = message.ServiceUrl
                };
                var userData = await botDataStore.LoadAsync(addressKey, BotStoreType.BotUserData, CancellationToken.None);

                return userData.GetProperty<string>(key);
            }
        }
    }

    public class AddressKey : IAddress
    {
        public string BotId { get; set; }
        public string ChannelId { get; set; }
        public string ConversationId { get; set; }
        public string ServiceUrl { get; set; }
        public string UserId { get; set; }
    }
}
