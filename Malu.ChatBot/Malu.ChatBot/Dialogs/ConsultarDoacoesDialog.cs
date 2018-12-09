using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using System.Text;

namespace Malu.ChatBot.Dialogs
{
    [Serializable]
    public class ConsultarDoacoesDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(PerguntarCPF);
            
            return Task.CompletedTask;
        }

        private async Task PerguntarCPF(IDialogContext context, IAwaitable<object> result)
        {
            if (context.Activity.Type != ActivityTypes.Message)
            {
                context.Wait(PerguntarCPF);
                return;
            }

            var text = context.Activity.AsMessageActivity().Text;
            await Typing(context, 600);
            await context.PostAsync("Olá, para consultar o destino de suas doações digite o seu CPF ou CNPJ por favor!");

            context.Wait(MessageReceivedAsync);
        }

        private async Task<bool> Typing(IDialogContext context, int miliseconds)
        {
            var reply = context.MakeMessage();
            reply.Type = ActivityTypes.Typing;
            await context.PostAsync(reply);

            Thread.Sleep(miliseconds);

            return true;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = context.MakeMessage();

            HttpClient client = new HttpClient();

            var text = context.Activity.AsMessageActivity().Text;

            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var jsonObject = new { dateMin = text.ToLowerInvariant() };
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                        
            var heroCards = new List<Attachment>();
            
            var url = "http://apaesorocaba.org.br/";
            await Typing(context, 900);
            await context.PostAsync("Esse mês você nos ajudou com R$ 2.500 no dia 08/12/2018, essa doação nos ajudou na realização de dois eventos, onde conseguimos angariar fundos para a reforma da nossa unidade!");
            await Typing(context, 900);
            await context.PostAsync("Em nome da nossa instituição eu Malu gostaria de agradecer você pela sua participação, você é muito importante pra gente, segue abaixo uma lista com mais detalhes sobre algumas das ações que sua doação nos ajudou a realizar!");

            
            heroCards.Add(new HeroCard
            {
                // title of the card  
                Title = "Compra de material didatico para as aulas de ingresso ao mercado de trabalho!",
                //subtitle of the card  
                Subtitle = "Nossos jovens agora terão material didatico novinho, graças a sua contribuição",                
                //Detail Text  
                Text = "Evento realizado pela APAE Sorocaba, contou com a presença da sociedade, e além de muita comida boa teve também muita soliedariedade!",
                // list of  Large Image  
                Images = new List<CardImage> { new CardImage("https://2.bp.blogspot.com/-lA9N49dJQ8I/WTHuzgVIGiI/AAAAAAAAddg/iaLofCGMMX4FClN5b0hYNtEEkfISUq6vQCK4B/s1600/_DSC0211.jpg") },
                // list of buttons   
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Saiba mais em.", value: "http://site.siteargus.com.br/564/vergaleria/1460") }
            }.ToAttachment());

            heroCards.Add(new HeroCard
            {
                // title of the card  
                Title = "17º Boi no Rolete",
                //subtitle of the card  
                Subtitle = "Evento arrecadou mais de 13 mil reais para a APAE Sorocaba!",
                // navigate to page , while tab on card  
                Tap = new CardAction(ActionTypes.OpenUrl, "Veja mais fotos.", value: "http://site.siteargus.com.br/564/vergaleria/1460"),
                //Detail Text  
                Text = "Evento realizado pela APAE Sorocaba, contou com a presença da sociedade, e além de muita comida boa teve também muita soliedariedade!",
                // list of  Large Image  
                Images = new List<CardImage> { new CardImage("http://site.siteargus.com.br/sendThumb.asp?path=d:\\web\\localuser\\siteargus\\www\\material_apae\\CA_564\\galeria\\1460&file=21_5_564_1460_Fotos%20apae-65.jpg") },
                // list of buttons   
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Veja mais fotos.", value: "http://site.siteargus.com.br/564/vergaleria/1460") }
            }.ToAttachment());

            heroCards.Add(new HeroCard
            {
                // title of the card  
                Title = "Festa Junina Solidária - Junho de 2018",
                //subtitle of the card  
                Subtitle = "Evento arrecadou mais de 5 mil reais para a APAE Sorocaba!",
                // navigate to page , while tab on card  
                Tap = new CardAction(ActionTypes.OpenUrl, "Veja mais fotos.", value: "http://site.siteargus.com.br/564/vergaleria/1675"),
                //Detail Text  
                Text = "Tivemos nesse evento um mar de comidas tipicas, fogueira, dança tipica!",
                // list of  Large Image  
                Images = new List<CardImage> { new CardImage("http://site.siteargus.com.br/sendThumb.asp?path=d:\\web\\localuser\\siteargus\\www\\material_apae\\CA_564\\galeria\\1675&file=13_6_564_1675_IMG-20180613-WA0029.jpg") },
                // list of buttons   
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Veja mais fotos.", value: "http://site.siteargus.com.br/564/vergaleria/1675") }
            }.ToAttachment());

            message.Attachments = heroCards;
            await Typing(context, 2600);
            await context.PostAsync(message);           
            context.Wait(ResumeAfterSupportDialog);
        }              

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<object> serverName)
        {

            context.Done<string>(null);
        }
    }
}