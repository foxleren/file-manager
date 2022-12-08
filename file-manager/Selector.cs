using static file_manager.UserInterface;

namespace file_manager
{
    public static class Selector
    {
        /// <summary>
        /// Метод отвечает за логику обработку команд. 
        /// </summary>
        /// <param name="listOfOptions">опции для меню</param>
        /// <param name="isMainMenu">явлеяется ли главным меню</param>
        /// <param name="isAdditionalMenu">явлеяется ли доп меню</param>
        /// <param name="isAvalailableCancel">можно ли сделать шаг назад и нажать Cancel</param>
        /// <returns></returns>
        public static int RunSelector(string[] listOfOptions, bool isMainMenu, bool isAdditionalMenu,
            bool isAvalailableCancel)
        {
            int pointer = 0;
            MakeMove(ref pointer, listOfOptions);

            ConsoleKey key;
            if (isMainMenu && !isAdditionalMenu && isAvalailableCancel)
            {
                while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter && key != ConsoleKey.F10)
                {
                    MovePointer(key, ref pointer, listOfOptions);
                }

                if (key == ConsoleKey.F10)
                {
                    ClearConsoleAndShowMessage("Нажмите повторно F10 для завершения программы. \n" +
                                               "Для возобновления программы нажмите любую другую клавишу.");
                    return -1;
                }
            }

            if (!isMainMenu && !isAdditionalMenu && isAvalailableCancel)
            {
                while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter && key != ConsoleKey.Escape)
                {
                    MovePointer(key, ref pointer, listOfOptions);
                }

                if (key == ConsoleKey.Escape)
                {
                    ClearConsoleAndShowMessage("Вы нажали Escape. \nДля возврата в меню нажмите любую клавишу.");
                    return -1;
                }
            }

            if (!isMainMenu && isAdditionalMenu && isAvalailableCancel)
            {
                while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter && key != ConsoleKey.F1)
                {
                    MovePointer(key, ref pointer, listOfOptions);
                }

                if (key == ConsoleKey.F1)
                {
                    ClearConsoleAndShowMessage("Вы нажали F1.\nДля завершения выбора расположения вашего файла " +
                                               "нажмите повторно F1.\nВернуться к выбору расположения можно, нажав любую клавишу.");
                    return -1;
                }
            }

            if (!isAvalailableCancel)
            {
                while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
                {
                    MovePointer(key, ref pointer, listOfOptions);
                }
            }

            Console.Clear();
            return pointer;
        }

        /// <summary>
        /// Запуск смены позиции курсора.
        /// </summary>
        /// <param name="pointer">положение курсора</param>
        /// <param name="listOfOptions">опции меню</param>
        private static void MakeMove(ref int pointer, string[] listOfOptions)
        {
            Console.SetCursorPosition(0, 1);
            Console.CursorVisible = false;

            for (int i = 0; i < listOfOptions.Length; i++)
            {
                WriteOption(listOfOptions[i], i, pointer);
            }

            Console.SetCursorPosition(0, 1);
        }

        /// <summary>
        /// Смена позиции курсора.
        /// </summary>
        /// <param name="key">нажатая клавиша</param>
        /// <param name="pointer">положение курсора</param>
        /// <param name="listOfOptions">список опций</param>
        private static void MovePointer(ConsoleKey key, ref int pointer, string[] listOfOptions)
        {
            if (key == ConsoleKey.DownArrow)
            {
                if (pointer + 1 < listOfOptions.Length)
                {
                    pointer++;
                    ChangePositionOfPointer(pointer - 1, pointer, listOfOptions);
                }
            }

            if (key == ConsoleKey.UpArrow)
            {
                if (pointer - 1 >= 0)
                {
                    pointer--;
                    ChangePositionOfPointer(pointer + 1, pointer, listOfOptions);
                }
            }
        }

        /// <summary>
        /// Смена цветов для строк.
        /// </summary>
        /// <param name="previous">предыдущая строка</param>
        /// <param name="currentPointer">новое положение</param>
        /// <param name="listOfOptions">опции</param>
        private static void ChangePositionOfPointer(int previous, int currentPointer, string[] listOfOptions)
        {
            Console.SetCursorPosition(0, previous + 1);
            Console.Write(listOfOptions[previous]);

            Console.SetCursorPosition(0, currentPointer + 1);

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;

            Console.Write(listOfOptions[currentPointer]);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(0, currentPointer + 1);
        }

        /// <summary>
        /// Показ опции.
        /// </summary>
        /// <param name="option">опция</param>
        /// <param name="currentPointer">старое положение курсора</param>
        /// <param name="pointer">новое положение курсора</param>
        private static void WriteOption(string option, int currentPointer, int pointer)
        {
            if (currentPointer == pointer)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;

                Console.WriteLine(option);

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }

            Console.WriteLine(option);
        }
    }
}