namespace PALM.DeveloperTools.API.Helpers.Utilities
{
    public static class InputValidation
    {
        public static bool FileValidation(IFormFile file)
        {
            // Check file length
            if (file.Length <= 0)
                return false;
            // Check filename extension
            if (!file.FileName.ToLower().EndsWith(".xlsx"))
                return false;
            // Check true extension based on stream contents
            using (var stream = file.OpenReadStream())
            {
                var fileType = FileTypeChecker.FileTypeValidator.GetFileType(stream);
                if (fileType.Extension != "xlsx")
                    return false;
            }
            return true;
        }
    }
}
