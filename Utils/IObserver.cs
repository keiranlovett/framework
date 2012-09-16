using UnityEngine;
using System.Collections;

namespace FistBump.Framework
{
    public interface IObserver
    {
        void OnNotify(IObservable observable);
    }
    
    public interface IObservable
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
    }
}