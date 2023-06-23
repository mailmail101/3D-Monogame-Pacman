using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using  Shapes;

namespace QuokkaEngine;
public enum GameState
    {
        GameOver,
        Playing,
        WinGame
    }
public enum CameraType
    {
        freeCamera,
        playerCamera,
        topDownCamera
    }
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private BasicEffect _basicEffect;
    public int tick = 0; 
    public float playerSpeed = 1.3f;
    public float cameraSpeed = 1f;

    public Camera freeCamera;
    public int timeInvincible = 0;
    public Camera topDownCamera;
    public Camera playerCamera;
    public Physics physics;
    public CameraType currentCamera = CameraType.topDownCamera;
    private SpriteBatch _spriteBatch;
    public List<RectangularPrism> ghosts = new List<RectangularPrism>();
    public List<RectangularPrism> powerOrbs = new List<RectangularPrism>();
    private Vector2 mouseDelta;
    public GameState gameState = GameState.Playing;
    List<RectangularPrism> wallSegments;
    public RectangularPrism player;
    private Vector3 position = new Vector3(0f, 0f, 0f);
    public List<VertexPositionColorNormalTexture> _vertexs = new List<VertexPositionColorNormalTexture>();
    private float angle = 0;
    List<RectangularPrism> dots = new List<RectangularPrism>();

    public Game1()
    {
        this.IsMouseVisible = false;
        Window.AllowUserResizing = true;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Mouse.SetPosition((GraphicsDevice.Viewport.Width / 2), (GraphicsDevice.Viewport.Height / 2));
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        physics = new Physics();
        freeCamera = new Camera();
        topDownCamera = new Camera(new Vector3(130,290,-145));
        topDownCamera.XRotation = -1.5707963f;
        topDownCamera.YRotation = 3.141592f;
        playerCamera = new Camera();
        player = new RectangularPrism(new Vector3(142, 0, -227), new Vector3( 2, 6, 2), Color.YellowGreen);
        _basicEffect = new BasicEffect(GraphicsDevice);
        _basicEffect.VertexColorEnabled = true;
        _basicEffect.LightingEnabled = true;
        _basicEffect.AmbientLightColor = new Vector3(1f,1f,1f);
       wallSegments = WallCode();
       ghosts.Add(new RectangularPrism(new Vector3(17, 1, -17), new Vector3(4, 7, 4), Color.Red));
       ghosts.Add(new RectangularPrism(new Vector3(157, 1, -7), new Vector3(4, 7, 4), Color.Cyan));
       ghosts.Add(new RectangularPrism(new Vector3(57, 1, -197), new Vector3(4, 7, 4), Color.Pink));
       ghosts.Add(new RectangularPrism(new Vector3(187, 1, -197), new Vector3(4, 7, 4), Color.Orange));
       dots = DotCode();
        Matrix projection = Matrix.
        CreatePerspectiveFieldOfView(
        (float)Math.PI / 3.0f, 
            (float)this.Window.ClientBounds.Width / 
            (float)this.Window.ClientBounds.Height, 
            1f, 1000);
        _basicEffect.Projection = projection;
       //Matrix V = Matrix.CreateTranslation(position);
        _basicEffect.View = freeCamera.View;
        // TODO: use this.Content to load your game content here
        powerOrbs.Add(new RectangularPrism(new Vector3(17 , 2, -27) ,new Vector3(4, 3, 4), Color.LightGoldenrodYellow));        powerOrbs.Add(new RectangularPrism(new Vector3(17 , 2, -27) ,new Vector3(4, 3, 4), Color.LightGoldenrodYellow));
        powerOrbs.Add(new RectangularPrism(new Vector3(267 , 2, -27) ,new Vector3(4, 3, 4), Color.LightGoldenrodYellow));
        powerOrbs.Add(new RectangularPrism(new Vector3(17 , 2, -227) ,new Vector3(4, 3, 4), Color.LightGoldenrodYellow));
        powerOrbs.Add(new RectangularPrism(new Vector3(267 , 2, -227) ,new Vector3(4, 3, 4), Color.LightGoldenrodYellow));
    }

    protected override void Update(GameTime gameTime)
    {
        tick++;
        if(timeInvincible > 0){timeInvincible --;}
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // change cameras
        if(Keyboard.GetState().IsKeyDown(Keys.D1)) {currentCamera = CameraType.playerCamera;}
        else if(Keyboard.GetState().IsKeyDown(Keys.D2)) {currentCamera = CameraType.topDownCamera;}
        else if(Keyboard.GetState().IsKeyDown(Keys.D3)) {currentCamera = CameraType.freeCamera;}
        GhostMovement(ghosts);
        // mouse tracking 
        mouseDelta = new Vector2(Mouse.GetState().X - (GraphicsDevice.Viewport.Width / 2), Mouse.GetState().Y - (GraphicsDevice.Viewport.Height / 2));
        Mouse.SetPosition((GraphicsDevice.Viewport.Width / 2), (GraphicsDevice.Viewport.Height / 2));
        if(currentCamera == CameraType.freeCamera)
        {
            FreeCamControls(cameraSpeed);
            _basicEffect.View = freeCamera.Update();
        }
        else if(currentCamera == CameraType.playerCamera)
        {
            PlayerCamControls(cameraSpeed);
            _basicEffect.View = playerCamera.Update();
        }
        else if(currentCamera == CameraType.topDownCamera)
        {
            // player controls
            if(Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) {player.velocity.Z = playerSpeed;}
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) {player.velocity.Z = -playerSpeed;}
            else{player.velocity.Z = 0;}
            if(Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) {player.velocity.X = playerSpeed;}
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) {player.velocity.X = -playerSpeed;}
            else{player.velocity.X = 0;}
            player.Update();
            _basicEffect.View = topDownCamera.Update();
        }
        _vertexs = new List<VertexPositionColorNormalTexture>();
        foreach(RectangularPrism prism in wallSegments)
        {
            _vertexs.AddRange(prism._vertexs);
        }
        physics.Collision(player, wallSegments.ToArray());
        if((currentCamera == CameraType.playerCamera) == false)
        {
        _vertexs.AddRange(player._vertexs);

        }
        RectangularPrism toBeRemoved = new RectangularPrism(Vector3.Zero, Vector3.Zero, Color.White);
        foreach(RectangularPrism dot in dots)
        {
        // collision of yellow somthings?
        RectangularPrism[] temparray = new RectangularPrism[1];
        temparray[0] = player;
        if(physics.Collision(dot, temparray)){toBeRemoved = dot;}
        _vertexs.AddRange(dot._vertexs);
        }
        dots.Remove(toBeRemoved);
        toBeRemoved = new RectangularPrism(Vector3.Zero, Vector3.Zero, Color.White);
        foreach(RectangularPrism orb in powerOrbs)
        {
        // collision of yellow somthings?
        RectangularPrism[] temparray = new RectangularPrism[1];
        temparray[0] = player;
        if(physics.Collision(orb, temparray)){toBeRemoved = orb; timeInvincible += 200;}
        _vertexs.AddRange(orb._vertexs);
        }
        powerOrbs.Remove(toBeRemoved);
        foreach(RectangularPrism ghost in ghosts)
        {
            ghost.Update();
            if (timeInvincible > 0)
            {
                for(int i = 0; i < ghost._vertexs.Length;i++)
                {
                    VertexPositionColorNormalTexture tempVertexGhost = ghost._vertexs[i];
                    tempVertexGhost.Color = Color.White;
                    _vertexs.Add(tempVertexGhost);
                }
            }
            else {_vertexs.AddRange(ghost._vertexs);}
        }
        // win condition
        if (dots.Count <= 0)
        {
            gameState = GameState.WinGame;
        }
        
        // collision with ghost
        if(physics.Collision(player, ghosts.ToArray()) && timeInvincible <= 0)
        {gameState = GameState.GameOver;}
        // sets the view point
        if (angle > 2 * Math.PI) angle = 0;

        // tele
        if(player._vertexs[0].Position.X < 5)
        {player._position.X += 275;}
        else if(player._vertexs[0].Position.X > 285)
        {player._position.X -= 275;}

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
         GraphicsDevice.Clear(Color.Purple);
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
        if(GameState.GameOver == gameState)
        {GraphicsDevice.Clear(Color.Red);}
        if(GameState.WinGame == gameState)
        {GraphicsDevice.Clear(Color.Green);}
        base.Draw(gameTime);
    }
    private void PlayerCamControls(float speed)
    {
         // player controls
         if(Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) {player.velocity.Z = playerSpeed;}
         else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) {player.velocity.Z = -playerSpeed;}
         else{player.velocity.Z = 0;}
         if(Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) {player.velocity.X = playerSpeed;}
         else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) {player.velocity.X = -playerSpeed;}
         else{player.velocity.X = 0;}
         player.velocity = - (Vector3.Transform(player.velocity, Matrix.CreateRotationY(playerCamera.YRotation)) );
        playerCamera.Position = player.middleOfBoundingBox + new Vector3(0, 3, 0);
        playerCamera.YRotation += -mouseDelta.X / 200f;
        playerCamera.XRotation += -mouseDelta.Y / 200f;
        if (playerCamera.XRotation > 0.7f){playerCamera.XRotation = 0.5f;}
        else if (playerCamera.XRotation < -0.5f){playerCamera.XRotation = -0.5f;}
        player.Update();
    }
    private void FreeCamControls(float speed)
    {
        // camera controls
        // up down
        if(Keyboard.GetState().IsKeyDown(Keys.Z))
            freeCamera.Up(speed);
        if(Keyboard.GetState().IsKeyDown(Keys.X))
            freeCamera.Down(speed);
        // back forward
        if(Keyboard.GetState().IsKeyDown(Keys.S))
            freeCamera.Back(speed);
        if(Keyboard.GetState().IsKeyDown(Keys.W))
            freeCamera.Forward(speed);
        // left right
        if(Keyboard.GetState().IsKeyDown(Keys.D))
            freeCamera.Right(speed);
        if(Keyboard.GetState().IsKeyDown(Keys.A))
            freeCamera.Left(speed);
        // rotate
        if(Keyboard.GetState().IsKeyDown(Keys.Q))
            freeCamera.YRotation += 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.E))
            freeCamera.YRotation -= 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.R))
            freeCamera.XRotation += 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.T))
            freeCamera.XRotation -= 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.F))
            freeCamera.ZRotation += 0.01f * (float)Math.PI;
        if(Keyboard.GetState().IsKeyDown(Keys.G))
            freeCamera.ZRotation -= 0.01f * (float)Math.PI;
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
    
    public List<RectangularPrism> DotCode()
    {
        List<RectangularPrism> dots = new List<RectangularPrism>();
        for (int i = 0; i < 12; i ++)
        {
            dots.Add(new RectangularPrism(new Vector3(16 + 10 * i, 2, -6) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(156 + 10 * i, 2, -6) ,new Vector3(2, 2, 2), Color.Yellow));
        }
        for (int i = 0; i < 26; i ++)
        {
            dots.Add(new RectangularPrism(new Vector3(16 + 10 * i, 2, -286) ,new Vector3(2, 2, 2), Color.Yellow));
        }
        for (int i = 0; i < 26; i ++)
        {
            dots.Add(new RectangularPrism(new Vector3(216, 2,i * 10 -256) ,new Vector3(2, 2, 2), Color.Yellow));
        }
        for (int i = 0; i < 26; i ++)
        {
            dots.Add(new RectangularPrism(new Vector3(66, 2,i * 10 -256) ,new Vector3(2, 2, 2), Color.Yellow));
        }
        dots.Add(new RectangularPrism(new Vector3(16, 2, -16) ,new Vector3(2, 2, 2), Color.Yellow));
        dots.Add(new RectangularPrism(new Vector3(267, 2,-16) ,new Vector3(2, 2, 2), Color.Yellow));
        for (int i = 0; i < 5;i++)
        {
            dots.Add(new RectangularPrism(new Vector3(16, 2,i * 10 -76) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(267, 2,i * 10 -76) ,new Vector3(2, 2, 2), Color.Yellow));
        }
        for(int i = 0;i< 4; i++)
        {
            dots.Add(new RectangularPrism(new Vector3(10 * i + 26, 2,-46) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(-10 * i + 56, 2,-76) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(10 * i + 226, 2,-46) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(-10 * i + 256, 2,-76) ,new Vector3(2, 2, 2), Color.Yellow));
        }
        for(int i =0; i<3;i++)
        {
            dots.Add(new RectangularPrism(new Vector3(16, 2,i * 10 -276) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(126, 2,i * 10 -276) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(156, 2,i * 10 -276) ,new Vector3(2, 2, 2), Color.Yellow));
            dots.Add(new RectangularPrism(new Vector3(256, 2,i * 10 -276) ,new Vector3(2, 20, 2), Color.Yellow));
        }

        return dots;

    }
    private void GhostMovement(List<RectangularPrism> ghost)
    {
        int ghostTick1 = tick % 540;
        if ((tick % 20) >= 10){ghost[0]._position.Y  -= 0.025f;}
        else {ghost[0]._position.Y += 0.025f;}
        if (ghostTick1 <= 10){ghost[0]._position.Z ++;}
        else if (ghostTick1 <= 120){ghost[0]._position.X ++;}
        else if (ghostTick1 <= 160){ghost[0]._position.Z --;}
        else if (ghostTick1 <= 190){ghost[0]._position.X --;}
        else if (ghostTick1 <= 220){ghost[0]._position.Z --;}
        else if (ghostTick1 <= 250){ghost[0]._position.X ++;}
        else if (ghostTick1 <= 280){ghost[0]._position.Z --;}
        else if (ghostTick1 <= 310){ghost[0]._position.X --;}
        else if (ghostTick1 <= 340){ghost[0]._position.Z --;}
        else if (ghostTick1 <= 370){ghost[0]._position.X --;}
        else if (ghostTick1 <= 430){ghost[0]._position.Z ++;}
        else if (ghostTick1 <= 480){ghost[0]._position.X --;}
        else if (ghostTick1 <= 540){ghost[0]._position.Z ++;}

         if ((tick % 20) >= 10){ghost[1]._position.Y  -= 0.03f;}
        else {ghost[1]._position.Y += 0.03f;}
        int ghostTick2 = tick % 540;
        if (ghostTick2 <= 60){ghost[1]._position.X ++;}
        else if (ghostTick2 <= 100){ghost[1]._position.Z --;}
        else if (ghostTick2 <= 150){ghost[1]._position.X ++;}
        else if (ghostTick2 <= 180){ghost[1]._position.Z --;}
        else if (ghostTick2 <= 230){ghost[1]._position.X --;}
        else if (ghostTick2 <= 290){ghost[1]._position.Z --;}
        else if (ghostTick2 <= 320){ghost[1]._position.X --;}
        else if (ghostTick2 <= 350){ghost[1]._position.Z ++;}
        else if (ghostTick2 <= 380){ghost[1]._position.X --;}
        else if (ghostTick2 <= 410){ghost[1]._position.Z  ++;}
        else if (ghostTick2 <= 440){ghost[1]._position.X ++;}
        else if (ghostTick2 <= 470){ghost[1]._position.Z ++;}
        else if (ghostTick2 <= 500){ghost[1]._position.X --;}
        else if (ghostTick2 <= 540){ghost[1]._position.Z ++;}
        if(ghostTick2 == 539){ghost[1]._position = Vector3.Zero;}

        if ((tick % 20) >= 10){ghost[2]._position.Y  -= 0.03f;}
        else {ghost[2]._position.Y += 0.03f;}
        int ghostTick3 = tick % 440;
        if (ghostTick3 <= 10){ghost[2]._position.X ++;}
        else if (ghostTick3 <= 40){ghost[2]._position.Z --;}
        else if (ghostTick3 <= 70){ghost[2]._position.X ++;}
        else if (ghostTick3 <= 100){ghost[2]._position.Z --;}
        else if (ghostTick3 <= 130){ghost[2]._position.X ++;}
        else if (ghostTick3 <= 160){ghost[2]._position.Z --;}
        else if (ghostTick3 <= 270){ghost[2]._position.X --;}
        else if (ghostTick3 <= 300){ghost[2]._position.Z ++;}
        else if (ghostTick3 <= 320){ghost[2]._position.X ++;}
        else if (ghostTick3 <= 350){ghost[2]._position.Z ++;}
        else if (ghostTick3 <= 370){ghost[2]._position.X --;}
        else if (ghostTick3 <= 400){ghost[2]._position.Z ++;}
        else if (ghostTick3 <= 440){ghost[2]._position.X ++;}
         
        if ((tick % 20) >= 10){ghost[3]._position.Y  -= 0.03f;}
        else {ghost[3]._position.Y += 0.03f;}
        int ghostTick4 = tick % 340;
        if (ghostTick4 <= 80){ghost[3]._position.X ++;}
        else if (ghostTick4 <= 110){ghost[3]._position.Z --;}
        else if (ghostTick4 <= 130){ghost[3]._position.X --;}
        else if (ghostTick4 <= 160){ghost[3]._position.Z --;}
        else if (ghostTick4 <= 190){ghost[3]._position.X --;}
        else if (ghostTick4 <= 220){ghost[3]._position.Z ++;}
        else if (ghostTick4 <= 280){ghost[3]._position.X --;}
        else if (ghostTick4 <= 310){ghost[3]._position.Z ++;}
        else if (ghostTick4 <= 340){ghost[3]._position.X ++;}
    }
}
