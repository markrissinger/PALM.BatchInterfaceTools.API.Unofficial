namespace PALM.DeveloperTools.API.Helpers.Utilities
{
    public static class FileHelpers
    {
        public static bool InputFileTypeValidation(IFormFile file)
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

        public static string FileNameGenerator(string interfaceID, string? agency, string? agencyBusinessSystem)
        {
            return $"{agency ?? "Agency"}_{interfaceID}_{agencyBusinessSystem ?? "ABS"}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt";
        }
    }
}
