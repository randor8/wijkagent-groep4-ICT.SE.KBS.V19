﻿namespace WijkagentWPF
{
    public interface IObservable
    {
        void Attach(IObserver observer);

        void Notify();
    }
}