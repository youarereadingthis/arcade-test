using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sandbox;


[Category( "Game" )]
public partial class SceneRenderer : SceneCustomObject
{
	public SceneScreen SceneScreen { get; set; }

	public SceneRenderer( SceneWorld world, SceneScreen scrn ) : base( world )
	{
		Log.Info( "SceneRenderer created." );
		SceneScreen = scrn;
	}

	public override void RenderSceneObject()
	{
		// PROBLEM: This block of code isn't being ran even though the SceneObject is being created.
		Log.Info( "SceneRenderer.RenderSceneObject()" );

		SceneScreen.RenderScreen();
		base.RenderSceneObject();
	}
}