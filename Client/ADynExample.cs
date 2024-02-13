using System.Numerics;
using ADyn;
using ADyn.Components;
using Arch.Core;
using Client.Systems;
using Raylib_cs;

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
    private QueryDescription _queryDescription = new();

    private readonly DrawShapesSystem _drawShapesSystem;

    #endregion

    public ADynExample(string name, string description)
    {
        Name = name;
        Description = description;

        World = World.Create();

        Simulation = new Simulation(World, SimulationConfiguration.Default with {
            ExecutionMode = SimulationExecutionMode.Sequential,
        });

        _drawShapesSystem = new(World);
    }

    public virtual void Init()
    {
        // imguiCreate();

        _camera.Projection = CameraProjection.CAMERA_PERSPECTIVE;
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
//
//         m_fixed_dt_ms = static_cast<int>(edyn::get_fixed_dt(*m_registry) * 1000);
//         m_num_velocity_iterations = edyn::get_solver_velocity_iterations(*m_registry);
//         m_num_position_iterations = edyn::get_solver_position_iterations(*m_registry);
//         m_gui_gravity = m_gravity = -edyn::get_gravity(*m_registry).y;
//
//         // Input bindings
//         m_bindings = (InputBinding*)BX_ALLOC(entry::getAllocator(), sizeof(InputBinding)*3);
//         m_bindings[0].set(entry::Key::KeyP, entry::Modifier::None, 1, cmdTogglePause,  this);
//         m_bindings[1].set(entry::Key::KeyL, entry::Modifier::None, 1, cmdStepSimulation, this);
//         m_bindings[2].end();
//
//         inputAddBindings("base", m_bindings);
//
//         m_footer_text = m_default_footer_text;

        CreateScene();
    }

    public virtual void Shutdown()
    {
        DestroyScene();

        // Cleanup.
        Console.WriteLine("TODO: ddShutdown();");

        Console.WriteLine("TODO: imguiDestroy();");

        Console.WriteLine("TODO: inputRemoveBindings(\"base\");");
        Console.WriteLine("TODO: BX_FREE(entry::getAllocator(), m_bindings);");
    }

    protected virtual void UpdatePhysics()
    {
        Simulation.Update();
    }

    public virtual bool Update()
    {
//     int64_t now = bx::getHPCounter();
//     const int64_t frameTime = now - m_timestamp;
//     m_timestamp = now;
//     const double freq = double(bx::getHPFrequency());
//     const float deltaTime = float(frameTime/freq);
//
        var deltaTime = Raylib.GetFrameTime();


//
//     // Set view and projection matrix for view 0.
//     float viewMtx[16];
//     cameraGetViewMtx(viewMtx);
//
//     float proj[16];
//     bx::mtxProj(proj, 60.0f, float(m_width)/float(m_height), 0.1f, 10000.0f, bgfx::getCaps()->homogeneousDepth);
//
//     bgfx::setViewTransform(0, viewMtx, proj);
//     bgfx::setViewRect(0, 0, 0, uint16_t(m_width), uint16_t(m_height) );
//
// #ifdef EDYN_SOUND_ENABLED
//     auto camPos = cameraGetPosition();
//     m_soloud.set3dListenerPosition(camPos.x, camPos.y, camPos.z);
//     m_soloud.update3dAudio();
// #endif
//
//     updateGUI();
//
//     updateSettings();
//
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
