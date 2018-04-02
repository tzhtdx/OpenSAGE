﻿using OpenSage.Viewer.UI;
using Veldrid;

namespace OpenSage.Viewer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Platform.Start();

            using (var window = new GameWindow("OpenSAGE Viewer", 100, 100, 1024, 768))
            using (var commandList = window.GraphicsDevice.ResourceFactory.CreateCommandList())
            using (var imGuiRenderer = new ImGuiRenderer(window.GraphicsDevice, window.GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription, 1024, 768))
            using (var gameTimer = new GameTimer())
            {
                window.ClientSizeChanged += (sender, e) =>
                {
                    imGuiRenderer.WindowResized(window.ClientBounds.Width, window.ClientBounds.Height);
                };

                gameTimer.Start();

                var mainForm = new MainForm();

                while (true)
                {
                    commandList.Begin();

                    commandList.SetFramebuffer(window.GraphicsDevice.MainSwapchain.Framebuffer);

                    commandList.ClearColorTarget(0, RgbaFloat.Clear);

                    gameTimer.Update();

                    if (!window.PumpEvents())
                    {
                        break;
                    }

                    imGuiRenderer.Update(
                        (float) gameTimer.CurrentGameTime.ElapsedGameTime.TotalSeconds,
                        window.CurrentInputSnapshot);

                    mainForm.Draw();

                    imGuiRenderer.Render(window.GraphicsDevice, commandList);

                    commandList.End();

                    window.GraphicsDevice.SubmitCommands(commandList);

                    window.GraphicsDevice.SwapBuffers();
                }
            }

            Platform.Stop();
        }
    }
}