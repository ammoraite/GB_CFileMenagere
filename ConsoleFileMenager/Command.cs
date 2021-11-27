using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileMenager
{

    public class Command
    {
        public const int NumberComandHistori = 15;
        public static string[] ComandHistori = new string[NumberComandHistori];
        public enum CommandName
        {
            cpf,
            rmf,
            mkdir,
            rmdir,
            exit,
            cd,
            ComandNotExist,
            cpdir,
            cpg
        }

        public static CommandName GetCommandNameInLine(int NumberHistoriCommand)
        {
            var CurentCommandName=CommandName.ComandNotExist;
            if (Command.ComandHistori[NumberHistoriCommand].Contains(Command.CommandName.cd.ToString()))
            {                
                CurentCommandName = CommandName.cd;
            }
            else if (Command.ComandHistori[NumberHistoriCommand].Contains($"{Command.CommandName.cpf.ToString()} "))
            {                
                CurentCommandName = CommandName.cpf;
            }
            else if (Command.ComandHistori[NumberHistoriCommand].Contains(Command.CommandName.rmf.ToString()))
            {                
                CurentCommandName = CommandName.rmf;
            }
            else if (Command.ComandHistori[NumberHistoriCommand].Contains(Command.CommandName.mkdir.ToString()))
            {                
                CurentCommandName = CommandName.mkdir;
            }
            else if (Command.ComandHistori[NumberHistoriCommand].Contains(Command.CommandName.rmdir.ToString()))
            {                
                CurentCommandName = CommandName.rmdir;
            }
            else if (Command.ComandHistori[NumberHistoriCommand].Contains(Command.CommandName.cpdir.ToString()))
            {         
                CurentCommandName = CommandName.cpdir;
            }
            else if (Command.ComandHistori[NumberHistoriCommand].Contains(Command.CommandName.cpg.ToString()))
            {
                CurentCommandName = CommandName.cpg;
            }
            return CurentCommandName;
        }
        private static string GetPathInLineCommand(int NumberHistoriCommand)
        {      
            return Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cd} ".ToCharArray());
        }


        //cd     — перейти в дирректорию;
        public static void cd(int NumberHistoriCommand,Config CurrentConfig)
        {
            if (Directory.Exists(Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cd} ".ToCharArray())))
            {

                CurrentConfig.CurrentPagesPaths=Config.GetPagesPats(Directory.GetFileSystemEntries(GetPathInLineCommand(NumberHistoriCommand)), CurrentConfig);
                UI.ShowPagePaths(CurrentConfig);
                UI.ShowSystemInfo($"Выполнен переход в директорию {Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cd} ".ToCharArray())}");
               
            }
            else
            {
                UI.ShowSystemInfo($"Директории {Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cd} ".ToCharArray())} не существует");
            }

        }

        //cpf     — скопировать файл;
        public static void cpf(int NumberHistoriCommand)
        {
            var paths = Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cpf} ".ToCharArray());
            string[] words = paths.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            FileInfo fileOut = new FileInfo(words[0]);
            FileInfo fileIn = new FileInfo(words[1]);
            if (fileOut.Exists)
            {
                if (!fileOut.Exists)
                {
                    fileOut.CopyTo(words[1]);
                    UI.ShowSystemInfo($"Файл {words[1]} скопирован в {words[0]}");
                }
                else
                {
                    UI.ShowSystemInfo($"В {words[1]} файл {words[0]} уже существует. Заменить(y/n)?");

                    if (Console.ReadLine() == "y")
                    {
                        File.Delete(words[1]);
                        if (fileOut.Exists)
                        {
                            fileOut.CopyTo(words[1]);
                            UI.ShowSystemInfo($"Файл {words[1]} скопирован в {words[0]}");
                        }
                    }
                    else if (Console.ReadLine() == "n")
                    {
                        UI.ClearSystemInfo();
                        UI.SetCursorToWriteCommand();
                    }

                }
            }
        }

        //rmf     — удалить файл;
        public static void rmf(int NumberHistoriCommand)
        {
            
            var path = Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.rmf} ".ToCharArray());
            FileInfo fileforgel = new FileInfo(path);

            if (fileforgel.Exists)
            {
                fileforgel.Delete();
                UI.ShowSystemInfo($"Файл {path} удален");
            }
            else
            {
                UI.ShowSystemInfo($"Файла {path} не существует");
            }
        }

        //mkdir   — создать директорию 
        public static void mkdir(int NumberHistoriCommand)
        {
            if (!Directory.Exists(Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.mkdir} ".ToCharArray())))
            {
                Directory.CreateDirectory(Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.mkdir} ".ToCharArray()));
                UI.ShowSystemInfo($"Создан каталог {Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.mkdir} ".ToCharArray())}");
            }
            else
            {
                UI.ShowSystemInfo($"Директория {Command.ComandHistori[NumberHistoriCommand].Trim($"{ Command.CommandName.mkdir} ".ToCharArray())} уже существует");
            }
        }


        //cpdir  — копировать директорию
        public static void cpdir(int NumberHistoriCommand)
        {
            var paths = Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cpdir} ".ToCharArray());

            string[] words = paths.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            DirectoryCopy(words[0], words[1],true);

            UI.ShowSystemInfo($"Директория {words[1]} скопирована в {words[0]}");

        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }






        //rmdir  — удалить директорию;
        public static void rmdir(int NumberHistoriCommand)
        {
            var paths = Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cpdir} ".ToCharArray());
            if (Directory.Exists(paths))
            {
               
                Directory.Delete(paths, true);
                UI.ShowSystemInfo($"Директория {paths} удалена");
            }
            else
            {
                UI.ShowSystemInfo($"Директория {paths} не существует");
            }
        }

        public static void cpg(int NumberHistoriCommand, Config CurrentConfig)
        {
            int page= int.Parse(Command.ComandHistori[NumberHistoriCommand].Trim($"{Command.CommandName.cpg} ".ToCharArray()));
            if (page >=0&&page<= CurrentConfig.NumberAllPages+1)
            {
                CurrentConfig.NumberCurrentPage = page;
                UI.ShowPagePaths(CurrentConfig);
            }
            else
            {
                UI.ShowSystemInfo($"Введенный номер ({page}) страницы меньше 0 или больше ({CurrentConfig.NumberAllPages + 1}) ");
            } 


        }

    }
}

       



