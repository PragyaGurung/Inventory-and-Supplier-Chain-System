using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.InputValidation
{
    public static class InputValidator
    {
        public static string ReadString(string prompt, bool required = true)
        {
            string input;

            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();

                if (!required && string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("This field is required!");
                }
            } while (required && string.IsNullOrEmpty(input));

            return input;
        }

        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue) 
        {
            int value;
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out value))
                {
                    if (value >= min && value <= max)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a value between {min} and {max}.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }             
            }
        }

        public static bool ReadBool(string prompt) 
        {
            while (true) 
            {
                Console.Write($"{prompt} (Y/N): ");

                string input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Y") 
                {
                    return true;
                }
                else if (input == "N") 
                {
                    return false;
                }
                else 
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' for Yes or 'N' for No.");
                }
            }

        }

    }
}
