// Omar Gutierrez / 2021-1106 

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// Definimos la clase Hurricane que representa los registros de huracanes
public class Hurricane
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Category { get; set; }
    public string Type { get; set; }
    public double Speed { get; set; }

}

// Definimos el contexto de la base de datos utilizando Entity Framework Core
public class HurricaneContext : DbContext
{
    public DbSet<Hurricane> Hurricanes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=OMAR-PC;Database=HurricaneDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

// Creamos la clase Program que contiene la lógica de la aplicación
class Program
{
    static void Main(string[] args)
    {
        // Creamos una instancia del contexto de la base de datos
        using (var context = new HurricaneContext())
        {
            // Pedimos al usuario que ingrese los datos del nuevo huracán
            Console.Write("Ingrese el nombre del huracán: ");
            string name = Console.ReadLine();

            Console.Write("Ingrese la fecha de inicio del huracán (formato MM/dd/yyyy): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Ingrese la fecha de fin del huracán (formato MM/dd/yyyy): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Ingrese la categoría del huracán (1-5): ");
            int category = int.Parse(Console.ReadLine());

            Console.Write("Ingrese el tipo de huracán (tormenta o huracán): ");
            string type = Console.ReadLine();

            Console.Write("Ingrese la velocidad del huracán en millas por hora: ");
            double speed = double.Parse(Console.ReadLine());



            // Agregamos un nuevo registro de huracán a la base de datos con los datos ingresados por el usuario
            var newHurricane = new Hurricane
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                Category = category,
                Type = type,
                Speed = speed
            };
            context.Hurricanes.Add(newHurricane);
            context.SaveChanges();


            Console.WriteLine("Seleccione 1 para ver el registro completo");
            Console.WriteLine("Seleccione 2 para actualizar el registro");
            Console.WriteLine("Seleccione 3 para eliminar elementos del registro");
            int Selector = int.Parse(Console.ReadLine());


            if (Selector == 1)
            {
                // Leemos todos los registros de huracanes en la base de datos y los imprimimos en la consola
                var hurricanes = context.Hurricanes.ToList();
                foreach (var hurricane in hurricanes)
                {
                    Console.WriteLine($"{hurricane.Name} ({hurricane.Type}): {hurricane.StartDate} - {hurricane.EndDate}, categoría {hurricane.Category}, velocidad {hurricane.Speed} mph");
                }
            }

            if (Selector == 2)
            {
                // Actualizamos un registro de huracán existente en la base de datos
                // Pedimos al usuario el nombre del huracán que desea modificar
                Console.WriteLine("Ingrese el nombre del huracán que desea modificar:");
                string hurricaneName = Console.ReadLine();

                // Buscamos el huracán que coincide con el nombre ingresado por el usuario
                var existingHurricane = context.Hurricanes.FirstOrDefault(h => h.Name == hurricaneName);

                if (existingHurricane == null)
                {
                    Console.WriteLine($"No se encontró el huracán '{hurricaneName}'.");
                }
                else
                {
                    // Pedimos al usuario qué campo desea modificar y cuál será su nuevo valor
                    Console.WriteLine("¿Qué campo desea modificar? (Name, Year, Category, Mortality, Damage)");
                    string fieldToModify = Console.ReadLine();

                    Console.WriteLine($"Ingrese el nuevo valor para '{fieldToModify}':");
                    string newValue = Console.ReadLine();

                    // Modificamos el campo correspondiente del huracán y guardamos los cambios en la base de datos
                    switch (fieldToModify)
                    {
                        case "Name":
                            existingHurricane.Name = newValue;
                            break;
                        case "Category":
                            existingHurricane.Category = int.Parse(newValue);
                            break;
                        case "Type":
                            existingHurricane.Type = newValue;
                            break;

                        case "Speed":
                            existingHurricane.Speed = int.Parse(newValue);
                            break;

                        default:
                            Console.WriteLine($"El campo '{fieldToModify}' no es válido.");
                            break;
                    }

                    context.SaveChanges();

                    Console.WriteLine($"El huracán '{hurricaneName}' fue modificado exitosamente.");
                }
            }

            if (Selector == 3)
            {
                Console.WriteLine("Ingrese el nombre del huracán a eliminar:");
                var nameToDelete = Console.ReadLine();

                // Buscamos el huracán en la base de datos y lo eliminamos si lo encontramos
                var hurricaneToDelete = context.Hurricanes.FirstOrDefault(h => h.Name == nameToDelete);
                if (hurricaneToDelete != null)
                {
                    context.Hurricanes.Remove(hurricaneToDelete);
                    context.SaveChanges();
                    Console.WriteLine($"El huracán {nameToDelete} fue eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine($"El huracán {nameToDelete} no fue encontrado en la base de datos.");
                }
            }
        }
    }
}