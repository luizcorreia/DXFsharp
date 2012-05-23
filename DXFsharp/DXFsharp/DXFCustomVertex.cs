
namespace DXFsharp
{
    public class DXFCustomVertex : DXFVisibleEntity
    {
        public SFPoint point;
        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 10:
                    point.X = Converter.FloatValue();
                    break;
                case 20:
                    point.Y = Converter.FloatValue();
                    break;
                case 30:
                    point.Z = Converter.FloatValue();
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
    }
}
