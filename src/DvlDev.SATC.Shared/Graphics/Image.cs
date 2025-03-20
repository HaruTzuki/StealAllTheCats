namespace DvlDev.SATC.Shared.Graphics;

public static class ImageHelper
{
	public static async Task<string> DownloadImageFromUrl(string url, string directory, string? webRoot, string? fileName)
	{
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentException("Value cannot be null or empty.", nameof(url));
		}

		if (string.IsNullOrEmpty(directory))
		{
			throw new ArgumentException("Value cannot be null or empty.", nameof(directory));
		}

		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		// Getting the extension from the image
		var extension = Path.GetExtension(url);
		if (string.IsNullOrEmpty(extension))
		{
			extension = ".jpg";
		}

		// Getting the file name from the url
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = $"{Guid.NewGuid():N}{extension}";
		}
		
		var filePath = Path.Combine(webRoot??"", directory, $"{fileName}{extension}");
		using var httpClient = new HttpClient();
		var imageBytes = await httpClient.GetByteArrayAsync(url);
		
		await File.WriteAllBytesAsync(filePath, imageBytes);
		
		return $"/{directory}/{fileName}{extension}";
	}

	public static bool DeleteImage(string imagePath)
	{
		if (string.IsNullOrEmpty(imagePath))
		{
			throw new ArgumentException("Value cannot be null or empty.", nameof(imagePath));
		}

		if (!File.Exists(imagePath))
		{
			return false;
		}
		
		File.Delete(imagePath);
		return true;
	}
	
	public static bool DeleteBulkImages(IEnumerable<string> imagePaths)
	{
        ArgumentNullException.ThrowIfNull(imagePaths);

        var result = true;
		foreach (var imagePath in imagePaths)
		{
			if (!DeleteImage(imagePath))
			{
				result = false;
			}
		}

		return result;
	}
}