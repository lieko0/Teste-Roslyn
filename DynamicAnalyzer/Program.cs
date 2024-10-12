// See https://aka.ms/new-console-template for more information

using DynamicAnalyzer.Aspects;

[assembly: LogMethodExecutionDuration(AttributePriority = 0)]
[assembly:
    LogMethodExecutionDuration(AttributePriority = 1, AttributeExclude = true,
        AttributeTargetTypes = "DynamicAnalyzer.Aspects.*")]

namespace DynamicAnalyzer
{
    internal static class Program
    {
        [LogMethodExecutionDuration(AttributePriority = 2, AttributeExclude = true)]
        private static void Main()
        {
            LongTask();
            ShortTask();
        }
    
        private static void LongTask()
        {
            Thread.Sleep(5000);
        }

        private static void ShortTask()
        {
            Thread.Sleep(1000);
        }
    }
}

