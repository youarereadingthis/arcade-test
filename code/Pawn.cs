using Sandbox;
using System;
using System.Linq;

namespace Sandbox;


public partial class Pawn : AnimatedEntity
{
	[Net]
	public ArcadeMachine Arcade { get; set; }

	public Vector3 EyePos => Position;
	public override Ray AimRay => new( EyePos, ViewAngles.Forward );

	[ClientInput] public Vector3 InputDirection { get; protected set; }
	[ClientInput] public Angles ViewAngles { get; set; }


	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;

		var look = Input.AnalogLook;

		var viewAngles = ViewAngles;
		viewAngles += look;
		ViewAngles = viewAngles.Normal;
	}

	public Trace GetUseTrace()
	{
		return Trace.Ray( AimRay, 70f )
			.Ignore( this );
	}

	/// <summary>
	/// Called every tick, clientside and serverside.
	/// </summary>
	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		// Arcade Machine Controller
		if ( Arcade != null )
		{
			Arcade.Simulate( cl );

			if ( Game.IsServer && Input.Pressed( "score" ) )
				Arcade.Exit();

			return;
		}

		// Use Trace
		if ( Game.IsServer && Input.Pressed( "use" ) )
		{
			var tr = GetUseTrace().Run();
			if ( tr.Entity.IsValid() && tr.Entity is IUse u )
				if ( u.IsUsable( this ) )
					u.OnUse( this );
		}

		Rotation = ViewAngles.ToRotation();

		// build movement from the input values
		var movement = InputDirection.Normal;
		var moveSpeed = Input.Down( "run" ) ? 500 : 200;

		// rotate it to the direction we're facing
		Velocity = Rotation * movement * moveSpeed;

		if ( Input.Down( "jump" ) )
		{
			Velocity += Vector3.Up * moveSpeed;
		}
		else if ( Input.Down( "duck" ) )
		{
			Velocity += Vector3.Down * moveSpeed;
		}

		Position += Velocity * Time.Delta;

		// Spawn Arcade Machines
		if ( Game.IsServer )
		{
			var endPos = Position + (ViewAngles.Forward * 2048);
			var tr = Trace.Ray( Position, endPos )
				.Ignore( this )
				.Run();

			// Depends on which number you press.
			ArcadeMachine a = null;
			var pos = tr.HitPosition;

			if ( Input.Pressed( "slot1" ) )
				a = new ArcadeMelon() { Position = pos }.Reorient( Rotation );
			else if ( Input.Pressed( "slot2" ) )
				a = new ArcadeDeadLines() { Position = pos }.Reorient( Rotation );

			if ( a.IsValid() )
				Log.Info( "Spawned a " + a.GetType().Name );
		}
	}


	/// <summary>
	/// Update the main camera.
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		Rotation = ViewAngles.ToRotation();

		Camera.Position = Position;
		Camera.Rotation = Rotation;
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
		Camera.FirstPersonViewer = this;

		// Pass FrameSimulate to the arcade machine.
		Arcade?.FrameSimulate( cl );
	}
}
