using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleFileMenager
{
    public class Config
    {

        public int NumberCurrentPage { get; set; }
        public int NumberAllPages { get; set; }
        public string[,] CurrentPagesPaths   { get; set; }
        




        public static Config InitialiseCurentConfig()
        {
            
            var CurrentConfig = new Config();

            if (File.Exists("settings.json"))
            {
                
                try
                {
                    CurrentConfig = JsonSerializer.Deserialize<Config>(File.ReadAllText("settings.json"));
                    
                    UI.ShowPagePaths(CurrentConfig);
                    
                   
                }
                catch (Exception ex)
                { 
                    string CurentError = $"ошибка загрузки кофигурации {ex}";
                    UI.ShowSystemInfo(CurentError);
                }

            }
            else
            {
               
                try
                {

                    CurrentConfig.CurrentPagesPaths = GetPagesPats(Directory.GetFileSystemEntries(Directory.GetCurrentDirectory()), CurrentConfig);
                    CurrentConfig.NumberCurrentPage = 1;

                    UI.ShowPagePaths(CurrentConfig);
                    UI.ShowSystemInfo($"Загружена дирректория по умолчанию ");


                }
                catch (Exception ex)
                {
                    string CurentError = $"ошибка Инициализации кофигурации {ex}";
                    UI.ShowSystemInfo(CurentError);
                }
            }
            return CurrentConfig;
        }

        public static string[,] GetPagesPats(string[] PathsForGetPages,Config CurrentConfig)
        {
            int MaxPathInPage = (Console.WindowHeight-10)/2;
            int MaxPages = 2000;
            string[,] PagesPath = new string[MaxPages, MaxPathInPage];
            CurrentConfig.NumberAllPages = 0;
            
            int i=0;
            int j=0;
            while(i < PathsForGetPages.Length&& j < MaxPathInPage)
            {
                if (PathsForGetPages[i].Length< MaxPathInPage* (Console.WindowWidth - 2))
                {
                    if (PathsForGetPages[i]!=null)
                    {
                        PagesPath[CurrentConfig.NumberAllPages, j] = PathsForGetPages[i];
                    }
                    else
                    {
                        PagesPath[CurrentConfig.NumberAllPages, j] = " ";
                    }

                    j += 1 + (PathsForGetPages[i].Length / (Console.WindowWidth - 2));
                    i++;
                    if (j == MaxPathInPage)
                    {
                        CurrentConfig.NumberAllPages++;
                        j = 0;
                    }
                }
                else
                {
                    PagesPath[CurrentConfig.NumberAllPages, j] = "Слишком длинный путь к файлу или директории";
                }
                
            }
            return PagesPath;
        }
    }
}






