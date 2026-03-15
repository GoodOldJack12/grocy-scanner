using System.Text.RegularExpressions;

namespace GrocyScanner.Core.Validators;

public class GtinValidator : IGtinValidator
{
    private static Regex _gtinRegex = new Regex("^(\\d{8}|\\d{12,14}|010\\d{13})$");
    
    public bool Validate(string barcode)
    {
        barcode = ParseBarcode(barcode);
        
        if (!_gtinRegex.IsMatch(barcode))
        {
            return false;
        }
        
        barcode = barcode.PadLeft(14, '0');
        int sum = barcode.Select((c,i) => (c - '0')  * (i % 2 == 0 ? 3 : 1)).Sum();
        return sum % 10 == 0;
    }

    public string ParseBarcode(string barcode)
    {
        // Parse GS1 barcode and return EAN13
        if (barcode.Length >= 16 && barcode.StartsWith("01"))
        {
            return barcode.Substring(3,13);
        }

        return barcode;
    }
}