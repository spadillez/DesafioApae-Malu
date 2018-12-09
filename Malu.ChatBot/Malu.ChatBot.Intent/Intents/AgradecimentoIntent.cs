using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malu.ChatBot.IntentService.Interfaces;

namespace Malu.ChatBot.IntentService.Intents
{
    public class AgradecimentoIntent : IIntent
    {
        public async Task Responder(IDialogContext context)
        {
            await context.PostAsync("😁 Eu que agradeço. Posso ajudar em mais alguma coisa?");
            context.Done<string>(null);
        }
    }
}
