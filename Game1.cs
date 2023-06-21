using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using  Shapes;

namespace QuokkaEngine;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private BasicEffect _basicEffect;
    public Camera camera;
    private SpriteBatch _spriteBatch;
    List<RectangularPrism> wallSegments;
    private Vector3 position = new Vector3(0f, 0f, 0f);
    public List<VertexPositionColorNormalTexture> _vertexs = new List<VertexPositionColorNormalTexture>();
    private float angle = 0;

    public Game1()
    {
        Window.AllowUserResizing = true;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        camera = new Camera();

        _basicEffect = new BasicEffect(GraphicsDevice);
        _basicEffect.VertexColorEnabled = true;
        _basicEffect.LightingEnabled = true;
        _basicEffect.AmbientLightColor = new Vector3(0f,0f,1f);
       wallSegments = WallCode();
        Matrix projection = Matrix.
        CreatePerspectiveFieldOfView(
        (float)Math.PI / 3.0f, 
            (float)this.Window.ClientBounds.Width / 
            (float)this.Window.ClientBounds.Height, 
            1f, 1000);
        _basicEffect.Projection = projection;
       //Matrix V = Matrix.CreateTranslation(position);
        _basicEffect.View = camera.View;
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        float speed = 1f;
        // camera controls
        // up down
        if(Keyboard.GetState().IsKeyDown(Keys.Z))
            camera.Up(speed);
        if(Keyboard.GetState().IsKeyDown(Keys.X))
            camera.Down(speed);
        // back forward
        if(Keyboard.GetState().IsKeyDown(Keys.S))
            camera.Back(speed);
        if(Keyboard.GetState().IsKeyDown(Keys.W))
            camera.Forward(speed);
        // left right
        if(Keyboard.GetState().IsKeyDown(Keys.D))
            camera.Right(speed);
        if(Keyboard.GetState().IsKeyDown(Keys.A))
            camera.Left(speed);
        // rotate
        if(Keyboard.GetState().IsKeyDown(Keys.Q))
            camera.YRotation += 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.E))
            camera.YRotation -= 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.R))
            camera.XRotation += 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.T))
            camera.XRotation -= 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.F))
            camera.ZRotation += 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.G))
            camera.ZRotation -= 0.01f * (float)Math.PI;
        _vertexs = new List<VertexPositionColorNormalTexture>();
        foreach(RectangularPrism prism in wallSegments)
        {
            _vertexs.AddRange(prism._vertexs);
        }
        // sets the view point
        _basicEffect.View = camera.Update();
        if (angle > 2 * Math.PI) angle = 0;



        // TODO: Add your update logic here
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
         GraphicsDevice.Clear(Color.Black);
        _graphics.GraphicsDevice.RasterizerState= new RasterizerState()
        {
            FillMode = FillMode.Solid,
            CullMode = CullMode.None,
            DepthBias = 0,
            MultiSampleAntiAlias = true,
            SlopeScaleDepthBias = 0,
        };
        EffectPassCollection effectPassCollection = _basicEffect.CurrentTechnique.Passes ;
        foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
        {
        pass.Apply();
        _graphics.GraphicsDevice.DrawUserPrimitives
        <VertexPositionColorNormalTexture>(
            PrimitiveType.TriangleList,_vertexs.ToArray(), 
                                        0, _vertexs.Count / 3);
        }

        base.Draw(gameTime);
    }
    public List<RectangularPrism> WallCode()
    {
        List<RectangularPrism> walls = new List<RectangularPrism>();
        walls.Add(new RectangularPrism(new Vector3 (280, 0, 0), new Vector3(280, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (10, 0, -90), new Vector3(10, 10, 100), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -90), new Vector3(10, 10, 100), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (150, 0, -40), new Vector3(20, 10, 40), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -300), new Vector3(280, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (10, 0, -300), new Vector3(10, 10, 110), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -300), new Vector3(10, 10, 110), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -40), new Vector3(40, 10, 30), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (260, 0, -40), new Vector3(40, 10, 30), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (120, 0, -40), new Vector3(50, 10, 30), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (210, 0, -40), new Vector3(50, 10, 30), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -70), new Vector3(40, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (260, 0, -70), new Vector3(40, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -90), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -90), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -130), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -130), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -150), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -150), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -190), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (280, 0, -190), new Vector3(60, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (150, 0, -100), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (180, 0, -70), new Vector3(80, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (210, 0, -130), new Vector3(20, 10, 80), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (90, 0, -130), new Vector3(20, 10, 80), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (120, 0, -100), new Vector3(30, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (190, 0, -100), new Vector3(30, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (210, 0, -190), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (90, 0, -190), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (30, 0, -250), new Vector3(20, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (270, 0, -250), new Vector3(20, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -130), new Vector3(10, 10, 40), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -190), new Vector3(10, 10, 40), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (230, 0, -130), new Vector3(10, 10, 40), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (230, 0, -190), new Vector3(10, 10, 40), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (150, 0, -220), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (180, 0, -190), new Vector3(80, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (90, 0, -260), new Vector3(20, 10, 30), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (120, 0, -280), new Vector3(100, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (210, 0, -260), new Vector3(20, 10, 30), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (260, 0, -280), new Vector3(100, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (120, 0, -220), new Vector3(50, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (210, 0, -220), new Vector3(50, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (60, 0, -250), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (240, 0, -250), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (40, 0, -220), new Vector3(20, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (260, 0, -220), new Vector3(20, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (150, 0, -280), new Vector3(20, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (180, 0, -250), new Vector3(80, 10, 20), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (110, 0, -160), new Vector3(10, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (180, 0, -160), new Vector3(10, 10, 50), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (130, 0, -120), new Vector3(30, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (180, 0, -120), new Vector3(30, 10, 10), Color.Blue));
        walls.Add(new RectangularPrism(new Vector3 (180, 0, -160), new Vector3(80, 10, 10), Color.Blue));
        return walls;
    }
}
