using System.Collections.Generic;
using System.Xml.Schema;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeMachine : AnimatedEntity
{
	/// <summary>
	/// A 3-lined triangular shape.
	/// </summary>
	public class ShapeRect : Shape
	{
		public override PrimitiveType DrawType { get; set; } = PrimitiveType.TriangleStrip;
		public override List<Vertex> Lines { get; set; } = new();

		public ShapeRect( float width, float height )
		{
			var w = height;
			var h = width;

			Lines = new List<Vertex>()
			{
				new( new(0, -h) ),
				new( new(w, -h) ),
				new( new(w, 0) ),
				new( new(0, 0) ),
				new( new(0, -h) ),
			};

			VertCount = Lines.Count;
		}
	}
}