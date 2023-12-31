using Sandbox.UI;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public SceneCamera Cam => Screen.Cam;

	public Player Ply { get; set; }
	public Crosshair Cursor { get; set; }
	public ArenaLines Perimeter { get; set; }

	public static float ArenaSize { get; set; } = 200f;


	public override void ClientSpawn()
	{
		base.ClientSpawn();

		Screen = new( SceneWorld, 180, 610, 480 )
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
		Perimeter = new( SceneWorld );
		Cursor = new( SceneWorld ) { Position = Vector3.Zero, };
		Ply = new( SceneWorld, PhysicsWorld )
		{
			Position = Vector3.Zero,
			Cursor = Cursor
		};
		Ply.Line = new( SceneWorld, Ply, Cursor );

		var e = new Square( SceneWorld )
		{
			Ply = Ply,
			Position = Vector3.Right * 30f,
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

	public override void PressedUI( string btn )
	{
		if ( btn == "mouseleft" )
		{
			Ply?.Attack();
		}
	}


	public override void ClientTick()
	{
		base.ClientTick();
		if ( Paused ) return;

		foreach ( var o in SceneWorld.SceneObjects )
		{
			if ( o is Enemy e )
			{
				e.Tick();
			}
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
			Cam.Position = Ply.Position + (Vector3.Up * 256f);
		}

		base.FrameSimulate( cl );
	}
}