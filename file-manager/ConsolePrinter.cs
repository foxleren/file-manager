namespace file_manager {
    
    /// <summary>
    /// Класс для вывода текстовой информации пользователю.
    /// </summary>
    public static class UserInterface {
        
        /// <summary>
        /// Очистка консоли и вывод.
        /// </summary>
        /// <param name="message"></param>
        public static void ClearConsoleAndShowMessage(string message) {
            Console.Clear();
            ConsoleStyle.SetStyleForInfo();
            Console.WriteLine(message);
        }

        /// <summary>
        /// Вывод сообщения со стилем.
        /// </summary>
        /// <param name="message"></param>
        public static void ShowInfoMessage(string message) {
            ConsoleStyle.SetStyleForInfo();
            Console.WriteLine(message);
        }

        /// <summary>
        /// Вывод сообщения с выбранной опцией.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="option"></param>
        public static void ShowChosenOption(string message, string option) {
            ConsoleStyle.SetStyleForInfo();
            Console.Write(message);
            ConsoleStyle.SetDefault();
            Console.WriteLine(option);
        }

        /// <summary>
        /// Линия).
        /// </summary>
        public static void ShowLine() {
            Console.WriteLine("----------------------------------------------");
        }
        
        /// <summary>
        /// Сообщение: нажмите клавишу.
        /// </summary>
        /// <param name="message"></param>
        public static void ShowContinueMessage(string message) {
            ConsoleStyle.SetStyleForInfo();
            Console.WriteLine(message);
        }
        
        /// <summary>
        /// Вывод  приветствия.
        /// </summary>
        public static void ShowEntranceMessage() {
            Console.WriteLine("Привет, дорогой друг! Добро пожаловать в приложение.");
            ConsoleStyle.SetStyleForImportant();
            Console.WriteLine(
                "!!! Для правильного отображения интерфейса откройте, пожалуйста, окно на весь экран !!!");
            Console.WriteLine("!!! Перед началом работы с файловой системой ОБЯЗАТЕЛЬНО прочтите INFO !!!");
            ConsoleStyle.SetDefault();
            Console.WriteLine("Нажми Enter для продолжения. Для выхода необходимо нажать F10." +
                              "\n*Выход из приложения доступен только в главном меню для избежания мискликов.*");
            WaitForConsoleKey(ConsoleKey.Enter);
        }
        
        /// <summary>
        /// Вывод важной инфы.
        /// </summary>
        public static void ShowInfo() {
            Console.WriteLine("Краткое описание работы программы: \n" +
                              "------------------------------------------------------------- \n" +
                              "Принцип программы: \n" +
                              "1. Выберете диск, с которым собираетесь работать.\n" +
                              "2. Далее, вы можете начать проход по директориям с файлами.\n" +
                              "   При нажатии на файл открывает меню с доступными опциями."+
                              "3. Также вам доступны опции помимо прохода по папкам и взаимодействия с файлами.\n\n" +
                              "Важное уточнение для понимания подсказок программы: \nГлавное меню - самое первое меню," +
                              " которое вы видете при запуске программы, \nдалее вы будете сталкиваться с подразделами" +
                              " главного, либо попадете \nв дополнительное меню для выбора расположения второго файла/" +
                              "перемещения файла и тд.\n\n" +
                              "Немного про горячие клавиши:\n" +
                              "Escape доступен в тех разделах меню, где подразумевается возможность \nмгновенно " +
                              "вернуться в главное меню. \n" +
                              "Для выхода нажмите F10, НО выход из программы возможен ТОЛЬКО В ГЛАВНОМ МЕНЮ. \n" +
                              "Когда определенные функции будут требовать вашей команды для завершения, то вы \n" +
                              "увидите подсказку с необходимой горячей клавишей." +
                              "*Если функция подразумевает работу с двумя файлами \nили выбор места расположения, " +
                              "то программа сама предложит вам выбрать \nвторую директорию/(директорию и файл при" +
                              " необходимости). \n\nПросто следуйте подсказкам интерфейса и у вас" +
                              " все получится :)))))");
        }
        
        /// <summary>
        /// Ожидание нажатия Enter.
        /// </summary>
        /// <param name="key"></param>
        public static void WaitForConsoleKey(ConsoleKey key) {
            while (Console.ReadKey(true).Key != key) ;
        }
        
    }
}