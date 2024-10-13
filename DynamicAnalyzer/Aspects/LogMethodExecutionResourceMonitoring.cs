using System.Diagnostics;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using System.Text;
 
namespace DynamicAnalyzer.Aspects;
/// <summary>
/// Aspect that, when applied, ...
/// </summary>
///
[PSerializable]
public class LogMethodExecutionResourceMonitoring : OnMethodBoundaryAspect
{
    private long _startMemory;

    public override void OnEntry(MethodExecutionArgs args)
    {
        args.MethodExecutionTag = Stopwatch.StartNew();
        _startMemory = GC.GetTotalMemory(false);
        Console.WriteLine($"Entering method {args.Method.Name}");
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        Stopwatch stopwatch = (Stopwatch)args.MethodExecutionTag;
        var endMemory = GC.GetTotalMemory(false);

        LogMethod(args.Method, stopwatch.ElapsedMilliseconds, endMemory - _startMemory);
    }

    private static void LogMethod(MethodBase method, long elapsedMilliseconds, long memoryUsage)
    {
        StringBuilder message = new StringBuilder();
        message.Append($"Method {method.Name} executed in {elapsedMilliseconds} ms. ");
        message.Append($"Memory used: {memoryUsage} bytes");
        Console.WriteLine(message.ToString());
    }
}



// // Add logging to every method in the assembly.
// [assembly: LogMethodExecutionResourceMonitoring(AttributePriority = 0)]
//
// // Remove logging from the Aspects namespace to avoid infinite recursions (logging would log itself).
// [assembly:
//     LogMethodExecutionResourceMonitoring(AttributePriority = 1, AttributeExclude = true,
//         AttributeTargetTypes = "DynamicAnalyzer.Aspects.*")]

// [LogMethodExecutionResourceMonitoring(AttributePriority = 2, AttributeExclude = true)]