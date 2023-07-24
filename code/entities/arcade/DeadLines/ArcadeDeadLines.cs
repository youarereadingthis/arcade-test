using Sandbox.UI;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public SceneCamera Cam => Screen.Cam;

	public Player Ply { get; set; }
	public Crosshair Cursor { get; set; }


	public override void ClientSpawn()
	{
		base.ClientSpawn();

		Screen = new( SceneWorld, 256, 610, 480 )
		{
			FollowMainCamera = false,
			RequireCabinet = true,
			Cabinet = this,
		};

		// Point the camera downward.
		Cam.Position = Vector3.Up * 256f;
		Cam.Rotation = Rotation.FromPitch( 90f );

		Cam.Ortho = true;
		Cam.OrthoWidth = 1024;
		Cam.OrthoHeight = 1024;

		UpdateScreen();

		// Spawn the player.
		Cursor = new( SceneWorld ) { Position = Vector3.Zero, };
		Ply = new( SceneWorld, PhysicsWorld )
		{
			Position = Vector3.Zero,
			Cursor = Cursor
		};
	}


	public override void BuildInput()
	{
		base.BuildInput();

		var p = Game.LocalPawn as Pawn;
		if ( !p.IsValid() || !Screen.IsValid() || !Screen.Img.IsValid() ) return;

		if ( Screen.Img.RayToLocal( p.ScreenRay(), out var wPos, out var scrPos, out var onScreen ) )
		{
			Cursor.Position = wPos.WithZ( 0f );
			Ply.Rotation = Rotation.LookAt( (wPos - Ply.Position).Normal, Vector3.Up )
				.RotateAroundAxis( Vector3.Up, 90f );

			SetCursor( onScreen );
		}
	}


	public override void Simulate( IClient cl )
	{
		Ply?.Simulate( cl );

		base.Simulate( cl );
	}

	public override void FrameSimulate( IClient cl )
	{
		Ply?.FrameSimulate( cl );

		base.FrameSimulate( cl );
	}
}