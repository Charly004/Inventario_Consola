using System.Data;

public static class Printer
{
    public static void Linea(int tamaño)
    {
        Console.WriteLine("".PadLeft(tamaño,'-'));
    }

    public static void TituloMenuPrincipal(int tamaño)
    {
        Console.WriteLine("".PadLeft(tamaño,'-')+" Menu Principal "+"".PadRight(tamaño,'-')+"\n");
    }

    public static void TituloInventario(int tamaño)
    {
        Console.WriteLine("".PadLeft(tamaño,'-')+" Inventario "+"".PadRight(tamaño,'-')+"\n");
    }
}