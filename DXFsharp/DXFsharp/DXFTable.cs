
namespace DXFsharp
{
    public class DXFTable : DXFGroup
    {
        public string Name;
        public override void ReadProperty()
        {
            if (Converter.FCode == 2)
            {
                Name = Converter.FValue;
                Name = Name.ToUpper();
                if (Name == "LAYER")
                    Converter.layers = this;
                //if(Name == "LTYPE")
                //	Converter.FLTypes = this;
                //if(Name == "BLOCK_RECORD") 
                //	Converter.FBlockRecords = this;
                //if(Name == "STYLE") 
                //	Converter.FStyles = this;
            }
        }
    }
}
