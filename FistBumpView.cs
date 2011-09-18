using UnityEngine;
using System.Collections;

public class FistBumpView : MonoBehaviour
{
    protected FistBumpController m_controller;

    public void Awake() { }
    public void Update() { }

    public void initialize(FistBumpController c)
    {
        m_controller = c;
    }
}
