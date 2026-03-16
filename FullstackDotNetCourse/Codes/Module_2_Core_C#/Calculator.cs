using System;

public class SimpleCalculator
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Simple Calculator ---");

        //Get first number from user
        Console.Write("Enter the first number: ");
        string input1 = Console.ReadLine();
        double num1;

        //Get second number from user
        Console.Write("Enter the second number: ");
        string input2 = Console.ReadLine();
        double num2;

        if(!double.TryParse(input1, out num1))
        {
            Console.WriteLine("Invalid input for the first number. please enter a valid number.");
            Console.ReadKey();
            return;
        }

        if(!double.TryParse(input2, out num2))
        {
            Console.WriteLine("Invalid input for the second number. please enter a valid number.");
            Console.ReadKey();
            return;
        }

        //Get the operation from user
        Console.Write("Enter the operation (+, -, *, /): ");
        string operatorInput = Console.ReadLine();
        char operation;

        if(string.IsNullOrEmpty(operatorInput) || operatorInput.Length != 1)
        {
            Console.WriteLine("Invalid operator. Please enter one of +, -, *, /.");
            Console.ReadKey();
            return;
        }

        operation = operatorInput[0]; // Get the first character of the input

        double result;
        bool error = false;

        switch(operation)
        {
            case '+':
                result = num1 + num2;
                break;
            case '-':
                result = num1 - num2;
                break;
            case '*':
                result = num1 * num2;
                break;
            case '/':
                if(num2 == 0)
                {
                    Console.WriteLine("Error: Division by zero is not allowed.");
                    error = true;
                    result = 0; // Default value, won't be used
                }
                else
                {
                    result = num1 / num2;
                }
                break;
            default:
                Console.WriteLine("Invalid operator. Please enter one of +, -, *, /.");
                error = true;
                result = 0; // Default value, won't be used
                break;
        }

        if (!error)
        {
            Console.WriteLine($"Result: {num1} {operation} {num2} = {result}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
} 