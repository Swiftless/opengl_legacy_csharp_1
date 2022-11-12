﻿using Silk.NET.Maths;
using Silk.NET.OpenGL.Legacy;
using Silk.NET.Windowing;

using System.Numerics;

namespace _2;

public class Program
{
    private static IWindow window;
    private static GL openGLApi;

    private static void Main(string[] args)
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.Position = new Vector2D<int>(100, 100);
        windowOptions.Size = new Vector2D<int>(500, 500);
        windowOptions.Title = "Your first OpenGL Window";

        window = Window.Create(windowOptions);

        window.Load += OnLoad;
        window.Update += OnUpdate;
        window.Render += OnRender;
        window.Resize += OnResize;

        window.Run();
    }

    private static void OnLoad()
    {
        openGLApi = GL.GetApi(window);
    }

    private static void OnRender(double obj)
    {
        openGLApi.ClearColor(0.0f, 0.5f, 1.0f, 1.0f);
        openGLApi.Clear(ClearBufferMask.ColorBufferBit);

        openGLApi.LoadIdentity();

        openGLApi.Flush();
    }

    private static void OnUpdate(double obj) { }

    private static void OnResize(Vector2D<int> windowSize)
    {
        openGLApi.Viewport(0, 0, (uint)windowSize.X, (uint)windowSize.Y);
        openGLApi.MatrixMode(GLEnum.Projection);

        var projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView((float)((Math.PI / 180f) * 60f), windowSize.X / windowSize.Y, 0.1f, 100.0f);
        openGLApi.LoadMatrix(new float[] { 
            projectionMatrix.M11, projectionMatrix.M12, projectionMatrix.M13, projectionMatrix.M14,
            projectionMatrix.M21, projectionMatrix.M22, projectionMatrix.M23, projectionMatrix.M24,
            projectionMatrix.M31, projectionMatrix.M32, projectionMatrix.M33, projectionMatrix.M34,
            projectionMatrix.M41, projectionMatrix.M42, projectionMatrix.M43, projectionMatrix.M44
        });

        openGLApi.MatrixMode(GLEnum.Modelview);
    }
}