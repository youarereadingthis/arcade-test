using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox;


[Category( "Scene" )]
public partial class SceneScreen : Entity
{

	public SceneCamera Cam { get; set; }
	public ScreenImage ScreenImage { get; set; }
	public ScenePanelObject SceneObject { get; private set; }

	public Texture RT { get; set; }
	public VertexBuffer VB { get; set; }
	public Material Material { get; set; } = Material.Load( "materials/tools/toolswhite.vmat" );


	public SceneScreen( SceneWorld world = null )
	{
		ScreenImage = new ScreenImage( this );
		SceneObject = new ScenePanelObject( world ?? Game.SceneWorld, ScreenImage );
		SceneObject.Flags.IsOpaque = false;
		SceneObject.Flags.IsTranslucent = true;

		ScreenImage.SceneObject = this.SceneObject;
		ScreenImage.Position = this.Position;
		ScreenImage.Rotation = this.Rotation;

		Cam = new()
		{
			Position = this.Position,
			Rotation = this.Rotation,
			AmbientLightColor = Color.White,
			BackgroundColor = Color.Black,
			World = Game.SceneWorld,
			FieldOfView = Camera.Main.FieldOfView,
			ZFar = ScreenImage.MaxInteractionDistance
		};

		Refresh();
	}


	public Texture GetRenderTexture()
	{
		if ( RT == null ) Init();
		return RT;
	}


	[GameEvent.Screen.SizeChanged]
	public void Refresh()
	{
		Init();
	}

	public void Init( int width = 0, int height = 0 )
	{
		var defRes = 128;
		if ( width <= 0 ) width = defRes;//(int)Screen.Width;
		if ( height <= 0 ) height = defRes;//(int)Screen.Height;

		RT = Texture.CreateRenderTarget()
			.WithFormat( ImageFormat.Default )
			.WithScreenFormat()
			// .WithScreenMultiSample()
			.WithSize( width, height )
			.Create();

		Log.Info( $"Created RT ({width}x{height})" );
	}


	[GameEvent.Tick.Client]
	public void Tick()
	{
		Cam.Position = Camera.Main.Position;
		Cam.Rotation = Camera.Main.Rotation;
		ScreenImage.Position = this.Position;
		ScreenImage.Rotation = this.Rotation;
	}

	public override void Simulate( IClient cl )
	{

	}
}