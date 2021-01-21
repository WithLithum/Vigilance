using System.Collections.Generic;
using LandtoryV.Engine.Entities;
using Rage;

namespace LandtoryV.Engine
{
    public static class Functions
    {
        internal static List<PedInfo> PedInfos = new List<PedInfo>();

        public static void IsPlayerInCountryside()
        { 
            
        }

        public static void MakePedSurrender(Ped ped)
        {
            if (!ped) return;
            int index;
            PedInfo info = CreatePedInfoIfPedDoesNotExists(ped, out index);

            if (info.IsSurrendered) return;
            if (info.IsArrested) return;


            ped.BlockPermanentEvents = true;
            ped.PlayAmbientSpeech("GENERIC_CURSE_MID");
            ped.Tasks.ClearImmediately();
            AnimationDictionary arrests = new AnimationDictionary("random@arrests");
            AnimationDictionary arrestsBusted = new AnimationDictionary("random@arrests@busted");
            TaskSequence ts = new TaskSequence(ped);
            ts.Tasks.PlayAnimation(arrests, "idle_2_hands_up", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "enter", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "idle_a", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "exit", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrests, "kneeling_arrest_get_up", 1f, AnimationFlags.StayInEndFrame);
            ts.Execute();
            info.IsSurrendered = true;
            PedInfos[index] = info;
#if DEBUG
            Log.Trace("Functions", $"{PedInfos[index].IsSurrendered}");
#endif
        }

        public static void ArrestPedByPlayer(Ped ped)
        {
            if (!ped) return;
            int index;
            PedInfo info = CreatePedInfoIfPedDoesNotExists(ped, out index);
            if (!info.IsSurrendered) return;
            if (info.IsArrested) return;
            ped.Tasks.Clear();
            Game.LogTrivial("Arresting the target!");
            Game.DisplayHelp("The suspect is being arrested.");

            ped.Position = Game.LocalPlayer.Character.GetOffsetPositionFront(0.7f);
            ped.Tasks.PlayAnimation(new AnimationDictionary("mp_arresting"), "idle", 9.0f, AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame);
            ped.Heading = Game.LocalPlayer.Character.Heading;
            ped.Tasks.StandStill(3500);
            Task t = Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("mp_uncuff_paired"), "crook_01_p2_fwd", 1f, AnimationFlags.None);
            t.WaitForCompletion();

            Group player = Game.LocalPlayer.Group;
            player.AddMember(ped);
            info.IsArrested = true;
            PedInfos[index] = info;
        }

        public static bool IsPedArrested(Ped ped)
        {
            if (!ped) return false;
            int _;
            PedInfo info = CreatePedInfoIfPedDoesNotExists(ped, out _);
            return info.IsArrested;
        }

        public static bool IsPedSurrendered(Ped ped)
        {
            if (!ped) return false;
            int _;
            PedInfo info = CreatePedInfoIfPedDoesNotExists(ped, out _);
            return info.IsSurrendered;
        }

        public static void RemovePedFromArrest(Ped ped)
        {
            if (!ped) return;
            int index;
            PedInfo info = CreatePedInfoIfPedDoesNotExists(ped, out index);

            if (!info.IsArrested) return;

            ped.Tasks.Clear();
            ped.Tasks.ClearSecondary();

            Group g = Game.LocalPlayer.Group;
            if (g.IsMember(ped)) g.RemoveMember(ped);
            ped.ClearLastVehicle();
            ped.Dismiss();

            PedInfos[index] = info;
        }

        internal static PedInfo CreatePedInfoIfPedDoesNotExists(Ped ped, out int index)
        {
            foreach (var info in PedInfos)
            {
                if (info.Handle == ped)
                {
                    index = PedInfos.IndexOf(info);
                    return info;
                }
            }
            var newInfo = new PedInfo(ped);
            PedInfos.Add(newInfo);
            index = PedInfos.IndexOf(newInfo);
            return newInfo;
        }
    }
}