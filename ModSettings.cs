// Copyright (c) 2023 EchKode
// SPDX-License-Identifier: BSD-3-Clause

using System.IO;

using UnityEngine;

namespace EchKode.PBMods.ConcussionWeightAdjustment
{
	partial class ModLink
	{
		internal sealed class ModSettings
		{
			[System.Flags]
			internal enum LoggingFlag
			{
				None = 0,
				System = 0x1,
				Call = 0x2,
				All = 0xFF,
			}

			[System.Flags]
			internal enum UnitClassFlag
			{
				None = 0,
				Mech = 0x1,
				Tank = 0x2,
			}

#pragma warning disable CS0649
			public LoggingFlag logging;
			public UnitClassFlag unitClasses = UnitClassFlag.Mech;
#pragma warning restore CS0649

			internal bool IsLoggingEnabled(LoggingFlag flag) => (logging & flag) == flag;
		}

		internal static ModSettings Settings;

		internal static void LoadSettings()
		{
			var settingsPath = Path.Combine(modPath, "settings.yaml");
			Settings = UtilitiesYAML.ReadFromFile<ModSettings>(settingsPath, false);
			if (Settings == null)
			{
				Settings = new ModSettings();

				Debug.LogFormat(
					"Mod {0} ({1}) no settings file found, using defaults | path: {2}",
					modIndex,
					modID,
					settingsPath);
			}
			if (Settings.IsLoggingEnabled(ModSettings.LoggingFlag.System))
			{
				Debug.LogFormat(
					"Mod {0} ({1}) settings | unit classes: {2} | logging: {3}",
					modIndex,
					modID,
					Settings.unitClasses,
					Settings.logging);
			}
		}
	}
}
