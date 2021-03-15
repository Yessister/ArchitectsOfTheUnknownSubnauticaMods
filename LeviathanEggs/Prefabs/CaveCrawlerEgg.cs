using LeviathanEggs.MonoBehaviours;
using LeviathanEggs.Prefabs.API;
using static LeviathanEggs.Helpers.AssetsBundleHelper;
using UnityEngine;

namespace LeviathanEggs.Prefabs
{
    public class CaveCrawlerEgg : EggPrefab
    {
        public CaveCrawlerEgg()
            : base("CaveCrawlerEgg", "Blood Crawler Egg", "Blood Crawlers hatch from these.")
        {}
        public override GameObject Model => LoadGameObject("RobotEgg");
        public override TechType HatchingCreature => TechType.CaveCrawler;
        public override float HatchingTime => 2f;
        public override Sprite ItemSprite => LoadSprite("RobotEgg");

        public override GameObject GetGameObject()
        {
            var prefab = base.GetGameObject();
            
            Material material = new Material(Shader.Find("MarmosetUBER"))
            {
                mainTexture = LoadTexture2D("RobotEggDiffuse"),
            };
            material.EnableKeyword("MARMO_NORMALMAP");
            material.EnableKeyword("MARMO_SPECMAP");
            material.EnableKeyword("MARMO_EMISSION");

            material.SetTexture(ShaderPropertyID._Illum, LoadTexture2D("RobotEggIllum"));
            material.SetTexture(ShaderPropertyID._SpecTex, LoadTexture2D("RobotEggDiffuse"));
            material.SetTexture(ShaderPropertyID._BumpMap, LoadTexture2D("RobotEggNormal"));

            Renderer[] renderers = prefab.GetAllComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                rend.material = material;
                rend.sharedMaterial = material;
            }

            prefab.AddComponent<SpawnLocations>();

            return prefab;
        }
    }
}