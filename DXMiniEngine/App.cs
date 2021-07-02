using Silk.NET.GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.DXGI;

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

        public App()
        {
            CreateWindow();
            InitializeDX();
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
                DrawScene();
                Dispose();
            }
        }

        void InitializeDX()
        {
            var glfwWindow = new GlfwNativeWindow(glfw, windowHandle);

            IntPtr hwnd = glfwWindow.Win32.Value.Hwnd;

            GraphicsDevice = new(hwnd, new(width, height));
        }



        void UpdateScene()
        {

        }

        void DrawScene()
        {
            if(GraphicsDevice is not null)
            {
                GraphicsDevice.DeviceContext.Flush();
            }
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
