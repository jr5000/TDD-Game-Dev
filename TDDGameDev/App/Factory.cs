namespace App
{
    public interface Factory<out T, in TArgs>
    {
        T Create(TArgs args);
    }
}