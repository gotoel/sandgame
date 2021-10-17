using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace sandgame.UI
{
	public class DebugPanel : Panel
	{
		public Label VelocityLabel, RotLabel;

		public DebugPanel()
		{
			VelocityLabel = Add.Label( "100", "value" );
			RotLabel = Add.Label( "100", "value" );
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			VelocityLabel.Text = $"Velocity: ({player.Velocity.x},{player.Velocity.y},{player.Velocity.z})";
			RotLabel.Text = $"Angle: ({player.Rotation.Yaw()})";
		}
	}
}
