using Vigilance.Entities;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigilance.Engine.API
{
    /// <summary>
    /// Extensions to Ped.
    /// </summary>
    [Obsolete]
    public class LPed : Ped
    {
        public bool IsArrested { get; private set; }

        public bool IsSurrendered { get; private set; }

        public Persona Persona { get; internal set; }

        internal void ExecuteSurrenderAnimTask()
        {
            if (IsSurrendered) return;
            if (IsArrested) return;
            BlockPermanentEvents = true;
            PlayAmbientSpeech("GENERIC_CURSE_MID");
            Tasks.ClearImmediately();
            AnimationDictionary arrests = new AnimationDictionary("random@arrests");
            AnimationDictionary arrestsBusted = new AnimationDictionary("random@arrests@busted");
            TaskSequence ts = new TaskSequence(this);
            ts.Tasks.PlayAnimation(arrests, "idle_2_hands_up", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "enter", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "idle_a", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "exit", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrests, "kneeling_arrest_get_up", 1f, AnimationFlags.StayInEndFrame);
            ts.Execute();
            IsSurrendered = true;
        }

        internal void ExecuteRemoveFromArrestAnimTask()
        {
            Tasks.ClearSecondary();
            Tasks.Clear();

            IsArrested = false;
            IsSurrendered = false;

            Game.LocalPlayer.Group.RemoveMember(this);

            Dismiss();
        }

        internal void ExecuteArrestAnimTask()
        {
            if (!IsSurrendered) throw new InvalidOperationException("A ped must surrender first before getting arrested.");
            if (IsArrested) return;
            Tasks.Clear();

            Position = Game.LocalPlayer.Character.GetOffsetPositionFront(0.7f);
            Tasks.PlayAnimation(new AnimationDictionary("mp_arresting"), "idle", 9.0f, AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame);
            Heading = Game.LocalPlayer.Character.Heading;
            Tasks.StandStill(3500);
            Rage.Task t = Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("mp_uncuff_paired"), "crook_01_p2_fwd", 1f, AnimationFlags.None);
            t.WaitForCompletion();

            Group player = Game.LocalPlayer.Group;
            player.AddMember(this);
            MarkAsArrested();
        }

        internal void MarkAsArrested()
        {
            IsArrested = true;
        }
    }
}
