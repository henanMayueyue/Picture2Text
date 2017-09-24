using System;
using System.IO;

namespace Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Проверяем, созданы ли файлы
            if (!File.Exists(@"img\32.bmp"))
                BitmapClass.DrawSymbols();
            string path = String.Empty;
            int scale = 0;
            //Если не введено ни одно условие - вводим путь до файла в консоли
            if (args.Length < 1)
            {
                Console.WriteLine("Введите имя или путь до файла с изображением:");
                path = Console.ReadLine();
                while (!File.Exists(path))
                {
                    Console.Clear();
                    Console.WriteLine("Данный файл не найден. Повторите ввод:");
                    path = Console.ReadLine();
                }
            }
            //Если в аргументах нет масштаба - вводим в консоли
            if (args.Length < 2)
            {
                Console.WriteLine("Введите масштаб для изображения (количество пикселей в стороне " +
                    "квадрата n*n который будет посчитан как 1 ячейка для каждого сектора 4*3):");
                
                while (!Int32.TryParse(Console.ReadLine(), out scale) || scale < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Масштаб должен быть целым положительным числом. Повторите ввод:");
                }
            }
            //Если все аргументы присутствуют при вызове программы - записываем их
            if (args.Length == 2)
            {
                path = args[0];
                while (!Int32.TryParse(args[1], out scale) || scale < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Масштаб должен быть целым положительным числом. Повторите ввод:");
                }
            }
            //Основная работа программы
            BitmapClass.PicConversion(path, scale);
        }
    }

    

    
}
