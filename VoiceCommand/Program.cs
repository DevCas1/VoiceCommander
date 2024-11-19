﻿using Microsoft.Extensions.Configuration;
using VoiceCommand.Input;

namespace VoiceCommand;

internal class Program
{
    const string CONFIG_FILE_NAME = "vcconfig.json";
    const int CONFIG_VERSION = 1;
    
    private static readonly ConfigurationManager _configurationManager = new(CONFIG_FILE_NAME);
    private static RecognitionHandler? _recognitionHandler;

    private static void Main() // Add (string[] args) to use command line arguments when starting the program
    {
        try
        {
            if (
                _configurationManager.VoiceCommandConfig.Version == 0 || 
                _configurationManager.VoiceCommandConfig.Version != CONFIG_VERSION
            )
            {
                string errorMessage = $"Config version does not match expected version! (Expected: {CONFIG_VERSION}, detected: {_configurationManager.VoiceCommandConfig.Version})" +
                    "\nEither restore original config file, or have a new one generated by renaming the incorrect one!";
                Log.LogToConsole(errorMessage, Log.LogType.Error);
                throw new ApplicationException(message: errorMessage);
            }
        }
        catch (InvalidOperationException)
        {
            Log.LogToConsole("Config version could not be parsed or found!", Log.LogType.Error);
            return;
        }

        _recognitionHandler = new RecognitionHandler(_configurationManager.VoiceCommandConfig);
        _recognitionHandler.Start();

        // Console.WriteLine();
        // Console.WriteLine("Press any key to exit...");
        // Console.Read();
    }
}
