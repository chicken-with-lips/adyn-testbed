using ADyn;
using ADyn.Components;
using ADyn.Math;
using ADyn.Shapes;

namespace Client.Examples;

public class CapsulesExample : ADynExample
{
    public CapsulesExample()
        : base("02-capsules", "Capsules.")
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

        var capsuleDef = new RigidBodyDefinition<CapsuleShape>() {
            Position = new AVector3(0, AScalar.CreateChecked(0.7), 0),
            Mass = 100,
            Material = new Material() {
                Friction = AScalar.CreateChecked(0.8),
                RollFriction = AScalar.CreateChecked(0.001),
                Restitution = 0,
            },
            Shape = new CapsuleShape() {
                Radius = AScalar.CreateChecked(0.2),
                HalfLength = AScalar.CreateChecked(0.35),
                Axis = CoordinateAxis.Z,
            },
        };

        Simulation.CreateRigidBody(capsuleDef);
        Simulation.CreateRigidBody(capsuleDef);

        Simulation.CreateRigidBody(capsuleDef with {
            Position = new AVector3(
                AScalar.CreateChecked(-0.2),
                AScalar.CreateChecked(1.9),
                0
            ),
        });

        Simulation.CreateRigidBody(capsuleDef with {
            Position = new AVector3(
                AScalar.CreateChecked(-1.5),
                AScalar.CreateChecked(0.7),
                0
            ),
            Orientation = AQuaternion.CreateFromAxisAngle(AVector3.UnitZ, AScalar.CreateChecked(1.4)),
            AngularVelocity = new AVector3(0, 4, 0),
        });

        Simulation.CreateRigidBody(capsuleDef with {
            Position = new AVector3(
                AScalar.CreateChecked(3.1),
                AScalar.CreateChecked(1.5),
                0
                ),
        });
        Simulation.CreateRigidBody(capsuleDef with {
            Position = new AVector3(
                AScalar.CreateChecked(3.0),
                AScalar.CreateChecked(0.5), 
                0
            ),
            Shape = new CapsuleShape() {
                Radius = AScalar.CreateChecked(0.22),
                HalfLength = AScalar.CreateChecked(0.5),
            },
        });

        Simulation.CreateRigidBody(capsuleDef with {
            Position = new AVector3(
                AScalar.CreateChecked(2.87),
                AScalar.CreateChecked(2.3),
                0
            ),
            Orientation = AQuaternion.CreateFromAxisAngle(AVector3.UnitY, AScalar.CreateChecked(1.57)),
        });
    }
}
