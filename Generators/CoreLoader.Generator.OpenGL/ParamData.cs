using System;

namespace CoreLoader.Generator.OpenGL
{
    public class ParamData
    {
        public string Name { get; }
        public string TypeName { get; }
        public Type Type { get; }
        public EnumType EnumType { get; set; }

        public ParamData(string name, string typeName, Type type, EnumType enumType = EnumType.None)
        {
            Name = name;
            TypeName = typeName;
            Type = type;
            EnumType = enumType;
        }
    }
}