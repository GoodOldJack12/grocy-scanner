namespace GrocyScanner.Core.Validators;

public interface IGtinValidator
{
    public bool Validate(string barcode);

    public string ParseBarcode(string barcode);
}