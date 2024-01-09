namespace ApplePieStore.Menu;

public class MenuSelection
{
    public List<Option> ListOfOptions { get; set; } = new List<Option>();

    public string OptionalMessage { get; set; } = null;

    public MenuSelection(params Option[] options)
    {
        foreach (var option in options)
        {
            ListOfOptions.Add(option);
        }
    }
    public MenuSelection(string optionalMessage, params Option[] options)
    {
        OptionalMessage = optionalMessage;
        foreach (var option in options)
        {
            ListOfOptions.Add(option);
        }
    }
    public MenuSelection(string optionalMessage, IEnumerable<Option> options) // to simplify ShopMenu
    {
        OptionalMessage = optionalMessage;
        foreach (var option in options)
        {
            ListOfOptions.Add(option);
        }
    }
    private void MarkCurrentlyChosenOption(Option selectedOption)
    {
        Console.Clear();
        if (OptionalMessage != null)
        {
            Console.WriteLine(OptionalMessage);
        }
        foreach (var option in ListOfOptions)
        {
            if (option == selectedOption)
            {
                Console.Write(">>");
            }
            else
            {
                Console.Write("  ");
            }
            Console.Write(option.ToString());
            Console.WriteLine();
        }
    }

    public void DisplayMenu()
    {
        if (!ListOfOptions.Any())
        {
            return;
        }

        int selectedOptionIndex = 0; //It works as a starting position and later will help in navigating and limiting user possible actions


        MarkCurrentlyChosenOption(ListOfOptions[selectedOptionIndex]);

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.UpArrow)
            {
                if (selectedOptionIndex > 0)
                {
                    selectedOptionIndex--; // regulates index so it always is accurate 
                    MarkCurrentlyChosenOption(ListOfOptions[selectedOptionIndex]); // sends information which option needs to be highlighted 
                }
            }
            if (key.Key == ConsoleKey.DownArrow)
            {
                if (selectedOptionIndex < ListOfOptions.Count - 1)
                {
                    selectedOptionIndex++;
                    MarkCurrentlyChosenOption(ListOfOptions[selectedOptionIndex]);
                }
            }

            // for food make < > that their amount can be change and visible for user directly 
            if (ListOfOptions[selectedOptionIndex] is ProductOption productOption)
            {
                if (key.Key == ConsoleKey.RightArrow)
                {
                    // Add to cart
                    ListOfOptions[selectedOptionIndex].InvokeSelectedMethodWithKey(key.Key);
                    MarkCurrentlyChosenOption(ListOfOptions[selectedOptionIndex]);
                }

                if (key.Key == ConsoleKey.LeftArrow)
                {
                    //Remove from cart
                    ListOfOptions[selectedOptionIndex].InvokeSelectedMethodWithKey(key.Key);
                    MarkCurrentlyChosenOption(ListOfOptions[selectedOptionIndex]);
                }
            }

            if (!(ListOfOptions[selectedOptionIndex] is ProductOption))
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    ListOfOptions[selectedOptionIndex].InvokeSelectedMethod();
                    MarkCurrentlyChosenOption(ListOfOptions[selectedOptionIndex]);
                }

            }

        }


    }
}