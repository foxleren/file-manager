namespace file_manager {

    /// <summary>
    /// Класс для изменения цвета в консоли.
    /// </summary>
    public static class ConsoleStyle {
        /// <summary>
        /// Цвет для информации.
        /// </summary>
        public static void SetStyleForInfo() {
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
        
        /// <summary>
        /// Цвет для важного.
        /// </summary>
        public static void SetStyleForImportant() {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }

        /// <summary>
        /// Обычный цвет.
        /// </summary>
        public static void SetDefault() {
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}