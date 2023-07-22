using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Win32;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox;


[Category( "Screens" )]
public partial class SceneScreen : Entity
{
	public SceneCamera Cam { get; set; }
	public ScreenImage ScreenImage { get; set; }
	public ScenePanelObject SceneObject { get; private set; }

	public Texture RT { get; set; }

	public ArcadeMachine Cabinet { get; set; }
	public bool RequireCabinet { get; set; } = false;
	public bool FollowMainCamera { get; set; } = true;


	public override Vector3 Position
	{
		get => base.Position;
		set
		{
			base.Position = value;
			UpdateScreen();
		}
	}

	public override Rotation Rotation
	{
		get => base.Rotation;
		set
		{
			base.Rotation = value;
			UpdateScreen();
		}
	}


	public SceneScreen( SceneWorld world, int res, int width, int height )
	{
		if ( Game.IsServer ) return;

		ScreenImage = new ScreenImage( this, width, height );
		SceneObject = new ScenePanelObject( Game.SceneWorld, ScreenImage );
		SceneObject.Flags.IsOpaque = true;
		SceneObject.Flags.IsTranslucent = true;

		ScreenImage.SceneObject = this.SceneObject;
		UpdateScreen();

		Cam = new()
		{
			Position = this.Position,
			Rotation = this.Rotation,
			BackgroundColor = Color.Black,
			AmbientLightColor = Color.White,
			FieldOfView = Camera.Main.FieldOfView,
			ZFar = 512f,
			World = world,
		};

		Init( res, res );
	}


	public Texture GetRenderTexture()
	{
		if ( RT == null ) Init();
		return RT;
	}


	public void Init( int width = 0, int height = 0 )
	{
		var defRes = 64;
		if ( width <= 0 ) width = defRes;//(int)Screen.Width;
		if ( height <= 0 ) height = defRes;//(int)Screen.Height;

		RT = Texture.CreateRenderTarget()
			.WithFormat( ImageFormat.Default )
			.WithScreenFormat()
			// .WithScreenMultiSample()
			.WithSize( width, height )
			.Create();

		// Log.Info( $"Created RT ({width}x{height})" );
	}


	[GameEvent.Tick.Client]
	public void Tick()
	{
		if ( FollowMainCamera )
		{
			Cam.Position = Camera.Main.Position;
			Cam.Rotation = Camera.Main.Rotation;
		}

		if ( RequireCabinet )
		{
			if ( !Cabinet.IsValid() )
			{
				// Log.Info( "Parent was removed." );
				// ScreenImage.Delete(false);
				// SceneObject.Delete();
				Delete();
			}
		}
	}

	public void UpdateScreen()
	{
		if ( Game.IsServer || !SceneObject.IsValid() || !ScreenImage.IsValid() ) return;
		ScreenImage.Position = this.Position;
		ScreenImage.Rotation = this.Rotation;
	}


	public override void Simulate( IClient cl )
	{

	}
}