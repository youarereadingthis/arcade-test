using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Sandbox.UI;
using static Sandbox.Graphics;

namespace Sandbox;


public partial class ArcadeMachine : AnimatedEntity
{
	/// <summary>
	/// A shape made of lines.
	/// </summary>
	public class Shape
	{
		public virtual PrimitiveType DrawType { get; set; } = PrimitiveType.Lines;
		public virtual List<Vertex> Lines { get; set; } = new List<Vertex>()
		{
			new( new(-1f, -1f) ),
			new( new(1f, -1f) ),

			new( new(1f, -1f) ),
			new( new(1f, 1f) ),

			new( new(1f, 1f) ),
			new( new(-1f, 1f) ),

			new( new(-1f, 1f) ),
			new( new(-1f, -1f) ),
		};
		public int VertCount { get; set; }

		public RenderAttributes RA { get; set; }
		public static Material Material { get; set; } = Material.Load( "materials/vector/line.vmat" );


		public Shape()
		{
			VertCount = Lines.Count;
		}

		public static T Create<T>() where T : Shape, new()
		{
			return new T();
		}


		/// <summary>
		/// Draw the vector art at a position and angle.
		/// </summary>
		public void Draw()
		{
			Graphics.Draw( Lines, VertCount, Material, RA, DrawType );
			// Graphics.Draw( LineArt, VertCount, Material, RA, PrimitiveType.LinesWithAdjacency );
		}

		public static void DrawLine( Vector3 a, Vector3 b, RenderAttributes ra = null )
		{
			var v = new List<Vertex>() { new( a ), new( b ) };
			Graphics.Draw( v, 2, Material, ra, PrimitiveType.Lines );
		}

		public static void DrawLines( List<Vertex> v, RenderAttributes ra = null )
		{
			Graphics.Draw( v, 2, Material, ra, PrimitiveType.Lines );
		}
	}
}