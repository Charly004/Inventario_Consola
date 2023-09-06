using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Extensibility;
public class Metodos
{
    public static string CadenaConexion = "";

    public Metodos(string conexion)
    {
        CadenaConexion = conexion;

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
        Console.WriteLine("4- Actualizar Registro");        
        Console.WriteLine("5- Salir" + "\n");
        Answer = Int32.Parse(Console.ReadLine());

        return Answer;
    }

    public static void ShowRecords()
    {
        Printer.TituloInventario(42);
        int contador = 1;
        Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}\n", "Id", "Codigo", "Descripcion", "Existencia", "Salidas", "Stock");
        foreach (var i in Listas.registros)
        {
            Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}", contador, i.codigoproducto, i.descripcion, i.existencia, i.salida, i.stock);
            contador++;
        }
        Printer.Linea(97);
        Thread.Sleep(4000);
    }

    public static void AddRecords()
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
        
        registro.stock = registro.existencia - registro.salida;

        using (SqlConnection connection = new SqlConnection(CadenaConexion))
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
                Printer.Linea(97);
            }
            connection.Close();
        }
        Thread.Sleep(2000);
    }

    public static void RemoveRecord()
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

        using (SqlConnection connection = new SqlConnection(CadenaConexion))
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
        Thread.Sleep(2000);
    }

    public static void UpdateRecord()
    {
        
        int contador = 1;
        string idUpdate = "";

        string QueryUpdateId = "UPDATE REGISTRO SET Descripcion = @E_Descripcion, ExistenciaInicial = @E_ExistenciaInicial, Salidas = @E_Salidas, Stock = @E_Stock WHERE CodigoProducto = @idUpdate";

        Printer.TituloInventario(42);
        Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}\n", "Id", "Codigo", "Descripcion", "Existencia", "Salidas", "Stock");
        foreach (var i in Listas.registros)
        {
            Console.WriteLine("{0,0}{1,10}{2,30}{3,15}{4,15}{5,15}", contador, i.codigoproducto, i.descripcion, i.existencia, i.salida, i.stock);
            contador++;
        }
        Printer.Linea(97);
        Console.WriteLine("Ingrese el código del producto a actualizar ");
        idUpdate = Console.ReadLine();
        Console.WriteLine("Ingrese la nueva descripción");
        string E_Descripcion = Console.ReadLine();
        Console.WriteLine("Ingrese la nueva existencia");
        int E_ExistenciaInicial = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese la nueva salida");
        int E_Salidas = Int32.Parse(Console.ReadLine());

        int E_Stock = E_ExistenciaInicial - E_Salidas;

        using (SqlConnection connection = new SqlConnection(CadenaConexion))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(QueryUpdateId, connection))
            {
                command.Parameters.AddWithValue("@idUpdate", idUpdate);
                command.Parameters.AddWithValue("@E_Descripcion", E_Descripcion);
                command.Parameters.AddWithValue("@E_ExistenciaInicial", E_ExistenciaInicial);
                command.Parameters.AddWithValue("@E_Salidas", E_Salidas);
                command.Parameters.AddWithValue("@E_Stock", E_Stock);

                Registro registro = Listas.registros.Find(p => p.codigoproducto == idUpdate);

                registro.descripcion = E_Descripcion;
                registro.existencia = E_ExistenciaInicial;
                registro.salida = E_Salidas;
                registro.stock = E_Stock; 

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Registro Actualizado con éxito.");
                }
                else
                {
                    Console.WriteLine("No se encontró ningún registro con el ID especificado.");
                }
            }
        }
        Thread.Sleep(3000);
    }

}