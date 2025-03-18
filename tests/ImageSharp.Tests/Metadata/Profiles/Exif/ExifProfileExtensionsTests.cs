// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System.Text;
using static SixLabors.ImageSharp.Metadata.Profiles.Exif.EncodedString;
using System.Buffers.Binary;
using System.Text;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Tests.TestUtilities;
namespace SixLabors.ImageSharp.Tests.Metadata.Profiles.Exif;

public class ExifProfileExtensionsTests
{
    [Theory]
    [InlineData("こんにちは世界", CharacterCode.Unicode)]
    [InlineData("Hello, World!", CharacterCode.ASCII)]
    [InlineData("Mixed: こんにちは!", CharacterCode.Unicode)]
    public void SetUserComment_ShouldUseSpecifiedEncoding(string text, CharacterCode encoding)
    {
        // Arrange
        var profile = new ExifProfile();

        // Act
        profile.SetUserComment(text, encoding);

        // Assert
        var value = profile.GetValue(ExifTag.UserComment);
        Assert.NotNull(value);

        var comment = Assert.IsType<ExifEncodedString>(value).Value;
        Assert.Equal(text, comment.Text);
        Assert.Equal(encoding, comment.Code);
    }

    [Theory]
    [InlineData("日本語テキスト")]
    [InlineData("Mixed Text 漢字")]
    public void SetUnicodeUserComment_ShouldUseUnicodeEncoding(string text)
    {
        // Arrange
        var profile = new ExifProfile();

        // Act
        profile.SetUnicodeUserComment(text);

        // Assert
        var value = profile.GetValue(ExifTag.UserComment);
        Assert.NotNull(value);

        var comment = Assert.IsType<ExifEncodedString>(value).Value;
        Assert.Equal(text, comment.Text);
        Assert.Equal(CharacterCode.Unicode, comment.Code);
    }

    [Theory]
    [InlineData("UTF8テキスト")]
    [InlineData("Mixed UTF8 Text 漢字")]
    public void SetUtf8UserComment_ShouldUseAsciiCodeWithUtf8Encoding(string text)
    {
        // Arrange
        var profile = new ExifProfile();

        // Act
        profile.SetUtf8UserComment(text);

        // Assert
        var value = profile.GetValue(ExifTag.UserComment);
        Assert.NotNull(value);

        var comment = Assert.IsType<ExifEncodedString>(value).Value;
        Assert.Equal(text, comment.Text);
        Assert.Equal(CharacterCode.ASCII, comment.Code);

        // Verify the actual bytes are UTF-8 encoded
        var buffer = new byte[1024];
        int bytesWritten = ExifEncodedStringHelpers.Write(comment, buffer);
        var contentBytes = buffer.AsSpan(ExifEncodedStringHelpers.CharacterCodeBytesLength,
            bytesWritten - ExifEncodedStringHelpers.CharacterCodeBytesLength).ToArray();
        string decodedText = Encoding.UTF8.GetString(contentBytes);
        Assert.Equal(text, decodedText);
    }
}
