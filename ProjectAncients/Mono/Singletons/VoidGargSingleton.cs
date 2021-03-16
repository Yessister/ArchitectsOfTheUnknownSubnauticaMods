﻿using ECCLibrary.Internal;
using UnityEngine;

namespace ProjectAncients.Mono
{
	// Ensures there is only ever one NATURALLY spawned adult.
	public class VoidGargSingleton : MonoBehaviour
	{
		private static VoidGargSingleton main;

		public static bool AdultGargExists
		{
			get
			{
				if (main == null)
				{
					return false;
				}
				if (main.gameObject.activeSelf == false)
				{
					return false;
				}
				return true;
			}
		}

		void Awake()
		{
			main = this;
		}

		void CheckDistance()
		{
			if (!VoidGargSpawner.IsVoidBiome(Player.main.GetBiomeString()))
			{
				float distance = Vector3.Distance(MainCameraControl.main.transform.position, transform.position);
				if (distance > 250f)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}