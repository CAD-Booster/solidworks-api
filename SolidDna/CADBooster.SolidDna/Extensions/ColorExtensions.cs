using System.Diagnostics.Contracts;

namespace CADBooster.SolidDna;

/// <summary>
/// Convert colors to COLORREF / Windows Color integer values.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Convert a System.Windows.Media.Color (WPF color) to a COLORREF / Windows Color integer value.
    /// Uses r + 256 * g + 65536 * b
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>An integer COLORREF value</returns>
    [Pure]
    public static int ToColorRef(this System.Windows.Media.Color color) => color.R | (color.G << 8) | (color.B << 16);

    /// <summary>
    /// Convert a System.Drawing.Color to a COLORREF / Windows Color integer value.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>An integer COLORREF value.</returns>
    [Pure]
    public static int ToColorRef(this System.Drawing.Color color) => System.Drawing.ColorTranslator.ToWin32(color);
}