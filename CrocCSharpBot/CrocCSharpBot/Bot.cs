using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CrocCSharpBot
{
    public class Bot
    {
        /// <summary>
        /// Клиент Telegram
        /// </summary>
        private TelegramBotClient client;
        /// <summary>
        /// Неинициализирующий конструктор
        /// </summary>
        public Bot()
        {
            //создание клиента для Telegram
            client = new TelegramBotClient("1085126912:AAEUp7iuUa0Uc6kke8-GbIT7iupocjjHZXA");
            //обработчик событий 
            client.OnMessage += MessageProcessor; //???

        }
        /// <summary>
        /// Обработчик входящего сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            client.SendTextMessageAsync(e.Message.Chat.Id, "Привет!");
            Console.WriteLine(e.Message.Text);
        }

        /// <summary>
        /// Запуск бота
        /// </summary>
        public void Run()
        {
            //Запуск приёма сообщений
            client.StartReceiving();
        }
    }
}
