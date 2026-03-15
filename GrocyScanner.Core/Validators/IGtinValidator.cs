namespace GrocyScanner.Core.Validators;

public interface IGtinValidator
{
    public bool Validate(string barcode);

    public string ParseBarcode(string barcode);

    public bool isGS1(string barcode);

    public DateTime? GetExpiryDate(string barcode);
}