using Vigilance.Engine;
using Rage;
using System.Collections.Generic;

namespace Vigilance.Functional.Fibers
{
    internal static class ArrestManager
    {
        private static readonly List<Ped> ArrestedSubjects = new List<Ped>();

        private static void Fiber()
        {
            Log.Trace("Arrest Manager", "Starting instance");
            var player = Game.LocalPlayer;

            var station = new Vector3(463f, -990f, 24f);

            while (Common.IsRunning)
            {
                GameFiber.Yield();
                foreach (var member in Game.LocalPlayer.Group.Members)
                {
                    if (member.IsDead) Game.LocalPlayer.Group.RemoveMember(member);
                }

                foreach(var detainee in ArrestedSubjects)
                {
                    if (Game.IsControlPressed(0, GameControl.Context) && detainee.DistanceTo(player.Character) < 2f && detainee.IsStopped)
                    {
                        if (!Functions.IsPedArrested(detainee) && Functions.IsPedSurrendered(detainee))
                        {
                            Functions.ArrestPedByPlayer(detainee);
                        }
                        else
                        {
                            if (Functions.IsPedArrested(detainee))
                            {
                                Functions.RemovePedFromArrest(detainee);
                            }
                        }
                        
                    }
                }

                for (var i = 0; i < ArrestedSubjects.Count; i++)
                {
                    if (ArrestedSubjects.Count >= i && !Functions.IsPedArrested(ArrestedSubjects[i]))
                    {
                        ArrestedSubjects.RemoveAt(i);
                    }
                }

                if (player.Character.DistanceTo(station) < 5.0f)
                {
                    Game.DisplayHelp("Press ~INPUT_CONTEXT~ to drop off suspects.");
                    if (Game.IsControlPressed(0, GameControl.Context) && ArrestedSubjects.Count != 0)
                    {
                        foreach(var detainee in ArrestedSubjects)
                        {
                            player.Character.Group.RemoveMember(detainee);
                            detainee.Delete();
                        }
                        ArrestedSubjects.Clear();
                    }
                }
                

                if (!player.IsFreeAimingAtAnyEntity || !Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    continue;
                }
                Log.TraceVerbose("ArrestManager", "Found key down.");
                var entity = player.GetFreeAimingTarget();
                Log.TraceVerbose("ArrestManager", "Player is really aiming at something");
                if (!entity.Exists() || !entity.Model.IsPed) continue;
                var p = (Ped)entity;
                Log.TraceVerbose("ArrestManager", "Casting it to ped");
                if (!p.Exists()) continue;
                Log.TraceVerbose("ArrestManager", "Found entity.");

                if (!Functions.IsPedSurrendered(p))
                {
                    Log.TraceVerbose("Ped is not surrendered.", "ArrestManager");
                    Functions.MakePedSurrender(p);
                }
                else
                {
                    Log.TraceVerbose("Arresting the ped.", "ArrestManager");
                    Functions.ArrestPedByPlayer(p);
                    ArrestedSubjects.Add(p);
                }
                
            }
        }

        internal static GameFiber StartFiber()
        {
            var result = new GameFiber(Fiber);
            result.Start();
            return result;
        }
    }
}
