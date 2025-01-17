﻿using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SRTS
{
  public static class SRTSArrivalActionUtility
  {
    public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(
      Func<FloatMenuAcceptanceReport> acceptanceReportGetter,
      Func<T> arrivalActionGetter,
      string label,
      CompLaunchableSRTS representative,
      int destinationTile,
      Caravan car)
      where T : TransportPodsArrivalAction
    {
      FloatMenuAcceptanceReport rep = acceptanceReportGetter();
      if (rep.Accepted || !rep.FailReason.NullOrEmpty() || !rep.FailMessage.NullOrEmpty())
      {
        if (!rep.FailReason.NullOrEmpty())
          yield return new FloatMenuOption(label + " (" + rep.FailReason + ")", (Action) null, MenuOptionPriority.Default, (Action) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null);
        else
          yield return new FloatMenuOption(label, (Action) (() =>
          {
            FloatMenuAcceptanceReport acceptanceReport = acceptanceReportGetter();
            if (acceptanceReport.Accepted)
            {
              representative.TryLaunch(destinationTile, (TransportPodsArrivalAction) arrivalActionGetter(), car);
            }
            else
            {
              if (acceptanceReport.FailMessage.NullOrEmpty())
                return;
              Messages.Message(acceptanceReport.FailMessage, (LookTargets) new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
            }
          }), MenuOptionPriority.Default, (Action) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null);
      }
    }

    public static IEnumerable<FloatMenuOption> GetATKFloatMenuOptions(
      CompLaunchableSRTS representative,
      IEnumerable<IThingHolder> pods,
      SettlementBase settlement,
      Caravan car)
    {
      Func<FloatMenuAcceptanceReport> acceptanceReportGetter1 = (Func<FloatMenuAcceptanceReport>)(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement));
      Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter1 = (Func<TransportPodsArrivalAction_AttackSettlement>)(() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop));
      object[] objArray1 = new object[1]
      {
        (object) settlement.Label
      };
      foreach (FloatMenuOption floatMenuOption in SRTSArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter1, arrivalActionGetter1, "AttackAndDropAtEdge".Translate(objArray1), representative, settlement.Tile, car))
      {
        FloatMenuOption f = floatMenuOption;
        yield return f;
        f = (FloatMenuOption)null;
      }
      Func<FloatMenuAcceptanceReport> acceptanceReportGetter2 = (Func<FloatMenuAcceptanceReport>) (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement));
      Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter2 = (Func<TransportPodsArrivalAction_AttackSettlement>) (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop));
      object[] objArray2 = new object[1]
      {
        (object) settlement.Label
      };
      foreach (FloatMenuOption floatMenuOption in SRTSArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter2, arrivalActionGetter2, "AttackAndDropInCenter".Translate(objArray2), representative, settlement.Tile, car))
      {
        FloatMenuOption f2 = floatMenuOption;
        yield return f2;
        f2 = (FloatMenuOption) null;
      }
    }

    public static IEnumerable<FloatMenuOption> GetGIFTFloatMenuOptions(
      CompLaunchableSRTS representative,
      IEnumerable<IThingHolder> pods,
      SettlementBase settlement,
      Caravan car)
    {
      if (settlement.Faction == Faction.OfPlayer)
        return Enumerable.Empty<FloatMenuOption>();
      return SRTSArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>((Func<FloatMenuAcceptanceReport>) (() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement)), (Func<TransportPodsArrivalAction_GiveGift>) (() => new TransportPodsArrivalAction_GiveGift(settlement)), "GiveGiftViaTransportPods".Translate((object) settlement.Faction.Name, (object) FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()), representative, settlement.Tile, car);
    }

    public static IEnumerable<FloatMenuOption> GetVisitFloatMenuOptions(
      CompLaunchableSRTS representative,
      IEnumerable<IThingHolder> pods,
      SettlementBase settlement,
      Caravan car)
    {
      return SRTSArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>((Func<FloatMenuAcceptanceReport>) (() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement)), (Func<TransportPodsArrivalAction_VisitSettlement>) (() => new TransportPodsArrivalAction_VisitSettlement(settlement)), "VisitSettlement".Translate((object) settlement.Label), representative, settlement.Tile, car);
    }
  }
}
