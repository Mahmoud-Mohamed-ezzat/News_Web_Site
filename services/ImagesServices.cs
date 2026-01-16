using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Schema;
using System.IO;
namespace News_App.services
{
    public class ImagesServices
    {
        public async Task<List<string>> CreateImages(IFormFileCollection images)
        {
            if (images == null) return null;
            var imagesList = new List<string>();
            string[] AllowedFileExtensions = [".jpg", ".jpeg", ".png"];
            var path = @"F:\News App\Images";
            foreach (var image in images)
            {
                if (!AllowedFileExtensions.Contains(Path.GetExtension(image.FileName)))
                {
                    return ["the ALLowed extension is .jpg, .jpeg, .png"];
                }
                if (image.Length > (1024 * 1024))
                {
                    return ["the ALLowed file size 1MB"];
                }
                var filename = $"{Guid.NewGuid().ToString()}_{Path.GetFileNameWithoutExtension(image.FileName)}{Path.GetExtension(image.FileName)}";
                var fullpath = Path.Combine(path, filename);

                using var stream = new FileStream(fullpath, FileMode.Create);
                await image.CopyToAsync(stream);
                imagesList.Add(fullpath);
            }
            return imagesList;
        }
        public async Task<bool> DeleteImages(List<string> images)
        {
            if (images == null) return false;
            foreach (var image in images)
            {
                if (File.Exists(image))
                {
                    File.Delete(image);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}