namespace schedule_evaluator;

public class DataObject
{
    public string Id { get; set; } = "";
    public int ReadTS { get; set; } = 0;
    public int WriteTS { get; set; } = 0;

    public DataObject(string id)
    {
        Id = id;
    }
    
    public void ResetTS()
    {
        ReadTS = 0;
        WriteTS = 0;
    }

    public string GetFileName()
    {
        return $"{Id}-out.txt";
    }

    public string ActualStatus()
    {
        return $"ObjectID: {Id} | ReadTS: {ReadTS} | WriteTS: {WriteTS}";
    }
}