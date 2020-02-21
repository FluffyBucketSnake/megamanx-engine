using System;
using System.Collections.Generic;

namespace MegamanX.GameObjects.Debug
{
    public class DebugProfilerContainer
    {
        private Dictionary<Type, IDebugProfiler> profilers = new Dictionary<Type, IDebugProfiler>();

        public void RegisterProfiler(Type targetType, IDebugProfiler profiler)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            else if (!typeof(GameObject).IsAssignableFrom(targetType))
            {
                throw new ArgumentException("Specified target type is not a GameObject.");
            }
            if (profiler == null)
            {
                throw new ArgumentNullException(nameof(profiler));
            }

            profilers.Add(targetType, profiler);
        }

        public void RegisterProfiler<T>(IDebugProfiler profiler)
        {
            RegisterProfiler(typeof(T), profiler);
        }

        public void UnregisterProfiler(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            else if (!typeof(GameObject).IsAssignableFrom(targetType))
            {
                throw new ArgumentException("Specified target type is not a GameObject.");
            }

            profilers.Remove(targetType);
        }

        public void UnregisterProfiler<T>()
        {
            UnregisterProfiler(typeof(T));
        }

        public IDebugProfiler GetProfiler(Type targetType)
        {
            IDebugProfiler profiler;
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (!typeof(GameObject).IsAssignableFrom(targetType))
            {
                return null;
            }
            else if (profilers.TryGetValue(targetType, out profiler))
            {
                return profiler;
            }
            else
            {
                return GetProfiler(targetType.BaseType);
            }
        }

        public IDebugProfiler GetProfiler<T>()
        {
            return GetProfiler(typeof(T));
        }
    }
}