using Rage;

namespace Vigilance.Engine.Scripting
{
    internal abstract class ProcessFiber
    {
        internal bool IsRunning { get; set; }
        internal GameFiber Fiber { get; set; }
        internal virtual void Run()
        {
            IsRunning = true;
            Fiber = GameFiber.StartNew(() =>
            {
                Initialize();
                while (IsRunning)
                {
                    Process();
                }
            });
            
            
        }
        internal abstract void Initialize();
        internal abstract void Process();
        internal virtual void End()
        {
            IsRunning = false;
        }
    }
}
