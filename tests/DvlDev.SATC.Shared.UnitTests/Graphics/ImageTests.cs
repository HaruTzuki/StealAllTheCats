using DvlDev.SATC.Shared.Graphics;

public class ImageHelperTests
{
    [Fact]
    public async Task DownloadImageFromUrl_ValidUrlAndDirectory_ReturnsFilePath()
    {
        var url = "https://cdn.pixabay.com/photo/2016/06/14/00/14/cat-1455468_1280.jpg";
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "images"); // Convert to absolute path
        var fileName = "testImage";
        
        // Act
        await ImageHelper.DownloadImageFromUrl(url, directory, null, fileName);
        var expectedPath = Path.Combine(directory, fileName + Path.GetExtension(url)); // Expected absolute path

        // Normalize and check existence
        Assert.True(File.Exists(expectedPath), $"File not found at: {expectedPath}");

        // Cleanup
        File.Delete(expectedPath);
    }

    [Fact]
    public async Task DownloadImageFromUrl_EmptyUrl_ThrowsArgumentException()
    {
        var directory = "images";
        var fileName = "testImage";

        await Assert.ThrowsAsync<ArgumentException>(() => ImageHelper.DownloadImageFromUrl(string.Empty, directory, null, fileName));
    }

    [Fact]
    public async Task DownloadImageFromUrl_EmptyDirectory_ThrowsArgumentException()
    {
        var url = "https://cdn.pixabay.com/photo/2016/06/14/00/14/cat-1455468_1280.jpg";
        var fileName = "testImage";

        await Assert.ThrowsAsync<ArgumentException>(() => ImageHelper.DownloadImageFromUrl(url, string.Empty, null, fileName));
    }

    [Fact]
    public void DeleteImage_ValidImagePath_ReturnsTrue()
    {
        var filePath = "testImage.jpg";
        File.WriteAllText(filePath, "test content");

        var result = ImageHelper.DeleteImage(filePath);

        Assert.True(result);
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public void DeleteImage_InvalidImagePath_ReturnsFalse()
    {
        var filePath = "nonExistentImage.jpg";

        var result = ImageHelper.DeleteImage(filePath);

        Assert.False(result);
    }

    [Fact]
    public void DeleteBulkImages_ValidImagePaths_ReturnsTrue()
    {
        var filePath1 = "testImage1.jpg";
        var filePath2 = "testImage2.jpg";
        File.WriteAllText(filePath1, "test content");
        File.WriteAllText(filePath2, "test content");

        var result = ImageHelper.DeleteBulkImages(new[] { filePath1, filePath2 });

        Assert.True(result);
        Assert.False(File.Exists(filePath1));
        Assert.False(File.Exists(filePath2));
    }

    [Fact]
    public void DeleteBulkImages_InvalidImagePaths_ReturnsFalse()
    {
        var filePath1 = "nonExistentImage1.jpg";
        var filePath2 = "nonExistentImage2.jpg";

        var result = ImageHelper.DeleteBulkImages(new[] { filePath1, filePath2 });

        Assert.False(result);
    }
}