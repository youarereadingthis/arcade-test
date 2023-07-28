
namespace Sandbox;


public partial class ArcadePlatformer : ArcadeMachine
{
	public class Player : SceneVectorArt
	{
		public override Shape Shape { get; set; }
		public float Width { get; set; } = 4f;
		public float Height { get; set; } = 8f;

		public float MoveSpeed { get; set; } = 150f;



		public Player( SceneWorld world, PhysicsWorld physWorld ) : base( world )
		{
			Shape = new ShapeRect( Width, Height );
			Transform = Transform.WithScale( 5.0f );

			ColorTint = Color.Green;
		}


		public override void Simulate( IClient cl )
		{
			base.Simulate( cl );

			if ( Input.Pressed( "attack1" ) )
			{
			}
		}

		public override void FrameSimulate( IClient cl )
		{
			var moveDir = Input.AnalogMove;

			// Movement
			if ( moveDir != Vector3.Zero )
			{
				Position += Vector3.Forward * moveDir.x * MoveSpeed * Time.Delta;
				Position += Vector3.Right * -moveDir.y * MoveSpeed * Time.Delta;
			}
		}


		public override void RenderSceneObject()
		{
			base.RenderSceneObject();
		}
	}
}