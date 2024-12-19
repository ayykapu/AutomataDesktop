using System;
using System.Runtime.CompilerServices;
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

        static int _cellSize = 1; //5
        static List<byte[,]> _generationList;
        static int[] _currentGeneration; //this is where non 0s goes

        static int currentGenerationIndex = 0;

        public Window(int width, int height, string title, List<byte[,]> generations) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            _width = width;
            _height = height;
            _generationList = generations;

            SetWindowIcon("icon.ico"); // Set the icon file path
            CenterWindow();
        }
        private static int[] TranslateBytesIntoInts(byte[,] generation)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < generation.GetLength(0); i++)
            {
                for (int j = 0; j < generation.GetLength(1); j++)
                {
                    if (generation[i, j] != 0)
                    {
                        for (int yOffset = 0; yOffset < _cellSize; yOffset++)
                        {
                            for (int xOffset = 0; xOffset < _cellSize; xOffset++)
                            {
                                result.Add(j * _cellSize + xOffset);
                                result.Add(i * _cellSize + yOffset);
                            }
                        }
                    }
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
        private void SetWindowIcon(string iconPath)
        {
            //todo
        }
        private void SetDefaultCellsSize()
        {
            int generationWidth = _generationList[0].GetLength(1);
            int generationHeiht = _generationList[0].GetLength(0);

            _cellSize = _width / generationWidth;
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            SetDefaultCellsSize();

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

            ////////
            //if (KeyboardState.IsKeyReleased(Keys.Up))
            if (KeyboardState.IsKeyDown(Keys.Up))
            {


                if (currentGenerationIndex < _generationList.Count - 1)
                {
                    currentGenerationIndex++;
                }
            }

            //if (KeyboardState.IsKeyReleased(Keys.Down))
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                if (currentGenerationIndex > 0)
                {
                    currentGenerationIndex--;
                }
            }

            _currentGeneration = TranslateBytesIntoInts(_generationList[currentGenerationIndex]);
            float[] vertices = ConvertToNDC(_currentGeneration, _width, _height);

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
            GL.DrawArrays(PrimitiveType.Points, 0, _currentGeneration.Length / 2);
            GL.BindVertexArray(0);

            SwapBuffers();
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