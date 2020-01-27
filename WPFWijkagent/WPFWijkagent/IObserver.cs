namespace WijkagentWPF
{
    public interface IObserver
    {
        public void Update(IObservable observable);
    }
}