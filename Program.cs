// See https://aka.ms/new-console-template for more information

using System.Globalization;

namespace sort_merge_join
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isLinux = false;
            string rootDirectory = isLinux ? @"/home/vitor/tmp/sort_merge_join/" : @"C:\temp\schedule-evaluator\";
            string in_path  = Path.Combine(Directory.GetCurrentDirectory(), "in.txt");
            string line = "";
            
            //ler o in
            if (!File.Exists(in_path))
                throw new FileNotFoundException($"Arquivo in.txt não encontrado em {in_path}");

            using StreamReader reader = new StreamReader(in_path);
            
            //Lê o nome dos objetos de dados
            line = reader.ReadLine();
            
            
            //Lê os ids das transações
            line = reader.ReadLine();
            
            //Lê os tempos das transações
            line = reader.ReadLine();
            
            //Lê o primeiro escalonamento e a cada linha tem um novo escalonamento  
            while ((line = reader.ReadLine()) != null)
            {
                //Para cada operação 
                // for (int i = 0; i < offSet * 10; i++)
                {
                    line = reader.ReadLine();
                }
            }
            
            
            
        }
    }
}