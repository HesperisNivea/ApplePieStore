using ApplePieStore.Customers.Enums;
using ApplePieStore.Customers;

namespace ApplePieStore;

public class CustomerFileManager
{
    public void CreateCustomerFile(string path, Customer customer)
    {
        if (!File.Exists(path))
        {
            using StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(customer.ToString());
            sw.Close();
        }
    }

    private Customer DetermineStatusOfCustomerAndCreateOne(string name, string password, string discountStatus)
    {
        if (CustomerDiscountStatus.Gold.ToString() == discountStatus)
        {
            GoldenCustomer GoldenCustomer = new GoldenCustomer(name, password);
            return GoldenCustomer;
        }
        else if (discountStatus == CustomerDiscountStatus.Silver.ToString())
        {
            SilverCustomer tempSilverCustomer = new SilverCustomer(name, password);
            return tempSilverCustomer;
        }
        else if (discountStatus == CustomerDiscountStatus.Bronze.ToString())
        {
            BronzeCustomer tempBronzeCustomer = new BronzeCustomer(name, password);
            return tempBronzeCustomer;
        }
        else
        {
            Customer tempCustomer = new Customer(name, password);
            return tempCustomer;
        }
    }

    public Customer RewriteCustomerFileAndCustomer(Customer customer, CustomerDiscountStatus discountStatus, string path)
    {
        using StreamReader sr = new StreamReader(Path.Combine(path, $"{customer.Name}.txt"));
        string? line = "";
        string name = "";
        string password = "";
        while (!line.StartsWith("Discount Status: "))
        {
            line = sr.ReadLine();
            if (line.StartsWith("Name: "))
            {
                name = line.Substring(6);
            }
            else if (line.StartsWith("Password: "))
            {
                password = line.Substring(10);
            }
        }
        sr.Close();
        Customer tempCustomer = DetermineStatusOfCustomerAndCreateOne(name, password, discountStatus.ToString());

        if (File.Exists(Path.Combine(path, $"{customer.Name}.txt")))
        {
            using StreamWriter sw = new StreamWriter(Path.Combine(path, $"{customer.Name}.txt"));
            sw.WriteLine(tempCustomer.ToString());
            sw.Close();
        }

        return tempCustomer;
    }

    public List<Customer> ReadCustomersFile(string path)
    {
        string[] directPath = Directory.GetFiles(path);
        List<Customer> CustomersList = new List<Customer>();

        foreach (var pathName in directPath)
        {
            if (!File.Exists(pathName))
            {
                return CustomersList;
            }

            string? line = "";

            string name = "";
            string password = "";
            string discountStatus = "";

            using StreamReader sr = new StreamReader(pathName);

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("Name: "))
                {
                    name = line.Substring(6);
                }
                else if (line.StartsWith("Password: "))
                {
                    password = line.Substring(10);
                }
                else if (line.StartsWith("Discount Status: "))
                {
                    discountStatus = line.Substring(17);
                }
                else
                {
                    CustomersList.Add(DetermineStatusOfCustomerAndCreateOne(name, password, discountStatus));
                }
            }
        }

        return CustomersList;
    }
}