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
			// TODO: solve the camera bounds cutoff issue
			// This at least makes it take longer to happen.
			Bounds = new BBox( Vector3.Zero, 512f );
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