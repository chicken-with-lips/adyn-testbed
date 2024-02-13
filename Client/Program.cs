using System.Numerics;
using Client.Examples;
using Raylib_cs;
using Silk.NET.Maths;

public static class Program
{
    [STAThread]
    public static unsafe void Main()
    {
        // initialize job scheduler singleton
        new JobScheduler.JobScheduler("ADyn", 0);

        // var scheduler = new JobScheduler(new JobScheduler.Config() {
        //     ThreadPrefixName =    "ADyn",
        //     ThreadCount = 0,
        //     MaxExpectedConcurrentJobs = 12,
        // });

        Raylib.InitWindow(1280, 1024, "ADyn Test Bed");
        Raylib.SetTargetFPS(60);
        // Raylib.DisableCursor();

        // var example = new BoxesExample();
        // var example = new CapsulesExample();
        // var example = new CylindersExample();
        // var example = new BilliardsExample();
        var example = new EverythingExample();
        example.Init();

        while (!Raylib.WindowShouldClose()) {
            // Raylib.PollInputEvents();

            // Raylib.UpdateCamera(ref example.Camera, CameraMode.CAMERA_FREE);

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(new Color(68, 51, 85, 0));

                Raylib.BeginMode3D(example.Camera);
                {
                    example.Update();
                }
                Raylib.EndMode3D();
            }
            Raylib.EndDrawing();
        }

        example.Shutdown();

        Raylib.CloseWindow();

        // scheduler.Dispose();
    }
}
