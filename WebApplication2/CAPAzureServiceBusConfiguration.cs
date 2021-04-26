using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class CAPAzureServiceBusConfiguration
    {
        public string ConnectionString { get; set; }
        public string TopicPath { get; set; } //mb2-dev
    }

    public class SqlDatabaseConfiguration
    {
        public string MbDataServicesConnectionString { get; set; }
    }
}
