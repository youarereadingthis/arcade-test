namespace Sandbox;


public partial class ArcadeDeadLines : ArcadeMachine
{
	public class AimLine : SceneVectorArt
	{
		public Player Ply { get; set; }
		public Crosshair Cursor { get; set; }


		public AimLine( SceneWorld world, Player player, Crosshair cursor ) : base( world )
		{
			Ply = player;
			Cursor = cursor;

			ColorTint = new Color( 0.2f );
		}


		public override void RenderSceneObject()
		{
			Transform = Ply.Transform;

			Shape.DrawLine( Vector3.Zero, Transform.PointToLocal( Cursor.Position ) );
		}
	}
}