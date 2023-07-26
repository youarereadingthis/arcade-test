using System;
using System.Collections.Generic;
using Sandbox.UI;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class Enemy : SceneVectorArt
	{
		public override Shape Shape { get; set; }

		public Vector3 Velocity { get; set; }
		public virtual float Drag { get; set; }

		public Player Ply { get; set; }

		public virtual float Health { get; set; }

		public TimeUntil FlashEnd { get; set; } = 0f;
		public virtual Color Color { get; set; } = Color.White;


		public Enemy( SceneWorld world ) : base( world )
		{
			ColorTint = Color;
		}


		[GameEvent.Tick.Client]
		public virtual void Tick()
		{
			Position += Velocity * Time.Delta;
			Velocity -= Velocity * Drag * Time.Delta;

			if ( FlashEnd && ColorTint != Color )
				ColorTint = Color;
		}


		public virtual void Knockback( Vector3 vel )
		{
			Velocity += vel;

			FlashEnd = 0.2f;
			ColorTint = Color.White;
		}


		public virtual bool TestRay( Ray ray, float dist )
		{
			// return new Sphere().Trace(ray, dist, out var hitDist);
			return false;
		}
	}
}