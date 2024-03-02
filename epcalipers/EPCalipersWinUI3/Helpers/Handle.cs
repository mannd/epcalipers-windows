using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Helpers
{
    class EmptyHandle : SafeHandle
    {
        public EmptyHandle() : base(IntPtr.Zero, false)
        {
        }

        public override bool IsInvalid => true;

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }
}
