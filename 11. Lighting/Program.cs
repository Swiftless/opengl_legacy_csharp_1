﻿using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Legacy;
using Silk.NET.Windowing;

using System.Numerics;

namespace _11;

public class Program
{
    private static IWindow window;
    private static GL openGLApi;

    private static float sceneRotationAngle = 0.0f;
    private static float sceneTranslationY = 0.0f;
    private static bool movingUp = true;

    private static void Main(string[] args)
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.API = new GraphicsAPI(ContextAPI.OpenGL, new APIVersion(2, 1));
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

        // Iterate over all keyboards and bind to key down event
        IInputContext input = window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
        {
            input.Keyboards[i].KeyDown += OnKeyDown;
        }

        OnResize(window.Size);
    }

    private static void OnRender(double obj)
    {
        openGLApi.Enable(EnableCap.DepthTest);

        openGLApi.Enable(EnableCap.ColorMaterial);
        openGLApi.Enable(EnableCap.Lighting);
        openGLApi.Enable(EnableCap.Light0);

        openGLApi.ClearColor(0.0f, 0.5f, 1.0f, 1.0f);
        openGLApi.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        openGLApi.LoadIdentity();

        openGLApi.Translate(0.0f, 0.0f, -5.0f); // Push eveything 5 units back into the scene, otherwise we won't see the primitive  
        openGLApi.Rotate(sceneRotationAngle, 1, 1, 0); // Rotate everything -60 degrees on the X and Y axis, to show off multiple sides of the cube

        openGLApi.Scale(0.5f, 1.0f, 2.0f); // Make the shape half as wide, the same height and twice as deep  

        openGLApi.Begin(GLEnum.Quads); // Start drawing a quad primitive  

        foreach (var side in Objects.Cube.Vertices)
        {
            var colour = Objects.Cube.Colours[side.Key];
            openGLApi.Color3(colour.X, colour.Y, colour.Z);

            var normal = Objects.Cube.Normals[side.Key];
            openGLApi.Normal3(normal.X, normal.Y, normal.Z);

            foreach (var vertex in side.Value)
            {
                openGLApi.Vertex3(vertex.X, vertex.Y, vertex.Z);
            }
        }

        openGLApi.End();

        openGLApi.Flush();
    }

    private static void OnUpdate(double obj)
    {
        if (sceneRotationAngle > 360f)
            sceneRotationAngle = 0f;
        else
            sceneRotationAngle += 0.5f;

        if (sceneTranslationY > 4f)
        {
            movingUp = false;
            sceneTranslationY = 4f;
        }
        else if (sceneTranslationY < -4f)
        {
            movingUp = true;
            sceneTranslationY = -4f;
        }
        else
        {
            if (movingUp)
                sceneTranslationY += 0.05f;
            else
                sceneTranslationY -= 0.05f;
        }
    }

    private static void OnResize(Vector2D<int> windowSize)
    {
        openGLApi.Viewport(0, 0, (uint)windowSize.X, (uint)windowSize.Y);
        openGLApi.MatrixMode(GLEnum.Projection);

        var projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView((MathF.PI / 180f) * 60f, windowSize.X / (float)windowSize.Y, 0.1f, 100.0f);

        openGLApi.LoadMatrix(new float[] {
            projectionMatrix.M11, projectionMatrix.M12, projectionMatrix.M13, projectionMatrix.M14,
            projectionMatrix.M21, projectionMatrix.M22, projectionMatrix.M23, projectionMatrix.M24,
            projectionMatrix.M31, projectionMatrix.M32, projectionMatrix.M33, projectionMatrix.M34,
            projectionMatrix.M41, projectionMatrix.M42, projectionMatrix.M43, projectionMatrix.M44
        });

        openGLApi.MatrixMode(GLEnum.Modelview);
    }

    private static void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
    {
        if (key == Key.Escape)
        {
            window.Close();
        }

        if (key == Key.W)
        {
            window.Position = new Vector2D<int>(100, 50);
            window.Size = new Vector2D<int>(500, 600);
        }

        if (key == Key.S)
        {
            window.Position = new Vector2D<int>(100, 100);
            window.Size = new Vector2D<int>(500, 500);
        }
    }
}