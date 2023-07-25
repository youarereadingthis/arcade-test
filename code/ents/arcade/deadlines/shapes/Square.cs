using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	/// <summary>
	/// A 3-lined triangular shape.
	/// </summary>
	public class Square : Shape
	{
		public override List<Vertex> Lines { get; set; } = new List<Vertex>()
		{
			new( new(-0.5f, -0.5f) ),
			new( new(0.5f, -0.5f) ),

			new( new(0.5f, -0.5f) ),
			new( new(0.5f, 0.5f) ),
            
			new( new(0.5f, 0.5f) ),
			new( new(-0.5f, 0.5f) ),

			new( new(-0.5f, 0.5f) ),
			new( new(-0.5f, -0.5f) ),
		};
	}
}