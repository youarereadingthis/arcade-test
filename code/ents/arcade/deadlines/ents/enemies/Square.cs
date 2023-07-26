using System;
using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class Square : Enemy
	{
		public float Speed { get; set; } = 150.0f;
		public override float Drag { get; set; } = 0.5f;

		public override Color Color { get; set; } = Color.Cyan;

		public float HullSize = 18f;


		public Square( SceneWorld world ) : base( world )
		{
			Shape = new ShapeSquare();
			Transform = Transform.WithScale( HullSize );
		}


		[GameEvent.Client.Frame]
		public override void Tick()
		{
			if ( Ply.IsValid() )
			{
				var dir = (Ply.Position - Position).Normal;
				Velocity += dir * Speed * Time.Delta;
				// Log.Info("ply valid");
			}
			Transform = Transform.RotateAround( Position, Rotation.FromYaw( Velocity.Length * 2f * Time.Delta ) );
			base.Tick();
		}


		public override bool TestRay( Ray ray, float dist )
		{
			var hull = new BBox( Position, HullSize );//.Rotate( Transform.Rotation );
			return hull.Trace( ray, dist, out var hitDist );
		}

		public override void RenderSceneObject()
		{
			Tick();
			base.RenderSceneObject();
		}
	}
}