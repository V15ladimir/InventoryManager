namespace InventoryManager.Integration.PowerAutomate.Services {

    public interface IDropBoxService {
        Task UploadFileAsync(string content, string fileName);
    }
}
