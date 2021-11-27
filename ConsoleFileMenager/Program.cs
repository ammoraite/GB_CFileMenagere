using ConsoleFileMenager;
using System.Text.Json;
using System.Text.Json.Serialization;

UI.createUI();
var CurrentConfig = Config.InitialiseCurentConfig();

bool Exit = true;

while (Exit)
{
    for (int i = 0; i < Command.NumberComandHistori; i++)
    {
        UI.SetCursorToWriteCommand();
        Command.ComandHistori[i] = Console.ReadLine();

        if (Command.ComandHistori[i] != null && Command.ComandHistori[i] != ""&& Command.ComandHistori[i] != "exit")
        { 
            try
            {
                switch (Command.GetCommandNameInLine(i))
                {
                    case Command.CommandName.cd: Command.cd(i,CurrentConfig);break;
                    case Command.CommandName.cpf : Command.cpf(i); break;
                    case Command.CommandName.rmf: Command.rmf(i); break;
                    case Command.CommandName.mkdir: Command.mkdir(i); break;
                    case Command.CommandName.cpdir: Command.cpdir(i); break;
                    case Command.CommandName.rmdir : Command.rmdir(i); break;
                    case Command.CommandName.cpg: Command.cpg(i,CurrentConfig); break;

                    default:UI.ShowSystemInfo($"Ошибка ввода команды"); break;
                }               
            }
            catch (Exception ex)
            {
                string CurentError = $"Ошибка ввода пути или команды {ex}";
                UI.ShowSystemInfo(CurentError);               
            }
        }
        else if (Command.ComandHistori[i] == "exit")
        {
            Exit = false;
            
            File.WriteAllText("settings.json", JsonSerializer.Serialize(CurrentConfig));
            
        }
    }
}




