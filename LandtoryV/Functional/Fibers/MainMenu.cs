using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LandtoryV.User.UI;
using Rage.Native;

namespace LandtoryV.Functional.Fibers
{
    public static class MainMenu
    {
        private static MenuPool pool;
        private static UIMenu main;
        private static UIMenuItem version;
        private static UIMenuItem debugCuff;
        private static UIMenuItem debugPursuit;
    
        internal static void Loop()
        {
            Game.LogTrivial("Thread Loop > Main Menu");
            pool = new MenuPool();
            main = new UIMenu("Landtory", "Control Menu");
            version = new UIMenuItem("Version", "Current version of Landtory.");
            version.RightLabel = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            version.BackColor = Color.BlueViolet;
            main.AddItem(version);

            debugCuff = new UIMenuItem("Cuff", "Debug");
            debugCuff.Activated += DebugCuff_Activated;
            
            debugPursuit = new UIMenuItem("Pursuit Visual", "Debug");
            debugPursuit.Activated += (sender, item) =>
            {
                Interface.PursuitVisualScreen = !Interface.PursuitVisualScreen;
            };

            main.AddItem(debugCuff);
            main.AddItem(debugPursuit);
            main.RefreshIndex();
            pool.Add(main);

            while(true)
            {
                GameFiber.Yield();
                pool.ProcessMenus();
                if(Game.IsKeyDown(System.Windows.Forms.Keys.N))
                {
                    main.Visible = !main.Visible;
                }
            }
        }

        private static void DebugCuff_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("mp_arresting"), "idle", 9, AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame);
        }
    }
}
