using System.Globalization;
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
    
    public bool isGS1(string barcode)
    {
        return barcode.Length >= 24 && barcode.StartsWith("01") && barcode.Substring(16, 2) == "17";
    }
    
    public DateTime? GetExpiryDate(string barcode)
    {
        if (!isGS1(barcode))
            return null;

        string dateStr = barcode.Substring(18, 6);
    
        if (dateStr.EndsWith("00"))
            dateStr = dateStr.Substring(0, 4) + "01"; // parse as 1st, then get end of month
    
        var date = DateTime.ParseExact(dateStr, "yyMMdd",CultureInfo.InvariantCulture);
    
        if (barcode.Substring(22, 2) == "00")
            date = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    
        return date;
    }
}