using FubuCore.Reflection;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Diagnostics.Features;

namespace FubuMVC.Diagnostics.Core.Configuration
{
    public static class ActionCallExtensions
    {
        public static bool IsDiagnosticsHandler(this ActionCall call)
        {
            var diagnosticsAssembly = typeof(DiagnosticsFeatures).Assembly;
            return HandlersUrlPolicy.IsHandlerCall(call)
                   && call.HandlerType.Assembly.Equals(diagnosticsAssembly)
                   && !call.HasAttribute<FubuDiagnosticsUrlAttribute>();
        }

        public static bool IsDiagnosticsCall(this ActionCall call)
        {
            return call.IsDiagnosticsHandler() || call.Method.HasAttribute<FubuDiagnosticsUrlAttribute>();
        }
    }
}