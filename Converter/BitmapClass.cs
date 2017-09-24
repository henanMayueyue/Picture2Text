using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Converter
{
    class BitmapClass
    {
        /// <summary>
        /// Процесс отрисовки и сохранения изображений символов
        /// </summary>
        public static void DrawSymbols()
        {
            if (!Directory.Exists("img"))
                Directory.CreateDirectory("img");
            for (int i = 32; i < 256; i++)
            {
                if (i > 126 && i < 161 || i == 173)
                    continue;
                Bitmap bmp = new Bitmap(18, 28);
                RectangleF rectf = new RectangleF(-3, -3, 0, 0);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, bmp.Width, bmp.Height);
                g.DrawString((char)i + "", new Font("Consolas", 20, FontStyle.Regular), Brushes.Black, rectf);
                g.Flush();
                bmp.Save($"img\\{i}.bmp");
            }
        }

        /// <summary>
        /// Процесс перевода изображения в символьную форму
        /// </summary>
        /// <param name="name">Имя или полный путь до файла изображения</param>
        /// <param name="scale">Заданный масштаб</param>
        public static void PicConversion(string name, int scale)
        {
            try
            {
                Bitmap pic = new Bitmap(name);
                //Ресайз изображения, чтобы можно было успешно поделить на части
                int w = pic.Width + (pic.Width % (3 * scale) == 0 ? 0 : 3 * scale - pic.Width % (3 * scale)),
                    h = pic.Height + (pic.Height % (4 * scale) == 0 ? 0 : 4 * scale - pic.Height % (4 * scale));
                pic = new Bitmap(pic, w, h);
                Symbols s = new Symbols();
                int counter = 0;
                while (File.Exists($"ascii ({counter}).txt"))
                {
                    counter++;
                }
                string path = $"ascii ({counter}).txt";
                using (StreamWriter file = new StreamWriter(path, true))
                {
                    //теперь это счетчик переведенных секторов в ascii-символы
                    counter = 1;
                    for (int i = 0; i < h; i+=4*scale)
                    {
                        for (int j = 0; j < w; j+=3*scale)
                        {
                            file.Write(s.Collection.FindSimmilar(GetParticleTone(pic, scale, scale, j, i)));
                        }
                        file.WriteLine();
                        Console.Clear();
                        Console.WriteLine("Выполнено: {0}/{1}", counter, h / (4 * scale));
                        //Cчитаем только линии секторов, ибо с влучае с каждым сектором
                        //действие проходит слишком быстро и готовность не различима пользователем
                        counter++;
                    }
                }
                Process.Start(@"C:\Windows\System32\notepad.exe", path);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка при загрузке изображения: Неверный формат файла");
            }
        }
        /// <summary>
        /// Получение матрицы тонов сектора изображения
        /// </summary>
        /// <param name="bmp">Изображение</param>
        /// <param name="scaleW">Ширина одной ячейки сектора</param>
        /// <param name="scaleH">Высота одной ячейки сектора</param>
        /// <returns>Матрица тонов</returns>
        public static float[,] GetParticleTone(Bitmap bmp, int scaleW, int scaleH)
        {
            return GetParticleTone(bmp, scaleW, scaleH, 0, 0);
        }

        /// <summary>
        /// Получение матрицы тонов сектора изображения, начинающегося с определенной точки изображения
        /// </summary>
        /// <param name="bmp">Изображение</param>
        /// <param name="scaleW">Ширина одной ячейки сектора</param>
        /// <param name="scaleH">Высота одной ячейки сектора</param>
        /// <param name="startW">Координата начала сектора по x</param>
        /// <param name="startH">Координата начала сектора по Y</param>
        /// <returns>Матрица тонов</returns>
        public static float[,] GetParticleTone(Bitmap bmp, int scaleW, int scaleH, 
            int startW, int startH)
        {
            float[,] result = new float[4, 3];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float count = 0f;
                    for (int x = startW + scaleW * j; x < startW + scaleW * (j + 1); x++)
                    {
                        for (int y = startH + scaleH * i; y < startH + scaleH * (i + 1); y++)
                        {
                            Color px = bmp.GetPixel(x, y);
                            count += (float)(px.R + px.G + px.B) / 3 / 255;
                        }
                    }
                    result[i, j] = count / (scaleW * scaleH);
                }
            }
            return result;
        }
    }
}
