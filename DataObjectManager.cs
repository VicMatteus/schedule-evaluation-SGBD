namespace sort_merge_join;

public class DataObjectManager
{
    public string Id { get; set; } = "";
    public List<DataObject> DataObjects {get; set;} = new List<DataObject>();
    public string dataObjectPath { get; set; } = "";

    public void ClockIn(string dataID, string newTS, string operation)
    {
        
    }

    public void AddDataObject(string dataID)
    {
        DataObjects.Add(new DataObject(dataID));
    }

    public void ResetClock()
    {
        //para cada D.O., resetar os tempos de R e W;
    }

    public void WriteDataObjectOnDisk(string dataObjectLog)
    {
        //gravar (flush) log no caminho raiz + nome do arquivo do d.o.

        // using (var writer = new StreamWriter(outputFile))
        // {
        //     writer.WriteLine(string.Join(",", tuple));
        // }
        
        //ou
        // File.AppendAllText(Path.Combine(RunT1Directory, $"run_0_{run_counter}.txt"), result);
    }

}