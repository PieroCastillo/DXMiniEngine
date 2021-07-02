using System;
using System.Runtime.CompilerServices;

namespace DXMiniEngine
{
    public interface IGraphicsDevice : IDisposable
    {
        bool DrawFrame(Action<int, int> draw, [CallerMemberName] string? frameName = null);
    }
}