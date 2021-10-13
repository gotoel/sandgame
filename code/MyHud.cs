using Sandbox.UI;

namespace sandgame
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class MyHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public MyHudEntity()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/MyHud.html" );
			}
		}
	}
}
