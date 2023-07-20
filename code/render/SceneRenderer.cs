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
		SceneScreen = scrn;
	}

	public override void RenderSceneObject()
	{
		SceneScreen.RenderScreen();

		base.RenderSceneObject();
	}
}