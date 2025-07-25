namespace schedule_evaluator;

public class Transaction
{
    public string Id { get; set; } = "";
    public int TS { get; set; } = 0;

    public Transaction(string id)
    {
        Id = id;
    }
}