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
using Vortice.Direct3D;
using System.Runtime.CompilerServices;
using Vortice.Mathematics;

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

        string pxshader = @".\Shaders\PixelShader.fx";
        string vsshader = @".\Shaders\VertexShader.fx";

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
            Compiler.CompileFromFile(vsshader, "main", "vs_5_0", out Blob vsBlob, out Blob vsError);
            if (vsBlob == null)
            {
                throw new Exception($"Failed to compile HLSL code: {Encoding.ASCII.GetString(vsError.GetBytes())}");
            }

            Compiler.CompileFromFile(pxshader, "main", "ps_5_0", out Blob psBlob, out Blob psError);
            if (vsBlob == null)
            {
                throw new Exception($"Failed to compile HLSL code: {Encoding.ASCII.GetString(psError.GetBytes())}");
            }

            GraphicsDevice.DeviceContext.VSSetShader(new ID3D11VertexShader(vsBlob.NativePointer));
            GraphicsDevice.DeviceContext.PSSetShader(new ID3D11PixelShader(psBlob.NativePointer));

            Vertex[] v =
            {
               new Vertex( 0.0f, 0.5f, 0.5f),
               new Vertex( 0.5f, -0.5f, 0.5f),
               new Vertex( -0.5f, -0.5f, 0.5f)
            };

            BufferDescription bufferDescription = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = sizeof(Vertex) * 3,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            IntPtr data;

            fixed (Vertex* ptr = v)
                data = new(ptr);

            SubresourceData vertexBufferData = new(data);

            var vertexBuffer = GraphicsDevice.Device.CreateBuffer(bufferDescription, vertexBufferData);

            int stride = sizeof(Vertex);
            int offset = 0;

            InputElementDescription[] layout = new[]
            {
                new InputElementDescription("POSITION", 0, Format.R16G16B16A16_Float, 0, 0, InputClassification.PerVertexData, 0)
            };

            GraphicsDevice.DeviceContext.IASetVertexBuffers(0, 1, new ID3D11Buffer[] { vertexBuffer }, new int[] { stride }, new int[] { offset });
            var inputLayout = GraphicsDevice.Device.CreateInputLayout(layout, vsBlob);

            GraphicsDevice.DeviceContext.IASetInputLayout(inputLayout);
            GraphicsDevice.DeviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);

            Viewport viewport = new(0,0, width, height);
            GraphicsDevice.DeviceContext.RSSetViewport(viewport);
        }

        void UpdateScene()
        {

        }

        void DrawScene(int width, int height)
        {
            GraphicsDevice.DeviceContext.Flush();

            GraphicsDevice.DeviceContext.ClearRenderTargetView(GraphicsDevice.RenderTargetView, System.Drawing.Color.LightGreen);
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
