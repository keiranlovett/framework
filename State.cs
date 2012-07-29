#region Using statements
using System;
#endregion

namespace FistBump.Framework
{
    public abstract class State
    {
        UInt16 m_StateId;
        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnExitState();
    }
}
