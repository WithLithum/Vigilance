using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace Vigilance.Engine.Entities
{
    internal class PedInfo
    {
        internal PedInfo(Ped ped)
        {
            Handle = ped;
        }

        internal Ped Handle { get; set; }
        internal bool IsArrested { get; set; }
        internal bool IsSurrendered { get; set; }

        internal bool IsValid() => Handle.Exists();
    }
}
