namespace Base.Inject
{
    public interface IInjectReceiver
    {
    }
    
    public interface IInjectReceiver<in T> : IInjectReceiver
    {
        public void Inject(T reference);
    }
}
