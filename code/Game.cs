using sandgame;
using Sandbox;
using System.Threading.Tasks;

public partial class SandGame : Sandbox.Game
{
	enum GameState
	{
		PreRound,
		InRound
	}
	TrafficLight trafficLight;
	GameState state;

	public SandGame()
	{
		if ( IsServer )
		{
			Log.Info( "My Gamemode Has Created Serverside!" );

			new MyHudEntity();
		}

		if ( IsClient )
		{
			Log.Info( "My Gamemode Has Created Clientside!" );
		}
	}

	public override void Spawn()
	{
		base.Spawn();
		_ = StartSecondTimer();
	}

	void StartRound()
	{
		if ( IsServer )
		{
			trafficLight.SetState( TrafficLight.LightState.GREEN );
			state = GameState.InRound;
			_ = StartRoundTimer();
		}

		foreach ( var c in Client.All )
		{
			Client.SetInt( "wins", 0 );
		}
	}

	public override void ClientJoined(Client client)
	{
		base.ClientJoined(client);

		// Create a pawn and assign it to the client.
		var player = new MyPlayer();
		client.Pawn = player;

		player.Respawn();
	}

	public async Task StartSecondTimer()
	{
		while ( true )
		{
			if ( IsServer )
			{
				await Task.DelaySeconds( 1 );
				OnSecond();
			}
		}
	}

	private void OnSecond()
	{
	}

	public override void PostLevelLoaded()
	{
		if ( IsServer )
		{
			trafficLight = new TrafficLight();
		}

		_ = StartSecondTimer();

		StartRound();

		base.PostLevelLoaded();
	}

	public async Task StartRoundTimer()
	{
		while ( state == GameState.InRound )
		{
			if ( IsServer )
			{
				await Task.Delay( 200 );
				OnTick();
			}
		}
	}

	private void OnTick()
	{
		trafficLight.Tick();

		foreach ( var c in Client.All )
		{
			if ( c.Pawn.LifeState == LifeState.Dead && (c.Pawn as MyPlayer).TimeSinceDeath > 5 )
			{
				if(trafficLight.State == TrafficLight.LightState.GREEN)
					(c.Pawn as MyPlayer).Respawn();
			}
		}
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( IsServer )
		{
			if ( trafficLight.State == TrafficLight.LightState.RED )
			{
				foreach ( var c in Client.All )
				{
					if ( c.Pawn != null )
					{
						if ( c.Pawn.Velocity != Vector3.Zero )
						{
							Log.Info( "GOT ONE!!!" );
							c.Pawn.TakeDamage( DamageInfo.Generic( 100 ) );
						}
					}
				}
			}
		}
	}
}
