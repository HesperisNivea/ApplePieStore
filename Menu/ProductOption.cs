using ApplePieStore.Customers.Enums;
using ApplePieStore.Enums;

namespace ApplePieStore.Menu;

public class ProductOption : Option
{
    public Product Product { get; set; }
    public int Amount { get; private set; }
    public Currency Currency { get; set; } = Currency.SEK;
    public CustomerDiscountStatus Discount { get; set; } = CustomerDiscountStatus.Default;
    public Action<Product, ConsoleKey> SelectedActionWithProduct { get; set; }

    public ProductOption(Action<Product, ConsoleKey> selectedActionWithProduct, Product product, int amount)
    {
        Amount = amount;
        Product = product;
        SelectedActionWithProduct = selectedActionWithProduct;
    }

    //add prop delegate with argument (for AddToCart) or not we will se
    public override void InvokeSelectedMethodWithKey(ConsoleKey key)
    {
        SelectedActionWithProduct(Product, key);
        // regulate visible numbers of products in ShopMenu
        if (key == ConsoleKey.RightArrow)
        {
            Amount++;
        }
        else if (key == ConsoleKey.LeftArrow)
        {
            if (Amount > 0)
            {
                Amount--;
            }
        }
    }

    public override string ToString()
    {
        int nameLength = Product.Name.Length;
        int distanceBetweenNameAndAmount = 30;
        string separationDots = ".......";
        string dots = "";
        for (int i = nameLength; i < distanceBetweenNameAndAmount; i++)
        {
            dots += ".";
        }

        double price = Product.Price * (int)Currency / 100;
        return $"{Product.Name}{dots}<< {Amount} >>{separationDots}{price:F}{Currency}";
    }
}