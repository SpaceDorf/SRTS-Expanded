﻿using Harmony;
using RimWorld;
using System;
using Verse;

namespace SRTS
{
  [HarmonyPatch(typeof (ActiveDropPod), "PodOpen", new System.Type[] {})]
  public static class HarmonyTest_dp
  {
    public static void Prefix(ActiveDropPod __instance)
    {
      ActiveDropPodInfo activeDropPodInfo = Traverse.Create((object) __instance).Field("contents").GetValue<ActiveDropPodInfo>();
      for (int index = activeDropPodInfo.innerContainer.Count - 1; index >= 0; --index)
      {
        Thing thing = activeDropPodInfo.innerContainer[index];
        if(thing?.TryGetComp<CompLaunchableSRTS>() != null)
        {
          GenSpawn.Spawn(thing, __instance.Position, __instance.Map, thing.Rotation);
          break;
        }
      }
    }
  }
}
