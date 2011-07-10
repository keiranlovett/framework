using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class FistBumpState
{
    UInt16  m_stateId;
	public abstract void OnEnterState();
	public abstract void OnUpdateState();
    public abstract void OnExitState();
}

