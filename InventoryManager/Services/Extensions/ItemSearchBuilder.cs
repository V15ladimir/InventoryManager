using InventoryManager.Models.Entitites.Items;

namespace InventoryManager.Services.Extensions {

    public static class ItemSearchBuilder {
        public static string Build(Item item) {
            var values = item.ItemValues
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .Select(x => x.Value)
                .ToList();
            values.Add(item.CustomId);
            return string.Join(" ", values);
        }
    }
}
