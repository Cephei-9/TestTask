using Chrono;
using Engine;
using Time.Infrastructure;
using UnityEngine;

namespace Bullet.ConcreteBullets
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class SimpleBullet : BulletBase<BulletData>
	{
		[SerializeField]
		private ChronoObjectCollider ChronoCollider;

		private PhysicsChronoEngine _engine;
		private ChronoObject _chronoObject;

		public override void Shoot(Vector2 velocity, ITimeProvider timeProvider)
		{
			_chronoObject = new ChronoObject(timeProvider, transform);
			ChronoCollider.Init(_chronoObject);
			
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			_engine = new PhysicsChronoEngine(_chronoObject, timeProvider, _data.EngineData, rb);
			_engine.AddChronoForce(velocity * _data.ShootVelocityMultiply, ForceMode.VelocityChange);
			
			base.Shoot(velocity, _chronoObject);
		}

		protected override void OnBulletCollision(Collision2D col)
		{
			base.OnBulletCollision(col);
			
			_engine.OnCollision();
		}

		private void FixedUpdate()
		{
			_engine.UpdateWork();
		}

		private void Update()
		{
			_chronoObject.UpdateWork();
		}
	}
}