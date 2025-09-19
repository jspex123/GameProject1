using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameProject1.Collisions
{
	public struct BoundingCircle
	{
		public Vector2 Center;

		public float Radius;

		public BoundingCircle(Vector2 center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public bool CollidesWith(BoundingCircle other)
		{
			return CollisionHelper.Collides(this, other);
		}

		public bool CollidesWith(BoundingRectangle other)
		{
			return CollisionHelper.Collides(this, other);
		}
	}
}
