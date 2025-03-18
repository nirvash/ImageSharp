// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using static SixLabors.ImageSharp.Metadata.Profiles.Exif.EncodedString;

namespace SixLabors.ImageSharp.Metadata.Profiles.Exif;

/// <summary>
/// Extension methods for the <see cref="ExifProfile"/> class.
/// </summary>
public static class ExifProfileExtensions
{
    /// <summary>
    /// Sets a UserComment value with specified character encoding.
    /// </summary>
    /// <param name="profile">The exif profile.</param>
    /// <param name="text">The text to encode.</param>
    /// <param name="encoding">The character encoding to use.</param>
    public static void SetUserComment(this ExifProfile profile, string text, CharacterCode encoding)
    {
        Guard.NotNull(profile);
        Guard.NotNull(text);

        var comment = new EncodedString(encoding, text);
        profile.SetValue(ExifTag.UserComment, comment);
    }

    /// <summary>
    /// Sets a UserComment value with UTF-8 encoding using Unicode character code.
    /// </summary>
    /// <param name="profile">The exif profile.</param>
    /// <param name="text">The text to encode.</param>
    public static void SetUnicodeUserComment(this ExifProfile profile, string text)
        => SetUserComment(profile, text, CharacterCode.Unicode);

    /// <summary>
    /// Sets a UserComment value with UTF-8 encoding using ASCII character code.
    /// </summary>
    /// <param name="profile">The exif profile.</param>
    /// <param name="text">The text to encode.</param>
    public static void SetUtf8UserComment(this ExifProfile profile, string text)
        => SetUserComment(profile, text, CharacterCode.ASCII);

    /// <summary>
    /// Sets a UserComment value with UTF-16BE encoding using Unicode character code.
    /// This method configures the system to use Big Endian Unicode encoding when writing the comment.
    /// </summary>
    /// <param name="profile">The exif profile.</param>
    /// <param name="text">The text to encode.</param>
    public static void SetUtf16BEUserComment(this ExifProfile profile, string text)
    {
        Guard.NotNull(profile);
        Guard.NotNull(text);

        // Unicode文字コードを使用しながらBig Endianエンコーディングを設定する
        ExifEncodedStringHelpers.SetUnicodeBigEndian(true);
        try
        {
            var comment = new EncodedString(CharacterCode.Unicode, text);
            profile.SetValue(ExifTag.UserComment, comment);
        }
        finally
        {
            // 他の処理に影響を与えないようにリセットする
            ExifEncodedStringHelpers.SetUnicodeBigEndian(false);
        }
    }
}
