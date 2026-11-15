namespace Inventory.Application.Exceptions;

public class DuplicateProductNameException : Exception
{
    public DuplicateProductNameException(string productName)
        : base($"ٍSorry the product name '{productName}' is already in use.")
    {
    }
}