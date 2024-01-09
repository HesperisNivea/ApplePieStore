using ApplePieStore.Customers.Enums;

namespace ApplePieStore.Customers;

public class BronzeCustomer : Customer
{
    public BronzeCustomer(string name, string password) : base(name, password)
    {
        DiscountStatus = CustomerDiscountStatus.Bronze;
    }
}