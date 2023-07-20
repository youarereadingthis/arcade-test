using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox;


[Category( "Scene Camera" )]
public partial class SceneScreen : Entity
{
	public Texture RT { get; set; }
	public VertexBuffer VB { get; set; }
	public Material Material { get; set; } = Material.Load( "materials/tools/toolswhite.vmat" );

	public SceneCamera Cam { get; set; }
	public SceneRenderer Renderer { get; set; }

	public WorldPanel WorldPanel { get; set; }
	public ScenePanel ScenePanel { get; set; }


	public SceneScreen()
	{
		Position = Vector3.Up * 128f; // temp

		/*Cam = new()
		{
			Position = this.Position,
			Rotation = this.Rotation,
			AmbientLightColor = Color.White,
			BackgroundColor = Color.Black,
			World = Game.SceneWorld
		};*/

		Renderer = new SceneRenderer( Game.SceneWorld, this )
		{
			Position = this.Position,
			Rotation = this.Rotation,
			RenderingEnabled = true
		};

		SetSize( 512, 512 );

		WorldPanel = new WorldPanel( Game.SceneWorld )
		{
			Position = Position,
			Rotation = Rotation,
			WorldScale = 1
		};

		var camRot = Rotation.RotateAroundAxis( Vector3.Right, 90f );
		// ScenePanel = WorldPanel.Add.ScenePanel( Game.SceneWorld, Vector3.Zero, camRot, 90 );
		ScenePanel = new ScenePanel() { World = Game.SceneWorld, };
		WorldPanel.AddChild( ScenePanel );

		Cam = ScenePanel.Camera;
		Cam.Position = Position;
		Cam.Rotation = camRot;
		Cam.FieldOfView = 90;
		Cam.AmbientLightColor = Color.White;
		Cam.BackgroundColor = Color.Black;

		// So that I know where the WorldPanel is.
		WorldPanel.Add.Image( "ui/black_transparent.png" );
		// ScenePanel.Add.Image( "ui/black_transparent.png" ); // crashes
	}

	public void SetSize( int width, int height )
	{
		RT = Texture.CreateRenderTarget()
			.WithFormat( ImageFormat.Default )
			.WithScreenFormat()
			.WithScreenMultiSample()
			.WithSize( width, height )
			.Create();

		VB = new();
		VB.Init( true );
		VB.AddCube( Vector3.Zero, 256, Rotation.Identity, Color.White );
	}

	public void RenderScreen()
	{
		var o = Renderer;
		Log.Info( o );

		Graphics.RenderToTexture( Cam, RT );

		o.Attributes.Set( "Texture", RT );

		VB.Draw( Material, o.Attributes );
	}

	[GameEvent.Tick.Client]
	public void Tick()
	{

	}


	public override void Simulate( IClient cl )
	{

	}
}