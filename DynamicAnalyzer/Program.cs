// See https://aka.ms/new-console-template for more information

using DynamicAnalyzer.Aspects;

// Add logging to every method in the assembly.
[assembly: LogMethodExecutionResourceMonitoring(AttributePriority = 0)]
// Remove logging from the Aspects namespace to avoid infinite recursions (logging would log itself).
[assembly:
    LogMethodExecutionResourceMonitoring(AttributePriority = 1, AttributeExclude = true,
        AttributeTargetTypes = "DynamicAnalyzer.Aspects.*")]

namespace DynamicAnalyzer
{
    internal static class Program
    {
        [LogMethodExecutionResourceMonitoring(AttributePriority = 2, AttributeExclude = true)]
        private static void Main()
        {
            ShortTask();
            ShortTaskWithParams(1);
            LongTask();
            ShortTaskWithAllocation();
            LongTaskWithAllocation();
        }
    
        private static void LongTask()
        {
            Thread.Sleep(5000);
        }

        private static void ShortTask()
        {
            Thread.Sleep(1000);

        }
        
        private static void LongTaskWithAllocation()
        {
            List<MyObject> objects = new List<MyObject>();

            for (int i = 0; i < 1; i++)
            {
                MyObject obj = new MyObject(i);
                objects.Add(obj);
                // Simula o uso de memória sem liberar os objetos
            }
            Thread.Sleep(4000);

        }

        private static void ShortTaskWithAllocation()
        {
            List<MyObject> objects = new List<MyObject>();

            for (int i = 0; i < 1; i++)
            {
                MyObject obj = new MyObject(i);
                objects.Add(obj);
                // Simula o uso de memória sem liberar os objetos
            }
            Thread.Sleep(2000);

        }
        private static int ShortTaskWithParams(int a)
        {
            Thread.Sleep(2000);
            return a;
        }
    }
    [LogMethodExecutionResourceMonitoring(AttributePriority = 3, AttributeExclude = true)]
    class MyObject
    {
        public int Id { get; }
        public string Data { get; }
        public MyObject(int id)
        {
            Id = id;
            Data = new string('A', 10000); // Alocação de memória
        }
    }
}

