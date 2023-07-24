using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class Player : SceneVectorArt
	{
		public override Shape Shape { get; set; }
		public float MoveSpeed { get; set; } = 100f;


		public Player( SceneWorld world ) : base( world )
		{
			Shape = new Triangle();

			ColorTint = Color.White;
			Transform = Transform.WithScale( 20f );
		}


		public override void FrameSimulate( IClient cl )
		{
			var moveDir = Input.AnalogMove;

			// Movement
			if ( moveDir != Vector3.Zero )
			{
				Position += Vector3.Forward * moveDir.x * MoveSpeed * Time.Delta;
				Position += Vector3.Right * -moveDir.y * MoveSpeed * Time.Delta;
			}

			// DEBUG: Rotation
			if ( Input.Down( "use" ) )
				Rotation = Rotation.RotateAroundAxis( Vector3.Up, -90f * Time.Delta );
			else if ( Input.Down( "menu" ) )
				Rotation = Rotation.RotateAroundAxis( Vector3.Up, 90f * Time.Delta );
		}
	}
}