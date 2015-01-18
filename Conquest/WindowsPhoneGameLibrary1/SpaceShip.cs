using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace ConquestLibrary
{
	public abstract class SpaceShip
	{
		Microsoft.Xna.Framework.Game game;
		Color color;
		Texture2D texture;
		Vector2 topLeft;
		Vector2 velocity;
		Vector2 origin;
		Vector2 NosePoint;		


		float height;
		float width;

		float actualRotation;
		float required_rotation;
		bool isRotating = false;

		public SpaceShip(Microsoft.Xna.Framework.Game game, Color color, Texture2D texture, Vector2 topLeft, Vector2 velocity)
		{
			this.game = game;
			this.color = color;
			this.texture = texture;
			this.topLeft = topLeft;
			this.velocity = velocity;
			
			
			height = texture.Height;
			width = texture.Width;

			origin = new Vector2(width / 2, height / 2);

			Scale = 1;
			actualRotation = Rotation;
		}

		public float Scale { get; set; }

		private Vector2 Nose(Vector2 newTopLeft)
		{
			newTopLeft = newTopLeft + velocity;
			float angle = RotationInTurns % 4;

			if (angle < 0)
				angle = 4 + angle;

			float x = 0;
			float y = 0;

			if (angle >=0 && angle <= 1)
			{					
				float adjuster = angle - 0;
				float xstart = topLeft.X;
				float xfinish = topLeft.X + width;
				float ystart = topLeft.Y;
				float yfinish = topLeft.Y - (0.5f * height);
					
				x = CalculateDifference(xstart, xfinish, adjuster);
				y = CalculateDifference(ystart, yfinish, adjuster);
			}
			else if (angle > 1 && angle <= 2)
			{					
				float adjuster = angle - 1;
				float xstart = topLeft.X + (width) - (0.5f * height);
				float xfinish = topLeft.X + (width);
				float ystart = topLeft.Y - (0.5f * height);
				float yfinish = topLeft.Y + height;

				x = CalculateDifference(xstart, xfinish, adjuster);
				y = CalculateDifference(ystart, yfinish, adjuster);
			}
			else if (angle > 2 && angle <= 3)
			{					
				float adjuster = angle - 2;
				float xstart = topLeft.X;
				float xfinish = topLeft.X - (width);
				float ystart = topLeft.Y + height;
				float yfinish = topLeft.Y - (0.5f * height);						

				x = CalculateDifference(xstart, xfinish, adjuster);
				y = CalculateDifference(ystart, yfinish, adjuster);
			}
			else if (angle > 3 && angle < 4)
			{					
				float adjuster = angle - 3;
				float xstart = topLeft.X - (width);
				float xfinish = topLeft.X;
				float ystart = topLeft.Y - (0.5f * height);
				float yfinish = topLeft.Y;
					
				x = CalculateDifference(xstart, xfinish, adjuster);
				y = CalculateDifference(ystart, yfinish, adjuster);
			}

			NosePoint = new Vector2(x, y);
			return NosePoint;
		}

		private float CalculateDifference(float start, float finish, float adjuster)
		{
			float difference = Math.Abs(start - finish);
			difference = difference * adjuster;

			if (finish < start)
				difference = difference * -1;

			return start + difference;
		}

		private float Rotation {
			get
			{
				float rotation = velocity.Y / velocity.X;

				if ( velocity.Y < 0 && velocity.X > 0)
					rotation = velocity.X / velocity.Y * -1;

				if (velocity.Y < 0 && velocity.X < 0)
					rotation = velocity.X / velocity.Y * -1;

				return rotation * 0.5f * (float)Math.PI;
			}
		}


		private float RotationInTurns
		{
			get
			{
				float rotation = velocity.Y / velocity.X;

				if (velocity.Y < 0 && velocity.X > 0)
					rotation = velocity.X / velocity.Y * -1;

				if (velocity.Y < 0 && velocity.X < 0)
					rotation = velocity.X / velocity.Y * -1;

				return rotation;
			}
		}

		public void Draw(SpriteBatch batch)
		{
			batch.Begin();
			batch.Draw(texture, topLeft, null, color, actualRotation, origin, Scale, SpriteEffects.None, 0);			
			//batch.DrawString(game.Content.Load<SpriteFont>("Segoe14"), "O", NosePoint, Color.Red);
			batch.End();
		}

		public void Update()
		{

			Rotate();
			
			if (isRotating) return;

			MoveShip();
			topLeft += velocity; 
		}

		private void MoveShip()
		{
			Vector2 newTopLeft = Nose(topLeft + velocity);

			if (newTopLeft.Y < 0 || newTopLeft.Y > game.GraphicsDevice.Viewport.Height)
			{
				velocity.Y *= -1;
			}

			if (newTopLeft.X < 0 || newTopLeft.X > game.GraphicsDevice.Viewport.Width)
			{
				velocity.X *= -1;
			}
			
		}

		private void Rotate()
		{
			if (!isRotating)
				required_rotation = Rotation;

			if (actualRotation < required_rotation)
			{	
				actualRotation = actualRotation + 0.1f;				
				isRotating = true;
			}
			else if (actualRotation > required_rotation)
			{
				actualRotation = actualRotation - 0.1f;
				isRotating = true;
			}
			else
			{
				isRotating = false;
			}

			if (Math.Abs(actualRotation - required_rotation) < 0.1)
			{
				isRotating = false;
				actualRotation = required_rotation;
			}
		}	
	}
}
