using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CADBooster.SolidDna;

/// <summary>
/// Extension methods for strings
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns true if the string is null or an empty string
    /// </summary>
    /// <param name="content">The string</param>
    /// <returns></returns>
    [Pure]
    public static bool IsNullOrEmpty(this string content) => string.IsNullOrEmpty(content);

    /// <summary>
    /// Returns true if the string is null or an empty string or just whitespace
    /// </summary>
    /// <param name="content">The string</param>
    /// <returns></returns>
    [Pure]
    public static bool IsNullOrWhiteSpace(this string content) => string.IsNullOrWhiteSpace(content);

    /// <summary>
    /// Get if a list of strings contains a needle string and ignores the case.
    /// </summary>
    /// <param name="hayStack"></param>
    /// <param name="needle"></param>
    /// <returns></returns>
    [Pure]
    public static bool ContainsIgnoreCase(this List<string> hayStack, string needle) => hayStack.Any(x => x.Equals(needle, StringComparison.InvariantCultureIgnoreCase));
}