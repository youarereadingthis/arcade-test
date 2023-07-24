using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	/// <summary>
	/// A player object.
	/// </summary>
	public class SceneVectorArt : SceneCustomObject
	{
		public virtual Shape Shape { get; set; }


		public SceneVectorArt( SceneWorld world ) : base( world )
		{
		}


		public override void RenderSceneObject()
		{
			Shape?.Draw();

			// Log.Info( "Player.RenderSceneObject()" );
			base.RenderSceneObject();
		}


		public virtual void Simulate( IClient cl )
		{
		}

		public virtual void FrameSimulate( IClient cl )
		{
		}


		public void Destroy()
		{
			OnDestroy();
			Delete();
		}

		public virtual void OnDestroy()
		{
		}
	}
}