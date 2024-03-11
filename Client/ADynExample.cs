using System.Numerics;
using ADyn;
using ADyn.Components;
using Arch.Core;
using Client.Systems;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace Client;

public abstract class ADynExample
{
    #region Properties

    public string Name { get; }
    public string Description { get; }
    public ref Camera3D Camera => ref _camera;
    public Simulation Simulation { get; }
    public World World { get; }

    #endregion

    #region Members

    private Camera3D _camera = new();
    private string _footerText;
    private int _fixedDeltaTimeMs;

    private readonly DrawShapesSystem _drawShapesSystem;
    private int _velocityIterationCount;
    private int _positionIterationCount;
    private AScalar _guiGravity;
    private float _gravity;

    #endregion

    public ADynExample(string name, string description)
    {
        Name = name;
        Description = description;

        World = World.Create();

        _footerText = "Press 'P' to pause and 'L' to step simulation while paused.";

        Simulation = new Simulation(World, SimulationConfiguration.Default with {
            ExecutionMode = SimulationExecutionMode.Sequential,
        });

        _drawShapesSystem = new(World);
    }

    public virtual void Init()
    {
        rlImGui.Setup();

        _camera.Projection = CameraProjection.Perspective;
        _camera.FovY = 60f;
        _camera.Up = Vector3.UnitY;
        _camera.Position = new Vector3(0, 2, -10.0f);
        _camera.Target = Vector3.Zero;
//
//         m_timestamp = bx::getHPCounter();
//
//         m_registry.reset(new entt::registry);
//

        World.SubscribeComponentAdded<IslandTag>(OnCreateIsland);
//
//         auto config = edyn::init_config{};
//         config.execution_mode = edyn::execution_mode::asynchronous;
//         edyn::attach(*m_registry, config);

        _fixedDeltaTimeMs = (int)(Simulation.FixedDeltaTime * 1000);
        _velocityIterationCount = (int)Simulation.SolverVelocityIterationCount;
        _positionIterationCount = (int)Simulation.SolverPositionIterationCount;
        _guiGravity = _gravity = -Simulation.Gravity.Y;

        CreateScene();
    }

    public virtual void Shutdown()
    {
        DestroyScene();

        // Cleanup.
        Console.WriteLine("TODO: ddShutdown();");

        rlImGui.Shutdown();

        Console.WriteLine("TODO: inputRemoveBindings(\"base\");");
    }

    protected virtual void UpdatePhysics()
    {
        Simulation.Update();
    }

    public virtual bool Update()
    {
        var deltaTime = Raylib.GetFrameTime();

        if (Raylib.IsKeyReleased(KeyboardKey.P)) {
            Simulation.IsPaused = !Simulation.IsPaused;
        }

        if (Raylib.IsKeyReleased(KeyboardKey.L)) {
            if (Simulation.IsPaused) {
                Simulation.StepSimulation();
            }
        }

        UpdateSettings();

//     updatePicking(viewMtx, proj);
//
        UpdatePhysics();

//     // Draw stuff.
//     DebugDrawEncoder dde;
//     dde.begin(0);
//

        Raylib.DrawGrid(24, 24);

//
//     auto shape_views_tuple = edyn::get_tuple_of_shape_views(*m_registry);

        // Draw dynamic entities.
        {
//         auto com_view = m_registry->view<edyn::center_of_mass>();

            _drawShapesSystem.BeforeUpdate(deltaTime);
            _drawShapesSystem.Update(deltaTime);
            _drawShapesSystem.AfterUpdate(deltaTime);
        }
//
//     // Draw AABBs.
//     #if 0
//     {
//         dde.push();
//
//         const uint32_t color = 0xff0000f2;
//         dde.setColor(color);
//         dde.setWireframe(true);
//
//         auto view = m_registry->view<edyn::AABB>();
//         view.each([&](edyn::AABB &aabb) {
//             dde.draw(Aabb{{aabb.min.x, aabb.min.y, aabb.min.z}, {aabb.max.x, aabb.max.y, aabb.max.z}});
//         });
//
//         dde.pop();
//     }
//     #endif
//
//     // Draw static and kinematic entities.
//     {
//         auto view = m_registry->view<edyn::shape_index, edyn::position, edyn::orientation>();
//         view.each([&](auto ent, auto &sh_idx, auto &pos, auto &orn) {
//             if (!m_registry->any_of<edyn::static_tag>(ent) &&
//                 !m_registry->any_of<edyn::kinematic_tag>(ent)) {
//                 return;
//             }
//
//             dde.push();
//
//             uint32_t color = 0xff303030;
//
//             if (auto *color_comp = m_registry->try_get<ColorComponent>(ent)) {
//                 color = *color_comp;
//             }
//
//             dde.setColor(color);
//
//             auto bxquat = to_bx(orn);
//             float rot[16];
//             bx::mtxQuat(rot, bxquat);
//             float rotT[16];
//             bx::mtxTranspose(rotT, rot);
//             float trans[16];
//             bx::mtxTranslate(trans, pos.x, pos.y, pos.z);
//
//             float mtx[16];
//             bx::mtxMul(mtx, rotT, trans);
//             dde.pushTransform(mtx);
//
//             edyn::visit_shape(sh_idx, ent, shape_views_tuple, [&](auto &&s) {
//                 draw(dde, s);
//             });
//
//             dde.drawAxis(0, 0, 0, m_rigid_body_axes_size);
//             dde.popTransform();
//             dde.pop();
//         });
//     }
//
//     // Draw amorphous entities.
//     {
//         auto view = m_registry->view<edyn::position, edyn::orientation>(entt::exclude<edyn::shape_index>);
//         view.each([&](auto ent, auto &pos, auto &orn) {
//             dde.push();
//
//             auto bxquat = to_bx(orn);
//
//             float rot[16];
//             bx::mtxQuat(rot, bxquat);
//             float rotT[16];
//             bx::mtxTranspose(rotT, rot);
//             float trans[16];
//             bx::mtxTranslate(trans, pos.x, pos.y, pos.z);
//
//             float mtx[16];
//             bx::mtxMul(mtx, rotT, trans);
//
//             dde.pushTransform(mtx);
//             dde.drawAxis(0, 0, 0, 0.1);
//             dde.popTransform();
//
//             dde.pop();
//         });
//     }
//
//     // Draw constraints.
//     {
//         std::apply([&](auto ...c) {
//             (
//             m_registry->view<decltype(c)>().each([&](auto ent, auto &con) {
//                 draw(dde, ent, con, *m_registry);
//             }), ...);
//         }, edyn::constraints_tuple);
//     }
//
//     // Draw manifolds with no contact constraint.
//     {
//         auto view = m_registry->view<edyn::contact_manifold>(entt::exclude<edyn::contact_constraint>);
//         for (auto [ent, manifold] : view.each()) {
//             draw(dde, ent, manifold, *m_registry);
//         }
//     }
//
//     drawRaycast(dde);
//
//     dde.end();
//
        return true;
    }

    protected abstract void CreateScene();

    protected virtual void DestroyScene()
    {
    }

    public void UpdateGui()
    {
        rlImGui.Begin();

        // ImGui.SetNextWindowPos(
            // new Vector2(Raylib.GetScreenWidth() - Raylib.GetScreenWidth() / 3.0f - 10.0f, 10.0f),
            // ImGuiCond.FirstUseEver
        // );
        ImGui.SetNextWindowSize(
            new Vector2(200, 400),
            ImGuiCond.FirstUseEver
        );
        
        ImGui.Begin("Example");

        ImGui.SliderInt("Time Step (ms)", ref _fixedDeltaTimeMs, 1, 50);
        ImGui.SliderInt("Velocity Iterations", ref _velocityIterationCount, 1, 100);
        ImGui.SliderInt("Position Iterations", ref _positionIterationCount, 0, 100);
        ImGui.SliderFloat("Gravity (m/s^2)", ref _guiGravity, 0, 50, "%.2f");

        ImGui.End();
        
        ShowSettings();
        ShowFooter();



        rlImGui.End();
    }

    private void UpdateSettings()
    {
        var fixedTimeDeltaMs = (int)(Simulation.FixedDeltaTime * 1000);

        if (fixedTimeDeltaMs != _fixedDeltaTimeMs) {
            Simulation.FixedDeltaTime = _fixedDeltaTimeMs * AScalar.CreateChecked(0.001);
        }

        if (Simulation.SolverVelocityIterationCount != _velocityIterationCount) {
            Simulation.SolverVelocityIterationCount = (uint)_velocityIterationCount;
        }

        if (Simulation.SolverPositionIterationCount != _positionIterationCount) {
            Simulation.SolverPositionIterationCount = (uint)_positionIterationCount;
        }

        if (Math.Abs(_guiGravity - _gravity) > AScalar.Epsilon) {
            _gravity = _guiGravity;
            Simulation.Gravity = new AVector3(0, -_gravity, 0);
        }
    }

    private void ShowSettings()
    {
        ImGui.SetNextWindowPos(
            new Vector2(Raylib.GetScreenWidth() - Raylib.GetScreenWidth() / 3.0f - 10.0f, 10.0f),
            ImGuiCond.FirstUseEver
        );
        ImGui.SetNextWindowSize(
            new Vector2(Raylib.GetScreenWidth() / 3.0f, Raylib.GetScreenHeight() / 3.5f),
            ImGuiCond.FirstUseEver
        );

        ImGui.Begin("Settings");

        ImGui.SliderInt("Time Step (ms)", ref _fixedDeltaTimeMs, 1, 50);
        ImGui.SliderInt("Velocity Iterations", ref _velocityIterationCount, 1, 100);
        ImGui.SliderInt("Position Iterations", ref _positionIterationCount, 0, 100);
        ImGui.SliderFloat("Gravity (m/s^2)", ref _guiGravity, 0, 50, "%.2f");

        ImGui.End();
    }

    private void ShowFooter()
    {
        ImGui.SetNextWindowPos(new Vector2(10, Raylib.GetScreenHeight() - 40f));
        ImGui.SetNextWindowSize(new Vector2(Raylib.GetScreenWidth() - 20, 20));
        ImGui.SetNextWindowBgAlpha(0.4f);

        ImGui.Begin("Footer", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoMouseInputs);
        ImGui.Text(_footerText);
        ImGui.End();
    }

    private void OnCreateIsland(in Entity entity, ref IslandTag tag)
    {
        var color = System.Drawing.Color.FromArgb((int)(0xff000000 | (0x00ffffff & new Random().NextInt64())));

        World.Add<ColorComponent>(entity, new() {
            Color = color.ToRaylib(),
        });
    }
}

public struct ColorComponent
{
    public Color Color;
}

public static class ColorExtensions
{
    public static Color ToRaylib(this System.Drawing.Color color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }
}
