using SMLHelper.V2.Assets;
using ECCLibrary;
using ProjectAncients.Mono;
using UnityEngine;
using UWE;

namespace ProjectAncients.Prefabs
{
    class ExplosionRoarInitializer : Spawnable
    {
        public ExplosionRoarInitializer()
            : base("KAJWGHDKJAGWKDGAIWUEYOAW", ".", ".")
        {
            OnFinishedPatching = () =>
            {
                StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(this.TechType, new Vector3(1775f, 0f, 536f),
                    "GargantuanRoarAfterExplosion", 20000f));
            };
        }

        public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
        {
            classId = ClassID, cellLevel = LargeWorldEntity.CellLevel.Global, localScale = Vector3.one,
            slotType = EntitySlot.Type.Creature, techType = this.TechType
        };

        public override GameObject GetGameObject()
        {
            GameObject obj = new GameObject();
            obj.EnsureComponent<ExplosionRoar>();
            obj.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
            obj.SetActive(true);
            return obj;
        }
        
    }
}