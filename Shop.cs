using ApplePieStore.Customers.Enums;
using ApplePieStore.Customers;
using ApplePieStore.Enums;
using ApplePieStore.Menu;

namespace ApplePieStore;

public class Shop
{
    public CustomerFileManager CustomerFileManager = new CustomerFileManager();
    public MenuSelection MenuSelection { get; set; }
    private Customer CurrentCustomer { get; set; }
    private List<Product> Products { get; set; } = new List<Product>()
        {
            new Product("Milk", 19.67),
            new Product("Butter", 50.50),
            new Product("Flour",25.99),
            new Product("Brown sugar",45.65),
            new Product("Salt",12.54),
            new Product("Egg",5.67),
            new Product("Powdered sugar", 23.46),
            new Product("Apple",45.55),
            new Product("Lemon",12.88),
            new Product("Cardamon",29.43),
            new Product("Cinnamon",23.48),
            new Product("Nutmeg", 32.03),
            new Product("Baking Powder", 17.83),
            new Product("Ginger", 9.60),

        };
    private List<Customer> Customers { get; set; } = new List<Customer>();
    public Currency Currency { get; set; } = Currency.SEK;
    public string DefaultPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Customers");
    public Shop() // When shop is initialized it prepares default customers which can be used without registration.
                  // If files for all customers(including prePreparedCustomers) are existing, constructor creates list Customers with them. 
    {
        //var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Customers");
        Directory.CreateDirectory(DefaultPath);

        List<Customer> prePreparedCustomers = new List<Customer>()
                {
                    new SilverCustomer("Knatte", "123"),
                    new Customer("Fnatte", "321"),
                    new GoldenCustomer("Tjatte", "1213")
                };

        foreach (var customer in prePreparedCustomers) //checks if prePreparedCustomer has a existing file. If not Adds it to the folder
        {
            var path = Path.Combine(DefaultPath, $"{customer.Name}.txt");
            if (!File.Exists(path))
            {
                CustomerFileManager.CreateCustomerFile(path, customer);
            }
        }

        foreach (var customer in CustomerFileManager.ReadCustomersFile(DefaultPath)) // Reads all files in the folder and adds them to Customers
        {
            Customers.Add(customer);
        }

        Customers = Customers.DistinctBy(c => c.ToString()).ToList(); // it looks for doubles in a Customers and removes them

    }
    public void MainMenu()
    {
        if (CurrentCustomer != null)
        {
            CurrentCustomer = null;
        }
        MenuSelection = new MenuSelection(new Option("Login...", Login), new Option("Register...", Register));
        MenuSelection.DisplayMenu();
    }
    private void Login()
    {

        while (true)
        {
            bool correctUsername = false;  // collects information from foreach loop  
            bool correctPassword = false;
            Console.WriteLine("Username: ");
            string customerName = Console.ReadLine();
            Console.WriteLine("Password:");
            string customerPassword = Console.ReadLine();
            if (Customers.Count > 0)
            {
                foreach (var customer in Customers) //looking thru a list to find matching customer name
                {
                    if (customer.Name == customerName && customer.CheckPassword(customerPassword))
                    {
                        CurrentCustomer = customer;
                        ShopMenu();
                    }
                    else if (customer.Name != customerName)
                    {
                        correctUsername = false;
                    }
                    else // wrong password
                    {
                        correctUsername = true;
                        correctPassword = false;
                        break;

                    }
                }

                if (!correctUsername)
                {
                    MenuSelection = new MenuSelection(
                        "Wrong username!",
                        new Option("Try Again...", Login),
                        new Option("Register now!..", Register));

                    MenuSelection.DisplayMenu();
                }
                else if (!correctPassword)
                {
                    MenuSelection = new MenuSelection(
                        "Wrong password!",
                        new Option("Try Again...", Login),
                        new Option("Go back to Login Page...", MainMenu));

                    MenuSelection.DisplayMenu();
                }

            }


        }


    }
    private void Register()
    {
        Console.Clear();
        int count = Customers.Count;
        while (count == Customers.Count) // while loop iterates till a new customer is added (Customers list changers its size)
        {
            // Add check for identical username 
            Customer customer;
            Console.WriteLine("Username: ");
            string customerName = Console.ReadLine();
            Console.WriteLine("password:");
            string customerPassword = Console.ReadLine();

            // checks if there isn't any other customer with given name 
            if (Customers.Exists(c => c.Name == customerName))
            {
                MenuSelection = new MenuSelection(
                        "This name is taken!",
                        new Option("Try Again...", Register),
                        new Option("Go back to Login Page...", MainMenu));
                MenuSelection.DisplayMenu();
            }
            //Add this new customer to file 
            Customer tempCustomer = new Customer(customerName, customerPassword);
            Customers.Add(new Customer(customerName, customerPassword));
            //Adds this new customer to Customers file
            var path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Customers", $"{customerName}.txt"));
            CustomerFileManager.CreateCustomerFile(path, tempCustomer);

        }

        MainMenu();

    }
    private void Logout()
    {
        //Look after customer file with name matching CurrentCustomer and change information inside
        int indexOfActiveUser = Customers.FindIndex(c => c.Name == CurrentCustomer.Name);

        if (indexOfActiveUser == -1) // feels unnecessary, but i am trying to lear to implement guard clauses
        {
            return;
        }

        Customers[indexOfActiveUser].SavingCartContent(CurrentCustomer.Cart); // saves content of CurrentCustomer cart to cart of specific customer in Customers, which makes it possible to update it under many login sessions
                                                                              // log out, by setting CurrentCustomer to null
        CurrentCustomer = null;
        Console.Clear();
        Console.WriteLine("Thank you for your visit, welcome back");
        Thread.Sleep(2000);
        MainMenu();
    }
    private void ShopMenu()
    {
        if (CurrentCustomer == null)
        {
            return;
        }
        Console.Clear();

        List<Option> AllOptionsAndAllProducts()
        {
            var options = new List<Option>();
            foreach (var product in Products)
            {
                options.Add(new ProductOption(MenageNumberOfProductsInACart, product,
                    CurrentCustomer.CheckAnAmountOfGivenProductInTheCart(product)));
            }

            options.Add(new Option("Checkout...", Checkout));
            options.Add(new Option("CartView...", ViewCart));
            options.Add(new Option("Logout...", Logout));
            options.Add(new Option("Alter currency type...", AlterCurrencyType));

            return options;
        }

        MenuSelection = new MenuSelection("Shop Groceries", AllOptionsAndAllProducts());
        MenuSelection.DisplayMenu();
    }
    private void AlterCurrencyType() //checks which currency is active/currently used by Shop class and adjust it to user wish 
    {
        foreach (var option in MenuSelection.ListOfOptions)
        {
            if (option is ProductOption productOption)
            {
                if (productOption.Currency == Currency.SEK)
                {
                    productOption.Currency = Currency.EUR;
                    Currency = Currency.EUR;

                }

                else if (productOption.Currency == Currency.EUR)
                {
                    productOption.Currency = Currency.PLN;
                    Currency = Currency.PLN;
                }

                else if (productOption.Currency == Currency.PLN)
                {
                    productOption.Currency = Currency.SEK;
                    Currency = Currency.SEK;
                }

            }
        }
    }
    private void MenageNumberOfProductsInACart(Product product, ConsoleKey key) // is used in ShopMenu, reads key to determent to add or remove product 
    {

        switch (key)
        {
            case ConsoleKey.RightArrow:

                CurrentCustomer.AddToCart(product);
                break;
            case ConsoleKey.LeftArrow: // removes item from customers cart if there is more than 0 
                if (CurrentCustomer.Cart.Contains(product))
                {
                    CurrentCustomer.RemoveFromCart(product);
                }
                break;
        }

    }
    private string ShowCartContains()
    {
        string output = string.Empty;

        output += "Cart:\n";
        foreach (var productType in Products)
        {
            double price = productType.Price * (int)Currency / 100;
            double totalPriceOfGivenProductInTheCart = price * CurrentCustomer.CheckAnAmountOfGivenProductInTheCart(productType);

            if (CurrentCustomer.Cart.Contains(productType)) // shows product with its price, currency and amount in the cart 
            {
                output += $"{productType.Name} {price:F}{Currency.ToString()}/{CurrentCustomer.CheckAnAmountOfGivenProductInTheCart(productType)}st = {totalPriceOfGivenProductInTheCart:F}{Currency.ToString()}\n ";

            }
        }

        if (CurrentCustomer.DiscountStatus == CustomerDiscountStatus.Default)
        {
            output += $"Total: {CurrentCustomer.CartTotal(Currency):F}{Currency}\n";

        }
        else
        {
            output += $"Total: {CurrentCustomer.CartTotal(Currency) * (int)CurrentCustomer.DiscountStatus / 100:F}{Currency}\n";
            int discount = 100 - (int)CurrentCustomer.DiscountStatus;
            output += $"You have {discount}% discount on this purchase.";
        }
        return output;
    }
    private void ViewCart()
    {
        Console.Clear();
        Console.WriteLine(ShowCartContains());
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey(true);
    }

    private void MakeAPurchase()
    {
        void CustomerWithNewStatus(CustomerDiscountStatus discount)
        {
            foreach (var customer in Customers)
            {
                if (CurrentCustomer.Name == customer.Name)
                {
                    Customers.Remove(customer);
                    CurrentCustomer = CustomerFileManager.RewriteCustomerFileAndCustomer(CurrentCustomer, discount, DefaultPath);
                    break; // without break I get exception System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
                }
            }
        }

        Console.Clear();

        if ((double)CurrentCustomer.CartTotal(Currency.SEK) * (int)CurrentCustomer.DiscountStatus / 100 > (double)1000)
        {
            CustomerWithNewStatus(CustomerDiscountStatus.Gold);
            Console.WriteLine("Thank you for your purchase, you will get a for your next shopping a 15% discount.");
        }
        else if ((double)CurrentCustomer.CartTotal(Currency.SEK) * (int)CurrentCustomer.DiscountStatus / 100 > (double)700)
        {
            CustomerWithNewStatus(CustomerDiscountStatus.Silver);
            Console.WriteLine("Thank you for your purchase, you will get a for your next shopping a 10% discount.");
        }
        else if ((double)CurrentCustomer.CartTotal(Currency.SEK) * (int)CurrentCustomer.DiscountStatus / 100 > (double)500)
        {
            CustomerWithNewStatus(CustomerDiscountStatus.Bronze);
            Console.WriteLine("Thank you for your purchase, you will get a for your next shopping a 5% discount.");
        }
        else
        {
            CustomerWithNewStatus(CustomerDiscountStatus.Default);
            Console.WriteLine("Thank you for your purchase.");
        }

        CurrentCustomer.Cart.RemoveAll((product => product == product));
        Customers.Add(CurrentCustomer);
        Thread.Sleep(2000);
        ShopMenu();
    }
    private void Checkout()
    {
        Console.Clear();
        MenuSelection = new MenuSelection(
            ShowCartContains(),
            new Option("Go back to shopping...", ShopMenu),
            new Option("Buy", MakeAPurchase));
        MenuSelection.DisplayMenu();
        Console.ReadKey();
    }
}
