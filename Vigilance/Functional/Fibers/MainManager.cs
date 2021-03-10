using Vigilance.Entities;
using Rage;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Vigilance.Engine;

namespace Vigilance.Functional.Fibers
{
    public static class MainManager
    {
        private static int rectangleY;

        internal static void Loop()
        {
            Game.LogTrivial("Thread Loop > Main Manager");
#if !DEBUG // prevent models being changed quickly
            Game.LocalPlayer.Model = "s_m_y_cop_01";
#endif
            Game.FrameRender += Game_FrameRender;
            Game.RawFrameRender += Game_RawFrameRender;
            while (Common.IsRunning)
            {
                GameFiber.Yield();

                rectangleY = 10;

                var length = Persona.Personas.Count;
                if (length <= 0) continue;
                var keys = Persona.Personas.Keys.ToList();

                for (var i = 0; i < length; i++)
                {
                    if (keys[i]) continue;
                    Persona.Personas.Remove(keys[i]);
                    length--;
                }

            }
        }

        private static void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            e.Graphics.DrawText("POSITION: " + Game.LocalPlayer.Character.Position.ToString(), "Arial", 12f, new PointF(12, 25), Color.White);
        }

        private static void Game_RawFrameRender(object sender, GraphicsEventArgs e)
        {
#if DEBUG
            e.Graphics.DrawRectangle(new RectangleF(10, rectangleY, 350, 15), Color.Black);
            e.Graphics.DrawText("LANDTORY MANUALLY BUILT VERSION", "Arial", 12f, new PointF(12, 12), Color.White);
#endif
        }
    }
}
