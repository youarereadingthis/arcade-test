using Sandbox.UI;

namespace Sandbox;


public partial class ArcadePlatformer : ArcadeMachine
{
	public SceneCamera Cam => Screen.Cam;

	public Player Ply { get; set; }


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
		Cam.Rotation = Rotation.FromPitch( 90f );

		Cam.Ortho = true;
		Cam.OrthoWidth = 256;
		Cam.OrthoHeight = 256;

		UpdateScreen();

		// Spawn objects.
		Ply = new( SceneWorld, PhysicsWorld )
		{
			Position = Vector3.Zero,
		};
	}


	public override void BuildInput()
	{
		base.BuildInput();

		var p = Game.LocalPawn as Pawn;
		if ( !p.IsValid() || !Screen.IsValid() || !Screen.Img.IsValid() ) return;

		if ( Screen.Img.RayToLocal( p.ScreenRay(), out var wPos, out var scrPos, out var onScreen ) )
		{
			// Ply.Rotation = Rotation.LookAt( (wPos - Ply.Position).Normal, Vector3.Up );

			SetCursor( onScreen );
		}
	}

	public override void PressedUI( string btn )
	{
		if ( btn == "mouseleft" )
		{
		}
	}


	public override void ClientTick()
	{
		base.ClientTick();
		if ( Paused ) return;

		foreach ( var o in SceneWorld.SceneObjects )
		{

		}
	}

	public override void Simulate( IClient cl )
	{
		Ply?.Simulate( cl );

		base.Simulate( cl );
	}

	public override void FrameSimulate( IClient cl )
	{
		if ( Ply.IsValid() )
		{
			Ply.FrameSimulate( cl );
			Cam.Position = Ply.Position
				- new Vector3( -Ply.Height, Ply.Width, 0f )
				+ (Vector3.Up * 256f);
		}

		base.FrameSimulate( cl );
	}
}