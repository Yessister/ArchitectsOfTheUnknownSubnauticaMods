﻿using SMLHelper.V2.Assets;
using ECCLibrary;
using ProjectAncients.Mono;
using UnityEngine;
using UWE;

namespace ProjectAncients.Prefabs.AlienBase
{
    public class TabletTerminalPrefab : Spawnable
    {
        private PrecursorKeyTerminal.PrecursorKeyType keyType;

        public TabletTerminalPrefab(string classId, PrecursorKeyTerminal.PrecursorKeyType keyType)
            : base(classId, "Forcefield Control", ".")
        {
            this.keyType = keyType;
        }

        public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
        {
            classId = ClassID,
            cellLevel = LargeWorldEntity.CellLevel.Medium,
            localScale = Vector3.one,
            slotType = EntitySlot.Type.Large,
            techType = this.TechType
        };

        public override GameObject GetGameObject()
        {
            PrefabDatabase.TryGetPrefab("c718547d-fe06-4247-86d0-efd1e3747af0", out GameObject prefab);
            GameObject obj = GameObject.Instantiate(prefab);
            obj.GetComponent<PrecursorKeyTerminal>().acceptKeyType = keyType;
            obj.SetActive(false);
            return obj;
        }
    }
}