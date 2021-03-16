﻿using SMLHelper.V2.Assets;
using ECCLibrary;
using ProjectAncients.Mono;
using UnityEngine;
using UWE;

namespace ProjectAncients.Prefabs.AlienBase
{
    public class PrecursorDoorPrefab : Spawnable
    {
        string terminalClassId;
        bool overrideTerminalPosition;
        Vector3 terminalPosition;
        Vector3 terminalRotation;

        public PrecursorDoorPrefab(string classId, string displayName, string terminalClassId, bool overrideTerminalPosition = false, Vector3 terminalLocalPosition = default, Vector3 terminalLocalRotation = default)
            : base(classId, displayName, ".")
        {
            this.terminalClassId = terminalClassId;
            this.overrideTerminalPosition = overrideTerminalPosition;
            this.terminalPosition = terminalLocalPosition;
            this.terminalRotation = terminalLocalRotation;
        }

        public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
        {
            classId = ClassID,
            cellLevel = LargeWorldEntity.CellLevel.Medium,
            localScale = Vector3.one,
            slotType = EntitySlot.Type.Large,
            techType = TechType
        };

        public override GameObject GetGameObject()
        {
            PrefabDatabase.TryGetPrefab("b816abb4-8f6c-4d70-b4c5-662e69696b23", out GameObject prefab);
            GameObject obj = GameObject.Instantiate(prefab);
            GameObject terminalPrefabPlaceholder = obj.SearchChild("PurpleKeyTerminal", ECCStringComparison.Contains);
            terminalPrefabPlaceholder.GetComponent<PrefabPlaceholder>().prefabClassId = terminalClassId;
            if (overrideTerminalPosition)
            {
                terminalPrefabPlaceholder.transform.localPosition = terminalPosition;
                terminalPrefabPlaceholder.transform.localEulerAngles = terminalRotation;
            }
            obj.SetActive(false);
            return obj;
        }
    }
}