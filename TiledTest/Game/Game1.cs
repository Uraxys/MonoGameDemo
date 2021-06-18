using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace TiledTest.Game 
{
	public class Game1 : Microsoft.Xna.Framework.Game 
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		
		private Vector2 _cameraPosition;
		private OrthographicCamera _camera;
		
		TiledMap _tiledMap;
		TiledMapRenderer _tiledMapRenderer;

		public Game1() 
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize() 
		{
			_graphics.PreferredBackBufferHeight = 540;
			_graphics.PreferredBackBufferWidth = 960;
			_graphics.ApplyChanges();
			
			_tiledMap = Content.Load<TiledMap>("tiled/DemoTiledMap");
			_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
			
			var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 480, 270);
			_camera = new OrthographicCamera(viewportadapter);

			_spriteBatch = new SpriteBatch(GraphicsDevice);
			base.Initialize();
		}

		protected override void LoadContent() 
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime) 
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			_tiledMapRenderer.Update(gameTime);
			
			MoveCamera(gameTime);
			_camera.LookAt(_cameraPosition);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) 
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			
			GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			_tiledMapRenderer.Draw(_camera.GetViewMatrix());

			_spriteBatch.Begin();
			_spriteBatch.End();
			
			base.Draw(gameTime);
		}
		
		private Vector2 GetMovementDirection()
		{
			var movementDirection = Vector2.Zero;
			var state = Keyboard.GetState();
			if (state.IsKeyDown(Keys.Down))
			{
				movementDirection += Vector2.UnitY;
			}
			if (state.IsKeyDown(Keys.Up))
			{
				movementDirection -= Vector2.UnitY;
			}
			if (state.IsKeyDown(Keys.Left))
			{
				movementDirection -= Vector2.UnitX;
			}
			if (state.IsKeyDown(Keys.Right))
			{
				movementDirection += Vector2.UnitX;
			}
    
			// Can't normalize the zero vector so test for it before normalizing
			if (movementDirection != Vector2.Zero)
			{
				movementDirection.Normalize(); 
			}
    
			return movementDirection;
		}

		private void MoveCamera(GameTime gameTime)
		{
			var speed = 200;
			var seconds = gameTime.GetElapsedSeconds();
			var movementDirection = GetMovementDirection();
			_cameraPosition += speed * movementDirection * seconds;
		}
	}
}