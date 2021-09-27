using Innovare.Shop.Business.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Exceptions
{
    public sealed class BusinessValidationException : ExceptionBase
    {
        public BusinessValidationException(string message) : base(message)
        {
        }
    }
}