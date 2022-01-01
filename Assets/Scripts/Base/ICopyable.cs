namespace Base
{
    public interface ICopyable<out T>
    {
        public T Copy();
    }
}
