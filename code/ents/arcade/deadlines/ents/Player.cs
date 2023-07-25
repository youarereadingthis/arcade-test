using System.Collections.Generic;
using System.Linq;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class Player : SceneVectorArt, ISceneCollider
	{
		public override Shape Shape { get; set; }
		public Crosshair Cursor { get; set; }
		public AimLine Line { get; set; }
		public PhysicsBody PhysBody { get; set; }

		public float MoveSpeed { get; set; } = 150f;



		public Player( SceneWorld world, PhysicsWorld physWorld ) : base( world )
		{
			Shape = new Triangle();
			Transform = Transform.WithScale( 7.0f );
			PhysBody = new( physWorld );

			ColorTint = Color.White;
		}


		public override void Simulate( IClient cl )
		{
			base.Simulate( cl );
		}

		public override void FrameSimulate( IClient cl )
		{
			var moveDir = Input.AnalogMove;

			// Movement
			if ( moveDir != Vector3.Zero )
			{
				Position += Vector3.Forward * moveDir.x * MoveSpeed * Time.Delta;
				Position += Vector3.Right * -moveDir.y * MoveSpeed * Time.Delta;

				Position = Position.Clamp( -ArenaSize, ArenaSize );
			}

			// DEBUG: Rotation
			/*if ( Input.Down( "use" ) )
				Rotation = Rotation.RotateAroundAxis( Vector3.Up, -90f * Time.Delta );
			else if ( Input.Down( "menu" ) )
				Rotation = Rotation.RotateAroundAxis( Vector3.Up, 90f * Time.Delta );*/
		}


		public override void RenderSceneObject()
		{
			base.RenderSceneObject();
		}
	}
}