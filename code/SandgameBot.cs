using Sandbox;
using System;

namespace sandgame
{
	public class SandgameBot : Bot
	{
		TrafficLight trafficLight;
		Player pawn;
		Random random;
		TimeSince timeSinceMovementUpdate;

		float targetYaw;

		[AdminCmd( "bot_custom", Help = "Spawn my custom bot." )]
		internal static void SpawnCustomBot()
		{
			Host.AssertServer();

			// Create an instance of your custom bot.
			_ = new SandgameBot();

		}

		public SandgameBot()
		{
			random = new Random();

			trafficLight = Entity.FindByName( "TrafficLight" ) as TrafficLight;
			pawn = (Player)Client.Pawn;

			//pawn.Rotation = Rotation.From( 0, 90, 0 );

			timeSinceMovementUpdate = 0;
			targetYaw = 89;
		}

		public override void BuildInput( InputBuilder builder )
		{
			base.BuildInput( builder );
			
			// Here we can choose / modify the bot's input each tick.
			//if ( trafficLight != null )
			//{
			//	if ( trafficLight.State != TrafficLight.LightState.RED )
			//	{
			//		builder.SetButton( InputButton.Walk, true );
			//	}
			//	else
			//	{
			//		builder.SetButton( InputButton.Walk, false );
			//	}
			//}
		}


		public override void Tick()
		{
			if ( timeSinceMovementUpdate > 0.01f )
			{
				if ( trafficLight != null && pawn != null )
				{
					TurnToTargetYaw();

					Log.Info( pawn.Rotation );

					
					//pawn.Rotation = Rotation.FromYaw( 100f );

					//pawn.Transform = new Transform( pawn.Transform.Position, Rotation.FromYaw( random.Next(0,180)), 1 );
					//pawn.EyeRot = Rotation.FromYaw( 45f );
					//if ( trafficLight.State == TrafficLight.LightState.GREEN )
					//{
					//	pawn.Velocity += pawn.Rotation.Forward * 25f;
					//}
					//else if ( trafficLight.State == TrafficLight.LightState.YELLOW )
					//{
					//	if ( random.NextDouble() < .20 )
					//	{
					//		if ( pawn.Velocity != Vector3.Zero )
					//			pawn.Velocity -= pawn.Rotation.Forward * 25f;
					//	}
					//	else
					//	{

					//		pawn.Velocity = Vector3.Zero;
					//	}
					//}
					//else
					//{
					//	pawn.Velocity = Vector3.Zero;
					//}
				}

				timeSinceMovementUpdate = 0;
			}
		}

		private void TurnToTargetYaw()
		{
			float currentAngle = pawn.Rotation.Angle();
			if ( currentAngle < targetYaw ) {
				currentAngle += 1f;
				Rotation currentRotation = Rotation.FromYaw( currentAngle ); ;
				//pawn.Rotation = Rotation.FromYaw( currentAngle );
				//pawn.LocalRotation = Rotation.FromYaw( currentAngle );
				//pawn.EyeRot = Rotation.FromYaw( currentAngle );

				//currentRotation = Rotation.From( new Angles( currentRotation.Pitch(), currentAngle, currentRotation.Roll() ) );
				//pawn.Transform.Rota
				
				//pawn.Rotation.RotateAroundAxis( pawn.Position, 10 );
				if(currentAngle > targetYaw)
					targetYaw = random.Next( 0, 90 );
			} else if( currentAngle > targetYaw )
			{
				currentAngle -= 1f;
				Rotation currentRotation = Rotation.FromYaw( currentAngle ); ;
				pawn.Rotation = Rotation.Lerp( pawn.Rotation, currentRotation, 1, true );
				if ( currentAngle < targetYaw )
					targetYaw = random.Next( 0, 90 );
			}
		}


	}
}
