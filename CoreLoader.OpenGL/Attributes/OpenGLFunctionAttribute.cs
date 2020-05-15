using System;

namespace CoreLoader.OpenGL.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class OpenGLFunctionAttribute : Attribute
    {
        public string Name { get; }

        public OpenGLFunctionAttribute(string name)
        {
            Name = name;
        }
    }
}