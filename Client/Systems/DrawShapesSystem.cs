using System.Numerics;
using System.Runtime.CompilerServices;
using ADyn.Components;
using ADyn.Math;
using ADyn.Shapes;
using Arch.Core;
using Arch.System;
using Raylib_cs;
using Silk.NET.Maths;

namespace Client.Systems;

public partial class DrawShapesSystem : BaseSystem<World, float>
{
    private Mesh _cubeMesh;
    private Model _cubeModel;

    public DrawShapesSystem(World world)
        : base(world)
    {
        _cubeMesh = Raylib.GenMeshCube(1, 1, 1);
        _cubeModel = Raylib.LoadModelFromMesh(_cubeMesh);
    }

    [Query]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawPlane(in Entity entity, in PlaneShape shape)
    {
        DrawShape(
            entity,
            shape,
            default,
            default,
            (in PlaneShape shape, in Vector3D<float> position, in Quaternion<float> orientation, in Color color) => {
                var center = shape.Normal * shape.Constant;
                Raylib.DrawPlane(center.ToSystem(), new Vector2(20, 20), color);
            }
        );
    }

    [Query]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawBox(in Entity entity, in Position position, in Orientation orientation, in BoxShape shape)
    {
        DrawShape(
            entity,
            shape,
            position,
            orientation,
            (in BoxShape boxShape, in Vector3D<float> position, in Quaternion<float> orientation, in Color color) => {
                var size = boxShape.HalfExtents * 2;

                var transform =
                    Matrix4x4.CreateScale(size.ToSystem())
                    * Matrix4x4.CreateFromQuaternion(orientation.ToSystem())
                    * Matrix4x4.CreateTranslation(position.ToSystem());

                _cubeModel.Transform = Matrix4x4.Transpose(transform);

                Raylib.DrawModelEx(_cubeModel, Vector3.Zero, Vector3.Zero, 0, Vector3.One, color);
            }
        );
    }

    [Query]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawCapsule(in Entity entity, in Position position, in Orientation orientation, in CapsuleShape shape)
    {
        DrawShape(
            entity,
            shape,
            position,
            orientation,
            (in CapsuleShape shape, in Vector3D<float> position, in Quaternion<float> orientation, in Color color) => {
                var axis = MathUtil.CoordinateAxisVector(shape.Axis);
                var startPosition = (axis * shape.HalfLength);
                var endPosition = (axis * -shape.HalfLength);

                var transform =
                    Matrix4x4.CreateScale(Vector3.One)
                    * Matrix4x4.CreateFromQuaternion(orientation.ToSystem())
                    * Matrix4x4.CreateTranslation(position.ToSystem());

                Rlgl.PushMatrix();
                Rlgl.MultMatrixf(Matrix4x4.Transpose(transform));
                {
                    Raylib.DrawCapsule(startPosition.ToSystem(), endPosition.ToSystem(), shape.Radius, 15, 15, color);
                }
                Rlgl.PopMatrix();
            }
        );
    }

    [Query]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawCylinder(in Entity entity, in Position position, in Orientation orientation, in CylinderShape shape)
    {
        DrawShape(
            entity,
            shape,
            position,
            orientation,
            (in CylinderShape shape, in Vector3D<float> position, in Quaternion<float> orientation, in Color color) => {
                var axis = MathUtil.CoordinateAxisVector(shape.Axis);

                var transform =
                    Matrix4x4.CreateScale(Vector3.One)
                    * Matrix4x4.CreateFromQuaternion(orientation.ToSystem())
                    * Matrix4x4.CreateTranslation(position.ToSystem());

                Rlgl.PushMatrix();
                Rlgl.MultMatrixf(Matrix4x4.Transpose(transform));
                {
                    Raylib.DrawCylinder(Vector3.Zero, shape.Radius, shape.Radius, shape.HalfLength * 2, 15, color);
                }
                Rlgl.PopMatrix();
            }
        );
    }

    [Query]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawSphere(in Entity entity, in Position position, in Orientation orientation, in SphereShape shape)
    {
        DrawShape(
            entity,
            shape,
            position,
            orientation,
            (in SphereShape shape, in Vector3D<float> position, in Quaternion<float> orientation, in Color color) => {
                var transform =
                    Matrix4x4.CreateScale(Vector3.One)
                    * Matrix4x4.CreateFromQuaternion(orientation.ToSystem())
                    * Matrix4x4.CreateTranslation(position.ToSystem());

                Rlgl.PushMatrix();
                Rlgl.MultMatrixf(Matrix4x4.Transpose(transform));
                {
                    Raylib.DrawSphere(Vector3.Zero, shape.Radius, color);
                }
                Rlgl.PopMatrix();
            }
        );
    }

    private unsafe void DrawShape<TBoxShape>(in Entity entity, in TBoxShape shape, in Position position, in Orientation orientation, DrawCallback<TBoxShape> callback)
        where TBoxShape : struct, IShape
    {
        var color = new Color(255, 255, 255, 255);

        if (World.Has<ColorComponent>(entity)) {
            color = World.Get<ColorComponent>(entity).Color;
        } else if (World.Has<SleepingTag>(entity)) {
            color = new Color(196, 180, 0, 100);
        } else if (World.Has<IslandResident>(entity) && World.Get<IslandResident>(entity).IslandEntity != EntityReference.Null) {
            var resident = World.Get<IslandResident>(entity);
            color = World.Get<ColorComponent>(resident.IslandEntity).Color;
        }

        var origin = Vector3D<float>.Zero;

        if (World.Has<CenterOfMass>(entity)) {
            var com = World.Get<CenterOfMass>(entity);
            origin = TransformUtil.ToWorldSpace(-com.Value, position.Value, orientation.Value);
        } else {
            origin = position.Value;
        }

        callback(shape, origin, orientation.Value, color);
        // TODO: dde.drawAxis(0, 0, 0, m_rigid_body_axes_size);
    }

    private delegate void DrawCallback<TBoxShape>(in TBoxShape shape, in Vector3D<float> position, in Quaternion<float> orientation, in Color color)
        where TBoxShape : struct, IShape;
}
