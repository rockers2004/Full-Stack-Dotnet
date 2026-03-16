using System;

public class VariableDemo
{
    public static void Main(string[] args)
    {
        // --- Primitive Data Types Declaraation and Intialization ---

        // Integer types
        int numberOfStudents = 30; // 4 bytes
        long populationOfCity = 5000000L; // 8 bytes
        byte maxByteValue = 255; // 1 byte

        //Floating-point types
        double averagScore = 85.5; // 8 bytes
        decimal priceOfItem = 19.99m; // 16 bytes
        float piValue = 3.14f; // 4 bytes

        // Boolean type
        bool isRaining = false; // 1 byte

        // Character type
        char grade = 'A'; // 2 bytes
        // String type
        string studentName = "John Doe"; // Reference type
        string welcomeMessage = "Hello, World!"; // Reference type

        //Using 'var' keyword for implicit typing
        var age = 25; // Compiler infers this as int
        var height = 5.9; // Compiler infers this as double
        var isActive = true; // Compiler infers this as bool

        // --- Output the values to the console ---
        Console.WriteLine("Number of Students: " + numberOfStudents);
        Console.WriteLine("Population of City: " + populationOfCity);
        Console.WriteLine("Max Byte Value: " + maxByteValue);
        Console.WriteLine("Average Score: " + averagScore);
        Console.WriteLine("Price of Item: " + priceOfItem);
        Console.WriteLine("Value of Pi: " + piValue);
        Console.WriteLine("Is it raining? " + isRaining);
        Console.WriteLine("Grade: " + grade);
        Console.WriteLine("Student Name: " + studentName);
        Console.WriteLine("Welcome Message: " + welcomeMessage);
        Console.WriteLine("Age: " + age);
        Console.WriteLine("Height: " + height);
        Console.WriteLine("Is Active? " + isActive);

        // --- Demonstrating Value vs. Reference Types ---
        Console.WriteLine("\n--- Value vs. Reference Types ---");
        // Value type example
        int a = 10;
        int b = a; // b gets a copy of the value of a
        b = 20; // Changing b does not affect a
        Console.WriteLine("Value Type Example:");
        Console.WriteLine("a: " + a); // Output: 10
        Console.WriteLine("b: " + b); // Output: 20

        // Reference type example
        string originalString = "Hello";
        string referenceString = originalString; // referenceString points to the same string as originalString
        referenceString = "World"; // Changing referenceString does not change originalString
        Console.WriteLine("\nReference Type Example:");
        Console.WriteLine("Original String: " + originalString); // Output: Hello
        Console.WriteLine("Reference String: " + referenceString); // Output: World

        //Note: In C#, strings are immutable reference types, so when we assign referenceString to originalString, they both point to the same string in memory. However, when we change referenceString, it creates a new string in memory, leaving originalString unchanged.
        //Note: For strings, assignment copies the reference, but modification creates a new string.
        //For mutable reference types (like classes). changing refB would change refA as well since they point to the same object in memory.

        Console.ReadKey();

    }
}