namespace SspUis.Core.Extensions;

public static class PhoneNumberExtensions
{
    public static string NormalizePhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return phoneNumber;

        phoneNumber = phoneNumber.Replace("+", "")
                                 .Replace("-", "")
                                 .Replace("(", "")
                                 .Replace(")", "")
                                 .Replace(" ", "");

        if (phoneNumber.Length == 9)
            phoneNumber = "998" + phoneNumber;

        return phoneNumber;
    }
}
