using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandtoryV.Proceed
{
    public class Subject
    {
        public Ped RagePed { get; private set; }
        public bool Arrested { get; internal set; }
        public bool Surrendered { get; private set; }
        public Subject(Ped ped)
        {
            RagePed = ped;

        }

        internal void Surrender()
        {
            if (Surrendered) return;
            if (Arrested) return;
            RagePed.BlockPermanentEvents = true;
            RagePed.PlayAmbientSpeech("GENERIC_CURSE_MID");
            RagePed.Tasks.ClearImmediately();
            AnimationDictionary arrests = new AnimationDictionary("random@arrests");
            AnimationDictionary arrestsBusted = new AnimationDictionary("random@arrests@busted");
            TaskSequence ts = new TaskSequence(RagePed);
            ts.Tasks.PlayAnimation(arrests, "idle_2_hands_up", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "enter", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "idle_a", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrestsBusted, "exit", 1f, AnimationFlags.None);
            ts.Tasks.PlayAnimation(arrests, "kneeling_arrest_get_up", 1f, AnimationFlags.StayInEndFrame);
            ts.Execute();
            Surrendered = true;
        }

        internal void Arrest()
        {
            if (!Surrendered) throw new InvalidOperationException("A ped must surrender first before getting arrested.");
            if (Arrested) return;
            RagePed.Tasks.Clear();

            RagePed.Position = Game.LocalPlayer.Character.GetOffsetPositionFront(0.7f);
            RagePed.Tasks.PlayAnimation(new AnimationDictionary("mp_arresting"), "idle", 9.0f, AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame);
            RagePed.Heading = Game.LocalPlayer.Character.Heading;
            RagePed.Tasks.StandStill(3500);
            Rage.Task t = Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("mp_uncuff_paired"), "crook_01_p2_fwd", 1f, AnimationFlags.None);
            t.WaitForCompletion();

            Group player = Game.LocalPlayer.Group;
            player.AddMember(RagePed);
            Arrested = true;
        }

        internal void DeArrest()
        {
            RagePed.Tasks.ClearSecondary();
            RagePed.Tasks.Clear();

            Arrested = false;
            Surrendered = false;
            
            Game.LocalPlayer.Group.RemoveMember(RagePed);

            RagePed.Dismiss();
        }
    }
}
