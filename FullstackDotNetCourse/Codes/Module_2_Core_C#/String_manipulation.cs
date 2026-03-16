using System;

public class StringManipulationDemo
{
    public static void Main(string[] args)
    {
        // --- String Concatenation Examples ---
        Console.WriteLine("--- String Concatenation ---");
        string firstName = "Bob";
        string lastName = "Johnson";
        string greeting = "Hello, ";

        // Using + operator
        string fullName = greeting + firstName + " " + lastName + "!";
        Console.WriteLine($"Full Name: {fullName}");

        // Using string.Join for a list of items
        string[] items = {"Apples", "Bananas", "Cherries"};
        string itemList = string.Join(", ", items);
        Console.WriteLine($"Item List: {itemList}");

        // --- String Formatting Examples ---
        Console.WriteLine("\n--- String Formatting ---");
        string item = "Widget";
        decimal unitPrice = 15.75m;
        int quantityOrdered = 5;
        DateTime orderDate = DateTime.Now;

        // Using String Interpolation
        string orderSummaryInterpolated = $"Order Summary:\n  Item: {item}\n  Unit Price: {unitPrice:C}\n  Quantity: {quantityOrdered}\n  Total: {unitPrice * quantityOrdered:C}\n  Order Date: {orderDate:yyyy-MM-dd}";
        Console.WriteLine(orderSummaryInterpolated);

        // Using string.Format()
        string orderSummaryFormatted = string.Format(
            "Order Summary:\n  Item: {0}\n  Unit Price: {1:C}\n  Quantity: {2}\n  Total: {3:C}\n  Order Date: {4:yyyy-MM-dd}",
            item, unitPrice, quantityOrdered, unitPrice * quantityOrdered, orderDate);
        Console.WriteLine(orderSummaryFormatted);

        // --- Other String Operations ---
        Console.WriteLine("\n--- Other String Operations ---");
        string sampleText = "  The quick brown fox jumps over the lazy dog.  ";
        Console.WriteLine($"Original Text: '{sampleText}'");

        Console.WriteLine($"Length: {sampleText.Length}");
        Console.WriteLine($"Trimmed Text: '{sampleText.Trim()}'");
        Console.WriteLine($"Uppercase: '{sampleText.Trim().ToUpper()}'");
        Console.WriteLine($"Contains 'fox'? {sampleText.Contains("fox")}");
        Console.WriteLine($"Index of 'jumps': {sampleText.IndexOf("jumps")}");

        string replacedText = sampleText.Replace("lazy", "energetic");
        Console.WriteLine($"Replaced Text: '{replacedText}'");

        string firstPart = sampleText.Substring(3, 5); // Extracts 'quick'
        Console.WriteLine($"Substring (index 3, length 5): '{firstPart}'");

        Console.ReadKey();
    }
}
