public interface IReactiveProperty<T> {
    void Subscribe(IObserverProperty<T> observer);
    void Unsubscribe(IObserverProperty<T> observer);
    void NotifyObservers();
}
