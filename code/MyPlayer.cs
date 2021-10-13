using Sandbox;

namespace sandgame
{
    class MyPlayer : Player
    {
		TimeSince timeSinceDeath;

		public TimeSince TimeSinceDeath
		{
			get { return timeSinceDeath; }
		}

		public override void Respawn()
		{
			DebugOverlay.ScreenText( "Respawning..." );
			SetModel("models/citizen/citizen.vmdl");

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

			LifeState = LifeState.Alive;

			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			Log.Error( "You died!" );
			LifeState = LifeState.Dead; 
			timeSinceDeath = 0;

			EnableDrawing = false;

			var ragdoll = new ModelEntity();
			ragdoll.SetModel( "models/citizen/citizen.vmdl" );
			ragdoll.Position = Position;
			ragdoll.Rotation = Rotation;
			ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			ragdoll.Velocity = EyeRot.Forward * 500f;
			ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000f;
		}
	}
}
