using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace OpenTKEngine
{
    public class Particle
    {
        public Vector3 Position;
        public Vector3 Direction;
        public int CurrentTime = 1;
        public Color4 Color = Color4.White;
        public bool Gravity;

        public void Update()
        {
            if (Gravity)
            {
                Direction.Y += 2f;
            }
            Position = Position + Direction;

            CurrentTime++;
        }
    }

    public class ParticleEffect
    {
        public Vector3 Position;
        public bool Active = false;

        private Texture _texture;
        private int _spawnRate;
        private int _lifeSpan;
        private bool _gravity;
        private int _ticker = 0;
        private List<Particle> _particles;
        private Random _rand;
 
        public ParticleEffect(Vector3 position, Texture texture, int spawnRate, int lifeSpan, bool gravity)
        {
            _rand = new Random(Guid.NewGuid().GetHashCode());

            Position = position;
            this._texture = texture;
            _spawnRate = spawnRate;
            _lifeSpan = lifeSpan;
            _gravity = gravity;

            _particles = new List<Particle>();
        }

        public void Update()
        {
            if (!Active) return;

            _ticker++;

            if (_ticker >= _spawnRate)
            {
                var p = new Particle();
                p.Position = Position;
                p.Gravity = _gravity;
                p.Direction = new Vector3(_rand.Next(-10, 10), _rand.Next(-10, 10), 0);
                _particles.Add(p);
                _ticker = 0;
            }

            for (int i = 0; i < _particles.Count - 1; i++)
            {
                var particle = _particles[i];

                if (particle == null) return;

                particle.Update();

                // Die
                if (particle.CurrentTime > _lifeSpan)
                {
                    _particles.Remove(particle);
                }
            }
        }

        public void Clear()
        {
            _particles.Clear();
        }

        public void Render()
        {
            if (!Active) return;

            foreach (var particle in _particles)
            {

                Graphics.DrawTexture(_texture, particle.Position, particle.Color);
            }
        }
    }
}
