using ApiProject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Specify the path to your input text file containing hex values
            string filePath = @"D:\ISSUES\EV1-T1209\CONFIG Controller.txt";

            Console.WriteLine("ENTER FILE NAME");
            string userFileName = Console.ReadLine();

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Create a list to hold Command objects
            List<Command> commands = new List<Command>();
            string filterStr = "Write";
            int lineNum = 0;
            foreach (string line in lines)
            {
                Console.WriteLine($"Line: {lineNum++}");
                if (line.Contains(filterStr))
                {
                    // Split each line into parts based on ":"
                    string[] parts = line.Split(':');

                    if (parts.Length >= 2)
                    {
                        int count = 0;
                        foreach (string part in parts)
                        {
                            string pattern = @"0x[\da-fA-F]+(?:,\s*0x[\da-fA-F]+)*";

                            // Use Regex to find matches in the input text
                            Match match = Regex.Match(part, pattern);

                            if (match.Success)
                            {
                                // Split hex values part by ","
                                string[] hexString = part.Split(',');
                                string[] hexStrings = match.Value.Split(',');

                                string[] filteredHexStrings = new string[hexStrings.Length];
                                for (int i = 0; i < hexStrings.Length; i++)
                                {
                                    if (string.IsNullOrWhiteSpace(hexStrings[i]))
                                    {
                                        filteredHexStrings[i] = "0x00";
                                    }
                                    else
                                    {
                                        filteredHexStrings[i] = hexStrings[i];
                                    }
                                }
                                // Convert hex strings to bytes
                                List<byte> hexValues = filteredHexStrings.Select(s => Convert.ToByte(s.Trim().Substring(2), 16)).ToList();

                                // Construct the Command object
                                Command command = new Command
                                {
                                    CommandName = parts[count - 1].ToString(),
                                    HexValues = hexValues,
                                    ApiDescription = parts.Length > count+1 ? parts[count+1].Trim() : string.Empty
                                };
                                // Add command to the list
                                commands.Add(command);
                                Console.WriteLine("Extracted hex values: " + hexValues);
                            }
                            count++;
                        }

                    }
                }
            }

            // Create a container object to hold all commands
            CommandsContainer container = new CommandsContainer
            {
                Commands = commands
            };

            // Serialize the container to JSON
            string json = JsonConvert.SerializeObject(container, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText($"D:\\ISSUES\\EV1-T1218\\{userFileName}.json", json);
            Console.WriteLine(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}

// Define the CommandsContainer class as shown earlier
public class CommandsContainer
{
    public List<Command> Commands { get; set; }

    public CommandsContainer()
    {
        Commands = new List<Command>();
    }
}
