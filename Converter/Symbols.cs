using System.Collections.Generic;

namespace Converter
{
    /// <summary>
    /// Класс, хранящий информацию о коллекции символов
    /// </summary>
    public class Symbols
    {
        /// <summary>
        /// Список символов
        /// </summary>
        public List<SymbolInfo> Collection;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Symbols()
        {
            Collection = new List<SymbolInfo>();
            for (int i = 32; i < 256; i++)
            {
                if (i > 126 && i < 161 || i == 173)
                    continue;
                Collection.Add(new SymbolInfo((byte)i));

            }
        }
    }
}
