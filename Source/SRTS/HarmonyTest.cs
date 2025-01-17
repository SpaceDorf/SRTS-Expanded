﻿using Harmony;
using RimWorld;
using Verse;

namespace SRTS
{
    [HarmonyPatch(typeof (DropPodUtility), "MakeDropPodAt", new System.Type[] {typeof (IntVec3), typeof (Map), typeof (ActiveDropPodInfo)})]
    public static class HarmonyTest
    {
        public static bool Prefix(IntVec3 c, Map map, ActiveDropPodInfo info)
        {
            for (int index = 0; index < info.innerContainer.Count; index++)
            {
                if (info.innerContainer[index].TryGetComp<CompLaunchableSRTS>() != null)
                {
                    Thing ship = info.innerContainer[index];
                    string shipType = ship.def.defName;
                    ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDef.Named(shipType + "_Active"), (ThingDef)null);
                    activeDropPod.Contents = info;
                    
                    EnsureInBoundsSRTS(ref c, info.innerContainer[index].def, map);
                    SRTSIncoming incomingSRTS = (SRTSIncoming)SkyfallerMaker.SpawnSkyfaller(ThingDef.Named(shipType + "_Incoming"), (Thing)activeDropPod, c, map);
                    incomingSRTS.SRTSRotation = ship.Rotation;
                    return false;
                }
            }
            return true;
        }

        private static void EnsureInBoundsSRTS(ref IntVec3 c, ThingDef shipDef, Map map)
        {
            int x = (int)shipDef.graphicData.drawSize.x;
            int y = (int)shipDef.graphicData.drawSize.y;
            int offset = x > y ? x : y;

            if (c.x < offset)
            {
                c.x = offset;
            }
            else if (c.x >= (map.Size.x - offset))
            {
                c.x = (map.Size.x - offset);
            }
            if (c.z < offset)
            {
                c.z = offset;
            }
            else if (c.z > (map.Size.z - offset))
            {
                c.z = (map.Size.z - offset);
            }
        }
    }
}
