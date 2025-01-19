﻿using System.Numerics;
using System.Text.RegularExpressions;

namespace NaturalSortSampleNew.Classes;
public static partial class Extensions
{
    public static IEnumerable<T> NaturalOrderBy<T>(this IEnumerable<T> items, Func<T, string> selector, StringComparer stringComparer = null)
    {
        var regex = DigitsRegex();

        int maxDigits = items
            .SelectMany(value => regex.Matches(selector(value))
                .Select(digitChunk => (int?)digitChunk.Value.Length)).Max() ?? 0;

        return items.OrderBy(value => regex.Replace(selector(value), 
            match => match.Value.PadLeft(maxDigits, '0')), 
            stringComparer ?? StringComparer.CurrentCulture);
    }
    
    public static IEnumerable<string> NaturalOrderBy(this IEnumerable<string> me)
    {
        return me.OrderBy(x => NumbersRegex().Replace(x, m => m.Value.PadLeft(50, '0')));
    }

    public static bool IsEven<T>(this T sender) where T : INumber<T>
        => T.IsEvenInteger(sender);

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    private static partial Regex DigitsRegex();
}
