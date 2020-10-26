using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CrocCSharpBot
{
    /// <summary>
    /// Пользователь бота
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя в Телеграм
        /// </summary>
        [XmlAttribute()]//Атрибуты, метаданные, навешивается на составляющие класса
        public long ID;
        /// <summary>
        /// Имя
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public string FirstName;
        /// <summary>
        /// Фамимлия
        /// </summary>
        [XmlElement(ElementName = "Family")]
        public string LastName;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName;
        /// <summary>
        /// Номер телефона пользователя
        /// </summary>
        public string PhoneNumber;
        /// <summary>
        /// Описание пользователя
        /// </summary>
        [XmlText()]
        public string Description;

    }
}
