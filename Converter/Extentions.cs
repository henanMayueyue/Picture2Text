using System;
using System.Collections.Generic;
using System.Linq;

namespace Converter
{
    /// <summary>
    /// Класс расширений
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Поиск символа, похожего на данный сектор изображения, представленный матрицей тона
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Список сущностей</param>
        /// <param name="segment">Сектор изображения</param>
        /// <returns>Наиболее похожий символ из имеющегося списка</returns>
        public static char FindSimmilar<T>(this List<T> list, float[,] segment)
            where T : SymbolInfo
        {
            /*float будет [4,3]*/
            int[] difArray = new int[256];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    list.SortByFieldDif(x, y, segment[x, y]);
                    //разница оттенком сегмента и ячейки символа
                    float dif = Math.Abs(segment[x, y] - list.ElementAt(0).Matrix[x, y]);
                    //Счетчик "близости" значений. Чем меньше - тем больше подходит этот символ
                    int count = 1;
                    foreach (T s in list)
                    {
                        float newDif = Math.Abs(segment[x, y] - s.Matrix[x, y]);
                        if (newDif > dif)
                        {
                            dif = newDif;
                            count++;
                        }
                        difArray[s.num] += count;
                    }
                }
            }
            int min = Int32.MaxValue, minIndex = 0;
            for (int i = 0; i < difArray.Length; i++)
            {
                if (difArray[i] != 0 && difArray[i] < min)
                {
                    min = difArray[i];
                    minIndex = i;
                }
            }
            return (char)minIndex;
        }

        /// <summary>
        /// Сортировка по разнице между эталонным значением и значениями в поле [i,j] матрицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Список сущностей</param>
        /// <param name="i">Номер строки ячейки матрицы</param>
        /// <param name="j">Номер столбца ячейки матрицы</param>
        /// <param name="cmp">Эталонное значение для сравнения</param>
        public static void SortByFieldDif<T>(this List<T> list, int i, int j, float cmp)
            where T : SymbolInfo
        {
            list.Sort((mat1, mat2) =>
            {
                float dif1 = Math.Abs(mat1.Matrix[i, j] - cmp),
                    dif2 = Math.Abs(mat2.Matrix[i, j] - cmp);
                return dif1.CompareTo(dif2);
            });
        }
    }
}
