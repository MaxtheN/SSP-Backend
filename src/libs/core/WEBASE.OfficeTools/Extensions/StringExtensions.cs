namespace WEBASE.OfficeTools.Extensions;

public static class StringExtensions
{
    public static string FormattedToString(this object value, string numberFormat = null)
    {
        if (value.GetType() == typeof(DateOnly))
        {
            DateOnly dateOnly = (DateOnly)value;
            return dateOnly.ToString("d.MM.yyyy");
        }

        if (value.GetType() == typeof(DateTime))
        {
            DateTime dateTime = (DateTime)value;
            return dateTime.ToString("d.MM.yyyy HH:mm");
        }

        if (value.GetType() == typeof(int) || value.GetType() == typeof(int?))
        {
            int intValue = (int)value;
            if (numberFormat != null)
                return intValue.ToString(numberFormat);

        }
        return value.ToString();
    }
}
