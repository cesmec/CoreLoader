# CoreLoader

Cross-Platform Window Library for .Net Core with OpenGL support.

## Usage

```csharp
using CoreLoader;
using CoreLoader.OpenGL;
```

```csharp
var window = new Window("Window Title", initialWidth, initialHeight);
window.UseOpenGL();
window.Show();
```

After `window.Show()` has been called, the OpenGL functions can be used.

The functions are defined as static members of the GL class. Most of them use enums as parameters, others use the constants defined in the GlConsts class.

Examples:

```csharp
GL.Enable(GlConsts.GL_DEPTH_TEST);

GL.Clear(ClearMask.ColorBufferBit | ClearMask.DepthBufferBit);
```

## How to update OpenGL functions

- Make changes to GL.cs or GlNative.cs
- Run CoreLoader.Generator.OpenGL
