using Rage.Native;

namespace LandtoryV.User.UI
{
    public static class Interface
    {
        internal static int PursuitWantedLevel { get; set; } = 2;
        
        private static bool pursuitVisualScreen;
        internal static bool PursuitVisualScreen
        {
            get => pursuitVisualScreen;
            set
            {
                if (value)
                {
                    NativeFunction.Natives.SET_FAKE_WANTED_LEVEL(PursuitWantedLevel);
                }
                else
                {
                    NativeFunction.Natives.SET_FAKE_WANTED_LEVEL(0);
                }
                pursuitVisualScreen = value;
            }
        }
    }
}