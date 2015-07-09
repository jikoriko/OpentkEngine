using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace OpenTKEngine
{
    public class Collider
    {
        public Vector2 Position;
        public Vector2 Size;

        /// <summary>
        ///     Who to collide with.
        /// </summary>
        public List<int> Tags;

        /// <summary>
        ///     Collider object to test collisions
        /// </summary>
        /// <param name="startingPos">inital (top left) position of hitbox.</param>
        /// <param name="size">the size of the hitbox</param>
        /// <param name="tags">tags of entities who should be interacted with. make null for all</param>
        public Collider(Vector2 startingPos, Vector2 size, List<int> tags)
        {
            Size = size;
            Position = startingPos;
            Tags = tags;
        }

        /// <summary>
        ///     Check collision with a point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CheckCollision(int x, int y)
        {
            if ((!(x >= Position.X)) || (!(x <= Position.X + Size.X))) return false;
            return (y >= Position.Y) && (y <= Position.Y + Size.Y);
        }

        /// <summary>
        ///     BBOX collision test with colliders.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckCollision(Collider other)
        {
            // Black magic to find common values
            List<int> both = Tags.Intersect(other.Tags).ToList();
            bool canCollide = both.Count > 0;

            if (!canCollide) return false;

            // Fack da po po
            return ((Position.X < other.Position.X + other.Size.X) && (other.Position.X < Position.X + Size.X) &&
                    (Position.Y < other.Position.Y + other.Size.Y) && (other.Position.Y < Position.Y + Size.Y));
        }

        public bool CheckCollision(float x, float y, float w, float h)
        {
            // For debug and some other stuff dunno. 
            return ((Position.X < x + w) && (x < Position.X + Size.X) &&
                    (Position.Y < y + h) && (y < Position.Y + Size.Y));
        }
    }
}