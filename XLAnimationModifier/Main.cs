﻿using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace XLAnimationModifier
{
#if DEBUG
	[EnableReloading]
#endif
	static class Main
	{
		public static bool Enabled;
		private static Harmony Harmony;
		private static GameObject UserInterfaceGameObject;

		static bool Load(UnityModManager.ModEntry modEntry)
		{
			Settings.Instance = UnityModManager.ModSettings.Load<Settings>(modEntry);
			Settings.ModEntry = modEntry;

			UserInterfaceGameObject = new GameObject();
			UserInterfaceGameObject.AddComponent<UserInterface>();
			Object.DontDestroyOnLoad(UserInterfaceGameObject);

			modEntry.OnToggle = OnToggle;
#if DEBUG
			modEntry.OnUnload = Unload;
#endif

			return true;
		}

		private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
		{
			if (Enabled == value) return true;
			Enabled = value;

			if (Enabled)
			{
				Harmony = new Harmony(modEntry.Info.Id);
				Harmony.PatchAll(Assembly.GetExecutingAssembly());
			}
			else
			{
				Object.DestroyImmediate(UserInterfaceGameObject);
				Harmony.UnpatchAll(Harmony.Id);
			}

			return true;
		}

#if DEBUG
		static bool Unload(UnityModManager.ModEntry modEntry)
		{
			Object.DestroyImmediate(UserInterfaceGameObject);

			Harmony?.UnpatchAll();
			return true;
		}
#endif
	}
}
