namespace Base
{
    public interface IInjectable<in T> : IInjectable
    {
        public void Inject(T reference);
    }

    public interface IInjectable
    {
        
    }
}
