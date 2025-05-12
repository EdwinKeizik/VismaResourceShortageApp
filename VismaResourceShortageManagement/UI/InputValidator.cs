namespace VismaResourceShortageManagement.UI
{

    public static class InputValidator
    {
        public static int GetValidMenuOption(string errorPrompt, int minValue, int maxValue)
        {
            int result;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out result) && result >= minValue && result <= maxValue)
                {
                    return result;
                }
                Console.Write(errorPrompt);
            }
        }

        public static string GetValidString(string prompt, string errorPrompt)
        {
            string? input;
            Console.Write(prompt);
            while (true)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.Write(errorPrompt);
            }
        }

        public static DateTime GetValidDate(string prompt, string errorPrompt)
        {
            DateTime result;
            Console.Write(prompt);
            while (true)
            {
                if (DateTime.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                Console.Write(errorPrompt);
            }
        }

        public static int GetValidPriority(string prompt, string errorPrompt, int minValue, int maxValue)
        {
            int result;
            Console.Write(prompt);
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out result) && result >= minValue && result <= maxValue)
                {
                    return result;
                }
                Console.Write(errorPrompt);
            }
        }

        public static string GetOptionByNumberFromArray(string prompt, string errorPromptBase, string[] validOptions)
            {
                string? userInput;
                int choiceNumber;

                while (true)
                {
                    Console.Write(prompt);
                    userInput = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.WriteLine("Input cannot be empty.");
                        continue;
                    }

                    if (int.TryParse(userInput, out choiceNumber))
                    {
                        if (choiceNumber >= 1 && choiceNumber <= validOptions.Length)
                        {
                            return validOptions[choiceNumber - 1];
                        }
                    }


                    Console.WriteLine(errorPromptBase + $" EnterNumber between 1 and {validOptions.Length}.");
                }
            }

    }
}