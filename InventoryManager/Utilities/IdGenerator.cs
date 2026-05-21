namespace InventoryManager.Utilities {

    public static class IdGenerator {

        public static string GetRandom20Bit(bool useHex = false, int? width = null) {
            var value = Random.Shared.Next(0, 1 << 20);
            return useHex ? 
                value.ToString($"X{width ?? 5}") : 
                value.ToString($"D{width ?? 5}");
        }

        public static string GetRandom32Bit(bool useHex = false, int? width = null) {
            var bytes = new byte[4];
            Random.Shared.NextBytes(bytes);
            var value = BitConverter.ToUInt32(bytes, 0);
            return useHex ? 
                value.ToString($"X{width ?? 8}") : 
                value.ToString($"D{width ?? 8}");
        }

        public static string GetRandom6Digit(int? width = null) {
            var value = Random.Shared.Next(0, 1_000_000);
            return value.ToString($"D{width ?? 6}");
        }
        public static string GetRandom9Digit(int? width = null) {
            var value = Random.Shared.Next(0, 1_000_000_000);
            return value.ToString($"D{width ?? 9}");
        } 

        public static string GetDate(string? format) => DateTime.UtcNow.ToString(format ?? "yyyyMMdd");
        
        public static string GetGuid(string? format) => Guid.NewGuid().ToString(format ?? "N");

        public static string GetSequence(int sequence = 1, int? width = null) => sequence.ToString(
            width.HasValue ? $"D{width}" : string.Empty);
    }
}
