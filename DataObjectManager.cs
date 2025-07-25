namespace schedule_evaluator;

public class DataObjectManager
{
    public string Id { get; set; } = "";
    public List<DataObject> DataObjects {get; set;} = new List<DataObject>();
    public string dataObjectPath { get; set; } = "";

    public void ClockIn(string dataObjectID, int newTS, string operation)
    {
        if (operation == "r")
            DataObjects[int.Parse(dataObjectID.Substring(1)) -1].ReadTS = newTS;
        else if (operation == "w")
            DataObjects[int.Parse(dataObjectID.Substring(1)) -1].WriteTS = newTS;
        
    }

    public void AddDataObject(string dataID)
    {
        DataObjects.Add(new DataObject(dataID));
    }

    public void ResetClock()
    {
        foreach (DataObject dataObject in DataObjects)
        {
            dataObject.ResetTS();
        }
    }

    public string DisplayStatus()
    {
        string status = "";
        foreach (DataObject dataObject in DataObjects)
        {
            status += dataObject.ActualStatus() + "\n";
        }
        return status;
    }
}