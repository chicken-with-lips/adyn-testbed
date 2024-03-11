using ADyn;
using ADyn.Components;
using ADyn.Math;
using ADyn.Shapes;

namespace Client.Examples;

public class EverythingExample : ADynExample
{
    public EverythingExample()
        : base("00-everything", "All the things!")
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

        CreateBoxes();
        CreateCapsules();
        CreateSpheres();
    }

    private void CreateSpheres()
    {
        var def = new RigidBodyDefinition<SphereShape>() {
            Mass = AScalar.CreateChecked(0.17),
            Material = new Material() {
                Friction = AScalar.CreateChecked(0.2),
                Restitution = AScalar.CreateChecked(0.95),
            },
            Shape = new SphereShape() {
                Radius = AScalar.CreateChecked(1) / 2,
            },
        };
        
        for (var i = 0; i < 15; i++) {
            for (var j = 0; j < 1; j++) {
                for (var k = 0; k < 1; k++) {
                    Simulation.CreateRigidBody(def with {
                        Position = new AVector3(
                            AScalar.CreateChecked(0.4) * j,
                            AScalar.CreateChecked(0.4) * i + AScalar.CreateChecked(0.6),
                            AScalar.CreateChecked(0.4) * k
                        ),
                    });
                }
            }
        }
    }

    private void CreateCapsules()
    {
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

    private void CreateBoxes()
    {
        var boxDef = new RigidBodyDefinition<BoxShape>() {
            Mass = 10,
            Material = new Material() {
                Friction = AScalar.CreateChecked(0.8),
                Restitution = 0,
            },
            Shape = new BoxShape() {
                HalfExtents = new AVector3(
                    AScalar.CreateChecked(0.2),
                    AScalar.CreateChecked(0.2),
                    AScalar.CreateChecked(0.2)
                ),
            },
            Orientation = AQuaternion.CreateFromAxisAngle(AVector3.UnitZ, AScalar.CreateChecked(1.15192)),
        };

        for (var i = 0; i < 15; i++) {
            for (var j = 0; j < 1; j++) {
                for (var k = 0; k < 1; k++) {
                    Simulation.CreateRigidBody(boxDef with {
                        Position = new AVector3(
                            AScalar.CreateChecked(0.4) * j,
                            AScalar.CreateChecked(0.4) * i + AScalar.CreateChecked(0.6),
                            AScalar.CreateChecked(0.4) * k
                        ),
                    });
                }
            }
        }
    }
}
