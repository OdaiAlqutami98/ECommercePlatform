namespace Odai.Api.Extension
{
    public static class Extension
    {
        public static async Task<UpladFileModel> UploadFile(IFormFile? ImagePath)
        {
            // Read the file
            var postedFile = ImagePath;
            UpladFileModel upladFileModel = new UpladFileModel();

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
            // Save the file to a specific location
            upladFileModel.FileName = Path.Combine("Images", fileName);
            upladFileModel.ContentType = postedFile.ContentType;
            using (var stream = new FileStream(upladFileModel.FileName, FileMode.Create))
            {
                await postedFile.CopyToAsync(stream);
            }
            return upladFileModel;
        }
        public static byte[] GetImageFile(string filePath)
        {
            var imagePath = Path.Combine(filePath); ;
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            var fileName = System.IO.Path.GetFileName(imagePath);
            return imageBytes;
        }
    }
    public class UpladFileModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}

