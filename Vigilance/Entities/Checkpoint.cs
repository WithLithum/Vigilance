using Rage;
using Rage.Native;
using System.Drawing;

namespace Vigilance.Entities
{
    internal class Checkpoint
    {
        public int Handle { get; private set; }
        public Checkpoint(Vector3 position, float radius, Color color)
        {
            Handle = NativeFunction.Natives.CREATE_CHECKPOINT<int>(45, position.X, position.Y, position.Z, 0f, 0f, 0f, radius, color.R, color.G, color.B, color.A, 0);
        }

        public Color CheckColor
        {
            set
            {
                NativeFunction.Natives.SET_CHECKPOINT_RGBA(Handle, value.R, value.G, value.B, value.A);
            }
        }

        public void Delete()
        {
            NativeFunction.Natives.DELETE_CHECKPOINT(Handle);
        }
    }
}
