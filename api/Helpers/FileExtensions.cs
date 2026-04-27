namespace api.Helpers;

public static class FileExtensions
{
    private static readonly string[] AllowedExtensions = { ".jpeg", ".jpg", ".png", ".webp" };

    public static bool IsValidImageExtension(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return false;
        var ext = Path.GetExtension(fileName).ToLower();
        return AllowedExtensions.Contains(ext);
    }
}
