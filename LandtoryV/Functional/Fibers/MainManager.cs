using Vigilance.Entities;
using Vigilance.Proceed;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vigilance.Functional.Fibers
{
    public static class MainManager
    {
        private static int rectangleY;

        internal static void Loop()
        {
            Game.LogTrivial("Thread Loop > Main Manager");
#if !DEBUG // prevent model changes quickly
            Game.LocalPlayer.Model = "s_m_y_cop_01";
#endif
            Game.FrameRender += Game_FrameRender;
            Game.RawFrameRender += Game_RawFrameRender;
            while (true)
            {
                GameFiber.Yield();

                rectangleY = Game.Resolution.Height - 25;

                int length = Persona.Personas.Count;
                if (length > 0)
                {
                    List<Ped> keys = Persona.Personas.Keys.ToList();

                    for (int i = 0; i < length; i++)
                    {
                        if (!keys[i])
                        {
                            Persona.Personas.Remove(keys[i]);
                            length--;
                        }
                    }
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
            e.Graphics.DrawText("LANDTORY MANUALLY BULIT VERSION", "Arial", 12f, new PointF(12, 12), Color.White);
#endif
        }

        public static bool IsCopModel(Subject s) => s.RagePed.Model.Name == "s_m_y_cop_01" || s.RagePed.Model.Name == "s_f_y_cop_01" || s.RagePed.Model.Name == "s_f_y_sheriff_01" || s.RagePed.Model.Name == "s_m_y_sheriff_01" || s.RagePed.Model.Name == "s_m_y_swat_01";
    }
}
