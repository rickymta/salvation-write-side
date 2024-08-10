using System.Text.Json;
using System.Text.RegularExpressions;

namespace Salvation.WriteSide.Commons;

public partial class StringProvider
{
    public static string FormatNumber(object number)
    {
        return $"{number:n0}";
    }
    public static string GenerateOTP(int length = 6)
    {
        var chars1 = "1234567890";
        var stringChars1 = new char[length];
        var random1 = new Random();

        for (int i = 0; i < stringChars1.Length; i++)
        {
            stringChars1[i] = chars1[random1.Next(chars1.Length)];
        }

        return new string(stringChars1);
    }
    public static string SerializeUtf8<T>(T data)
    {
        return JsonSerializer.Serialize(data, typeof(T), new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
    }
    public static bool CheckNumberic(string text)
    {
        return double.TryParse(text, out _);
    }
    public static string FormatPhoneNumber(string phoneNumber)
    {
        phoneNumber = phoneNumber.Replace(" ", "")
            .Replace(".", "")
            .Replace(",", "");

        if (!CheckNumberic(phoneNumber)) return phoneNumber.ToLower();

        if (phoneNumber.StartsWith("84") && phoneNumber.Length == 11) phoneNumber = "0" + phoneNumber[2..];
        else if (!phoneNumber.StartsWith("0") && phoneNumber.Length == 9) phoneNumber = "0" + phoneNumber;
        return phoneNumber.ToLower();
    }
    public static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public static bool CheckNomalText(string text)
    {
        var regexItem = MyRegex();

        return regexItem.IsMatch(text);
    }
    public static long? GetNumberFromText(string text, string preText, string nextText, List<string>? replace = null)
    {
        replace ??= [",", "."];

        foreach (var item in replace)
        {
            text = text.Replace(item, "");
        }
        var pattern = $@"{preText}([0-9]+){nextText}";
        Match m = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
        if (m.Success)
        {
            var numberString = m.Value.Replace(preText, "").Replace(nextText, "").Trim();

            return Convert.ToInt64(numberString);
        }

        return null;
    }
    public static string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return string.Concat(text.AsSpan(0, pos), replace, text.AsSpan(pos + search.Length));
    }
    [GeneratedRegex("^[a-zA-Z0-9]*$")]
    private static partial Regex MyRegex();
}
