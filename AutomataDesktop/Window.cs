using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AutomataDesktop
{
    public class Window : GameWindow
    {
        private int _width;
        private int _height;
        private int _vao;
        private int _vbo;

        //static private float pixelSize = 1; //5
        static List<byte[,]> _generations;
        static private int[] _seed;

        static int currentGenerationIndex = 0;

        public Window(int width, int height, string title, List<byte[,]> generations) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            _width = width;
            _height = height;
            _generations = generations;

            CenterWindow();
        }
        private static void UpdateSeed()
        {
            Console.WriteLine($"Generation: {currentGenerationIndex}");
            int[] newSeed = TranslateBytes(_generations[currentGenerationIndex]);

            if (currentGenerationIndex != _generations.Count - 1)
            {
                currentGenerationIndex++;
            }

            _seed = newSeed;
        }
        private static int[] TranslateBytes(byte[,] generation)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < generation.GetLength(0); i++)
            {
                for (int j = 0; j < generation.GetLength(1); j++)
                {
                    result.Add(j + 2);
                    result.Add(i + 2);
                }
            }

            return result.ToArray();
        }
        private float[] ConvertToNDC(int[] pixelCoordinates, int screenWidth, int screenHeight)
        {
            float[] ndcCoordinates = new float[pixelCoordinates.Length];
            for (int i = 0; i < pixelCoordinates.Length; i += 2)
            {
                int xPixel = pixelCoordinates[i];
                int yPixel = pixelCoordinates[i + 1];

                ndcCoordinates[i] = (xPixel + 0.5f) / screenWidth * 2.0f - 1.0f;
                ndcCoordinates[i + 1] = 1.0f - (yPixel + 0.5f) / screenHeight * 2.0f;
            }

            return ndcCoordinates;
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(1f, 1f, 1f, 1f);
            GL.Enable(EnableCap.ProgramPointSize);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            UpdateSeed();
            float[] vertices = ConvertToNDC(_seed, _width, _height);
            //int numberOfSquares = _seed.Length / 2;
            //Console.WriteLine($"Expected number of squares: {numberOfSquares}");

            //for (int i = 0; i < _seed.Length; i++)
            //{
            //    Console.WriteLine(_seed[i] + " -> " + vertices[i]);
            //}

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(_vao);
            GL.PointSize(1);
            GL.DrawArrays(PrimitiveType.Points, 0, _seed.Length / 2);
            GL.BindVertexArray(0);

            SwapBuffers();

            //////////////
            Console.ReadLine();
            /////////////
        }
        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            this._width = e.Width;
            this._height = e.Height;
        }
    }
}