using System.Text.RegularExpressions;

public static class Helper 
{
    public static int NumberExtractor(string text)
    {
        Regex regex = new Regex(@"\d+");
        Match match = regex.Match(text);

        if (match.Success) {
            return int.Parse(match.Value);
        } else {
            return -1; 
        }
    }
}
