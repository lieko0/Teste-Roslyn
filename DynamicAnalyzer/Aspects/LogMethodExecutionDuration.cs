using System.Diagnostics;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using System.Text;
 
namespace DynamicAnalyzer.Aspects;
/// <summary>
/// Aspect that, when applied, adds a counter to the method that
/// evaluates the execution time of the method.
/// </summary>
/// 
[PSerializable]
public class LogMethodExecutionDuration : OnMethodBoundaryAspect
{
    
    public override void OnEntry(MethodExecutionArgs args)
    {
        args.MethodExecutionTag = Stopwatch.StartNew();
    }
    
    public override void OnExit(MethodExecutionArgs args)
    {
        Stopwatch stopwatch = (Stopwatch) args.MethodExecutionTag;
        LogMethod(args.Method, stopwatch.ElapsedMilliseconds);
    }
    
    private static void LogMethod(MethodBase method, long elapsedMilliseconds)
    {
        StringBuilder message = new StringBuilder();
        message.Append("Method ");
        message.Append(method.Name);
        message.Append(" executed in ");
        message.Append(elapsedMilliseconds);
        message.Append(" ms.");
        Console.WriteLine(message.ToString());
    }
}