using ADyn;
using ADyn.Components;
using ADyn.Shapes;

namespace Client.Examples;

public class BoxesExample : ADynExample
{
    public BoxesExample()
        : base("01-boxes", "Box stacking.")
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
