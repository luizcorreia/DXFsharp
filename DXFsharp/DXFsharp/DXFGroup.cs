using System.Collections;

namespace DXFsharp
{
    public class DXFGroup : DXFEntity
    {
        public DXFGroup()
        {
            Complex = true;
        }

        public ArrayList Entities = new ArrayList();
        public override bool AddEntity(DXFEntity E)
        {
            Entities.Add(E);
            return (E != null);
        }
        public void Iterate(CADEntityProc Proc, CADIterate Params)
        {
            foreach (DXFEntity Ent in Entities)
            {
                Ent.Invoke(Proc, Params);
            }
        }
    }
}
