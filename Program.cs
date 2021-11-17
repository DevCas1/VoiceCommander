﻿using System;
using System.Speech.Recognition;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Input = VoiceCommand.Keyboard.Input;

namespace VoiceCommand
{
    internal class Program
    {
        private static ManualResetEvent Completed = null;
        private static string ShutdownWord = "Close Voice Command";
        private static Command[] LoadedCommands = null;

        private static void Main(string[] args) // Add (string[] args) to use command line arguments when starting the program
        {
            LogToConsole("Initializing...");

            Completed = new ManualResetEvent(false);
            //Robot = new Robot();

            SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine();

            recognitionEngine.LoadGrammarCompleted += (object o, LoadGrammarCompletedEventArgs a) => { Console.WriteLine("Grammars loaded."); };
            recognitionEngine.SpeechRecognized += OnSpeechRecognized;
            recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;

            AddGrammars(recognitionEngine, LoadedCommands = LoadCommands());

            recognitionEngine.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous
            LogToConsole("Ready!");

            Completed.WaitOne(); // wait until speech recognition is completed

            recognitionEngine.Dispose(); // dispose the speech recognition engine
        }

        private static Command[] LoadCommands() //TODO: Replace with JSON parser
        {
            Command[] commands = new Command[]
            {
                new Command("test", new string[]{""}),
                new Command("power the engines", new string[]{""}),
                new Command("power the weapons", new string[]{""}),
                new Command("power the shields", new string[]{""}),
                new Command("maximize engines", new string[]{""}),
                new Command("maximize weapons", new string[]{""}),
                new Command("maximize shields", new string[]{""})
            };

            return commands;
        }

        private static void AddGrammars(SpeechRecognitionEngine recognitionEngine, Command[] commands)
        {
            //new System.Globalization.CultureInfo("en-GB"); gr = new Grammar(grammarBuilder); // For SAPI errors regarding culture info
            LogToConsole("Loading commands...");
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(ShutdownWord)));

            foreach (var command in commands)
                recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(command.CommandPhrase)));

            LogToConsole($"{commands.Length} command{(commands.Length > 1 ? "s" : "")} loaded.");
        }

        private static void LogToConsole(string message) => Console.WriteLine(DateTime.Now + ": " + message);

        private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs args)
        {
            if (args.Result.Text == ShutdownWord)
            {
                ShutDown();
                return;
            }

            Command recognizedCommand = new Command();
            bool commandFound = false;

            for (int index = 0; index < LoadedCommands.Length; index++)
            {
                if (args.Result.Text == LoadedCommands[index].CommandPhrase)
                {
                    recognizedCommand = LoadedCommands[index];
                    commandFound = true;
                    break;
                }
            }

            if (commandFound) // Only use recognizedCommand when a command has actually been recognized.
            {
                LogToConsole($"Command recognized \"{recognizedCommand.CommandPhrase}\"");
                var inputs = GetTestInput();
                Keyboard.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            }
        }

        private static Input[] GetTestInput() // THIS WORKS, GENERATES A MEASLY 'w' EVERY TIME!
        {
            Input[] inputs = new Input[]
            {
                new Input
                {
                    type = (int)Keyboard.InputType.Keyboard,
                    u = new Keyboard.InputUnion
                    {
                        ki = new Keyboard.KeyboardInput
                        {
                            wVk = 0,
                            wScan = 0x11, // W-button
                            dwFlags = (uint)(Keyboard.KeyEventF.KeyDown | Keyboard.KeyEventF.Scancode),
                            dwExtraInfo = Keyboard.GetMessageExtraInfo()
                        }
                    }
                }
            };

            return inputs;
        }

        private static void ShutDown()
        {
            LogToConsole("Shutdown word recognized, shutting down...");
            Thread.Sleep(1000);
            Completed.Set();
        }

        private static void OnSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs args) => LogToConsole("Still listening...");
    }
}