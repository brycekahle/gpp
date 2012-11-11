using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gpp
{
    public class GameObject
    {
        /// <summary>
        /// (m*m*m)/kg/s/s
        /// </summary>
        public const float GravitationConstant = 6.673E-11f;

        public SupermassiveGame Game { get; private set; }

        /// <summary>
        /// Meters from origin
        /// </summary>
        public Vector2 Position { get; protected set; }

        public Vector2 Heading { get; protected set; }

        /// <summary>
        /// KG
        /// </summary>
        public float Mass { get; protected set; }

        protected float _scale;

        protected readonly Texture2D _texture;

        public BoundingSphere BoundingSphere { get; protected set; }

        private Animation _currentAnimation = Animation.Default;
        private double _timeOnAnimation = 0;
        private int _frameWidth = 200;
        private int _frameHeight = 200;
        private double _timePerFrame = 0.033f;

        public GameObject(SupermassiveGame game, Texture2D texture, Vector2 position, float scale, float mass)
        {
            Game = game;
            _texture = texture;
            Position = position;
            _scale = scale;
            Mass = mass;
            BoundingSphere = GetBoundingSphere();
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            
        }

        public void SetAnimation(Animation animation)
        {
            if (animation == _currentAnimation)
            {
                return;
            }
            _currentAnimation = animation;
            _timeOnAnimation = 0;
        }

        public virtual void Draw(SpriteBatch batch, TimeSpan elapsedTime)
        {
            var headingAngle = (float)AngleBetweenVectors2(new Vector2(0, -1), Heading);
            if (float.IsNaN(headingAngle)) headingAngle = 0;

            if (!(this is Player))
            {
                batch.Draw(_texture, Position, null, Color.White, headingAngle,
                           new Vector2((float)_texture.Width / 2, (float)_texture.Height / 2),
                           _scale, SpriteEffects.None, 0);
            }
            else
            {
                _timeOnAnimation += elapsedTime.TotalSeconds;
                var currentFrame = (int)(_timeOnAnimation/_timePerFrame) % GetFrameCount();
                var spriteSheetX = currentFrame*_frameWidth;
                var spriteSheetY = (int) _currentAnimation*_frameHeight;
                batch.Draw(_texture,
                           Position,
                           new Rectangle(spriteSheetX, spriteSheetY,
                                         _frameWidth, _frameHeight),
                           Color.White, headingAngle,
                           new Vector2((float) _frameWidth/2,
                                       (float) _frameHeight/2),
                           _scale,
                           SpriteEffects.None, 0);
            }
        }

        private int GetFrameCount()
        {
            switch (_currentAnimation)
            {
                case Animation.Default:
                    return 1;
                case Animation.Walking:
                    return 8;
                case Animation.Squatting:
                    return 3;
            }
            return 1;
        }

        public double AngleBetweenVectors2(Vector2 v1, Vector2 v2)
        {
            return ((v1.X - v2.X) > 0 ? -1 : 1) * (float)Math.Acos((double)Vector2.Dot(Vector2.Normalize(v1), Vector2.Normalize(v2)));
        }

        protected virtual BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(new Vector3(Position.X, Position.Y, 0), _scale*_frameHeight/2f);
        }
    }
}
