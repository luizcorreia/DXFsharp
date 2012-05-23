
namespace DXFsharp
{
    public class DXFSection : DXFTable
    {
        public override void ReadProperty()
        {
            if ((Name == null) && (Converter.FCode == 2))
            {
                Name = Converter.FValue;
            }
            switch (Name)
            {
                case "BLOCKS":
                    Converter.FBlocks = this;
                    break;
                case "ENTITIES":
                    Converter.FEntities = this;
                    break;
            }
        }
    }
}
