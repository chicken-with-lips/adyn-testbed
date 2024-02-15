using Client.Examples;
using Raylib_cs;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        // initialize job scheduler singleton
        new JobScheduler.JobScheduler("ADyn", 0);

        // var scheduler = new JobScheduler(new JobScheduler.Config() {
        //     ThreadPrefixName =    "ADyn",
        //     ThreadCount = 0,
        //     MaxExpectedConcurrentJobs = 12,
        // });

        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(1280, 1024, "ADyn Test Bed");
        Raylib.SetTargetFPS(60);
        // Raylib.DisableCursor();

        // var example = new BoxesExample();
        // var example = new CapsulesExample();
        // var example = new CylindersExample();
        // var example = new BilliardsExample();
        var example = new CenterOfMassExample();
        // var example = new EverythingExample();
        example.Init();

        var camera = new Camera2D();

        while (!Raylib.WindowShouldClose()) {
            // Raylib.UpdateCamera(ref example.Camera, CameraMode.Free);

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(new Color(68, 51, 85, 0));

                Raylib.BeginMode3D(example.Camera);
                {
                    example.Update();
                }
                Raylib.EndMode3D();

                example.UpdateGui();
            }
            Raylib.EndDrawing();
        }

        example.Shutdown();

        Raylib.CloseWindow();

        // scheduler.Dispose();
    }
}
