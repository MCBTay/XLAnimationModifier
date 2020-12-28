using UnityEngine;
using HarmonyLib;

namespace XLAnimationModifier.Patches
{
	public class PlayerState_PowerslidePatch
	{
		[HarmonyPatch(typeof(PlayerState_Powerslide), "ListenForRevert")]
		static class ListenForRevertPatch
		{
			static void Postfix(ref bool ____revertTriggered, ref bool ____revertAnimTriggered)
			{
				if (!Main.Enabled || !Settings.Instance.DisableRevertAnimation) return;

				if (____revertTriggered && ____revertAnimTriggered)
				{
					Debug.Log("XLAnimationModifier: Revert triggered and animation triggered.  Setting to false.");
					____revertAnimTriggered = false;
				}
			}
		}
	}
}
