namespace PALM.BatchInterfaceTools.API.Helpers.Utilities
{
    public static class FileHelpers
    {
        /// <summary>
        /// Validates input files to make sure they're the correct file type.
        /// </summary>
        /// <param name="file">File to be validated.</param>
        /// <returns>True if the file is valid. False if it is not.</returns>
        public static bool InputFileTypeValidation(IFormFile file)
        {
            // Check file length
            if (file.Length <= 0)
                return false;
            // Check filename extension
            if (!file.FileName.ToLower().EndsWith(".xlsx"))
                return false;
            // Check that it's an Office document by stream contents
            using (var stream = file.OpenReadStream())
            {
                if (!FileTypeChecker.FileTypeValidator.Is<FileTypeChecker.Types.MicrosoftOffice365Document>(stream))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceID"></param>
        /// <param name="agency"></param>
        /// <param name="agencyBusinessSystem"></param>
        /// <returns></returns>
        public static string FileNameGenerator(string interfaceID, string? agency, string? agencyBusinessSystem)
        {
            return $"{agency ?? "Agency"}_{interfaceID}_{agencyBusinessSystem ?? "ABS"}_{DateTime.Now:yyyyMMdd-HHmmss}.txt";
        }
    }
}
