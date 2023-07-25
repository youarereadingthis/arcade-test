using System;
using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class ArenaLines : SceneVectorArt
	{
		public override Shape Shape { get; set; }


		public ArenaLines( SceneWorld world ) : base( world )
		{
			Shape = new Square();

			ColorTint = Color.Gray;
			Transform = Transform.WithScale( ArenaSize * 2f )
				.WithPosition( Vector3.Down * 50f );
		}
	}
}