namespace MhLabs.DataAccessIoC.Abstraction
{
    public abstract class HandlerBase<T> : IHandler
    {
        protected T Repository { get; }

        protected HandlerBase(T repository)
        {
            Repository = repository;
        }
    }
}
