using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Drawing;
using System.Reflection;
using Vigilance.User.UI;
using Common = Vigilance.Engine.Common;

namespace Vigilance.Functional.Fibers
{
    public static class MainMenu
    {
#pragma warning disable S1450
        private static MenuPool pool;
        private static UIMenu main;
        private static UIMenuItem version;
        private static UIMenuItem debugCuff;
        private static UIMenuItem debugPursuit;
#pragma warning restore S1450

        internal static void Loop()
        {
            Game.LogTrivial("Thread Loop > Main Menu");
            pool = new MenuPool();
            main = new UIMenu("Vigilance", "Control Menu");
            version = new UIMenuItem("Version", "Current version of Vigilance.");
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

            while (Common.IsRunning)
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
