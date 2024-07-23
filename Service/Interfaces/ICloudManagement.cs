using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICloudManagement
    {
        Task<(string Url, string PublicId)> UploadImageWithPublicIdAsync(Stream fileStream, string fileName);
        Task DeleteImageAsync(string publicId);
    }
}
