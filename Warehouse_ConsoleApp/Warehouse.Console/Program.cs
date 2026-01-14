using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO.Enumeration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application;
using Warehouse.Model;
using Warehouse.Persistence.MsSql;
using Warehouse.Persistence.MsSql.Repository;

namespace Warehouse.Console;

class Program
{
    static void Main(string[] args)
    {
        // Starting console
        System.Console.Write("Starting the application");
        Thread.Sleep(200); 
        System.Console.Write(".");
        Thread.Sleep(200);
        System.Console.Write(".");
        Thread.Sleep(200);
        System.Console.Write(".\n\n");
        
        var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options => 
                    // Windows Connection String (LocalDB)
                //options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=persondb;Integrated Security=True;MultipleActiveResultSets=true;"),
                //ServiceLifetime.Transient)
                    // Mac Connection String
                options.UseSqlServer("Data Source=localhost,1433;Initial Catalog=OnePieceDatabase;User ID=sa;Password=SqlServerPassw0rd!;TrustServerCertificate=True"),
                    ServiceLifetime.Transient)
            .AddSingleton<XmlDataLoader>() 
            .AddSingleton<AllXmlDataExporter>()
            .AddSingleton<ShipmentService>()
            .AddSingleton<PirateShipService>()
            .AddSingleton<CargoService>()
            .AddSingleton<ReportService>()
            .AddSingleton<EasterEggService>()
            .AddSingleton<CargoDataProvider>()
            .AddSingleton<ShipmentDataProvider>()
            .AddSingleton<PirateShipDataProvider>()
            .BuildServiceProvider(); 
        
        var dbContext = serviceProvider.GetService<AppDbContext>();
        var xmlLoader = serviceProvider.GetService<XmlDataLoader>();
        var shipmentService = serviceProvider.GetService<ShipmentService>();
        var cargoService = serviceProvider.GetService<CargoService>();
        var pirateShipService = serviceProvider.GetService<PirateShipService>();
        var reportService = serviceProvider.GetService<ReportService>();
        var easterEggService = serviceProvider.GetService<EasterEggService>();
        var cargoDataProvider = serviceProvider.GetService<CargoDataProvider>();
        var shipmentDataProvider = serviceProvider.GetService<ShipmentDataProvider>();
        var pirateshipDataProvider = serviceProvider.GetService<PirateShipDataProvider>();
        var allxmlDataExporter = serviceProvider.GetService<AllXmlDataExporter>();
        
        if (dbContext == null || allxmlDataExporter == null || cargoService == null || pirateShipService == null ||xmlLoader == null || pirateshipDataProvider == null ||cargoDataProvider == null ||shipmentService == null || shipmentDataProvider == null || reportService == null || easterEggService == null)
        {
            System.Console.WriteLine("Error: One or more services could not be initialized.");
            return;
        }
        
        void RegisterShipmentDelayHandler(ShipmentService shipmentService)
        {
            foreach (var shipment in shipmentService.GetAllShipments())
            {
                shipment.ShipmentDelayed += (sender, args) =>
                {
                    System.Console.WriteLine($"Warning! Shipment ID: {args.ShipmentId} is delayed. Date: {args.Date}");
                };
            }
        }
        
        void CheckAndDisplayDelayedShipments(ShipmentService shipmentService)
        {
            var delayedShipments = shipmentService.GetAllShipments()
                .Where(s => s.IsDelayed)
                .ToList();

            if (delayedShipments.Any())
            {
                System.Console.WriteLine("Warning! The following shipments are delayed:");
                foreach (var shipment in delayedShipments)
                {
                    System.Console.WriteLine($"- Shipment ID: {shipment.Id} | Date: {shipment.Date} | PirateShip ID: {shipment.PirateShipId}");
                }
            }
            else
            {
                System.Console.WriteLine("No delayed shipments at the moment.");
            }
        }
        
        xmlLoader.SeedData("data.xml");
        
        bool running = true;
        while (running)
        {
            CheckAndDisplayDelayedShipments(shipmentService); 
            
            System.Console.WriteLine("\n\t\t\t\t\t[ OnePiece Shipment Management System ]");
            System.Console.WriteLine("1. Add Shipment");
            System.Console.WriteLine("2. Add Cargo");
            System.Console.WriteLine("3. Update Shipment");
            System.Console.WriteLine("4. Update Cargo");
            System.Console.WriteLine("5. Delete Shipment");
            System.Console.WriteLine("6. Delete Cargo");
            System.Console.WriteLine("7. Get All Shipments");
            System.Console.WriteLine("8. Get All Cargos");
            System.Console.WriteLine("9. Get All Pirate Ships");
            System.Console.WriteLine("10. Show Database");
            System.Console.WriteLine("11. Shipment report");
            System.Console.WriteLine("12. Capacity Utilization report");
            System.Console.WriteLine("13. Reflection Report");
            System.Console.WriteLine("14. Search by Cargo Type");
            System.Console.WriteLine("15. Exit");
            System.Console.Write("\nChoose an option: ");

            List<string> options = new List<string>()
            {
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"
            };
            
            string? option = System.Console.ReadLine();
            
            if (!options.Contains(option)) 
            {
                System.Console.Write("\nInvalid input. Try Again");
                Thread.Sleep(200);
                System.Console.Write(".");
                Thread.Sleep(200);
                System.Console.Write(".");
                Thread.Sleep(200);
                System.Console.Write(".");
                System.Console.Clear();
            }

            if (option == "16")
            {
                bool exit = false;
                while (!exit)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Welcome to the Easter Egg Login Panel!");
                    System.Console.WriteLine("1. Register");
                    System.Console.WriteLine("2. Login");
                    System.Console.WriteLine("3. Exit");
                    System.Console.Write("Choose an option: ");

                    int subOption = int.Parse(System.Console.ReadLine());

                    switch (subOption)
                    {
                        case 1:
                            System.Console.Clear();
                            System.Console.Write("Enter username: ");
                            string username = System.Console.ReadLine();
                            if (username == "")
                            {
                                System.Console.WriteLine("\nMust enter a username!");
                                break;
                            }
                            System.Console.Write("Enter password: ");
                            string password = ReadPassword(); // Segédmetódus a jelszó csillagozásához
                            
                            if (password == "no password")
                            {
                                System.Console.WriteLine("\nMust enter a password!");
                                break;
                            }
                            
                            System.Console.WriteLine(easterEggService.HandleOption(1, username, password));
                            break;

                        case 2:
                            System.Console.Clear();
                            System.Console.Write("Enter username: ");
                            username = System.Console.ReadLine();
                            
                            if (username == "")
                            {
                                System.Console.WriteLine("\nMust enter a username!");
                                break;
                            }
                            
                            System.Console.Write("Enter password: ");
                            password = ReadPassword();
                            
                            if (password == "no password")
                            {
                                System.Console.WriteLine("\nMust enter a password!");
                                break;
                            }
                            
                            System.Console.WriteLine(easterEggService.HandleOption(2, username, password));
                            PostLoginMenu(); 
                            break;

                        case 3:
                            System.Console.Clear();
                            System.Console.WriteLine(easterEggService.HandleOption(3));
                            exit = true;
                            break;

                        default:
                            System.Console.Clear();
                            System.Console.WriteLine("Invalid option. Try again.");
                            break;
                    }

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey();
                    System.Console.Clear();
                }
                
                string ReadPassword()
                {
                    var password = string.Empty;
                    ConsoleKeyInfo key;

                    do
                    {
                        key = System.Console.ReadKey(intercept: true);
                        if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                        {
                            password += key.KeyChar;
                            System.Console.Write("*");
                        }
                        else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password[0..^1];
                            System.Console.Write("\b \b");
                        }
                    } while (key.Key != ConsoleKey.Enter);

                    System.Console.WriteLine();
                    if (password == "")
                    {
                        return "no password";
                    }
                    return password;
                }

                void PostLoginMenu()
                {
                    while (easterEggService.IsLoggedIn)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("1. Export Database to XML");
                        System.Console.WriteLine("2. Display ASCII Art");
                        System.Console.WriteLine("3. Logout");
                        System.Console.Write($"\nUser: ");
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine($"{easterEggService.LoggedInUser}");
                        System.Console.ResetColor(); 

                        int postLoginOption = int.Parse(System.Console.ReadLine());

                        switch (postLoginOption)
                        {
                            case 1:
                                System.Console.Clear();
                                easterEggService.ExportDatabaseToXml();
                                string reportPath = Path.Combine("Reports", "DatabaseExport.xml");
                                Directory.CreateDirectory("Reports");
                                System.Console.WriteLine($"Database exported to {reportPath}");
                                break;

                            case 2:
                                System.Console.Clear();
                                System.Console.WriteLine(easterEggService.DisplayAsciiArt());
                                break;

                            case 3:
                                System.Console.Clear();
                                System.Console.WriteLine(easterEggService.HandleOption(3));
                                return;

                            default:
                                System.Console.Clear();
                                System.Console.WriteLine("Invalid option. Try again.");
                                break;
                        }

                        System.Console.WriteLine("\nPress any key to continue...");
                        System.Console.ReadKey();
                    }
                }
            }


            switch (option)
            {
                case "1":
                    // Szállítmány hozzáadása
                    System.Console.Write("Enter PirateShipId: ");
                    int shipId = int.Parse(System.Console.ReadLine());
                    
                    System.Console.WriteLine("Enter Shipment Date (yyyy-mm-dd): ");
                    DateTime shipmentDate = DateTime.Parse(System.Console.ReadLine());
                    
                    if (shipmentDataProvider.GetById(shipId) == null)
                    {
                        System.Console.WriteLine($"\nError: PirateShip with ID [{shipId}] does not exist. Please provide a valid PirateShip ID.\n");

                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    var find = shipmentDataProvider.GetShipmentByDateAndId(shipId, shipmentDate);
                    if (find != null)
                    {
                        System.Console.WriteLine($"\nA shipment with PirateShip ID [{shipId}] already exists for the date [{shipmentDate:yyyy-MM-dd}]. Shipment ID: [{find.Id}] is already recorded in the database.\n");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    shipmentService.AddShipment(shipId, shipmentDate);
                    
                    System.Console.WriteLine($"\nNew shipment has been successfully recorded:\nDate: [{shipmentDate}]\nPirateShip ID: [{shipId}].");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear();
                    break;

                case "2":
                    // Rakomány hozzáadás
                    System.Console.Write("Enter ShipmentId: ");
                    int shipmentId = int.Parse(System.Console.ReadLine());

                    System.Console.Write("Enter Cargo Type: ");
                    string type = System.Console.ReadLine();

                    System.Console.WriteLine("Enter Cargo Quantity: ");
                    int quantity = int.Parse(System.Console.ReadLine());

                    System.Console.Write("Enter Cargo Value: ");
                    decimal value = decimal.Parse(System.Console.ReadLine());

                    var shipment3 = shipmentDataProvider.GetShipmentById(shipmentId);
                    if (shipment3 == null)
                    {
                        System.Console.WriteLine($"\nError: Shipment with ID [{shipmentId}] does not exist. Please provide a valid Shipment ID.\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }

                    var pirateShip = pirateshipDataProvider.GetById(shipment3.PirateShipId);
                    if (pirateShip == null)
                    {
                        System.Console.WriteLine($"\nError: PirateShip associated with Shipment ID [{shipmentId}] does not exist.\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }

                    // Check capacity
                    int currentQuantity = shipment3.Cargos.Sum(c => c.Quantity); // Jelenlegi cargo quantity összeg
                    if (currentQuantity + quantity > pirateShip.Capacity)
                    {
                        System.Console.WriteLine($"\nError: Adding this cargo would exceed the capacity of PirateShip '{pirateShip.Name}' (Capacity: {pirateShip.Capacity}, Current: {currentQuantity}).\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }

                    // Ha minden ellenőrzés sikeres, cargo hozzáadása
                    cargoService.AddCargo(shipmentId, type, quantity, value);

                    System.Console.WriteLine($"\nCargo with Shipment ID [{shipmentId}] has been successfully recorded:\nType: [{type}]\nQuantity: [{quantity}]\nValue: [{value:C}].");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey();
                    System.Console.Clear();
                    break;

                case "3":
                    // Szállítmány módosítása
                    System.Console.Write("Enter ShipmentId: ");
                    shipmentId = int.Parse(System.Console.ReadLine());
                    
                    var shipmentTemp = shipmentDataProvider.GetShipmentById(shipmentId);
                    if (shipmentTemp == null) { System.Console.WriteLine($"\nShipment with ID [{shipmentId}] not found. Update operation aborted."); }
                    
                    DateTime oldDate = shipmentTemp.Date;
                    
                    System.Console.Write("Enter new Shipment Date (yyyy-mm-dd): ");
                    shipmentDate = DateTime.Parse(System.Console.ReadLine());
                    
                    DateTime newDate = shipmentDate;
                    
                    var find1 = shipmentDataProvider.GetShipmentByDateAndId(shipmentTemp.PirateShipId, newDate);
                    if (find1 != null)
                    {
                        System.Console.WriteLine($"\nA shipment with PirateShip ID [{shipmentTemp.PirateShipId}] already exists for the date [{newDate:yyyy-MM-dd}]. Shipment ID: [{find1.Id}] is already recorded in the database.\n");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    shipmentService.UpdateShipment(shipmentId, newDate);
                    
                    System.Console.WriteLine($"\nShipment with Shipment ID [{shipmentId}] has been successfully updated:\nDate updated from [{oldDate:yyyy-MM-dd}] to [{newDate:yyyy-MM-dd}].");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear();
                    break;

                case "4":
                    // Rakomány módosítása
                    System.Console.Write("Enter CargoId: ");
                    int cargoId = int.Parse(System.Console.ReadLine());

                    var cargoToUpdate = cargoDataProvider.GetCargoById(cargoId); 
                    if (cargoToUpdate == null)
                    {
                        System.Console.WriteLine($"\nCargo with ID [{cargoId}] not found. Update operation aborted.\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }

                    string oldType = cargoToUpdate.Type;
                    int oldQuantity = cargoToUpdate.Quantity;
                    decimal oldValue = cargoToUpdate.Value;

                    System.Console.Write("Enter new Cargo Type: ");
                    string newType = System.Console.ReadLine();

                    System.Console.Write("Enter new Cargo Quantity: ");
                    int newQuantity = int.Parse(System.Console.ReadLine());

                    System.Console.Write("Enter new Cargo Value: ");
                    decimal newValue = decimal.Parse(System.Console.ReadLine());

                    // Kapacitásellenőrzés
                    var shipmentForCargo = shipmentDataProvider.GetShipmentById(cargoToUpdate.ShipmentId);
                    if (shipmentForCargo == null)
                    {
                        System.Console.WriteLine($"\nError: Shipment with ID [{cargoToUpdate.ShipmentId}] not found. Update operation aborted.\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }

                    var pirateShipForCargo = pirateshipDataProvider.GetById(shipmentForCargo.PirateShipId); 
                    if (pirateShipForCargo == null)
                    {
                        System.Console.WriteLine($"\nError: PirateShip associated with Shipment ID [{shipmentForCargo.Id}] not found. Update operation aborted.\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    int currentQuantityExcludingCurrentCargo = shipmentForCargo.Cargos
                        .Where(c => c.Id != cargoId) 
                        .Sum(c => c.Quantity);

                    if (currentQuantityExcludingCurrentCargo + newQuantity > pirateShipForCargo.Capacity)
                    {
                        System.Console.WriteLine($"\nError: Updating this cargo would exceed the capacity of PirateShip '{pirateShipForCargo.Name}' (Capacity: {pirateShipForCargo.Capacity}, Current: {currentQuantityExcludingCurrentCargo}).\n");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    cargoService.UpdateCargo(cargoId, newType, newQuantity, newValue);

                    System.Console.WriteLine($"\nCargo with Cargo ID [{cargoId}] has been successfully updated:\nType updated from [{oldType}] to [{newType}]\nQuantity from [{oldQuantity}] to [{newQuantity}]\nValue from [{oldValue:C}] to [{newValue:C}].");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey();
                    System.Console.Clear();
                    break;

                case "5":
                    // Szállítmány törlése
                    System.Console.Write("Enter ShipmentId: ");
                    shipmentId = int.Parse(System.Console.ReadLine());
                    
                    var shipment1 = shipmentDataProvider.GetShipmentById(shipmentId);

                    if (shipment1 == null)
                    {
                        System.Console.WriteLine($"\nShipment with ID [{shipmentId}] not found.");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    shipmentService.DeleteShipment(shipmentId);
                    
                    System.Console.WriteLine($"\nShipment with ID [{shipmentId}] has been successfully deleted.");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;

                case "6":
                    // Rakomány törlése
                    System.Console.Write("Enter CargoId: ");
                    cargoId = int.Parse(System.Console.ReadLine());
                    
                    var cargo2 = cargoDataProvider.GetCargoById(cargoId);
                    if (cargo2 == null)
                    {
                        System.Console.WriteLine($"\nCargo with ID [{cargoId}] not found.");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    cargoService.DeleteCargo(cargoId);
                    
                    System.Console.WriteLine($"\nCargo with ID [{cargoId}] has been successfully deleted.");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;
                
                case "7":
                    var allShipments = shipmentService.GetAllShipments();
                    
                    if (allShipments == null)
                    {
                        System.Console.WriteLine($"\nNo shipments found.");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    System.Console.WriteLine($"\nRetrieved all shipments: {allShipments.Count()} shipments found.\n");
                    
                    foreach (var shipment in allShipments)
                    {
                        System.Console.WriteLine($"Shipment ID: {shipment.Id} | Date: {shipment.Date}");
                    }
                    
                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;
                
                case "8":
                    var allCargos = cargoService.GetAllCargos();

                    if (allCargos == null)
                    {
                        System.Console.WriteLine($"\nNo cargos found.");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    System.Console.WriteLine($"\nRetrieved all cargos: {allCargos.Count()} cargo items found.\n");
                    
                    foreach (var cargo in allCargos)
                    {
                        System.Console.WriteLine($"Cargo ID: {cargo.Id} | Type: {cargo.Type} | Quantity: {cargo.Quantity}");
                    }
                    
                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;
                
                case "9":
                    var allPirateShips = pirateShipService.GetAllPirateShips();

                    if (allPirateShips == null)
                    {
                        System.Console.WriteLine($"\nNo pirate ships found.");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    System.Console.WriteLine($"\nRetrieved all pirate ships: {allPirateShips.Count()} ships items found.\n");
                    
                    foreach (var pirate in allPirateShips)
                    {
                        System.Console.WriteLine($"Pirate Ship ID: {pirate.Id} | Name: {pirate.Name} | Captain Name: {pirate.CaptainName} | Capacity: {pirate.Capacity}");
                    }
                    
                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;
                
                case "10":
                    // Lekérjük az összes PirateShip-et az aktuális állapotuk szerint
                    var allPirateShips2 = pirateShipService.GetAllPirateShips();

                    if (!allPirateShips2.Any()) // Ellenőrizzük, hogy van-e adat
                    {
                        System.Console.WriteLine("\nNo pirate ships found.");
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }

                    System.Console.WriteLine($"\nRetrieved all data: {allPirateShips2.Count()} items found.");

                    foreach (var pirate in allPirateShips2)
                    {
                        System.Console.WriteLine($"\nPirate Ship ID: {pirate.Id} | Name: {pirate.Name} | Captain Name: {pirate.CaptainName} | Capacity: {pirate.Capacity}");
                        foreach (var ships in pirate.Shipments)
                        {
                            System.Console.WriteLine($"\tShipment ID: {ships.Id} | Date: {ships.Date} | IsDelayed: {ships.IsDelayed}");
                            foreach (var cargo in ships.Cargos)
                            {
                                System.Console.WriteLine($"\t\tCargo ID: {cargo.Id} | Type: {cargo.Type} | Quantity: {cargo.Quantity} | Value: {cargo.Value}");
                            }
                        }
                    }

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey();
                    System.Console.Clear();
                    break;
                    

                case "11":
                    System.Console.Write("Enter PirateShipId for Shipment Report: ");
                    shipId = int.Parse(System.Console.ReadLine());

                    if (pirateshipDataProvider.GetById(shipId) == null)
                    {
                        System.Console.WriteLine("\nError: PirateShip not found\n");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    reportService.GenerateShipmentReport(shipId);
                    
                    var reportDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Reports", pirateshipDataProvider.GetById(shipId).Name);
                    var reportPath = Path.Combine(reportDirectory, "ShipmentReport.xml");
                    
                    System.Console.WriteLine($"Shipment report saved to {reportPath}");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;

                case "12":
                    System.Console.Write("Enter PirateShipId for Capacity Utilization Report: ");
                    shipId = int.Parse(System.Console.ReadLine());

                    if (pirateshipDataProvider.GetById(shipId) == null)
                    {
                        System.Console.WriteLine("\nError: PirateShip not found.\n");
                        
                        System.Console.WriteLine("Try again [press any key to continue]");
                        System.Console.ReadLine();
                        System.Console.Clear();
                        break;
                    }
                    
                    reportService.GenerateCapacityUtilizationReport(shipId);
                    
                    var reportDirectory1 = Path.Combine(Directory.GetCurrentDirectory(), "Reports", pirateshipDataProvider.GetById(shipId).Name);
                    var reportPath1 = Path.Combine(reportDirectory1, "CapacityUtilizationReport.xml");
                    
                    System.Console.WriteLine($"Capacity utilization report saved to {reportPath1}");

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear();
                    break;

                case "13":
                    System.Console.Write("Enter PirateShipId for Reflection Report: ");
                    shipId = int.Parse(System.Console.ReadLine());
                    
                    using (var context = serviceProvider.GetService<AppDbContext>())
                    {
                        var ship = context.PirateShips.FirstOrDefault(p => p.Id == shipId);
                        if (ship != null) 
                        {
                            var reportDirectory2 = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "PirateShip");  
                            var reportPath2 = Path.Combine(reportDirectory2, $"PirateShipReflectionReport.xml");
                            
                            reportService.GenerateReportWithReflection(ship, "PirateShipReflectionReport");
                            
                            System.Console.WriteLine($"Reflection report generated and saved to {reportPath2}"); 
                        }
                        else 
                        {
                            System.Console.WriteLine("\nError: PirateShip not found.\n");
                            
                            System.Console.WriteLine("Try again [press any key to continue]");
                            System.Console.ReadLine();
                            System.Console.Clear();
                            break;
                        }
                    }

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear(); 
                    break;

                case "14":
                    System.Console.Write("Enter Cargo Type to search: ");
                    string cargoType = System.Console.ReadLine();
                    
                    var results = cargoDataProvider.SearchByCargoType(cargoType);

                    if (results.Count == 0) 
                    {
                        System.Console.WriteLine("No shipments found with the specified cargo type.");
                    }
                    else 
                    {
                        foreach (var result in results.Distinct()) 
                        {
                            System.Console.WriteLine($"Ship: {result.ShipName}, Date: {result.ShipmentDate}, Quantity: {result.Quantity}");
                        }
                    }

                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey(); 
                    System.Console.Clear();
                    break;

                case "15":
                    // Exiting console
                    System.Console.Write("\nExiting the application");
                    Thread.Sleep(200);
                    System.Console.Write(".");
                    Thread.Sleep(200);
                    System.Console.Write(".");
                    Thread.Sleep(200);
                    System.Console.Write(".\n");
                    running = false;
                    break;
            }
        }
    }
}