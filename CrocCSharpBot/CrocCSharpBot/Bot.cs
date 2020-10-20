using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CrocCSharpBot
{
    public class Bot
    {
        /// <summary>
        /// Клиент Telegram
        /// </summary>
        private TelegramBotClient client;
        /// <summary>
        /// Объект, осуществляющий протоколирование
        /// </summary>
        private NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Неинициализирующий конструктор
        /// </summary>
        public Bot()
        {
            //создание клиента для Telegram
            client = new TelegramBotClient("1085126912:AAEUp7iuUa0Uc6kke8-GbIT7iupocjjHZXA");
            var user = client.GetMeAsync();
            var name = user.Result.Username;
            log.Trace(name);
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
            log.Trace("->| MessageProcessor()");
            switch (e.Message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Contact://телефон
                    string phone = e.Message.Contact.PhoneNumber;
                    client.SendTextMessageAsync(e.Message.Chat.Id, $"Твой телефон: {phone}");
                    log.Trace(phone);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Text:
                    if (e.Message.Text.Substring(0, 1) == "/")
                    {
                        CommandProcessor(e.Message);
                    }
                    else
                    {
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Привет! Ты сказал мне: {e.Message.Text}");
                        log.Trace(e.Message.Text);
                    }              
                    break;

                default:
                    client.SendTextMessageAsync(e.Message.Chat.Id, $"Привет! Ты прислал мне: {e.Message.Type}, но я это пока не понимаю.");
                    Console.WriteLine(e.Message.Type);
                    break;
            }
            log.Trace("|-> MessageProcessor()");

        }
        /// <summary>
        /// Обработка команды
        /// </summary>
        /// <param name="message"></param>
        private void CommandProcessor(Telegram.Bot.Types.Message message)
        {
            //Отрезаем первый символ, который должен быть '/'
            string command = message.Text.Substring(1).ToLower();
            switch (command)
            {
                case "start":
                    var button = new KeyboardButton("Поделись телефоном");
                    button.RequestContact = true;
                    //создание массива из одной кнопки
                    var array = new KeyboardButton[] { button };
                    var reply = new ReplyKeyboardMarkup(array, true, true);
                    client.SendTextMessageAsync(message.Chat.Id, $"Привет, {message.Chat.FirstName}, скажи мне свой телефон", replyMarkup: reply);
                    break;
                default:
                    client.SendTextMessageAsync(message.Chat.Id, $"Я пока не понимаю команду /{command}.");
                    break;

            }
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
