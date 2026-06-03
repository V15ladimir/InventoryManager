using System.Text;
using Dropbox.Api;
using Dropbox.Api.Files;
using InventoryManager.Integration.PowerAutomate.Models;
using Microsoft.Extensions.Options;

namespace InventoryManager.Integration.PowerAutomate.Services {

    public class DropboxService(IOptions<DropboxOptions> options) : IDropBoxService {

        public async Task UploadFileAsync(string content, string fileName) {
            using var dbx = new DropboxClient(
                oauth2RefreshToken: options.Value.RefreshToken, 
                appKey: options.Value.AppKey, 
                appSecret: options.Value.AppSecret);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await dbx.Files.UploadAsync(
                path: $"{options.Value.TargetFolder}/{fileName}", 
                mode: WriteMode.Overwrite.Instance, 
                body:stream
            );
        }
    }
}
