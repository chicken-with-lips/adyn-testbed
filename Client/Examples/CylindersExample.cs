using ADyn;
using ADyn.Components;
using ADyn.Math;
using ADyn.Shapes;
using Silk.NET.Maths;

namespace Client.Examples;

public class CylindersExample : ADynExample
{
    public CylindersExample()
        : base("03-cylinders", "Cylinders.")
    {
    }

    protected override void CreateScene()
    {
        var floorDef = new RigidBodyDefinition<PlaneShape>() {
            Kind = RigidBodyKind.Static,
            Material = new Material() {
                Restitution = 0,
                Friction = AScalar.CreateChecked(0.5),
            },
            Shape = new PlaneShape() {
                Normal = new AVector3(0, 1, 0),
            }
        };

        Simulation.CreateRigidBody(floorDef);

        // Add many cylinders.
        var cylinderDef = new RigidBodyDefinition<CylinderShape>() {
            Mass = 10,
            Material = new Material() {
                Friction = AScalar.CreateChecked(0.8f),
                Restitution = 0,
                RollFriction = AScalar.CreateChecked(0.005)
            },
        };

        for (var i = 0; i < 5; i++) {
            // Mix cylinders of different orientations.
            if (i % 3 == 0) {
                cylinderDef.Shape = new CylinderShape() {
                    Radius = AScalar.CreateChecked(0.2),
                    HalfLength = AScalar.CreateChecked(0.2),
                    Axis = CoordinateAxis.X,
                };

                cylinderDef.Orientation = AQuaternion.CreateFromAxisAngle(AVector3.UnitZ, AScalar.Pi * AScalar.CreateChecked(0.5));
            } else if (i % 3 == 1) {
                cylinderDef.Shape = new CylinderShape() {
                    Radius = AScalar.CreateChecked(0.2),
                    HalfLength = AScalar.CreateChecked(0.2),
                    Axis = CoordinateAxis.Y,
                };

                cylinderDef.Orientation = AQuaternion.Identity;
            } else {
                cylinderDef.Shape = new CylinderShape() {
                    Radius = AScalar.CreateChecked(0.2),
                    HalfLength = AScalar.CreateChecked(0.2),
                    Axis = CoordinateAxis.Z,
                };

                cylinderDef.Orientation = AQuaternion.CreateFromAxisAngle(AVector3.UnitX, AScalar.Pi * AScalar.CreateChecked(0.5));
            }

            for (var j = 0; j < 1; j++) {
                for (var k = 0; k < 1; k++) {
                    cylinderDef.Position = new AVector3(
                        AScalar.CreateChecked(0.4) * j,
                        AScalar.CreateChecked(0.4) * i + AScalar.CreateChecked(0.6),
                        AScalar.CreateChecked(0.4) * k
                    );

                    Simulation.CreateRigidBody(cylinderDef);
                }
            }
        }
    }
}
