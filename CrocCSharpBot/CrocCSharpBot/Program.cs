using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Пространство имён приложения
/// </summary>
namespace CrocCSharpBot
{
    /// <summary>
    /// Главный класс приложения
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args">Параметры командной строки</param>
        static void Main(string[] args)
        {
            try
            {
                Bot bot;
                bot = new Bot();
                bot.Run();
                Console.WriteLine("Запуск бота в консольном режиме.");
            }
            catch (Exception ex)
            {
                //Отображение вложенных исключений
                do
                {
                    Console.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
                while (ex != null);
            }
            finally
            {
                Console.WriteLine("Нажмите Enter для завершения.");
                Console.ReadLine();
            }
        }
    }
}
