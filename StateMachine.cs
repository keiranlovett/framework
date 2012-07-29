#region Using statements

using System.Collections.Generic;

#endregion


namespace FistBump.Framework
{
    public class StateMachine
    {
        List<State> m_States;
        State m_PrevState;
        State m_CurrState;
        public void Update()
        {
        }
    }
}
