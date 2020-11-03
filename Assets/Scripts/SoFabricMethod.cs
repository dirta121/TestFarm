using System.Linq;
namespace TestFarm
{
    public class SoFabricMethod : Singleton<SoFabricMethod>
    {
        public Goods[] farmSubjects;
        public Goods GetGoodsByName(string name)
        {
            var prefab = farmSubjects.Where(w => w.name == name).SingleOrDefault();
            if (prefab == null) { return null; }
            return prefab;
        }
        public bool TryGetGoodsByName(string name, out Goods prefab)
        {
            prefab = farmSubjects.Where(w => w.name == name).SingleOrDefault();
            if (prefab == null) { return false; }
            return true;
        }
    }
}