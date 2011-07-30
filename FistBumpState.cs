#region Using statements
using System;
#endregion

public abstract class FistBumpState
{
    UInt16  m_StateId;
	public abstract void OnEnterState();
	public abstract void OnUpdateState();
    public abstract void OnExitState();
}

