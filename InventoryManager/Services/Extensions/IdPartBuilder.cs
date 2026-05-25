using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Enums;
using InventoryManager.Models.ViewModels.Inventories;
using InventoryManager.Utilities;

namespace InventoryManager.Services.Extensions {
    public static class IdPartBuilder {

        public static string Build(this IdPart part, int sequence) {
            return part switch {
                FixedTextPart x => x.Generate(),
                Random20bitPart x => x.Generate(),
                Random32bitPart x => x.Generate(),
                Random6Part x => x.Generate(),
                Random9Part x => x.Generate(),
                GuidPart x => x.Generate(),
                DateTimePart x => x.Generate(),
                SequencePart x => x.Generate(sequence),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static string BuildPreview(this InventoryIdPartViewModel part) {
            return part.Type switch {
                IdType.FixedText => part.Text ?? string.Empty,
                IdType.Random20bit => IdGenerator.GetRandom20Bit(part.UseHex ?? false, part.NumberWidth),
                IdType.Random32bit => IdGenerator.GetRandom32Bit(part.UseHex ?? false, part.NumberWidth),
                IdType.Random6 => IdGenerator.GetRandom6Digit(part.NumberWidth),
                IdType.Random9 => IdGenerator.GetRandom9Digit(part.NumberWidth),
                IdType.Guid => IdGenerator.GetGuid(part.Format),
                IdType.DateTime => IdGenerator.GetDate(part.Format),
                IdType.Sequence => IdGenerator.GetSequence(width:part.NumberWidth ?? 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static string Generate(this FixedTextPart part) => part.Text;

        private static string Generate(this Random20bitPart part) => IdGenerator.GetRandom20Bit(part.UseHex, part.Width);

        private static string Generate(this Random32bitPart part) => IdGenerator.GetRandom32Bit(part.UseHex, part.Width);

        private static string Generate(this Random6Part part) => IdGenerator.GetRandom6Digit(part.Width);

        private static string Generate(this Random9Part part) => IdGenerator.GetRandom9Digit(part.Width);

        private static string Generate(this GuidPart part) => IdGenerator.GetGuid(part.Format);

        private static string Generate(this DateTimePart part) => IdGenerator.GetDate(part.Format);

        private static string Generate(this SequencePart part, int sequence) => IdGenerator.GetSequence(sequence, part.Width);
    }
}
