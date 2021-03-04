﻿using ECCLibrary.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectAncients.Mono
{
    public class AdultGargSpawner : MonoBehaviour
    {
		private bool playerWasInVoid = false;
		private float timeToSpawnGarg;
		private TechType adultPrefab;
		private const float spawnOutDistance = 100f;
		private const float spawnYLevel = -150f;

		private void Start()
		{
			InvokeRepeating("UpdateSpawn", 1f, 4f);
			adultPrefab = Mod.gargVoidPrefab.TechType;
		}

		private void UpdateSpawn()
		{
			Player player = Player.main;
			if (player)
			{
				bool playerInVoidNow = IsVoidBiome(player.GetBiomeString());
				if (playerWasInVoid != playerInVoidNow)
				{
					if (playerInVoidNow == true)
					{
						timeToSpawnGarg = Time.time + 10f;
					}
					playerWasInVoid = playerInVoidNow;
				}
			}
		}

		private void Update()
		{
			if(playerWasInVoid && Time.time > timeToSpawnGarg)
			{
				if (!AdultGargSingleton.AdultGargExists)
				{
					GameObject newGargantuan = Instantiate(CraftData.GetPrefabForTechType(adultPrefab), GetGargSpawnPoint(Player.main.transform.position), Quaternion.LookRotation(Vector3.up));
					newGargantuan.SetActive(true);
				}
			}
		}

		private bool IsVoidBiome(string biomeName)
		{
			return string.Equals(biomeName, "void", StringComparison.OrdinalIgnoreCase);
		}

		private static Vector3 GetGargSpawnPoint(Vector3 playerWorldPosition)
		{
			Vector3 playerPositionAtY0 = new Vector3(playerWorldPosition.x, 0f, playerWorldPosition.z);
			Vector3 directionToAbyss = playerPositionAtY0.normalized;
			Vector3 spawnOffset = directionToAbyss * spawnOutDistance;
			Vector3 spawnPosition = playerWorldPosition + spawnOffset;
			Vector3 spawnPositionWithCorrectYLevel = new Vector3(spawnPosition.x, spawnYLevel, spawnPosition.z);
			return spawnPositionWithCorrectYLevel;
		}
	}
}
