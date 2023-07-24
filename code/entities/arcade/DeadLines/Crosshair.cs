using System;
using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class Crosshair : SceneVectorArt
	{
		public override Shape Shape { get; set; }
		public float MoveSpeed { get; set; } = 100f;


		public Crosshair( SceneWorld world ) : base( world )
		{
			Shape = new Square();

			ColorTint = Color.White;
			Transform = Transform.WithScale( 10f );
		}
	}
}