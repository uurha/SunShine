using System;
using CorePlugin.Logger;
using CorePlugin.ReferenceDistribution;
using CorePlugin.ReferenceDistribution.Interface;

namespace Base.Inject
{
    public static class InjectExtensions
    {
        public static T InjectReference<T>(this IInjectReceiver<T> receiver) where T : IDistributingReference
        {
            if (ReferenceDistributor.TryGetReference(out T reference))
            {
                receiver.Inject(reference);
            }
            else
            {
                DebugLogger.LogException(new NullReferenceException($"{typeof(T).Name} missing in scene"));
            }
            return reference;
        }

        public static void Inject<T>(this IInjectReceiver<T> receiver) where T : IDistributingReference
        {
            if (ReferenceDistributor.TryGetReference(out T reference))
            {
                receiver.Inject(reference);
            }
            else
            {
                DebugLogger.LogException(new NullReferenceException($"{typeof(T).Name} missing in scene"));
            }
        }
    }
}
