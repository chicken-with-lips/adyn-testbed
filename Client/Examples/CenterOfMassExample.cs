using ADyn;
using ADyn.Components;
using ADyn.Math;
using ADyn.Shapes;

namespace Client.Examples;

public class CenterOfMassExample : ADynExample
{
    public CenterOfMassExample()
        : base("14-center-of-mass", "Shifting the center of mass.")
    {
    }

    protected override void CreateScene()
    {
        var floorDef = new RigidBodyDefinition<PlaneShape>() {
            Kind = RigidBodyKind.Static,
            Material = new Material() {
                Restitution = 1,
                Friction = AScalar.CreateChecked(0.5),
            },
            Shape = new PlaneShape() {
                Normal = new AVector3(0, 1, 0),
                Constant = 0,
            }
        };

        Simulation.CreateRigidBody(floorDef);


        // Add some dynamic rigid bodies.
        Simulation.CreateRigidBody(
            new RigidBodyDefinition<BoxShape>() {
                Mass = 100,
                Material = new() {
                    Restitution = 0,
                    Friction = AScalar.CreateChecked(0.7),
                },
                Shape = new() {
                    HalfExtents = new AVector3(AScalar.CreateChecked(0.2), AScalar.CreateChecked(0.2), AScalar.CreateChecked(0.2)),
                },
                CenterOfMass = new AVector3(AScalar.CreateChecked(0.1), AScalar.CreateChecked(0.1), AScalar.CreateChecked(0.1)),
                Position = new AVector3(0, AScalar.CreateChecked(0.5), 0),
            }
        );

        Simulation.CreateRigidBody(
            new RigidBodyDefinition<CylinderShape>() {
                Mass = 100,
                Material = new() {
                    Restitution = 0,
                    Friction = AScalar.CreateChecked(0.7),
                },
                Shape = new() {
                    Axis = CoordinateAxis.Z,
                    Radius = AScalar.CreateChecked(0.15),
                    HalfLength = AScalar.CreateChecked(0.2),
                },
                CenterOfMass = new AVector3(AScalar.CreateChecked(0.01), AScalar.CreateChecked(0.05), AScalar.CreateChecked(-0.1)),
                Position = new AVector3(0, 1, 0),
            }
        );

        Simulation.CreateRigidBody(
            new RigidBodyDefinition<SphereShape>() {
                Mass = 100,
                Material = new() {
                    Restitution = 0,
                    Friction = AScalar.CreateChecked(0.7),
                },
                Shape = new() {
                    Radius = AScalar.CreateChecked(0.25),
                },
                CenterOfMass = new AVector3(0, AScalar.CreateChecked(0.2), AScalar.CreateChecked(0.02)),
                Position = new AVector3(0, 0, 0),
            }
        );
    }
}
