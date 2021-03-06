﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

namespace CrocCSharpBot
{
    /// <summary>
    /// Состояние бота
    /// </summary>
    [XmlRoot(ElementName = "State", Namespace = "http://www.croc.ru")]
    public class BotState
    {
        /// <summary>
        /// Массив пользователей
        /// </summary>
        [XmlElement(ElementName = "User")]
        public User[] Users;
        /// <summary>
        /// Индексатор по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя Telegram</param>
        /// <returns>Всегда возвращает пользователя, создаёт его по необходимости</returns>
        public User this[long id]
        {
            get
            {
                User user = Users.Where(a => a.ID == id).FirstOrDefault();
                if(user == null)
                {
                    //Создание нового пользователя; всё, что мы знаем - его идентификатор
                    user = new User()
                    {
                        ID = id
                    };
                    Array.Resize(ref Users, Users.Length + 1);
                    Users[Users.Length - 1] = user;                    
                }
                return user;
            }            
        }
        /// <summary>
        /// Добавление пользователя в массив
        /// </summary>
        /// <param name="user"></param>
        [Obsolete("Следует использовать индексатор [id]")]
        public bool AddUser(User user)
        {
            // Проверка на наличие пользователей
            if (Users == null)
            {
                Users = new User[1] { user };
                return true;
            }
            //Fluent, Link, L-выражения
            else if (Users.Where(a=>a.ID == user.ID).FirstOrDefault() == null)
            {
                Array.Resize(ref Users, Users.Length + 1);
                Users[Users.Length - 1] = user;
                return true;
            }
            {
                //Пользователь уже есть в массиве
                return false;
            }
        }
        /// <summary>
        /// Загрузка объекта из XML-файла
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BotState Load(string name)
        {
            try
            {
                //Объект, выполняющий сериализацию
                var s = new XmlSerializer(typeof(BotState));
                //Файл для чтения данных
                using (XmlReader r = XmlReader.Create(name))
                {
                    return (BotState)s.Deserialize(r);
                }
            }
            catch(System.IO.FileNotFoundException)
            {
                //Файл не найден - новое пустое состояние
                return new BotState();
            }
        }
        /// <summary>
        /// Сохранение состояния в виде XML-файла
        /// </summary>
        /// <param name="name">Имя файла</param>
        public void Save (string name)
        {
            //Объект, выполняющий сериализацию
            var s = new XmlSerializer(typeof(BotState));
            //Настройка формирования XML-файла
            var settings = new XmlWriterSettings()
            {
                Indent = true//Человекочитаемый XML-файл
            };
            //Файл для записи данных
            using (XmlWriter w = XmlWriter.Create(name, settings))//??
            {
                //СЕРИАЛИЗАЦИЯ!
                s.Serialize(w, this);
            }
        }
    }
}
