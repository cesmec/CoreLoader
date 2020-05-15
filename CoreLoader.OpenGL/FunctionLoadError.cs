using System;

namespace CoreLoader.OpenGL
{
    public class FunctionLoadError
    {
        public Exception Exception { get; }
        public string FunctionName { get; }

        public FunctionLoadError(Exception exception, string functionName)
        {
            Exception = exception;
            FunctionName = functionName;
        }
    }
}