using System;

namespace Innovare.Shop.Business.Abstractions
{
    public abstract class ExceptionBase : Exception
    {
        public ExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExceptionBase(string message) : base(message)
        {
        }
    }
}