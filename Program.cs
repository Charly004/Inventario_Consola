﻿using System;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=LAPTOP-59IDB8MH;Initial Catalog=Inventario;Integrated Security=True; Trust Server Certificate=true";

            int MenuSelected = 0;
            do
            {
                MenuSelected = Metodos.MainMenu();
                if ((Menu)MenuSelected == (Menu.READ))
                {
                    Metodos.ShowRecords(connectionString);

                }else if((Menu)MenuSelected == (Menu.CREATE))
                {
                    Metodos.AddRecords(connectionString);
                }else if((Menu)MenuSelected == (Menu.DELETE))
                {
                    Metodos.RemoveRecord();
                }
                

            } while (MenuSelected <= 3);
            Console.WriteLine("Gracias por su tiempo");
        }
    }

    public enum Menu
    {
        READ = 1,
        CREATE = 2,
        DELETE = 3,
        EXIT = 4
    }
}