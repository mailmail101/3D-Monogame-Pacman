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
    Cube[] cube;
    private Vector3 position = new Vector3(0f, 0f, 0f);
    public VertexPositionColorNormalTexture[] _vertexs = new VertexPositionColorNormalTexture[72];
    private float angle = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        camera = new Camera();
        cube = new Cube[2];
        cube[0] = new Cube(10, new Vector3 (10, -10, -20), new Color (254, 0, 0));
        cube[1] = new Cube(32.6f, new Vector3 (-30, -100, -10), new Color (0, 0, 255));
        _basicEffect = new BasicEffect(GraphicsDevice);
        _basicEffect.VertexColorEnabled = true;
        _basicEffect.AmbientLightColor = new Vector3(0f,0f,1f);
       
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
        
        
        // sets the view point
        _basicEffect.View = camera.Update();
        cube[0].YRotation += 0.01f;
        foreach(Cube cu in cube)
        {
            cu.Update();
        }
        for (int i = 0; 36 > i; i++)
        {
            _vertexs[i] = cube[0]._vertexs[i];
            _vertexs[i + 36] = cube[1]._vertexs[i];
        }
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
            PrimitiveType.TriangleList,_vertexs, 
                                        0, _vertexs.Length / 3);
        }

        base.Draw(gameTime);
    }
}
