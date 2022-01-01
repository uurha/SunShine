using CorePlugin.ReferenceDistribution.Interface;

namespace Base
{
    public interface IProvider<out T> : IDistributingReference
    {
        public T Provided { get; }
    }
}
