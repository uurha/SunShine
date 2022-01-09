using CorePlugin.ReferenceDistribution.Interface;

namespace Base
{
    public interface IProvider<out T>
    {
        public T Provided { get; }
    }
}
