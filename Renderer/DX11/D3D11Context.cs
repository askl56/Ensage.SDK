// <copyright file="D3D11Context.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Renderer.DX11
{
    using System;
    using System.ComponentModel.Composition;
    using System.Runtime.CompilerServices;

    using SharpDX.Direct2D1;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    using AlphaMode = SharpDX.Direct2D1.AlphaMode;
    using Factory = SharpDX.Direct2D1.Factory;

    [Export(typeof(ID3D11Context))]
    public class D3D11Context : ID3D11Context, IDisposable
    {
        public D3D11Context()
        {
            if (Drawing.RenderMode != RenderMode.Dx11)
            {
                throw new WrongRenderModeException(RenderMode.Dx11, Drawing.RenderMode);
            }

            this.RenderTarget = new RenderTarget(
                this.Direct2D1,
                this.Surface,
                new RenderTargetProperties(new PixelFormat(Format.R8G8B8A8_UNorm_SRgb, AlphaMode.Premultiplied)));

            Drawing.OnD3D11Present += this.OnPresent;
        }

        public event EventHandler Draw;

        public Factory Direct2D1 { get; } = new Factory();

        public SharpDX.DirectWrite.Factory DirectWrite { get; } = new SharpDX.DirectWrite.Factory();

        public RenderTarget RenderTarget { get; }

        private Texture2D BackBuffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return this.SwapChain.GetBackBuffer<Texture2D>(0);
            }
        }

        private Surface Surface
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return this.BackBuffer.QueryInterface<Surface>();
            }
        }

        private SwapChain SwapChain
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Drawing.SwapChain;
            }
        }

        public void Dispose()
        {
            Drawing.OnD3D11Present -= this.OnPresent;

            this.Direct2D1?.Dispose();
            this.DirectWrite?.Dispose();
            this.RenderTarget?.Dispose();
        }

        private void OnPresent(EventArgs args)
        {
            this.RenderTarget.BeginDraw();

            try
            {
                this.Draw?.Invoke(this, args);
            }
            finally
            {
                this.RenderTarget.EndDraw();
            }
        }
    }
}