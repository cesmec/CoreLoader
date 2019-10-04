﻿using System.Numerics;
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
        public static uint CreateRenderbuffer()
        {
            uint renderbuffer;
            GlNative.CreateRenderbuffers(1, &renderbuffer);
            return renderbuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] CreateRenderbuffers(int count)
        {
            var renderbuffers = new uint[count];
            fixed (uint* firstRenderbuffer = renderbuffers)
                GlNative.CreateRenderbuffers(count, firstRenderbuffer);
            return renderbuffers;
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
        public static void DeleteFramebuffer(uint framebuffer)
        {
            GlNative.DeleteFramebuffers(1, &framebuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteFramebuffers(uint[] framebuffers)
        {
            fixed (uint* firstFramebuffer = framebuffers)
                GlNative.DeleteBuffers(framebuffers.Length, firstFramebuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteRenderbuffer(uint renderbuffer)
        {
            GlNative.DeleteRenderbuffers(1, &renderbuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteRenderbuffers(uint[] renderbuffers)
        {
            fixed (uint* firstRenderbuffer = renderbuffers)
                GlNative.DeleteRenderbuffers(renderbuffers.Length, firstRenderbuffer);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BlendFunc(BlendFuncFactor sfactor, BlendFuncFactor dfactor)
        {
            GlNative.BlendFunc((uint)sfactor, (uint)dfactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BlendFunci(uint buf, BlendFuncFactor sfactor, BlendFuncFactor dfactor)
        {
            GlNative.BlendFunci(buf, (uint)sfactor, (uint)dfactor);
        }
    }

    public enum BufferDataUsage : uint
    {
        StreamDraw = GlConsts.GL_STREAM_DRAW,
        StreamRead = GlConsts.GL_STREAM_READ,
        StreamCopy = GlConsts.GL_STREAM_COPY,
        StaticDraw = GlConsts.GL_STATIC_DRAW,
        StaticRead = GlConsts.GL_STATIC_READ,
        StaticCopy = GlConsts.GL_STATIC_COPY,
        DynamicDraw = GlConsts.GL_DYNAMIC_DRAW,
        DynamicRead = GlConsts.GL_DYNAMIC_READ,
        DynamicCopy = GlConsts.GL_DYNAMIC_COPY
    }

    public enum BlendFuncFactor : uint
    {
        Zero = GlConsts.GL_ZERO,
        One = GlConsts.GL_ONE,
        SrcColor = GlConsts.GL_SRC_COLOR,
        OneMinusSrcColor = GlConsts.GL_ONE_MINUS_SRC_COLOR,
        DstColor = GlConsts.GL_DST_COLOR,
        OneMinusDstColor = GlConsts.GL_ONE_MINUS_DST_COLOR,
        SrcAlpha = GlConsts.GL_SRC_ALPHA,
        OneMinusSrcAlpha = GlConsts.GL_ONE_MINUS_SRC_ALPHA,
        DstAlpha = GlConsts.GL_DST_ALPHA,
        OneMinusDstAlpha = GlConsts.GL_ONE_MINUS_DST_ALPHA,
        ConstantColor = GlConsts.GL_CONSTANT_COLOR,
        OneMinusConstantColor = GlConsts.GL_ONE_MINUS_CONSTANT_COLOR,
        ConstantAlpha = GlConsts.GL_CONSTANT_ALPHA,
        OneMinusConstantAlpha = GlConsts.GL_ONE_MINUS_CONSTANT_ALPHA
    }
}