using Microsoft.AspNetCore.Http;

namespace CommonLibrary.Helpers
{
    public interface IFileHelper
    {
        IFormFile ConvertBase64ToIFormFile(string base64EncodedString, string fileName);
    }

    public class FileHelper : IFileHelper
    {
        public IFormFile ConvertBase64ToIFormFile(string base64EncodedString, string fileName)
        {
            if (string.IsNullOrEmpty(base64EncodedString))
            {
                throw new ArgumentNullException(nameof(base64EncodedString), "Base64 編碼的字串不能為空值。");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), "檔案名稱不能為空值。");
            }

            byte[] fileBytes = Convert.FromBase64String(base64EncodedString);

            // 創建 Memory Stream，並將 Base64 轉換成二進制資料寫入
            MemoryStream memoryStream = new MemoryStream(fileBytes);
            // 創建一個 IFormFile 實例，並返回
            return new FormFile(memoryStream, 0, fileBytes.Length, fileName, fileName);
        }
    }
}
