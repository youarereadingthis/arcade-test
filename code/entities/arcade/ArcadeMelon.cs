using Sandbox.UI;

namespace Sandbox;


[Category( "Arcade Machines" )]
public class ArcadeMelon : ArcadeMachine
{
	public SceneModel Melon { get; set; }


	public override void ClientSpawn()
	{
		base.ClientSpawn();

        // melon (helpful comment)
		Melon = new( SceneWorld, "models/sbox_props/watermelon/watermelon.vmdl", Transform.Zero )
		{
			Position = Vector3.Forward * 64f,
		};
	}


	public override void Simulate( IClient cl )
	{
	}

	public override void FrameSimulate( IClient cl )
	{
		var moveDir = Input.AnalogMove;

		if ( Melon.IsValid() )
		{
			Melon.Position += Vector3.Up * moveDir.x;
			Melon.Position += Vector3.Right * -moveDir.y;
		}
	}
}