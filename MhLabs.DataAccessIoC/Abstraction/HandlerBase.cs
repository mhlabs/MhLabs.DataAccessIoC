using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Runtime;
using MhLabs.DataAccessIoC.AWS;

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
