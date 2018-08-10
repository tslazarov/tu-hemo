using System.Threading.Tasks;

namespace Hemo.Utilities
{
    public interface IImageExtractor
    {
        Task<string> GetImageAsBase64Url(string accessToken);
    }
}