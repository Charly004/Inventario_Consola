using Microsoft.Data.SqlClient;
public class Metodos
{
    public Metodos(string conexion)
    {
        string consultaSql = "SELECT * FROM REGISTRO";

        using (SqlConnection connection = new SqlConnection(conexion))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(consultaSql, connection))
            {
                using (SqlDataReader lector = command.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Registro registro = new Registro();

                        registro.codigoproducto = lector.GetString(0);
                        registro.descripcion = lector.GetString(1);
                        registro.existencia = lector.GetInt32(2);
                        registro.salida = lector.GetInt32(3);
                        registro.stock = lector.GetInt32(4);

                        Listas.registros.Add(registro);
                    }
                }
            }
            connection.Close();
        }
    }

    public static int MainMenu()
    {
        int Answer = 0;
        Printer.TituloMenuPrincipal(40);
        Console.WriteLine("1- Mostrar Registros");
        Console.WriteLine("2- Insertar Registro");
        Console.WriteLine("3- Eliminar Registro");
        Console.WriteLine("4- Salir" + "\n");
        Answer = Int32.Parse(Console.ReadLine());

        return Answer;
    }

    public static void ShowRecords(string conexion)
    {
        Printer.TituloInventario(42);
        int contador = 1;
        Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}\n", "Id", "Codigo", "Descripcion", "Existencia", "Salidas", "Stock");
        foreach (var i in Listas.registros)
        {
            Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}", contador, i.codigoproducto, i.descripcion, i.existencia, i.salida, i.stock);
            contador++;
        }
        Printer.Linea(100);
        Thread.Sleep(5000);
    }

    public static void AddRecords(string conexion)
    {
        Registro registro = new Registro();
        string insertSql = "insert into Registro (CodigoProducto, Descripcion, ExistenciaInicial, Salidas, Stock) values (@campo1, @campo2, @campo3, @campo4, @campo5)";

        Console.WriteLine("Ingrese el codigo del producto");
        registro.codigoproducto = Console.ReadLine();
        Console.WriteLine("Ingrese la descripcion del producto");
        registro.descripcion = Console.ReadLine();
        Console.WriteLine("Ingrese la existencia inicial");
        registro.existencia = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese las salidas");
        registro.salida = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese el stock disponible");
        registro.stock = Int32.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(conexion))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(insertSql, connection))
            {
                command.Parameters.AddWithValue("@campo1", registro.codigoproducto);
                command.Parameters.AddWithValue("@campo2", registro.descripcion);
                command.Parameters.AddWithValue("@campo3", registro.existencia);
                command.Parameters.AddWithValue("@campo4", registro.salida);
                command.Parameters.AddWithValue("@campo5", registro.stock);

                Listas.registros.Add(registro);

                int filasAfectadas = command.ExecuteNonQuery();

                Console.WriteLine($"Se insertaron {filasAfectadas} filas.");
                Printer.Linea(100);
            }
            connection.Close();
        }
        Thread.Sleep(2000);
    }

    public static void RemoveRecord(string connectionString)
    {
        int contador = 1;
        string idRemove = "";

        string QueryDeleteId = "DELETE FROM REGISTRO WHERE CodigoProducto = @idRemove";

        Printer.TituloInventario(42);
        Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}\n", "Id", "Codigo", "Descripcion", "Existencia", "Salidas", "Stock");
        foreach (var i in Listas.registros)
        {
            Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}", contador, i.codigoproducto, i.descripcion, i.existencia, i.salida, i.stock);
            contador++;
        }
        Printer.Linea(97);
        Console.WriteLine("Ingrese el código del producto a eliminar ");
        idRemove = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(QueryDeleteId, connection))
            {
                command.Parameters.AddWithValue("@idRemove", idRemove);

                int rowsAffected = command.ExecuteNonQuery();
                Registro registro = Listas.registros.Find(p => p.codigoproducto == idRemove);
                Listas.registros.Remove(registro);

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Registro eliminado con éxito.");
                }
                else
                {
                    Console.WriteLine("No se encontró ningún registro con el ID especificado.");
                }
            }
        }
    }

}