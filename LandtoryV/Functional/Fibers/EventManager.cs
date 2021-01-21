using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace Vigilance.Functional.Fibers
{
    internal static class EventManager
    {
        internal static void Loop()
        {
            while (true)
            {
                GameFiber.Sleep(1000);
            }
        }
    }
}
