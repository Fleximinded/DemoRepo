using System;
using System.Collections.Generic;

public class CommandParser
{
    public string Command { get; private set; }
    public Dictionary<string, string> Options { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CommandParser(string[] args)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        Options = new Dictionary<string, string>();
        Parse(args);
    }

    private void Parse(string[] args)
    {
        if (args.Length == 0)
        {
            Command = "";
            return;
        }

        Command = args[0];
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i].StartsWith("-"))
            {
                string option = args[i].Substring(1);
                string value = "";
                if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                {
                    value = args[i + 1];
                    i++;
                }
                Options[option] = value;
            }
        }
    }
}

public class CustomConsoleApp
{
    private List<CustomFunctionality> customFunctionalities;

    public CustomConsoleApp()
    {
        customFunctionalities = new List<CustomFunctionality>();
    }

    public void Add(CustomFunctionality functionality)
    {
        customFunctionalities.Add(functionality);
    }

    public void RunCommand(string[] args)
    {
        CommandParser parser = new CommandParser(args);
        string command = parser.Command;
        Dictionary<string, string> options = parser.Options;

        switch (command)
        {
            case "exit":
                Environment.Exit(0);
                break;
            case "clear":
                Console.Clear();
                break;
            default:
                // Execute custom functionalities
                foreach (CustomFunctionality functionality in customFunctionalities)
                {
                    functionality.ExecuteCommand(command, options);
                }
                break;
        }
    }
}

public class CustomFunctionality
{
    public virtual void ExecuteCommand(string command, Dictionary<string, string> options)
    {
        // Implement custom functionality here
        Console.WriteLine($"Executing custom command: {command}");
        foreach (var option in options)
        {
            Console.WriteLine($"Option: {option.Key}, Value: {option.Value}");
        }
    }
}

// Example of a custom functionality
public class MyCustomFunctionality : CustomFunctionality
{
    public override void ExecuteCommand(string command, Dictionary<string, string> options)
    {
        base.ExecuteCommand(command, options);
        // Additional custom functionality
    }
}

class Program
{
    static void Main(string[] args)
    {
        CustomConsoleApp app = new CustomConsoleApp();
        app.Add(new MyCustomFunctionality());

        while (true)
        {
            Console.Write("Enter command: ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }
            string[] commandArgs = input.Split(' ');
            app.RunCommand(commandArgs);
        }
    }
}
