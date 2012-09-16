using System;

namespace FistBump.Framework.ExtensionMethods
{
    public static class Extensions
    {
        public static void SafeInvoke(this Action action)
        {
            if (action != null) action();
        }

        public static void SafeInvoke<T>(this Action<T> action, T t1)
        {
            if (action != null) action(t1);
        }

        public static void SafeInvoke(this EventHandler handler, object sender)
        {
            if (handler != null) handler(sender, EventArgs.Empty);
        }
    }
}
