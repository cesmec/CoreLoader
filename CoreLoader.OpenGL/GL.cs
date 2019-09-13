using System.Numerics;
using System.Runtime.CompilerServices;

namespace CoreLoader.OpenGL
{
    public unsafe partial class GL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShaderSource(uint shader, string source)
        {
            var length = source.Length;
            GlNative.ShaderSource(shader, 1, new[] { source }, &length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, in Matrix4x4 value)
        {
            fixed (float* first = &value.M11)
            {
                GlNative.ProgramUniformMatrix4fv(program, location, count, transpose, first);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UniformMatrix4fv(int location, int count, bool transpose, in Matrix4x4 value)
        {
            fixed (float* first = &value.M11)
                GlNative.UniformMatrix4fv(location, count, transpose, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProgramUniform1fv(uint program, int location, int count, float value)
        {
            GlNative.ProgramUniform1fv(program, location, count, &value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProgramUniform2fv(uint program, int location, int count, in Vector2 value)
        {
            fixed (float* first = &value.X)
                GlNative.ProgramUniform2fv(program, location, count, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProgramUniform3fv(uint program, int location, int count, in Vector3 value)
        {
            fixed (float* first = &value.X)
                GlNative.ProgramUniform3fv(program, location, count, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProgramUniform4fv(uint program, int location, int count, in Vector4 value)
        {
            fixed (float* first = &value.X)
                GlNative.ProgramUniform4fv(program, location, count, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Uniform1fv(int location, int count, float value)
        {
            GlNative.Uniform1fv(location, count, &value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Uniform2fv(int location, int count, in Vector2 value)
        {
            fixed (float* first = &value.X)
                GlNative.Uniform2fv(location, count, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Uniform3fv(int location, int count, in Vector3 value)
        {
            fixed (float* first = &value.X)
                GlNative.Uniform3fv(location, count, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Uniform4fv(int location, int count, in Vector4 value)
        {
            fixed (float* first = &value.X)
                GlNative.Uniform4fv(location, count, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BufferData(uint target, long size, void* data, BufferDataUsage usage)
        {
            GlNative.BufferData(target, size, data, (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BufferData<T>(uint target, T[] data, BufferDataUsage usage) where T : unmanaged
        {
            var size = data.Length * sizeof(T);
            fixed(T* first = data)
                GlNative.BufferData(target, size, first, (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NamedBufferData(uint buffer, long size, void* data, BufferDataUsage usage)
        {
            GlNative.NamedBufferData(buffer, size, data, (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NamedBufferData<T>(uint buffer, T[] data, BufferDataUsage usage) where T : unmanaged
        {
            var size = data.Length * sizeof(T);
            fixed (T* first = data)
                GlNative.NamedBufferData(buffer, size, first, (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ActiveTexture(uint texture)
        {
            GlNative.ActiveTexture(texture);
        }
    }

    public enum BufferDataUsage : uint
    {
        StreamDraw = 35040u,
        StreamRead = 35041u,
        StreamCopy = 35042u,
        StaticDraw = 35044u,
        StaticRead = 35045u,
        StaticCopy = 35046u,
        DynamicDraw = 35048u,
        DynamicRead = 35049u,
        DynamicCopy = 35050u
    }
}