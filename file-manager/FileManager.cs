using System.Text;
using static file_manager.UserInterface;

namespace file_manager {
    
    /// <summary>
    /// Класс реализовывает работу с файлами.
    /// </summary>
    public static class FileManager {
        
        /// <summary>
        /// Текущий путь.
        /// </summary>
        public static string CurrentPath { get; set; }

        /// <summary>
        /// Путь для доп файлов.
        /// </summary>
        public static string AdditionalPath { get; set; }

        /// <summary>
        /// Предыдущий уровень в менеджере.
        /// </summary>
        public static string PreviosLevel { get; set; }

        /// <summary>
        /// Предыдущий уровень для второго файла.
        /// </summary>
        public static string AdditionalPreviousLevel { get; set; }

        /// <summary>
        /// Задан ли диск.
        /// </summary>
        public static bool DriveIsSet { get; set; }

        /// <summary>
        /// Выбор диска.
        /// </summary>
        /// <param name="path">текущий путь</param>
        /// <param name="previousPath">Предыдущий уровень</param>
        public static void SetDrive(ref string path, ref string previousPath) {
            try { 
                ShowInfoMessage("Выберите диск:");
                
                DriveInfo[] drives = DriveInfo.GetDrives();
                string[] listOfDrives = new string[drives.Length];
                int i = 0;
                foreach (var drive in drives) {
                    listOfDrives[i] = "  " + drive.Name;
                    i++;
                }

                int indexOfDrive = Selector.RunSelector(listOfDrives, false, false, true);
                if (indexOfDrive == -1) {
                    return;
                }

                i = 0;
                foreach (var drive in drives) {
                    if (i == indexOfDrive) {
                        previousPath = path;
                        path = drive.Name;
                        break;
                    }

                    i += 1;
                }

                IsPathExists(path);

                ShowChosenOption("Выбран диск: ", $"{path}");
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
                DriveIsSet = true;
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Прохождение по папкам.
        /// </summary>
        /// <param name="path">текущий путь</param>
        /// <param name="previousPath">верхний уровень</param>
        /// <param name="isFinishedSurfing">закончен ли проход по папкам</param>
        /// <exception cref="Exception"></exception>
        private static void SurfDirectories(ref string path, ref string previousPath, ref bool isFinishedSurfing) {
            try {
                if (path == null) {
                    isFinishedSurfing = true;
                    throw new Exception("Опция не доступна. Сначала необходимо выбрать диск. \n" +
                                        "Для возврата нажмите любую клавишу.");
                }

                DirectoryInfo directories = new DirectoryInfo(path);

                string[] listOfDirectories = new string[directories.GetDirectories().Length + 1];
                listOfDirectories[0] = directories.Name + "... <- Текущая директория";
                int i = 1;
                foreach (var directory in directories.GetDirectories()) {
                    listOfDirectories[i] = "  " + directory.Name;
                    i++;
                }

                ShowListOfDirectories(ref path, ref previousPath, listOfDirectories, ref isFinishedSurfing,
                    directories);
            }
            catch (Exception e) {
                isFinishedSurfing = true;
                path = previousPath;
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Вывод текущий поддиректорий.
        /// </summary>
        /// <param name="path">текущий путь</param>
        /// <param name="previousPath">верхний уровень</param>
        /// <param name="listOfDirectories">список поддиректорий</param>
        /// <param name="isFinishedSurfing">закончен ли проход по папкам</param>
        /// <param name="directories">текущие поддиректории</param>
        private static void ShowListOfDirectories(ref string path, ref string previousPath,
            string[] listOfDirectories, ref bool isFinishedSurfing, DirectoryInfo directories) {
            try {
                ShowInfoMessage("Выведен список поддиректорий. Для возврата нажмите Escape.");

                int indexOfSelectedDirectory = Selector.RunSelector(listOfDirectories, false, false, true);
                if (indexOfSelectedDirectory == -1) {
                    isFinishedSurfing = true;
                    return;
                }

                if (indexOfSelectedDirectory == 0) {
                    GoUpper(ref path, ref previousPath, ref isFinishedSurfing);
                    if (isFinishedSurfing) {
                        return;
                    }
                }

                int i = 1;
                foreach (var directory in directories.GetDirectories()) {
                    if (i == indexOfSelectedDirectory) {
                        previousPath = path;
                        path = directory.FullName;
                        break;
                    }

                    i += 1;
                }
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Получение  и директорий, и файлов сразу.
        /// </summary>
        /// <param name="path">текущий путь</param>
        /// <param name="previousPath">уровень выше</param>
        /// <param name="isFinishedSurfing">закончен ли проход</param>
        /// <exception cref="Exception"></exception>
        public static void SurfDirectoriesAndFiles(ref string path, ref string previousPath, ref bool isFinishedSurfing) {
            try {
                if (path == null) {
                    isFinishedSurfing = true;
                    throw new Exception("Опция не доступна. Сначала необходимо выбрать диск.");
                }

                DirectoryInfo directories = new DirectoryInfo(path);

                string[] listOfDirectories = new string[directories.GetDirectories().Length + 1];
                listOfDirectories[0] = directories.FullName + "... <- Текущая директория";
                int i = 1;
                foreach (var directory in directories.GetDirectories()) {
                    listOfDirectories[i] = directory.Name;
                    i++;
                }

                int quantityOfDirectories = directories.GetDirectories().Length + 1;
                string[] listOfFiles = new string[directories.GetFiles().Length];
                i = 0;
                foreach (var directory in directories.GetFiles()) {
                    listOfFiles[i] = directory.Name;
                    i++;
                }

                string[] activeList = listOfDirectories.Concat(listOfFiles).ToArray();
                ShowListOfDirectoriesAndFiles(ref path, ref previousPath, activeList, ref isFinishedSurfing,
                    quantityOfDirectories);
            }
            catch (Exception e) {
                isFinishedSurfing = true;
                path = previousPath;
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Вывод  и директорий, и файлов сразу.
        /// </summary>
        /// <param name="path">текущий путь</param>
        /// <param name="previousPath">верхний уровень</param>
        /// <param name="listOfDirectoriesAndFiles"></param>
        /// <param name="isFinishedSurfing">закончен ли проход по папкам</param>
        /// <param name="quantityOfDirectories"></param>
        private static void ShowListOfDirectoriesAndFiles(ref string path, ref string previousPath,
            string[] listOfDirectoriesAndFiles, ref bool isFinishedSurfing, int quantityOfDirectories) {
            try {
                ShowInfoMessage("Выведен список поддиректорий. Если вы желаете работать в текущей директории, то " +
                                "просто нажмите Escape.");

                int indexOfSelectedItem = Selector.RunSelector(listOfDirectoriesAndFiles, false, false, true);
                if (indexOfSelectedItem == -1) {
                    isFinishedSurfing = true;
                    return;
                }

                if (indexOfSelectedItem == 0) {
                    GoUpper(ref path, ref previousPath, ref isFinishedSurfing);
                    if (isFinishedSurfing)
                    {
                        return;
                    }
                }

                for (int i = 1; i < listOfDirectoriesAndFiles.Length; i++) {
                    if (i == indexOfSelectedItem) {
                        if (i < quantityOfDirectories) {
                            previousPath = path;
                            path = path + Path.DirectorySeparatorChar + listOfDirectoriesAndFiles[i];
                            CurrentPath = path;
                            break;
                        }

                        do {
                            string filePath = path + Path.DirectorySeparatorChar + listOfDirectoriesAndFiles[i];
                            OperateWithFile(filePath, listOfDirectoriesAndFiles[i]);
                        } while (Console.ReadKey().Key != ConsoleKey.Escape);

                        Console.Clear();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Меню операций с файлом.
        /// </summary>
        /// <param name="filePath">путь до файла</param>
        /// <param name="fileName">имя файла</param>
        private static void OperateWithFile(string filePath, string fileName) {
            try {
                Console.Clear();
                ShowInfoMessage($"Опции для файла {fileName}:");

                int pointer = Selector.RunSelector(new[] {
                    "  Вывод файла в консоль в выбранной кодировке",
                    "  Копирование файла",
                    "  Перемещение файла",
                    "  Удаление файла",
                    "  Конкатенация файлов в кодировке UTF-8.",
                }, false, false, true);

                SwitchOptionForFileManager(pointer, filePath, fileName);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Выбор опции из меню функций для файла.
        /// </summary>
        /// <param name="pointer">выбранное положение</param>
        /// <param name="filePath">путь до файла</param>
        /// <param name="fileName">имя файла</param>
        private static void SwitchOptionForFileManager(int pointer, string filePath, string fileName) {
            switch (pointer) {
                case 0:
                    ShowContentOfFile(filePath);
                    break;
                case 1:
                    CopyFile(filePath, fileName);
                    break;
                case 2:
                    MoveFile(filePath, fileName);
                    break;
                case 3:
                    DeleteFile(filePath, fileName);
                    break;
                case 4:
                    ConcatenateFiles(filePath, fileName);
                    break;
            }
        }

        /// <summary>
        /// Подъем на уровень выше.
        /// </summary>
        /// <param name="path">текущий путь</param>
        /// <param name="previousPath">уровень выше</param>
        /// <param name="isFinishedSurfing">закончен ли проход</param>
        /// <exception cref="Exception"></exception>
        private static void GoUpper(ref string path, ref string previousPath, ref bool isFinishedSurfing) {
            try {
                DirectoryInfo directories = new DirectoryInfo(path);

                if (previousPath == null || directories.Parent == null) {
                    isFinishedSurfing = true;
                    throw new Exception("Вы уже находитесь на уровне диска. ");
                }

                path = directories.Parent.FullName;
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }


        /// <summary>
        /// Выбор файла из директории.
        /// </summary>
        /// <param name="directoryPath">Путь до директории</param>
        /// <param name="filePath">Путь до файла</param>
        /// <param name="infoMessage">Сообщение для вывода</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string SelectFile(string directoryPath, ref string filePath, string infoMessage) {
            string nameOfFile = "";
            try {
                IsPathNull(directoryPath);

                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                if (directory.GetFiles().Length == 0) {
                    throw new Exception("Файлы в текущей директории отсутствуют.");
                }

                string[] listOfFiles = new string[directory.GetFiles().Length];
                int i = 0;
                foreach (var file in directory.GetFiles()) {
                    listOfFiles[i] = file.Name;
                    i++;
                }

                ConsoleStyle.SetStyleForInfo();
                Console.WriteLine(infoMessage);
                int indexOfSelectedFile = Selector.RunSelector(listOfFiles, false, false, true);

                i = 0;
                foreach (var file in directory.GetFiles()) {
                    if (i == indexOfSelectedFile) {
                        filePath = file.FullName;
                        nameOfFile = file.Name;
                        break;
                    }

                    i += 1;
                }
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }

            return nameOfFile;
        }

        /// <summary>
        /// Вывод содержимого файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        private static void ShowContentOfFile(string filePath) {
            try {
                IsPathNull(filePath);

                Encoding encodingFormat = SetEncodingFormat();

                ShowInfoMessage($"Содержание файла по адресу {filePath}");
                ShowLine();
                ConsoleStyle.SetDefault();

                string[] contentOfFile = File.ReadAllLines(filePath, encodingFormat);
                foreach (var str in contentOfFile) {
                    Console.WriteLine(str);
                }

                ShowInfoMessage("Содержимое файла выведено успешно.");
                ShowLine();
                ShowContinueMessage("Для возврата в меню нажмите любую клавишу.");
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Органайзер для работы с путями к доп файлам.
        /// </summary>
        /// <param name="infoMessage">Вывод сообщение</param>
        /// <param name="canEnterPath">Можно ли ввести путь с клавиатуры</param>
        private static void ManagePath2(string infoMessage, bool canEnterPath) {
            try {
                ClearConsoleAndShowMessage(infoMessage);
                int pointer;
                if (!canEnterPath) {
                    pointer = Selector.RunSelector(new[] {
                        "  Просмотр списка дисков компьютера и выбор диска",
                        "  Выбор папки",
                    }, false, true, true);
                }
                else {
                    pointer = Selector.RunSelector(new[] {
                        "  Просмотр списка дисков компьютера и выбор диска",
                        "  Выбор папки",
                        "  Ввести адрес вручную"
                    }, false, true, true);
                }
                
                SwitchForManageFile2(pointer);
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Выбрать опуию для органайзера.
        /// </summary>
        /// <param name="pointer">выбранная опция</param>
        /// <exception cref="Exception"></exception>
        private static void SwitchForManageFile2(int pointer) {
            string path;
            string previousPath;
            bool isFinishedSurfing = false;
            switch (pointer) {
                case 0:
                    path = AdditionalPath;
                    previousPath = AdditionalPreviousLevel;
                    SetDrive(ref path, ref previousPath);
                    AdditionalPath = path;
                    AdditionalPreviousLevel = previousPath;
                    break;
                case 1:
                    path = AdditionalPath;
                    previousPath = AdditionalPreviousLevel;
                    do {
                        SurfDirectories(ref path, ref previousPath, ref isFinishedSurfing);
                    } while (!isFinishedSurfing);

                    AdditionalPath = path;
                    AdditionalPreviousLevel = previousPath;
                    break;
                case 2:
                    ShowInfoMessage("Введите адрес директории:");
                    Console.CursorVisible = true;
                    path = Console.ReadLine();
                    if (!Directory.Exists(path)) {
                        throw new Exception("Такой директории не существует.");
                    }
                    ShowContinueMessage("Нажмите любую клавишу для продолжения");
                    AdditionalPath = path;
                    break;
            }
        }

        /// <summary>
        /// Копирование файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="fileName">имя файла</param>
        /// <exception cref="Exception"></exception>
        private static void CopyFile(string filePath, string fileName) {
            try {
                IsPathNull(filePath);
                do {
                    ManagePath2("Когда выберете директорию, в которую хотите скопировать файл, нажмите F1", false);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                if (AdditionalPath == null) {
                    throw new Exception("Вы не выбрали расположение копии файла. Операция отменена.\n" +
                                        "Вы будете возвращены в главное меню. Для продолжения нажмите любую клавишу.");
                }

                ClearConsoleAndShowMessage("Вы покинули меню выбора расположения файла. Нажмите любую клавишу для" +
                                           " продолжения");
                File.Copy(filePath, $"{AdditionalPath}{Path.DirectorySeparatorChar}{fileName}", true);
                Console.Clear();
                ShowInfoMessage("Файл успешно скопирован. (Если это не изначальная директория, то  " +
                                "происходит перезапись)");
            }
            catch (IOException) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine("Файл уже присутствует в этой директории");
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Перемещение файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="fileName">имя файла</param>
        /// <exception cref="Exception"></exception>
        private static void MoveFile(string filePath, string fileName) {
            try {
                IsPathNull(CurrentPath);
                IsPathNull(filePath);

                do {
                    ManagePath2("Когда выберете директорию, в которую хотите переместить файл, нажмите F1", false);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                if (AdditionalPath == null) {
                    throw new Exception("Вы не выбрали новое расположение файла. Операция отменена.\n" +
                                        "Вы будете возвращены в главное меню. Для продолжения нажмите любую клавишу.");
                }

                ClearConsoleAndShowMessage(
                    "Вы покинули меню выбора расположения файла. Нажмите любую клавишу для продолжения");
                File.Move(filePath, $"{AdditionalPath}{Path.DirectorySeparatorChar}{fileName}");
                ShowInfoMessage("Файл успешно перемещен.");
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Удаление файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="fileName">имя файла</param>
        private static void DeleteFile(string filePath, string fileName) {
            try {
                IsPathNull(CurrentPath);
                IsPathNull(filePath);

                File.Delete(filePath);
                ShowInfoMessage($"Файл {fileName} удален успешно.");
                ShowContinueMessage("Для продолжения нажмите любую клавишу.");
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Создание файла.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void CreateFile() {
            try {
                Encoding encodingFormat = SetEncodingFormat();
                ShowInfoMessage("Введите имя, создаваемого файла: ");
                Console.CursorVisible = true;

                string nameOfFile = Console.ReadLine();
                do {
                    ManagePath2("Когда выберете директорию, в которой хотите создать файл, нажмите F1", false);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                if (AdditionalPath == null) {
                    throw new Exception("Вы не выбрали расположение копии файла. Операция отменена.\n" +
                                        "Вы будете возвращены в главное меню. Для продолжения нажмите любую клавишу.");
                }

                string filePath = $"{AdditionalPath}{Path.DirectorySeparatorChar}{nameOfFile}.txt";
                var myFile = File.Create(filePath);
                myFile.Close();
                Console.Clear();

                ShowInfoMessage("Введите строку для файла. Затем нажмите Enter.");
                Console.CursorVisible = true;
                ConsoleStyle.SetDefault();

                string contentOfFile = Console.ReadLine();
                File.AppendAllText(filePath, $"{contentOfFile}{Environment.NewLine}", encodingFormat);

                ClearConsoleAndShowMessage("Запись успешно завершена.");
                ShowContinueMessage("Для продолжения нажмите любую клавишу.");
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Выбор кодировки для файла.
        /// </summary>
        /// <returns></returns>
        private static Encoding SetEncodingFormat() {
            Encoding encodingFormat = Encoding.UTF8;
            try {
                ShowInfoMessage("Выберете нужную кодировку для файла:");
                int indexOfEncoding =
                    Selector.RunSelector(new[] {"UTF8", "ASCII", "Latin1", "Unicode"}, false, false, false);

                if (indexOfEncoding == 1) {
                    encodingFormat = Encoding.ASCII;
                }

                if (indexOfEncoding == 2) {
                    encodingFormat = Encoding.Latin1;
                }

                if (indexOfEncoding == 3) {
                    encodingFormat = Encoding.Unicode;
                }

                return encodingFormat;
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }

            return encodingFormat;
        }

        /// <summary>
        ///  Конкатенация файла.
        /// </summary>
        /// <param name="filePath">путь к файлу 1</param>
        /// <param name="fileName">имя файла 1</param>
        /// <exception cref="Exception"></exception>
        private static void ConcatenateFiles(string filePath, string fileName) {
            try {
                IsPathNull(filePath);

                do {
                    ManagePath2("Когда выберете директорию, из которой хотите взять следующий файл " +
                                "для конкатенации, нажмите F1", false);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                Console.Clear();
                string file2Path = "";
                string nameOfSelectedFile2 = SelectFile(AdditionalPath, ref file2Path,
                    "Выберите второй файл для конкатенации и нажмите Enter.");
                if (file2Path.Length == 0) {
                    return;
                }
                ShowInfoMessage("Результат конкатенации:");
                string[] contentOfFile1 = File.ReadAllLines(filePath);
                foreach (var str in contentOfFile1) {
                    Console.Write($"{str}{Environment.NewLine}");
                }
                string[] contentOfFile2 = File.ReadAllLines(file2Path);
                foreach (var str in contentOfFile2) {
                    Console.Write($"{str}{Environment.NewLine}");
                }

                ShowInfoMessage("Конкатенация завершена успешно");
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Получение списка файлов по маске.
        /// </summary>
        public static void GetFilesByMask() {
            try {
                do {
                    ManagePath2("Когда выберете директорию, из которой хотите взять второй файл " +
                                "для конкатенации, нажмите F1", false);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                IsPathNull(AdditionalPath);
                string mask = SetMask();

                string[] listOfFilesWithMask = GetListOfFilesWithMask(AdditionalPath, mask);
                ShowListOfFilesWithMask(listOfFilesWithMask, mask);
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Выбор маски.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string SetMask() {
            ClearConsoleAndShowMessage("Введите маску, по которой будет выполнен поиск \n(Пример, для file.txt нужно " +
                                       "ввести .txt,и ничего больше): ");
            ConsoleStyle.SetDefault();
            Console.CursorVisible = true;
            string mask = Console.ReadLine();
            if (mask == null || mask.Trim().Length == 0) {
                throw new Exception("Введена пустая маска. Нажмите любую клавишу для возврата в\n " +
                                    "главное меню.");
            }

            return mask;
        }

        /// <summary>
        /// Получение списка файлов по маске.
        /// </summary>
        /// <param name="path">путь к директории</param>
        /// <param name="mask">маска</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string[] GetListOfFilesWithMask(string path, string mask) {
            DirectoryInfo directory = new DirectoryInfo(path);
            string[] listOfFile = new string[directory.GetFiles("*" + mask).Length];
            try {
                if (directory.GetFiles("*" + mask).Length == 0) {
                    throw new Exception("Файлы с такой маской отсутствуют в текущей директории.");
                }


                int i = 0;
                foreach (var file in directory.GetFiles("*" + mask)) {
                    listOfFile[i] = file.Name;
                    i++;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }

            return listOfFile;
        }

        /// <summary>
        /// Вывод списка файлов с маской.
        /// </summary>
        /// <param name="listOfFiles">список файлов по маске</param>
        /// <param name="mask">маска</param>
        private static void ShowListOfFilesWithMask(string[] listOfFiles, string mask) {
            if (listOfFiles.Length != 0) {
                ShowChosenOption($"Список файлов с маской: ", mask);
                ShowLine();
                foreach (var file in listOfFiles) {
                    Console.WriteLine(file);
                }
            }
            
            
        }

        /// <summary>
        /// Проверка на нуль пути.
        /// </summary>
        /// <param name="path">путь</param>
        /// <exception cref="Exception"></exception>
        private static void IsPathNull(string path) {
            if (path == null) {
                throw new Exception("Опция не доступна. Сначала необходимо выбрать диск. \n" +
                                    "Для возврата нажмите любую клавишу.");
            }
        }

        /// <summary>
        /// Перенос файлов с маской в другую папку. Есть превышение на 8 строк, но в данном случае в коде содержится
        /// связанная логика и декомпозировать далее не целесообразно.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void ReplaceFilesWithMask() {
            try {
                do {
                    ManagePath2("Когда выберете директорию, из которой хотите переместить все файлы с " +
                                "маской, нажмите F1", true);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                if (AdditionalPath == null) {
                    throw new Exception("Вы не выбрали новое расположение файла. Операция отменена.\n" +
                                        "Вы будете возвращены в главное меню. Для продолжения нажмите любую клавишу.");
                }

                string mask = SetMask();
                string[] listOfAllFilesWithFullName = { };
                string[] listOfAllFilesWithName = { };
                FindAllFilesWithMask(AdditionalPath, ref listOfAllFilesWithName, ref listOfAllFilesWithFullName, mask);

                do {
                    ManagePath2("Когда выберете директорию, в которую хотите переместить файлы, нажмите F1", false);
                } while (Console.ReadKey().Key != ConsoleKey.F1);

                if (AdditionalPath == null) {
                    throw new Exception("Вы не выбрали расположение копии файла. Операция отменена.\n" +
                                        "Вы будете возвращены в главное меню. Для продолжения нажмите любую клавишу.");
                }

                ClearConsoleAndShowMessage("Вы покинули меню выбора расположения файла. Нажмите любую клавишу для продолжения");
                int selectedOption =
                    Selector.RunSelector(
                        new[] {"Перезаписать файлы с одинаковым именем", "Не перезаписывать файлы с одинаковым именем"},
                        false, true, false);
                
                bool isAvailableToOverwrite;
                if (selectedOption == 0) {
                    isAvailableToOverwrite = true;
                }
                else {
                    isAvailableToOverwrite = false;
                }
                CopyFilesByMask(listOfAllFilesWithName, listOfAllFilesWithFullName,  isAvailableToOverwrite);
                Console.WriteLine("Файлы успешно скопированы.");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        private static void CopyFilesByMask(string[] listOfAllFilesWithName, string[] listOfAllFilesWithFullName, bool isAvailableToOverwrite) {
            try
            {
                int i = 0;
                foreach (var item in listOfAllFilesWithFullName) { 
                    if (File.Exists(AdditionalPath + Path.DirectorySeparatorChar + listOfAllFilesWithName[i]) &&
                                                                       isAvailableToOverwrite) {
                    File.Copy(item, AdditionalPath + Path.DirectorySeparatorChar + listOfAllFilesWithName[i],
                        true); 
                    }
                    
                    if (!File.Exists(AdditionalPath + Path.DirectorySeparatorChar + listOfAllFilesWithName[i])) { 
                        File.Copy(item, AdditionalPath + Path.DirectorySeparatorChar + listOfAllFilesWithName[i], false); 
                    }
                    
                    i++; 
                }
            }
            catch (IOException) {
               Console.WriteLine();
            }
            
        }

        /// <summary>
        /// Нахождение всех файлов по маске.
        /// </summary>
        /// <param name="path">путь к папке</param>
        /// <param name="listOfAllFilesWithName">список файлов с полными путями</param>
        /// <param name="listOfAllFilesWithFullName">список файлов только с именами</param>
        /// <param name="mask">маска</param>
        public static void FindAllFilesWithMask(string path, ref string[] listOfAllFilesWithName,
            ref string[] listOfAllFilesWithFullName, string mask) {
            try {
                DirectoryInfo directory = new DirectoryInfo(path);

                int i = 0;
                string[] listOfFilesWithName =
                    new string[directory.GetFiles("*" + mask, SearchOption.AllDirectories).Length];
                string[] listOfFilesWithFullName =
                    new string[directory.GetFiles("*" + mask, SearchOption.AllDirectories).Length];
                foreach (var file in directory.GetFiles("*" + mask, SearchOption.AllDirectories)) {
                    listOfFilesWithFullName[i] = file.FullName;
                    listOfFilesWithName[i] = file.Name;
                    i++;
                }

                listOfAllFilesWithName = listOfAllFilesWithName.Concat(listOfFilesWithName).ToArray();
                listOfAllFilesWithFullName = listOfAllFilesWithFullName.Concat(listOfFilesWithFullName).ToArray();
            }
            catch (Exception e) {
                ConsoleStyle.SetStyleForImportant();
                Console.WriteLine(e.Message);
                ShowContinueMessage("Нажмите любую клавишу для продолжения.");
            }
        }

        /// <summary>
        /// Существует ли путь.
        /// </summary>
        /// <param name="path">путь</param>
        /// <exception cref="Exception"></exception>
        private static void IsPathExists(string path) {
            if (!Directory.Exists(path)) {
                throw new Exception("Некорректный путь. Вы вернетесь в меню.");
            }
        }
    }
}