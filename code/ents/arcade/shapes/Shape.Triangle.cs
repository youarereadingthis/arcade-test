using System.Collections.Generic;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeMachine : AnimatedEntity
{
	/// <summary>
	/// A 3-lined triangular shape.
	/// </summary>
	public class ShapeTriangle : Shape
	{
		public override List<Vertex> Lines { get; set; } = new List<Vertex>()
		{
			new( new(0, -1f) ),
			new( new(1f, 1f) ),

			new( new(1f, 1f) ),
			new( new(-1f, 1f) ),

			new( new(-1f, 1f) ),
			new( new(0f, -1f) ),
		};
	}
}