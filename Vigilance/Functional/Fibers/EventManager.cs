using Rage;
using Vigilance.Engine;

namespace Vigilance.Functional.Fibers
{
    internal static class EventManager
    {
        internal static void Loop()
        {
            while (Common.IsRunning)
            {
                // not implemented
                // TODO implement

                GameFiber.Sleep(1000);
            }
        }
    }
}
