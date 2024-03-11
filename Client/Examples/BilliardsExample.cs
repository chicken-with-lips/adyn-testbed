using System.Numerics;
using ADyn;
using ADyn.Shapes;
using Raylib_cs;
using Material = ADyn.Components.Material;

namespace Client.Examples;

public class BilliardsExample : ADynExample
{
    private const ushort BallMaterialId = 0;
    private const ushort TableMaterialId = 1;
    private const ushort RailMaterialId = 2;

    public BilliardsExample()
        : base("17-billiards", "Billiards.")
    {
    }

    protected override void CreateScene()
    {
        // Material properties obtained from
        // https://billiards.colostate.edu/faq/physics/physical-properties/
        var ballDiameter = AScalar.CreateChecked(0.05715);
        var ballRadius = AScalar.CreateChecked(ballDiameter / 2);

        var ballBallMaterial = new Material() {
            Friction = AScalar.CreateChecked(0.05),
            Restitution = AScalar.CreateChecked(0.95),
        };
        Simulation.MaterialMixTable.Insert(new ValueTuple<ushort, ushort>(BallMaterialId, BallMaterialId), ballBallMaterial);

        // Multiply rolling resistance by the ball radius because in adyn the rolling friction applies torque.
        var tableBallMaterial = new Material() {
            Friction = AScalar.CreateChecked(0.1),
            Restitution = AScalar.CreateChecked(0.5),
            SpinFriction = AScalar.CreateChecked(0.000057),
            RollFriction = AScalar.CreateChecked(0.006 * ballRadius),
        };
        Simulation.MaterialMixTable.Insert(new ValueTuple<ushort, ushort>(BallMaterialId, TableMaterialId), tableBallMaterial);

        var railBallMaterial = new Material() {
            Friction = AScalar.CreateChecked(0.2),
            Restitution = AScalar.CreateChecked(0.7),
        };
        Simulation.MaterialMixTable.Insert(new ValueTuple<ushort, ushort>(BallMaterialId, RailMaterialId), railBallMaterial);

        // Create table.
        var tableSize = new AVector3(
            AScalar.CreateChecked(1.268),
            AScalar.CreateChecked(0.76),
            AScalar.CreateChecked(2.385)
        );
        var tableDef = new RigidBodyDefinition<BoxShape>() {
            Kind = RigidBodyKind.Static,
            Material = new() {
                Id = TableMaterialId,
                Restitution = AScalar.CreateChecked(0.5),
                Friction = AScalar.CreateChecked(0.2),
            },
            Shape = new BoxShape() {
                HalfExtents = tableSize / AScalar.CreateChecked(2),
            },
            Position = new AVector3(
                0,
                tableSize.Y / AScalar.CreateChecked(2),
                0
            ),
        };

        var table = Simulation.CreateRigidBody(tableDef);
        World.Add<ColorComponent>(table, new ColorComponent() {
            Color = Color.DarkGray,
        });

        var railPosition = new AVector3(
            0,
            tableSize.Y + AScalar.CreateChecked(0.025),
            tableSize.Z / AScalar.CreateChecked(2) - AScalar.CreateChecked(0.075)
        );

        // Rail top.
        var railDef = new RigidBodyDefinition<BoxShape>() {
            Kind = RigidBodyKind.Static,
            Material = new() {
                Id = RailMaterialId,
                Restitution = AScalar.CreateChecked(0.7),
                Friction = AScalar.CreateChecked(0.2),
            },
            Shape = new BoxShape() {
                HalfExtents = new AVector3(
                    tableSize.X / AScalar.CreateChecked(2),
                    AScalar.CreateChecked(0.025),
                    AScalar.CreateChecked(0.075)),
            },
            Position = railPosition,
        };
        Simulation.CreateRigidBody(railDef);

        // Rail bottom.
        railPosition.Z *= AScalar.CreateChecked(-1);

        railDef.Position = railPosition;
        Simulation.CreateRigidBody(railDef);

        // Rail left.
        railPosition = new AVector3(
            tableSize.X / AScalar.CreateChecked(2) - AScalar.CreateChecked(0.075),
            tableSize.Y + AScalar.CreateChecked(0.025),
            0
        );

        railDef.Position = railPosition;
        railDef.Shape = new BoxShape() {
            HalfExtents = new AVector3(
                AScalar.CreateChecked(0.075),
                AScalar.CreateChecked(0.025),
                tableSize.Z / AScalar.CreateChecked(2)
            ),
        };
        Simulation.CreateRigidBody(railDef);

        // Rail right.
        railPosition.X *= -1;

        railDef.Position = railPosition;
        Simulation.CreateRigidBody(railDef);

        // Add the balls.
        var def = new RigidBodyDefinition<SphereShape>() {
            Mass = AScalar.CreateChecked(0.17),
            Material = new Material() {
                Id = BallMaterialId,
                Friction = AScalar.CreateChecked(0.2),
                Restitution = AScalar.CreateChecked(0.95),
            },
            Shape = new SphereShape() {
                Radius = ballRadius,
            },
        };

        // Cue ball.
        def.Position = new AVector3(
            0,
            tableSize.Y + ballRadius,
            -(tableSize.Z / AScalar.CreateChecked(2) - AScalar.CreateChecked(0.15)) / AScalar.CreateChecked(2)
        );

        def.LinearVelocity = new AVector3(AScalar.CreateChecked(0.001), 0, 3);
        def.AngularVelocity = new AVector3(0, -3, 1);

        Simulation.CreateRigidBody(def);

        // Other balls.
        def.LinearVelocity = new AVector3(0, 0, 0);
        def.AngularVelocity = new AVector3(0, 0, 0);

        for (var i = 0; i < 5; i++) {
            var n = i + 1;

            def.Position = new AVector3(
                def.Position.X,
                def.Position.Y,
                AScalar.CreateChecked(i)
                * ballDiameter
                * AScalar.Sin(AScalar.DegreesToRadians(60))
                + (tableSize.Z / AScalar.CreateChecked(2) - AScalar.CreateChecked(0.15)) / AScalar.CreateChecked(2)
            );

            for (var j = 0; j < n; j++) {
                def.Position = new AVector3(
                    (j - AScalar.CreateChecked(i) / AScalar.CreateChecked(2)) * ballDiameter,
                    def.Position.Y,
                    def.Position.Z
                );

                Simulation.CreateRigidBody(def);
            }
        }

        Camera.Position = new Vector3(0f, 1.6f, -2f);
    }
}
