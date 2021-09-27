using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Configurations.Validation
{
    public sealed class ValidationOptions
    {
        public bool IsEnabled { get; set; }

        public bool RunDefaultMvcValidationAfterFluentValidationExecutes { get; set; }

        public bool AutomaticValidationEnabled { get; set; }
    }
}