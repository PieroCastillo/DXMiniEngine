using Silk.NET.GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.D3DCompiler;
using static Vortice.Direct3D11.D3D11;
using static Vortice.DXGI.DXGI;

namespace DXMiniEngine
{
    public unsafe class App : IDisposable
    {
        string title = "DX App";
        int width = 800;
        int height = 600;

        Glfw glfw = Glfw.GetApi();
        WindowHandle* windowHandle;

        D3D11GraphicsDevice GraphicsDevice;

        int red, green, blue = 1;

        ID3D11Buffer triangleVertBuffer;
        ID3D11VertexShader VS;
        ID3D11PixelShader PS;
        ID3D11Buffer VS_Buffer;
        ID3D11Buffer PS_Buffer;
        ID3D11InputLayout vertLayout;

        public App()
        {
            CreateWindow();
            InitializeDX();
            LoadObjects();
            MainLoop();
        }

        void CreateWindow()
        {
            glfw.Init();
            glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);

            windowHandle = glfw.CreateWindow(width, height, title, (Monitor*)IntPtr.Zero.ToPointer(), null);
        }
        void MainLoop()
        {
            while (!glfw.WindowShouldClose(windowHandle))
            {
                UpdateScene();
                GraphicsDevice.DrawFrame((x, y) =>
                {
                    DrawScene(x,y);
                });

                glfw.PollEvents();
            }
        }

        void InitializeDX()
        {
            var glfwWindow = new GlfwNativeWindow(glfw, windowHandle);

            IntPtr hwnd = glfwWindow.Win32.Value.Hwnd;

            GraphicsDevice = new(hwnd, new(width, height));
        }

        void LoadObjects()
        {
           Compiler.CompileFromFile()
        }

        void UpdateScene()
        {
            //Update the colors of our scene
            red = red * 5;
            green = green * 3;
            blue = blue * 2;

            if (red >= 255 || red <= 0)
               red = 1;
            if (green >= 255 || green <= 0)
                green = 1;
            if (blue >= 255 || blue <= 0)
                blue = 1;
        }

        void DrawScene(int width, int height)
        {
            GraphicsDevice.DeviceContext.Flush();

            var color = Color.FromArgb(red,green,blue);

            GraphicsDevice.DeviceContext.ClearRenderTargetView(GraphicsDevice.RenderTargetView, color);
        }

        public void Dispose()
        {
            if(GraphicsDevice is not null)
            {
                GraphicsDevice.Dispose();
            }
        }
    }
}
