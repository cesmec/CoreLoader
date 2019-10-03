using System.Numerics;
using System.Runtime.CompilerServices;

namespace CoreLoader.OpenGL
{
    public unsafe partial class GL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CreateVertexArray()
        {
            uint vao;
            GlNative.CreateVertexArrays(1, &vao);
            return vao;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] CreateVertexArrays(int count)
        {
            var vaos = new uint[count];
            fixed (uint* firstVao = vaos)
                GlNative.CreateVertexArrays(count, firstVao);
            return vaos;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CreateBuffer()
        {
            uint buffer;
            GlNative.CreateBuffers(1, &buffer);
            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] CreateBuffers(int count)
        {
            var buffers = new uint[count];
            fixed (uint* firstBuffer = buffers)
                GlNative.CreateBuffers(count, firstBuffer);
            return buffers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CreateTexture(uint target)
        {
            uint texture;
            GlNative.CreateTextures(target, 1, &texture);
            return texture;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] CreateTextures(uint target, int count)
        {
            var textures = new uint[count];
            fixed (uint* firstTexture = textures)
                GlNative.CreateTextures(target, count, firstTexture);
            return textures;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CreateFramebuffer()
        {
            uint framebuffer;
            GlNative.CreateFramebuffers(1, &framebuffer);
            return framebuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] CreateFramebuffers(int count)
        {
            var framebuffers = new uint[count];
            fixed (uint* firstFramebuffer = framebuffers)
                GlNative.CreateBuffers(count, firstFramebuffer);
            return framebuffers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteVertexArray(uint vao)
        {
            GlNative.DeleteVertexArrays(1, &vao);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteVertexArrays(uint[] vaos)
        {
            fixed (uint* firstVao = vaos)
                GlNative.DeleteVertexArrays(vaos.Length, firstVao);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteBuffer(uint buffer)
        {
            GlNative.DeleteBuffers(1, &buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteBuffers(uint[] buffers)
        {
            fixed (uint* firstBuffer = buffers)
                GlNative.DeleteBuffers(buffers.Length, firstBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteTexture(uint texture)
        {
            GlNative.DeleteTextures(1, &texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteTextures(uint[] textures)
        {
            fixed (uint* firstTexture = textures)
                GlNative.DeleteTextures(textures.Length, firstTexture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DeleteFramebuffer()
        {
            uint framebuffer;
            GlNative.DeleteFramebuffers(1, &framebuffer);
            return framebuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] DeleteFramebuffers(int count)
        {
            var framebuffers = new uint[count];
            fixed (uint* firstFramebuffer = framebuffers)
                GlNative.DeleteBuffers(count, firstFramebuffer);
            return framebuffers;
        }

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
            BufferData(target, data, data.Length * sizeof(T), usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BufferData<T>(uint target, T[] data, long size, BufferDataUsage usage) where T : unmanaged
        {
            fixed (T* first = data)
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
            NamedBufferData(buffer, data, data.Length * sizeof(T), usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NamedBufferData<T>(uint buffer, T[] data, long size, BufferDataUsage usage) where T : unmanaged
        {
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