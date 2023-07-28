
namespace Sandbox;


[Category( "Arcade" )]
public partial class ArcadeMachine : AnimatedEntity, IUse
{
	public virtual Model ArcadeModel { get; set; } = Cloud.Model( "shadb.arcade_cabinet" );

	public SceneScreen Screen { get; set; }
	[Net] public Vector3 ScreenPosition { get; set; } = new( 13.1f, 0f, 57.85f );
	[Net] public Rotation ScreenRotation { get; set; } = Rotation.From( -9.1f, 0f, 0f );

	public SceneWorld SceneWorld { get; set; }
	public PhysicsWorld PhysicsWorld { get; set; }

	[Net]
	public Pawn Pawn { get; set; } = null;

	public bool Paused { get; set; } = false;


	public override void Spawn()
	{
		Model = ArcadeModel;
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
	}

	public override void ClientSpawn()
	{
		SceneWorld = new()
		{
			AmbientLightColor = Color.White,
			ClearColor = Color.White,
		};

		PhysicsWorld = new()
		{
			Gravity = Vector3.Zero,
		};
	}

	public virtual void SetCursor( bool onScreen )
	{
		if ( Game.RootPanel is not Hud hud ) return;

		hud.Style.Cursor = onScreen ? "none" : "default";
	}


	[GameEvent.Tick.Client]
	public virtual void ClientTick()
	{
	}

	public override void BuildInput()
	{
	}

	public virtual void PressedUI( string btn )
	{
		// Log.Info( "PressedUI: " + btn );
	}

	public override void Simulate( IClient cl )
	{
		UpdateStick( Input.AnalogMove );
	}

	public override void FrameSimulate( IClient cl )
	{
	}


	public void UpdateStick( Vector2 inputDir )
	{
		var fwd = inputDir.x;
		var side = -inputDir.y;

		SetAnimParameter( "up", fwd > 0.5f );
		SetAnimParameter( "down", fwd < -0.5f );
		SetAnimParameter( "left", side < -0.5f );
		SetAnimParameter( "right", side > 0.5f );
	}


	public bool IsUsable( Entity ent )
	{
		// Can't use a machine someone else is using.
		if ( Pawn.IsValid() && Pawn.Machine == this ) return false;

		return ent.IsValid() && ent is Pawn;
	}

	public bool OnUse( Entity ent )
	{
		if ( Game.IsServer )
		{
			if ( ent is Pawn p && p.Client.IsValid() )
			{
				Enter( p );
			}
		}

		return false; // once per press
	}

	/// <summary>
	/// Allow a Pawn to take control of this machine.
	/// </summary>
	public bool Enter( Pawn p )
	{
		// Only an actively controlled pawn can use this machine.
		if ( !Game.IsServer || !p.IsValid() || !p.Client.IsValid() )
			return false;

		// If somehow they try to control twice, kick them off the previous.
		if ( p.Machine.IsValid() )
		{
			if ( p.Machine == this )
				Log.Info( p + " tried to use the same machine twice." );
			else
				Log.Info( p + " tried to use multiple arcade machines!" );

			p.Machine.Exit();

			return false;
		}

		// Prevent entering if someone already controls this machine.
		if ( Pawn.IsValid() )
		{
			Log.Info( Pawn + " is already using " + this );
			return false;
		}

		// The arcade machine is free to be controlled.
		Pawn = p;
		Pawn.Machine = this;
		ClientEnter( Pawn.Client );

		Log.Info( p + " is entering " + this );

		return true;
	}

	/// <summary>
	/// Kick all users off of the machine.
	/// </summary>
	public void Exit()
	{
		if ( !Game.IsServer )
			return;

		if ( Pawn.IsValid() )
		{
			// Someone was using this machine.
			Log.Info( "Kicking " + Pawn + " off of " + this );

			Pawn.Machine = null;
			Pawn = null;
		}

		ClientExit();
	}

	[ClientRpc]
	public void ClientEnter( IClient cl )
	{
		if ( !cl.IsValid() || cl != Game.LocalClient ) return;
		if ( Screen.IsValid() && Screen.Img.IsValid() )
		{
			Screen.Img.Focus();
			Log.Info( "Focusing screen " + Screen );
		}
	}

	[ClientRpc]
	public void ClientExit()
	{
		Screen?.Img?.Blur();
	}


	/// <summary>
	/// Remove pitch and flip us around towards the viewer.
	/// </summary>
	public ArcadeMachine Reorient( Rotation rot )
	{
		Rotation = rot
			.RotateAroundAxis( Vector3.Right, rot.Pitch() )
			.RotateAroundAxis( Vector3.Up, 180f );

		return this;
	}


	Vector3 _lastPos = Vector3.Zero;
	Rotation _lastRot = Rotation.Identity;
	[GameEvent.Tick.Client]
	public void MoveWithScreen()
	{
		if ( Position != _lastPos || Rotation != _lastRot )
			UpdateScreen();

		_lastPos = Position;
		_lastRot = Rotation;
	}

	// [GameEvent.Tick.Client]
	public void UpdateScreen()
	{
		if ( !Screen.IsValid() ) return;
		// Log.Info( "ArcadeMachine.UpdateScreen()" );

		var sPos = this.Position;
		sPos += Rotation.Forward * ScreenPosition.x;
		sPos += Rotation.Right * -ScreenPosition.y;
		sPos += Rotation.Up * ScreenPosition.z;

		Screen.Position = sPos;
		Screen.Rotation = Transform.Rotation * ScreenRotation;
		// Screen.UpdateScreen();
	}
}