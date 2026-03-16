using System;

public class DecisionLogic
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Decision Logic ---");

        // Get user input
        Console.Write("Enter a number: ");
        string input = Console.ReadLine();
        int number;

        if (!int.TryParse(input, out number))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            Console.ReadKey();
            return;
        }

        // Decision making
        if (number > 0)
        {
            Console.WriteLine("The number is positive.");
        }
        else if (number < 0)
        {
            Console.WriteLine("The number is negative.");
        }
        else
        {
            Console.WriteLine("The number is zero.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}