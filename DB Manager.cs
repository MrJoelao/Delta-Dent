namespace Delta_Dent;
using MySqlConnector;
using Microsoft.Extensions.Configuration;

public class DbManager
{
    public MySqlConnection Conn { get; private set; }
    private readonly IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    
    public DbManager(string connStr = null)
    {
        Console.WriteLine("Attempting to open connection...");
        OpenConnection(connStr);
    }
    
    public void OpenConnection(string connStr = null)
    {
        try
        {
            connStr ??= _configuration.GetConnectionString("MySqlConnection");
            Conn = new MySqlConnection(connStr);
            Conn.Open(); 
            
            Console.WriteLine("Connection opened successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to open connection: {e.Message}");
        }
        
    }
    
    public async Task OpenConnectionAsync(string connStr = null)
    {
        try
        {
            connStr ??= _configuration.GetConnectionString("MySqlConnection");
            Console.WriteLine(connStr);
            Conn = new MySqlConnection(connStr);
            await Conn.OpenAsync();
            
            Console.WriteLine("Connection async opened successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to open async connection:" + e.Message);
        }
        
    }
    
    public void CloseConnection()
    {
        try
        {
            Conn.Close();
            Console.WriteLine("Connection closed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to close connection:" + e.Message);
        }
    }
    
    public async Task CloseConnectionAsync()
    {
        try
        {
            await Conn.CloseAsync();
            Console.WriteLine("Connection closed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to close connection:" + e.Message);
        }
    }
    
    //Metodo per eseguire una query
    public void MakeQuery(string query)
    {
        Console.WriteLine($"Executing query: {query}");
        MySqlCommand cmd = new MySqlCommand(query, Conn);
        try
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine("Query executed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to execute query:" + e.Message);
        }
    }
    
    public async Task MakeQueryAsync(string query)
    {
        Console.WriteLine($"Executing query: {query}");
        MySqlCommand cmd = new MySqlCommand(query, Conn);
        try
        {
            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("Query executed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to execute query:" + e.Message);
        }
    }
    
    
    
    // Metodo per eseguire una query di tipo SELECT
    public MySqlDataReader ExecuteReader(string query)
    {
        Console.WriteLine($"Executing query: {query}");
        MySqlCommand cmd = new MySqlCommand(query, Conn);
        MySqlDataReader reader = null;
        try
        {
            // Apri la connessione se non è già aperta
            if (Conn.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection not open, opening connection...");
                Conn.OpenAsync();
            }
            
            reader = cmd.ExecuteReader();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to execute query:" + e);
        }
        return reader;
    }
    
    // Metodo per eseguire una query di tipo SELECT
    public async Task<MySqlDataReader> ExecuteReaderAsync(string query)
    {
        Console.WriteLine($"Executing query: {query}");
        MySqlCommand cmd = new MySqlCommand(query, Conn);
        MySqlDataReader reader = null;
        try
        {
            // Apri la connessione se non è già aperta
            if (Conn.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection not open, opening connection...");
                await Conn.OpenAsync();
            }
            
             reader = await cmd.ExecuteReaderAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to execute query:" + e);
        }
        return reader;
    }
    
    // Metodo per chiudere il reader
    public void CloseReader(MySqlDataReader reader)
    {
        try
        {
            reader.Close();
            Console.WriteLine("Reader closed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to close reader:" + e);
        }
    }
    
    public async Task CloseReaderAsync(MySqlDataReader reader)
    {
        try
        {
            await reader.CloseAsync();
            Console.WriteLine("Reader closed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to close reader:" + e);
        }
    }
}