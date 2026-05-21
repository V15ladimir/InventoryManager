using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.Enums;

namespace InventoryManager.Models.Entitites.Custom {

    public abstract class IdPart {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int Order { get; set; }
        public IdType IdType { get; set; }
        public Inventory Inventory { get; set; } = null!;
    }

    public class FixedTextPart : IdPart {
        public required string Text { get; set; }
    }

    public class Random20bitPart : IdPart {
        public bool UseHex { get; set; }
        public int Width { get; set; }
    }

    public class Random32bitPart : IdPart {
        public bool UseHex { get; set; }
        public int Width { get; set; }
    }

    public class Random6Part : IdPart {
        public int Width { get; set; }
    }

    public class Random9Part : IdPart {
        public int Width { get; set; }
    }

    public class GuidPart : IdPart {
        public string? Format { get; set; }
    }

    public class DateTimePart : IdPart {
        public string? Format { get; set; }
    }

    public class SequencePart : IdPart {
        public int Width { get; set; }
        public int Sequence { get; set; }
    }
}
