// Copyright (c) 2023 EchKode
// SPDX-License-Identifier: BSD-3-Clause

using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;

using PBIDUtility = PhantomBrigade.IDUtility;
using PBCombatDamageSystem = PhantomBrigade.Combat.Systems.CombatDamageSystem;
using PBDataHelperStats = PhantomBrigade.Data.DataHelperStats;

namespace EchKode.PBMods.ConcussionWeightAdjustment
{
	[HarmonyPatch]
	static class Patch
	{
		private static MethodInfo idu_GetCombatEntity = AccessTools.DeclaredMethod(typeof(PBIDUtility), nameof(PBIDUtility.GetCombatEntity));
		private static MethodInfo dhs_GetWeightClassFromIndex = AccessTools.DeclaredMethod(typeof(PBDataHelperStats), nameof(PBDataHelperStats.GetWeightClassFromIndex));

		[HarmonyPatch(typeof(PBCombatDamageSystem), "Execute", new System.Type[] { typeof(List<CombatEntity>) })]
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var foundIDUCall = false;
			var haveCombatEntityIndex = false;
			object combatEntityIndex = null;

			foreach (var instruction in instructions)
			{
				if (!haveCombatEntityIndex)
				{
					if (!foundIDUCall)
					{
						foundIDUCall = instruction.Calls(idu_GetCombatEntity);
						yield return instruction;
						continue;
					}

					combatEntityIndex = instruction.operand;
					haveCombatEntityIndex = true;
					yield return instruction;
					continue;
				}

				if (!instruction.Calls(dhs_GetWeightClassFromIndex))
				{
					yield return instruction;
					continue;
				}

				// Replace call to GetWeightClassFromIndex with my function.
				// My call needs access to the unit to remove the weight of any weapons.
				yield return new CodeInstruction(OpCodes.Ldloc_S, combatEntityIndex);
				yield return CodeInstruction.Call(typeof(DataHelperStats).AssemblyQualifiedName + ":GetWeightClassFromIndex");
			}
		}
	}
}
