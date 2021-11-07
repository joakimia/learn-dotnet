using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ELDEL.Helpers
{
    // Reduce unnecesarry logs to App insights: https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling#filtering
    public class ApplicationInsightTelemetryFilterProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // next will point to the next TelemetryProcessor in the chain.
        public ApplicationInsightTelemetryFilterProcessor(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            // To filter out an item, return without calling the next processor.
            if (!IsOkToSend(item))
            {
                return;
            }

            this.Next.Process(item);
        }


        private bool IsOkToSend(ITelemetry item)
        {
            // Remove dependency telemetry ( logs regarding initializing azure storage queue/table etc. ).
            if (item is DependencyTelemetry)
            {
                return false;
            }
            // Remove http requests telemetry
            if (item is RequestTelemetry)
            {
                return false;
            }
            // Remove default logging by Entity Framework ( SQL Database commands, initializing context etc ).
            if (item is TraceTelemetry trace)
            {
                return !trace.Properties["CategoryName"].Contains("Microsoft.EntityFrameworkCore");
            }
            return true;
        }
    }
}
