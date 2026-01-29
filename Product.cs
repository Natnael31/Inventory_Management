public class Product
{
    public int Id;
    public string Name;
    public int Quantity;
    public double Price;

    // struct used here for storing details of a product
    public ProductStruct Description;

    public Product(int id, string name, int quantity, double price, ProductStruct description)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
        Description = description;
    }

    public double GetValue()
    {
        return Quantity * Price;
    }
}
