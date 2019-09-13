using System;

namespace CoreLoader.OpenGL.Events
{
    public class LoadErrorEvent
    {
        public Exception Exception { get; }
        public string FunctionName { get; }

        public LoadErrorEvent(Exception exception, string functionName)
        {
            Exception = exception;
            FunctionName = functionName;
        }
    }
}