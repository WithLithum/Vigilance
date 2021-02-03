using System.Diagnostics.CodeAnalysis;
using Vigilance.Functional.Fibers;
using Rage;
using Vigilance.Engine;

[assembly: Rage.Attributes.Plugin("Vigilance", Author = "RelaperCrystal", Description = "Yet another cop mod")]

namespace Vigilance
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    internal static class EntryPoint
    {
        private static GameFiber backup;
        private static GameFiber main;
        private static GameFiber menu;
        private static GameFiber arrest;
        private static GameFiber eventFiber;

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void Main()
        {
            Game.LogTrivial("Initializing Vigilance");
            Game.LogTrivial("Thread Started > Main Manager");
            main = GameFiber.StartNew(MainManager.Loop);
            
            Game.LogTrivial("Thread Started > Arrest Manager");
            arrest = ArrestManager.StartFiber();
            
            Game.LogTrivial("Thread Started > Menu");
            menu = GameFiber.StartNew(MainMenu.Loop);
            
            Game.LogTrivial("Thread Started > Backup Manager");
            backup = GameFiber.StartNew(BackupManager.Loop);

            Game.LogTrivial("Thread Started > Event Manager");
            eventFiber = GameFiber.StartNew(EventManager.Loop);

            Game.LogTrivial("Initialization > Complete");
            GameFiber.Hibernate();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void OnUnload(bool fail)
        {
            backup.Abort();
            main.Abort();
            menu.Abort();
            arrest.Abort();
            eventFiber.Abort();
            Common.IsRunning = false;
        }
    }
}
