using System;

namespace CastingConversions
{
    class Program
    {
        static void Main(string[] args)
        {
            // Implicit Casting (Widening Conversion)
            int myInt = 9;
            double myDouble = myInt; // Automatic conversion from int to double
            Console.WriteLine("Implicit Casting (int to double): " + myDouble); // Output: 9

            // Explicit Casting (Narrowing Conversion)
            double myDouble2 = 9.78;
            int myInt2 = (int)myDouble2; // Manual conversion from double to int
            Console.WriteLine("Explicit Casting (double to int): " + myInt2); // Output: 9

            // Using Convert class for conversions
            string strNumber = "123";
            int convertedNumber = Convert.ToInt32(strNumber);
            Console.WriteLine("Using Convert class: " + convertedNumber); // Output: 123

            // Using Parse method for conversions
            string strDouble = "45.67";
            double parsedDouble = double.Parse(strDouble);
            Console.WriteLine("Using Parse method: " + parsedDouble); // Output: 45.67

            // Using TryParse method for safe conversions
            string invalidStr = "abc";
            bool isParsed = int.TryParse(invalidStr, out int result);
            if (isParsed)
            {
                Console.WriteLine("TryParse succeeded: " + result);
            }
            else
            {
                Console.WriteLine("TryParse failed for input: " + invalidStr); // Output: TryParse failed for input: abc
            }

            string intStr = "42";

            // Using Parse (can throw FormatException)
            try
            {
                int parsedInt = int.Parse(intStr);
                Console.WriteLine($"Parsed integer: {parsedInt}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format for integer.");
            }

            // Using TryParse (safer)
            string doubleStr = "3.14159";
            double parsedDouble;

            if (double.TryParse(doubleStr, out parsedDouble))
            {
                Console.WriteLine($"Successfully parsed double: {parsedDouble}");
            }
            else
            {
                Console.WriteLine("Failed to parse double.");
            }

            string invalidStr = "abc";
            int tryParseResult;
            if (int.TryParse(invalidStr, out tryParseResult))
            {
                Console.WriteLine($"Parsed integer: {tryParseResult}");
            }
            else
            {
                Console.WriteLine($"Failed to parse '{invalidStr}' as an integer."); // This will be printed
            }
        }
    }
}