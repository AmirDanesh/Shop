using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Data.Configurations.DbContext
{
    public class DbContextOptions
    {
        public bool IsEnabled { get; set; }

        public string ConnectionString { get; set; }
    }
}