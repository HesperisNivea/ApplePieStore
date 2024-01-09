using ApplePieStore.Customers.Enums;
using ApplePieStore.Enums;

namespace ApplePieStore.Customers;

public class Customer
{
    public string Name { get; private set; }

    private string Password { get; set; }

    private List<Product> _cart;
    public List<Product> Cart { get { return _cart; } }

    public CustomerDiscountStatus DiscountStatus { get; private protected set; }

    public Customer(string name, string password)
    {
        Name = name;
        Password = password;
        _cart = new List<Product>();
        DiscountStatus = CustomerDiscountStatus.Default;
    }

    public bool CheckPassword(string password)
    {
        if (password == Password)
        {
            return true;
        }
        else { return false; }

    }

    public void AddToCart(Product product)
    {
        _cart.Add(product);

    }

    public void RemoveFromCart(Product product)
    {
        _cart.Remove(product);
    }

    public int CheckAnAmountOfGivenProductInTheCart(Product product)
    {
        int amount = Cart.Count(p => p.Name.Contains(product.Name));
        return amount;
    }

    public double CartTotal(Currency currency)
    {
        double total = 0;
        foreach (var product in Cart)
        {
            total += product.Price * (int)currency / 100;
        }

        return total;
    }

    public void SavingCartContent(List<Product> newCartContentList)
    {
        _cart = newCartContentList;
    }

    public override string ToString()
    {
        var output = string.Empty;
        output += $"Name: {Name}\n";
        output += $"Password: {Password}\n";
        output += $"Discount Status: {DiscountStatus}\n";
        foreach (var product in Cart)
        {
            output += product.ToString();

        }
        return output;
    }
}