using ApplePieStore.Customers.Enums;

namespace ApplePieStore.Customers;

public class GoldenCustomer : Customer
{
    public GoldenCustomer(string name, string password) : base(name, password)
    {
        DiscountStatus = CustomerDiscountStatus.Gold;
    }
}