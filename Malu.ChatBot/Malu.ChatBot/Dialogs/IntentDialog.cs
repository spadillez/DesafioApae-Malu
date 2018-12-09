using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Malu.ChatBot.IntentService.Intents;
using System.Threading;
using System.Collections.Generic;

namespace Malu.ChatBot.Dialogs
{
    [Serializable]
    public class IntentDialog : LuisDialog<object>
    {
        public IntentDialog(ILuisService service) : base(service) { }

        public IntentDialog(string serviceUrl) : base(CreateService(serviceUrl)) { }
        
        public static ILuisService CreateService(string serviceUrl)
        {
            var connector = new ConnectorClient(new Uri(serviceUrl));

            var attributes = new LuisModelAttribute(
                ConfigurationManager.AppSettings["LuisId"],
                ConfigurationManager.AppSettings["LuisSubscriptionKey"],
                LuisApiVersion.V2);

            return new LuisService(attributes);
        }

        [LuisIntent("")]
        public async Task IntencaoNaoReconhecida(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();

            message.AddKeyboardCard($"Infelizmente, não entendi o que você quis dizer, mas eu posso te ajudar nesses assuntos.",
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

        [LuisIntent("saudacao")]
        public async Task Saudar(IDialogContext context, LuisResult result) { await new SaudacaoIntent().Responder(context); }

        [LuisIntent("confirmacao")]
        public async Task Confirmacao(IDialogContext context, LuisResult result) { await new ConfirmacaoIntent().Responder(context); }

        [LuisIntent("despedida")]
        public async Task Despedida(IDialogContext context, LuisResult result) { await new DespedidaIntent().Responder(context); }

        [LuisIntent("agradecimento")]
        public async Task Agradecimento(IDialogContext context, LuisResult result) { await new DespedidaIntent().Responder(context); }

        [LuisIntent("consultar-doacoes")]
        public async Task ConsultarDoacoes(IDialogContext context, LuisResult result)
        {
            await context.Forward(new ConsultarDoacoesDialog(), this.ResumeAfterSupportDialog, "", CancellationToken.None);
        }

        [LuisIntent("saber-mais")]
        public async Task SaberMais(IDialogContext context, LuisResult result)
        {
            var heroCards = new List<Attachment>();
            var message = context.MakeMessage();
            var url = "http://apaesorocaba.org.br/";
            heroCards.Add(new HeroCard
            {
                Title = "APAE Sorocaba",
                Subtitle = $"Para maiores informações sobre a APAE, visite nosso portal",
                Tap = new CardAction(ActionTypes.OpenUrl, "Saiba Mais", value: url),
                Text = "Cinquenta anos... era tarde daquele 19 de setembro de 1967. Um grupo de pessoas amigas se reuniu para lançar a primeira base de um sonho: formar um espaço de apoio, cuidado e promoção a pessoas com deficiência. E assim foi feito..."
            }.ToAttachment());

            message.Attachments = heroCards;
            await context.PostAsync(message);
        }

        [LuisIntent("realizar-doacoes")]
        public async Task RealizaDoacoes(IDialogContext context, LuisResult result)
        {
            var heroCards = new List<Attachment>();
            var message = context.MakeMessage();
            var url = "http://site.siteargus.com.br/564/contribuicoes/564";
            heroCards.Add(new HeroCard
            {
                Title = "APAE Sorocaba",
                Subtitle = $"Para doar, clique aqui!",
                Tap = new CardAction(ActionTypes.OpenUrl, "Saiba Mais", value: url),
                Text = "Doações através de depósito bancário:" +
                        "\n\nBanco do Brasil:" +
                        "\n\nAg.: 0191 - 0" +
                        "\n\n C / c: 3725 - 7" +
                        "\n\n" +
                        "\n\n Caixa: Ag.: 4137" +
                        "\n\nC / c.: 977 - 7" + 
                        "\n\nCNPJ: 71.869.358 / 0001 - 01"
            }.ToAttachment());

            message.Attachments = heroCards;
            await context.PostAsync(message);
        }

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<object> serverName)
        {
            await context.PostAsync($"Em que mais posso te ajudar?");

            context.Done<string>(null);
        }

    }
}