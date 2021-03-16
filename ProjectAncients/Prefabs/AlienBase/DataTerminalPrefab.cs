﻿using SMLHelper.V2.Assets;
using ECCLibrary;
using ProjectAncients.Mono;
using UnityEngine;
using UWE;

namespace ProjectAncients.Prefabs.AlienBase
{
    public class DataTerminalPrefab : Spawnable
    {
        private string encyKey;
        private string terminalClassId;
        private string[] pingClassId;
        private string audioClipPrefix;
        private Vector3 pingPosition;
        private float delay = 7f;

        public DataTerminalPrefab(string classId, string encyKey, string[] pingClassId = default, string audioClipPrefix = "DataTerminal1", string terminalClassId = "b629c806-d3cd-4ee4-ae99-7b1359b60049")
            : base(classId, "Data terminal", ".")
        {
            this.encyKey = encyKey;
            this.terminalClassId = terminalClassId;
            this.pingClassId = pingClassId;
            this.audioClipPrefix = audioClipPrefix;
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
            PrefabDatabase.TryGetPrefab(terminalClassId, out GameObject prefab);
            GameObject obj = GameObject.Instantiate(prefab);
            StoryHandTarget storyHandTarget = obj.GetComponent<StoryHandTarget>();
            storyHandTarget.goal = new Story.StoryGoal(encyKey, Story.GoalType.Encyclopedia, delay);
            obj.SetActive(false);
            if(pingClassId != null && pingClassId.Length > 0)
            {
                foreach(string str in pingClassId)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        DataTerminalUnlockPing unlockPing = obj.AddComponent<DataTerminalUnlockPing>();
                        unlockPing.classId = str;
                        unlockPing.pos = pingPosition;
                        unlockPing.pingTypeName = str;
                    }
                }
            }
            if (!string.IsNullOrEmpty(audioClipPrefix))
            {
                obj.AddComponent<StoryHandTargetPlayAudioClip>().clipPrefix = audioClipPrefix;
            }
            return obj;
        }
    }
}