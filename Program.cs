// See https://aka.ms/new-console-template for more information

using System.Globalization;

namespace schedule_evaluator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Setar variável "isLinux" e criar as pastas nos caminhos informados
            bool isLinux = false;
            string rootDirectory = isLinux ? @"/home/vitor/tmp/sort_merge_join/" : @"C:\temp\schedule-evaluator\";
            string in_path = Path.Combine(rootDirectory, "in.txt");
            string dataObjectsLogPath = Path.Combine(rootDirectory, "data_objects_log");
            string line = "";
            string actions = "";
            string[] partLine;
            List<Transaction> transactions = new List<Transaction>();
            string actualSchedule = "";

            //ler o in
            if (!File.Exists(in_path))
                throw new FileNotFoundException($"Arquivo in.txt não encontrado em {in_path}");

            using StreamReader reader = new StreamReader(in_path);

            DataObjectManager DOM = new DataObjectManager();
            DOM.dataObjectPath = rootDirectory;
            Directory.CreateDirectory(dataObjectsLogPath);

            //Lê o nome dos objetos de dados
            line = reader.ReadLine()!.Replace(";", "");
            partLine = line.Split(',');
            foreach (string dataObjectId in partLine)
            {
                DOM.AddDataObject(dataObjectId.Trim());
            }

            //Lê os ids das transações
            line = reader.ReadLine()!.Replace(";", "");
            partLine = line.Split(',');
            foreach (string transactionId in partLine)
            {
                transactions.Add(new Transaction(transactionId.Trim()));
            }

            //Lê os tempos das transações
            line = reader.ReadLine()!.Replace(";", "");
            partLine = line.Split(',');
            for (int i = 0; i < partLine.Length; i++)
            {
                transactions[i].TS = int.Parse(partLine[i]);
            }

            
            // Variáveis para controlar o estado do escalonamento atual
            bool rollbackOcorreu = false;
            string logFinal = "";

            //Lê o primeiro escalonamento e a cada linha tem um novo escalonamento  
            while ((line = reader.ReadLine()) != null)
            {
                // Reinicia o estado para cada novo escalonamento
                rollbackOcorreu = false;
                actualSchedule = line.Substring(0, 3); // Ex: "E_1"
                actions = line.Substring(4); // Ex: "r1(A) r4(A)..."
                partLine = actions.Split(' ');

                // Para cada operação no escalonamento
                for (int momento = 0; momento < partLine.Length; momento++)
                {
                    string acao = partLine[momento];

                    //Se for commit, reseta os tempos dos objetos (libera eles)
                    if (acao.ToLower() == "c")
                    {
                        foreach (var dataObject in DOM.DataObjects)
                        {
                            dataObject.ResetTS();
                        }
                        continue;
                    }

                    // Parse da Ação
                    char operacao = acao[0];
                    string transacaoIdStr = acao.Substring(1, acao.IndexOf('(') - 1);
                    string dataObjectId = acao.Substring(acao.IndexOf('(') + 1, acao.IndexOf(')') - acao.IndexOf('(') - 1);

                    Transaction transacaoAtual = transactions.Find(t => t.Id == $"t{transacaoIdStr}")!;
                    DataObject dataObjectAtual = DOM.DataObjects.Find(d => d.Id.Trim() == dataObjectId.Trim())!;
                    
                    if (operacao == 'r')
                    {
                        // se TS(Tx) < R-TS(dado).TS-Write então
                        if (transacaoAtual.TS < dataObjectAtual.WriteTS)
                        {
                            rollbackOcorreu = true;
                            logFinal = $"{actualSchedule}-ROLLBACK-{momento}";
                            break;
                        }
                        else
                        {
                            // se R-TS(dado).TS-Read < TS(Tx) então
                            if (dataObjectAtual.ReadTS < transacaoAtual.TS)
                            {
                                dataObjectAtual.ReadTS = transacaoAtual.TS;
                            }

                            string logObjeto = $"Escalonamento: {actualSchedule}, Operacao: Read, Momento: {momento}{Environment.NewLine}";
                            File.AppendAllText(Path.Combine(dataObjectsLogPath, $"{dataObjectAtual.GetFileName()}"), logObjeto);
                        }
                    }
                    else if (operacao == 'w')
                    {
                        // se TS(Tx) < R-TS(dado).TS-Read OU TS(Tx) < R-TS(dado).TS-Write então
                        if (transacaoAtual.TS < dataObjectAtual.ReadTS || transacaoAtual.TS < dataObjectAtual.WriteTS)
                        {
                            rollbackOcorreu = true;
                            logFinal = $"{actualSchedule}-ROLLBACK-{momento}";
                            break;
                        }
                        else
                        {
                            dataObjectAtual.WriteTS = transacaoAtual.TS;
                            string logObjeto = $"Escalonamento: {actualSchedule}, Operacao: Write, Momento: {momento}{Environment.NewLine}";
                            File.AppendAllText(Path.Combine(dataObjectsLogPath, $"{dataObjectAtual.GetFileName()}"), logObjeto);
                        }
                    }
                }

                // Finalização do Escalonamento
                // Se o loop terminou sem rollback, o escalonamento é serializável
                if (!rollbackOcorreu)
                {
                    logFinal = $"{actualSchedule}-OK";
                }

                // Escreve o resultado final do escalonamento no arquivo out.txt
                File.AppendAllText(Path.Combine(rootDirectory, "out.txt"), logFinal + Environment.NewLine);

                // Reseta os Timestamps dos objetos de dados para o próximo escalonamento
                foreach (var dataObject in DOM.DataObjects)
                {
                    dataObject.ResetTS();
                }
            }
            
            Console.WriteLine("Avaliação de escalonamentos concluída! Verifique o arquivo out.txt.");
            // Console.WriteLine("Relógio final: \n" + DOM.DisplayStatus());
        }
    }
}