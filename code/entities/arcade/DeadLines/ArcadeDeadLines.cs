using Sandbox.UI;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public Player Ply { get; set; }


	public override void ClientSpawn()
	{
		base.ClientSpawn();

		// Point the camera downward.
		Screen.Cam.Position = Vector3.Up * 256f;
		Screen.Cam.Rotation = Rotation.FromPitch( 90f );

		// Spawn the player.
		Ply = new( SceneWorld )
		{
			Position = Vector3.Zero,
		};
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