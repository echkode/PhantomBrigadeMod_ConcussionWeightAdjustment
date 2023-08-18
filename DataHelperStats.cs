using System.Collections.Generic;

using PhantomBrigade.Data;

using PhantomBrigade;
using PBDataHelperStats = PhantomBrigade.Data.DataHelperStats;

using UnityEngine;

namespace EchKode.PBMods.ConcussionWeightAdjustment
{
	static class DataHelperStats
	{
		private static HashSet<string> unitClasses = new HashSet<string>();

		internal static void Initialize()
		{
			if ((ModLink.Settings.unitClasses & ModLink.ModSettings.UnitClassFlag.Mech) == ModLink.ModSettings.UnitClassFlag.Mech)
			{
				unitClasses.Add(UnitClassKeys.mech);
			}
			if ((ModLink.Settings.unitClasses & ModLink.ModSettings.UnitClassFlag.Tank) == ModLink.ModSettings.UnitClassFlag.Tank)
			{
				unitClasses.Add(UnitClassKeys.tank);
			}
		}

		internal static DataContainerUnitPerformanceClass GetWeightClassFromIndex(int index, CombatEntity combatUnit)
		{
			var unit = IDUtility.GetLinkedPersistentEntity(combatUnit);
			if (unit == null)
			{
				return PBDataHelperStats.GetWeightClassFromIndex(index);
			}
			if (!unitClasses.Contains(unit.dataKeyUnitClass.s))
			{
				return PBDataHelperStats.GetWeightClassFromIndex(index);
			}

			var bodyMass = 0f;
			var partsInUnit = EquipmentUtility.GetPartsInUnit(unit);
			foreach (var socket in DataHelperUnitEquipment.GetSocketsBody())
			{
				var partInUnit = EquipmentUtility.GetPartInUnit(unit, socket, parts: partsInUnit);
				if (partInUnit == null)
				{
					continue;
				}
				bodyMass += PBDataHelperStats.GetFinalStatForPart(UnitStats.mass, partInUnit);
			}

			if (ModLink.Settings.IsLoggingEnabled(ModLink.ModSettings.LoggingFlag.Call))
			{
				var wc = PBDataHelperStats.GetWeightClassFromIndex(index);
				var wc2 = PBDataHelperStats.GetWeightClass(bodyMass);
				Debug.LogFormat(
					"Mod {0} ({1}) adjusting weight class for body mass | total mass: {2} | body mass: {3} | original class: {4} | new class: {5}",
					ModLink.modIndex,
					ModLink.modID,
					PBDataHelperStats.GetCachedStatForUnit(UnitStats.mass, unit),
					bodyMass,
					wc.textName,
					wc2.textName);
			}

			return PBDataHelperStats.GetWeightClass(bodyMass);
		}
	}
}
