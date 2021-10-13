using Sandbox;

namespace sandgame
{
	partial class SandgamePlayer : Player
	{
		TimeSince timeSinceDeath;
		private DamageInfo lastDamage;


		public TimeSince TimeSinceDeath
		{
			get { return timeSinceDeath; }
		}

		public override void Respawn()
		{
			DebugOverlay.ScreenText( "Respawning..." );
			SetModel( "models/citizen/citizen.vmdl" );

			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			Controller = new WalkController();

			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			Animator = new StandardPlayerAnimator();

			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			Log.Error( "You died!" );
			timeSinceDeath = 0;

			EnableDrawing = false;

			BecomeRagdollOnClient( Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone( lastDamage.HitboxIndex ) );
		}

		public override void TakeDamage( DamageInfo info )
		{
			if ( GetHitboxGroup( info.HitboxIndex ) == 1 )
			{
				info.Damage *= 10.0f;
			}

			lastDamage = info;

			TookDamage( lastDamage.Flags, lastDamage.Position, lastDamage.Force );

			base.TakeDamage( info );
		}

		[ClientRpc]
		public void TookDamage( DamageFlags damageFlags, Vector3 forcePos, Vector3 force )
		{
		}
	}
}
