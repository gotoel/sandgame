using Sandbox;

namespace sandgame
{
	partial class SandgamePlayer
	{
		[ClientRpc]
		private void BecomeRagdollOnClientBasic()
		{
			var ragdoll = new ModelEntity();
			ragdoll.SetModel( GetModelName() );
			ragdoll.Position = Position;
			ragdoll.Rotation = Rotation.From(new Angles( Rotation.x, random.Next( -360, 360 ),  Rotation.z ));
			ragdoll.Scale = Scale;
			ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			ragdoll.PhysicsGroup.AddVelocity( new Vector3( random.Next( -100, 100 ), random.Next( -100, 100 ), 0) * 30f);
			ragdoll.DeleteAsync( 10.0f );

			foreach ( var child in Children )
			{
				if ( !child.Tags.Has( "clothes" ) ) continue;
				if ( child is not ModelEntity e ) continue;

				var model = e.GetModelName();

				var clothing = new ModelEntity();
				clothing.SetModel( model );
				clothing.SetParent( ragdoll, true );
				clothing.RenderColor = e.RenderColor;
				clothing.CopyBodyGroups( e );
				clothing.CopyMaterialGroup( e );
			}

			PlaySound( "ouch" );
		}

		private void BecomeRagdollOnClientFull( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force, int bone )
		{
			var ent = new ModelEntity();
			ent.Position = Position;
			ent.Rotation = Rotation;
			ent.Scale = Scale;
			ent.MoveType = MoveType.Physics;
			ent.UsePhysicsCollision = true;
			ent.EnableAllCollisions = true;
			ent.CollisionGroup = CollisionGroup.Debris;
			ent.SetModel( GetModelName() );
			ent.CopyBonesFrom( this );
			ent.CopyBodyGroups( this );
			ent.CopyMaterialGroup( this );
			ent.TakeDecalsFrom( this );
			ent.EnableHitboxes = true;
			ent.EnableAllCollisions = true;
			ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
			ent.RenderColor = RenderColor;
			ent.PhysicsGroup.Velocity = velocity;

			if ( Local.Pawn == this )
			{
				//ent.EnableDrawing = false; wtf
			}

			ent.SetInteractsAs( CollisionLayer.Debris );
			ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

			foreach ( var child in Children )
			{
				if ( !child.Tags.Has( "clothes" ) ) continue;
				if ( child is not ModelEntity e ) continue;

				var model = e.GetModelName();

				var clothing = new ModelEntity();
				clothing.SetModel( model );
				clothing.SetParent( ent, true );
				clothing.RenderColor = e.RenderColor;
				clothing.CopyBodyGroups( e );
				clothing.CopyMaterialGroup( e );
			}

			if ( damageFlags.HasFlag( DamageFlags.Bullet ) ||
				 damageFlags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				PhysicsBody body = bone > 0 ? ent.GetBonePhysicsBody( bone ) : null;

				if ( body != null )
				{
					body.ApplyImpulseAt( forcePos, force * body.Mass );
				}
				else
				{
					ent.PhysicsGroup.ApplyImpulse( force );
				}
			}

			//if ( damageFlags.HasFlag( DamageFlags.Blast ) )
			//{
			//	if ( ent.PhysicsGroup != null )
			//	{
			ent.PhysicsGroup.AddVelocity( (Position - (forcePos + Vector3.Down * 100.0f)).Normal * (force.Length * 0.2f) );
			var angularDir = (Rotation.FromYaw( 90 ) * force.WithZ( 0 ).Normal).Normal;
			ent.PhysicsGroup.AddAngularVelocity( angularDir * (force.Length * 0.02f) );
			//	}
			//}


			Corpse = ent;
			PlaySound( "ouch" );

			ent.DeleteAsync( 10.0f );
		}
	}
}
