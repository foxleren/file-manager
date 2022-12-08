using static file_manager.UserInterface;

namespace file_manager {
    /// <summary>
    /// Класс реализовывает часть основной логики по запуску программы.
    /// </summary>
    public static class App {
        /// <summary>
        /// Запуск основной логики.
        /// </summary>
        public static void LaunchApp() {
            ShowEntranceMessage();
            do {
                RunAlgorithm();
            } while (Console.ReadKey(true).Key != ConsoleKey.F10);

            ClearConsoleAndShowMessage("Вы нажали Выход. \nСпасибо за ваше внимание. Программа будет закрыта.");

            Thread.Sleep(1500);
        }

        /// <summary>
        /// Запуск алгоритма работы с файлами, вывод меню.
        /// </summary>
        private static void RunAlgorithm() {
            ClearConsoleAndShowMessage("Меню программы:");

            int pointer = Selector.RunSelector(new[] {
                "  Выбор диска",
                "  Просмотр дирректорий и файлов",
                "  Создать файл в кодировке",
                "  Нахождение файлов по маске в текущей директории",
                "  Переместить все файлы директории и поддиректории по маске",
                "  INFO"
            }, true, false, true);

            SwitchForRunAlgorithm(pointer);
        }

        /// <summary>
        /// Выбор опции из главного меню.
        /// </summary>
        /// <param name="pointer"></param>
        private static void SwitchForRunAlgorithm(int pointer) {
            switch (pointer) {
                case 0:
                    RunSetDrive();
                    break;
                case 1:
                    RunSurfDirectoriesAndFiles();
                    break;
                case 2:
                    FileManager.CreateFile();
                    break;
                case 3:
                    FileManager.GetFilesByMask();
                    break;
                case 4:
                    FileManager.ReplaceFilesWithMask();
                    break;
                case 5:
                    ShowInfo();
                    break;
            }
        }

        /// <summary>
        /// Запуск выбора диска.
        /// </summary>
        private static void RunSetDrive() {
            string path = FileManager.CurrentPath;
            string previousPath = FileManager.PreviosLevel;
            FileManager.SetDrive(ref path, ref previousPath);
            FileManager.CurrentPath = path;
            FileManager.PreviosLevel = previousPath;
        }

        /// <summary>
        /// Запуск прохода по папкам с файлами.
        /// </summary>
        private static void RunSurfDirectoriesAndFiles() {
            string path = FileManager.CurrentPath;
            string previousPath = FileManager.PreviosLevel;
            bool isFinishedSurfing = false;
            do {
                FileManager.SurfDirectoriesAndFiles(ref path, ref previousPath, ref isFinishedSurfing);
            } while (!isFinishedSurfing);

            FileManager.CurrentPath = path;
            FileManager.PreviosLevel = previousPath;
        }
    }
}