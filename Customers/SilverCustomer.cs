using ApplePieStore.Customers.Enums;

namespace ApplePieStore.Customers;

public class SilverCustomer : Customer
{
    public SilverCustomer(string name, string password) : base(name, password)
    {
        DiscountStatus = CustomerDiscountStatus.Silver;
    }
}