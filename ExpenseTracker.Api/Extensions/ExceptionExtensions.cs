namespace ExpenseTracker.Api.Extentions;

public static class ExceptionExtensions
{
    public static List<string> GetAllMessages(this Exception ex)
    {
        var messages = new List<string> { ex.Message };

        var inner = ex.InnerException;
        while (inner != null)
        {
            messages.Add(inner.Message);
            inner = inner.InnerException;
        }

        return messages;
    }
}