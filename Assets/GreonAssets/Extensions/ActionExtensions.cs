using System;

namespace GreonAssets.Extensions
{
    public static class ActionExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            if (action == null) return;

            foreach (var subscriber in action.GetInvocationList())
            {
                try
                {
                    ((Action)subscriber).Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while executing subscriber: {ex.Message}");
                }
            }
        }
        
        public static void SafeInvoke<T>(this Action<T> action, T arg)
        {
            if (action == null) return;

            foreach (var subscriber in action.GetInvocationList())
            {
                try
                {
                    ((Action<T>)subscriber).Invoke(arg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while executing subscriber: {ex.Message}");
                }
            }
        }
        
        public static void SafeInvoke<T0, T1>(this Action<T0, T1> action, T0 arg0, T1 arg1)
        {
            if (action == null) return;

            foreach (var subscriber in action.GetInvocationList())
            {
                try
                {
                    ((Action<T0, T1>)subscriber).Invoke(arg0, arg1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while executing subscriber: {ex.Message}");
                }
            }
        }
    }
}
