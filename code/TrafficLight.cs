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

			if( greenLight == null)
			{
				Log.Info( "Could not find green light." );
			} else
			{
				Log.Info( "Found light." );
			}

			state = LightState.GREEN;

			timeSinceLightChange = 0;

			random = new Random();
		}


		public void Tick()
		{
			Log.Info( "TimeSinceLightChange: " + timeSinceLightChange );

			if(state == LightState.GREEN)
			{
				if ( timeSinceLightChange > 1 )
				{
					if ( random.NextDouble() < .65 )
					{
						SetState( LightState.YELLOW );
						timeSinceLightChange = 0;
						UpdateLight();
					}
				}
			} else if(state == LightState.YELLOW)
			{
				if ( timeSinceLightChange > 0.6 )
				{
					if ( random.NextDouble() < .80 )
					{
						SetState( LightState.RED );
						timeSinceLightChange = 0;
						UpdateLight();
					}
				}
			} else if ( state == LightState.RED )
			{
				if ( timeSinceLightChange > 1 )
				{
					if ( random.NextDouble() < .40 )
					{
						SetState( LightState.GREEN );
						timeSinceLightChange = 0;
						UpdateLight();
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
			Log.Info( "Set light state: " + s );
		}

		public void TurnOff()
		{
			greenLight.TurnOff();
			yellowLight.TurnOff();
			redLight.TurnOff();
		}
	}
}
