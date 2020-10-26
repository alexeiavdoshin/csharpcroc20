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
        /// Ведение журнала событий
        /// </summary>
        private NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Состояние бота
        /// </summary>
        private BotState state;
        /// <summary>
        /// Неинициализирующий конструктор
        /// </summary>
        public Bot()
        {
            //создание клиента для Telegram
            string token = Properties.Settings.Default.Token;
            client = new TelegramBotClient(token);
            var user = client.GetMeAsync();
            var name = user.Result.Username;
            log.Trace(name);
            //обработчик событий 
            client.OnMessage += MessageProcessor; //???
            //Чтение сохранённого состояния из файла
            state = BotState.Load(Properties.Settings.Default.FileName);

        }
        /// <summary>
        /// Обработчик входящего сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                log.Trace("->| MessageProcessor()");
                switch (e.Message.Type)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Contact://телефон
                        if (e.Message.Contact.UserId != e.Message.Chat.Id)
                        {
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Некорректный контакт");
                            return;//??
                        }
                        string phone = e.Message.Contact.PhoneNumber;                        
                        log.Trace(phone);
                        //Регистрация пользователя
                        var user = new User()
                        //Использование инициализатора
                        {
                            ID = e.Message.Contact.UserId,
                            FirstName = e.Message.Contact.FirstName,
                            LastName = e.Message.Contact.LastName,
                            UserName = e.Message.Chat.Username,
                            PhoneNumber = phone,
                        };
                        if (state.AddUser(user))
                        {
                            state.Save(Properties.Settings.Default.FileName);
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Твой телефон добавлен в базу: {phone}");
                        }
                        else
                        {
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Твой телефон уже есть в базе: {phone}");
                        }
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
                        log.Info(e.Message.Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
            }
            finally
            {
                log.Trace("|-> MessageProcessor()");
            }
            

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
                case "help":
                    string m = "Список возможных команд:\n";
                    foreach (Commands s in Enum.GetValues(typeof(Commands)))
                    {
                        string cmd = s.ToString().ToLower();
                        string descr = s.ToDescription();
                        m += $"/{cmd} - {descr}\n";
                    }
                    client.SendTextMessageAsync(message.Chat.Id, m, replyMarkup: null);
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
