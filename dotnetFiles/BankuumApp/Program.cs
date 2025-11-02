using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Npgsql;

class Program
{
    static void Main()
    {
        // Mettete il vostro User e la vostra password il resto uguale
        string host = "bankuumtubo-bankuumtubo.f.aivencloud.com";
        int port = 28017;
        string database = "bankuumdb";
        string username = "avnadmin";
        string password = "AVNS_u-FjY5aszGHtMkGPQmY";

        string connString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;";

        try
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                Console.WriteLine("Connected to PostgreSQL!");

                //Fate la query qua
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO clients (first_name, last_name, email, phone) VALUES (@first, @last, @email, @phone)", conn))
                {
                    
                    cmd.Parameters.AddWithValue("first", "Federico");
                    cmd.Parameters.AddWithValue("last", "Bonezzi");
                    cmd.Parameters.AddWithValue("email", "bonni.smith@example.com");
                    cmd.Parameters.AddWithValue("phone", "+41 79 123 45 67");

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Inserted {rowsAffected} row(s) into clients table.");
                }

                
                using (var cmd = new NpgsqlCommand("SELECT client_id, first_name, last_name, email, phone FROM clients", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nClients in DB:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)} {reader.GetString(2)}, Email: {reader.GetString(3)}, Phone: {reader.GetString(4)}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}

