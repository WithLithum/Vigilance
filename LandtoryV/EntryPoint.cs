using LandtoryV.Functional.Fibers;
using Rage;

[assembly: Rage.Attributes.Plugin("LandtoryV", Author = "RelaperCrystal", Description = "Yet another cop mod")]

namespace LandtoryV
{
    static class EntryPoint
    {
        private static GameFiber backup;
        private static GameFiber main;
        private static GameFiber menu;
        private static GameFiber arrest;
        private static GameFiber eventFiber;

        public static bool Shutdown { get; private set; }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main()
        {
            Game.LogTrivial("Initializing Landtory");
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
            GameFiber.Sleep(-1);
        }

        static void ShutdownMod()
        {
            Shutdown = true;
        }
    }
}
