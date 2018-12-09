using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malu.ChatBot.IntentService.Interfaces;

namespace Malu.ChatBot.IntentService.Intents
{
    public class SaudacaoIntent : IIntent
    {
        public async Task Responder(IDialogContext context)
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).TimeOfDay;
            string saudacao;

            if (now < TimeSpan.FromHours(12)) saudacao = "Bom dia";
            else if (now < TimeSpan.FromHours(18)) saudacao = "Boa tarde";
            else saudacao = "Boa noite";

            var message = context.MakeMessage();

            message.AddKeyboardCard($"{saudacao}! Eu sou a Malu, estou aqui para te ajudar!, eu posso te ajudar nesses assuntos.",
                new List<string>
                {
                    "Consultar Doações",
                    "Realizar Doações",
                    "Saber Mais"
                }
            );


            await context.PostAsync(message);
            
            context.Done<string>(null);
        }
    }
}
