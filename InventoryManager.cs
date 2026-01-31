using System;
using System.Collections.Generic;
using System.IO;

public class InventoryManager
{
    private List<Product> products = new List<Product>();
    private const string FILE_NAME = "inventory.txt";

    public void AddProduct()
    {
        // Load current products to prevent duplicate IDs
        LoadFromFile();

        Console.Write("Enter Product ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        // Check if ID already exists
        if (products.Exists(p => p.Id == id))
        {
            Console.WriteLine("A product with this ID already exists.");
            return;
        }

        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter Category: ");
        string category = Console.ReadLine() ?? "";

        Console.Write("Enter Note: ");
        string note = Console.ReadLine() ?? "";

        Console.Write("Enter Quantity: ");
        int quantity = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter Unit Price: ");
        double price = double.Parse(Console.ReadLine() ?? "0");

        ProductStruct description = new ProductStruct
        {
            Category = category,
            Note = note
        };

        Product product = new Product(id, name, quantity, price, description);
        products.Add(product);

        SaveToFile(); // save immediately
        Console.WriteLine("Product added successfully.");
    }

    public void DisplayInventory()
    {
        LoadFromFile(); // load latest inventory from file

        if (products.Count == 0)
        {
            Console.WriteLine("Inventory is empty.");
            return;
        }

        double totalValue = 0;

        foreach (Product p in products)
        {
            Console.WriteLine(
                $"ID: {p.Id}, Name: {p.Name}, Qty: {p.Quantity}, Price: ${p.Price}, " +
                $"Category: {p.Description.Category}, Note: {p.Description.Note}"
            );

            totalValue += p.GetValue();
        }

        Console.WriteLine($"Total Inventory Value: ${totalValue}");
    }

    public void UpdateProduct()
    {
        LoadFromFile(); // load latest inventory

        Console.Write("Enter Product ID to update: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Product? product = products.Find(p => p.Id == id);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.Write($"Enter new Product Name (current: {product.Name}): ");
        string name = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(name)) product.Name = name;

        Console.Write($"Enter new Category (current: {product.Description.Category}): ");
        string category = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(category)) product.Description.Category = category;

        Console.Write($"Enter new Note (current: {product.Description.Note}): ");
        string note = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(note)) product.Description.Note = note;

        Console.Write($"Enter new Quantity (current: {product.Quantity}): ");
        string qtyInput = Console.ReadLine() ?? "";
        if (int.TryParse(qtyInput, out int qty)) product.Quantity = qty;

        Console.Write($"Enter new Unit Price (current: {product.Price}): ");
        string priceInput = Console.ReadLine() ?? "";
        if (double.TryParse(priceInput, out double price)) product.Price = price;

        SaveToFile(); // save changes
        Console.WriteLine("Product updated successfully.");
    }

    public void DeleteProduct()
    {
        LoadFromFile(); // load latest inventory

        Console.Write("Enter Product ID to delete: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Product? product = products.Find(p => p.Id == id);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        products.Remove(product);
        SaveToFile(); // save changes
        Console.WriteLine("Product deleted successfully.");
    }

    public void SaveToFile()
    {
        // Overwrite file with current products
        using (StreamWriter writer = new StreamWriter(FILE_NAME, false))
        {
            foreach (Product p in products)
            {
                writer.WriteLine(
                    $"{p.Id},{p.Name},{p.Quantity},{p.Price},{p.Description.Category},{p.Description.Note}"
                );
            }
        }
    }

    public void LoadFromFile()
    {
        products.Clear();

        if (!File.Exists(FILE_NAME))
            return;

        string[] lines = File.ReadAllLines(FILE_NAME);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');

            if (parts.Length < 6)
            {
                Console.WriteLine(line);
                continue;
            }

            try
            {
                ProductStruct description = new ProductStruct
                {
                    Category = parts[4].Trim(),
                    Note = parts[5].Trim()
                };

                Product p = new Product(
                    int.Parse(parts[0]),
                    parts[1].Trim(),
                    int.Parse(parts[2]),
                    double.Parse(parts[3]),
                    description
                );

                products.Add(p);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading line: {line}\n{ex.Message}");
            }
        }
    }
}
