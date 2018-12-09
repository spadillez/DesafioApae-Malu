using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malu.ChatBot.IntentService.Interfaces
{
    public interface IIntent
    {
        Task Responder(IDialogContext context);
    }
}
