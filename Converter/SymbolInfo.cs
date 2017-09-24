using System.Drawing;

namespace Converter
{
    /// <summary>
    /// Класс с информацией об одном символе
    /// </summary>
    public class SymbolInfo
    {
        /// <summary>
        /// Матрица тона
        /// </summary>
        private float[,] _matrix;
        //Размеры одной ячейки [4,3] изображения символа
        private static byte _x = 6, _y = 7;
        /// <summary>
        /// Номер символа в таблице ASCII
        /// </summary>
        public byte num;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="n">Номер ASCII-символа</param>
        public SymbolInfo(byte n)
        {
            num = n;
            _matrix = new float[4, 3];
            FillMatrix();
        }

        /// <summary>
        /// Матрица тона
        /// </summary>
        public float[,] Matrix
        {
            //Поле не может быть изменено извне
            get
            {
                return _matrix;
            }
        }

        /// <summary>
        /// Заполнение матрицы тона данного символа
        /// </summary>
        private void FillMatrix()
        {
            Bitmap bmp = new Bitmap($"img\\{num}.bmp");
            _matrix = BitmapClass.GetParticleTone(bmp, _x, _y);
        }

    }
}
