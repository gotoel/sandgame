using Sandbox;
using System;
using System.Threading.Tasks;

namespace sandgame
{
	class TrafficLight : Sandbox.Entity
	{
		public enum LightState
		{
			GREEN,
			YELLOW,
			RED
		}

		Sound announcerSound;
		SpotLightEntity greenLight, yellowLight, redLight;
		LightState state, previousState;
		TimeSince timeSinceLightChange;
		Random random;
		
		public LightState State
		{
			get { return state; }
		}

		public TrafficLight()
		{
			greenLight = FindByName( "green_light" ) as SpotLightEntity;
			yellowLight = FindByName( "yellow_light" ) as SpotLightEntity;
			redLight = FindByName( "red_light" ) as SpotLightEntity;

			TurnOff();

			greenLight.Brightness = 15;
			yellowLight.Brightness = 18;
			redLight.Brightness = 20;

			state = LightState.GREEN;

			timeSinceLightChange = 0;

			random = new Random();

			announcerSound = Sound.FromEntity( "light_announce", greenLight );
		}


		public void Tick()
		{
			if(state == LightState.GREEN)
			{
				if ( timeSinceLightChange > 3 )
				{
					if ( random.NextDouble() < .50 )
					{
						SetState( LightState.YELLOW );
						timeSinceLightChange = 0;
						UpdateLight();
					}
				}
			} else if(state == LightState.YELLOW)
			{
				if ( timeSinceLightChange > 1 )
				{
					if ( random.NextDouble() < .70 )
					{
						announcerSound.Stop();
						SetState( LightState.RED );
						timeSinceLightChange = 0;
						UpdateLight();
					}
				}
			} else if ( state == LightState.RED )
			{
				if ( timeSinceLightChange > 2 )
				{
					if ( random.NextDouble() < .40 )
					{
						SetState( LightState.GREEN );
						timeSinceLightChange = 0;
						UpdateLight();
						announcerSound = Sound.FromEntity( "light_announce", greenLight );
						announcerSound.SetRandomPitch( 1, 2.5f );
					}
				}
			}	
		}

		public void CycleLight()
		{
			if(state == LightState.YELLOW)
			{
				if(previousState == LightState.GREEN)
				{
					previousState = state;
					SetState(LightState.RED);
				} else
				{
					previousState = state;
					SetState(LightState.GREEN);
				}
			} else
			{
				previousState = state;
				SetState(LightState.YELLOW);
			}
		}

		public void UpdateLight()
		{
			if(state == LightState.GREEN)
			{
				greenLight.TurnOn();
				yellowLight.TurnOff();
				redLight.TurnOff();
			} else if(state == LightState.YELLOW)
			{
				greenLight.TurnOff();
				yellowLight.TurnOn();
				redLight.TurnOff();
			} else if ( state == LightState.RED )
			{
				greenLight.TurnOff();
				yellowLight.TurnOff();
				redLight.TurnOn();
			}
		}

		public void SetState(LightState s)
		{
			state = s;
			UpdateLight();
		}

		public void TurnOff()
		{
			greenLight.TurnOff();
			yellowLight.TurnOff();
			redLight.TurnOff();
		}
	}
}
