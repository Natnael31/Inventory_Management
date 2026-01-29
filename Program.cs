using System;

class Program
{
    static void Main(string[] args)
    {
        InventoryManager manager = new InventoryManager();
        manager.LoadFromFile();

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== Inventory Management System ===");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View Inventory");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Save to file and Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";


            switch (choice)
            {
                case "1":
                    manager.AddProduct();
                    break;

                case "2":
                    manager.DisplayInventory();
                    break;

                case "3":
                    manager.UpdateProduct();
                    break;
                case "4":
                    manager.DeleteProduct();
                    break;

                case "5":
                    manager.SaveToFile();
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }
}
