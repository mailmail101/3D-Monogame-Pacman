using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Shapes
{
/// <summary>is a cube</summary>
public class Cube
{
    private float _pie = 3.141592653589793238462643383279502884197f;
     private Vector3[] positionInRealitionToShapePosition = new Vector3[36];
    public VertexPositionColorNormalTexture[] _vertexs = new VertexPositionColorNormalTexture[36];
     ///<summary>the size of the cube</summary>
    private Vector3 _size;
    ///<summary>the size of the cube</summary>
    public Vector3 Size
    {
        get { return _size; }
        set {Update(); _size = value; }  
    }
    ///<summary>the location of the front top right point</summary>
    private Vector3 _position;
    ///<summary>The X rotation of the shape in radians</summary>
    public float XRotation {get; set;} = 0f;
    ///<summary>The Y rotation of the shape in radians</summary>
    public float YRotation {get; set;} = 0f;
    ///<summary>The Z rotation of the shape in radians</summary>
    public float ZRotation {get; set;} = 0f;
    ///<summary>the location of the front top right point</summary>
    public Vector3 Position
    {
        get { return _position; }
        set {Update(); _position = value; }  
    }
    ///<summary>Xna color of the cube using RGB colors</summary>
    private Color _color;
    ///<summary>Xna color of the cube using RGB colors</summary>
    public Color Color
    {
        get { return _color; }
        set {Update(); _color = value; }  
    }

    /// <summary>creates a cube at the given <paramref name="size"/> and <paramref name="position"/></summary>
    /// <param name="size">the size of the cube</param>
    /// <param name="position">the location of the front top right point</param>
    /// <param name="color">Xna color of the cube using RGB colors</param>
    public Cube  (float size, Vector3 position, Color color)
    {
        _position = position;
        _size = new Vector3(size, size, size);
        _color = color;
        Update();
        for(int i = 0; i < 36; i ++) 
        {
            positionInRealitionToShapePosition[i] = _vertexs[i].Position;
        }
    }
    public void Update()
    {
        Matrix rotationMatrix = Matrix.CreateRotationX(XRotation) * Matrix.CreateRotationY(YRotation)* Matrix.CreateRotationZ(ZRotation); 
        Vector3[] face = new Vector3[6];
        //TopRight
        face[0] = new Vector3(0, 0, 0);
        //BottomRight
        face[2] = new Vector3(0, _size.Y, 0);
        //BottomLeft
        face[1] = new Vector3( -_size.X, _size.Y, 0);
        //TopRight
        face[3] = new Vector3(0, 0, 0);
        //BottomLeft
        face[5] = new Vector3( -_size.X, _size.Y, 0);
        //TopLeft
        face[4] = new Vector3( -_size.X,  0, 0);
        
        Vector3[] reverseFace = new Vector3[6];
        Array.Copy(face, reverseFace,6);
        Array.Reverse( reverseFace, 0, 6 );
        //Array.Reverse( reverseFace, 3, 3 );
        Matrix rotateX90 = Matrix.CreateRotationX(_pie / 2);
        Matrix rotateY90 = Matrix.CreateRotationY(_pie / 2);
        for (int i = 0; i < 6; i++)
        {
            //front face
            _vertexs[i].Position = (face[i] + _position);
            // back face
            _vertexs[i + 6].Position = (reverseFace[i] + new Vector3 (0, 0, _size.Z) +_position);
            // top face
            _vertexs[i + 12].Position = (Vector3.Transform(reverseFace[i], rotateX90) + _position);
            // bottom face
            _vertexs[i + 18].Position = (Vector3.Transform(face[i], rotateX90) + _position + new Vector3 (0, _size.Y, 0));
            // left face
            _vertexs[i + 24].Position = (Vector3.Transform(reverseFace[i], rotateY90) + _position);
            // right face
            _vertexs[i + 30].Position = (Vector3.Transform(face[i], rotateY90) + _position - new Vector3 (_size.X, 0, 0));
        }    
         for(int i = 0; i < 36; i += 3)
        {
            Vector3 normal = Vector3.Cross(_vertexs[i + 1].Position - _vertexs[i].Position, _vertexs[i + 2].Position - _vertexs[i].Position);
            _vertexs[i].Normal = normal;
            _vertexs[i + 1].Normal = normal;
            _vertexs[i + 2].Normal = normal;
        }
        for(int i = 0; i < 36; i++)
        {
            _vertexs[i].Position = Vector3.Transform(_vertexs[i].Position, rotationMatrix);
            _vertexs[i].TextureCoordinate = new Vector2(0, 0);
            _vertexs[i].Color = _color;
            
        }
    }
    public Cube(string fileName)
    {
        if (!File.Exists(fileName)){throw new Exception($"{fileName} does not exist");}
        int i = 0;
        foreach (string line in File.ReadLines(fileName))
        {
            string[] splitLine = line.Split(" ");
            switch(i)
            {
                case 0:
                    int numOfVertexs = Convert.ToInt32(line);
                    _vertexs = new VertexPositionColorNormalTexture[numOfVertexs];
                    break;
                case 1:
                    _position.X = float.Parse(splitLine[0]);
                    _position.Y = float.Parse(splitLine[1]);
                    _position.Z = float.Parse(splitLine[2]);
                    break;
                case 2:
                    _size.X = float.Parse(splitLine[0]);
                    _size.Y = float.Parse(splitLine[1]);
                    _size.Z = float.Parse(splitLine[2]);
                    break;
                case 3:
                    XRotation = float.Parse(splitLine[0]);
                    YRotation = float.Parse(splitLine[1]);
                    ZRotation = float.Parse(splitLine[2]);
                    break;
                default:
                    _vertexs[i].Position = new Vector3(float.Parse(splitLine[0]), float.Parse(splitLine[1]), float.Parse(splitLine[2]));
                    _vertexs[i].Normal = new Vector3(float.Parse(splitLine[3]), float.Parse(splitLine[4]), float.Parse(splitLine[5]));
                    _vertexs[i].Color = new Color(Convert.ToInt32(splitLine[6]), Convert.ToInt32(splitLine[7]), Convert.ToInt32(splitLine[8]));
                    _vertexs[i].TextureCoordinate = new Vector2(float.Parse(splitLine[9]), float.Parse(splitLine[10]));
                    break;

            }
            i++;
        }
    }
    public void Save(string fileName)
    {
        StreamWriter writer = new StreamWriter(fileName);
        writer.WriteLine(_vertexs.Length);
        writer.WriteLine($"{_position.X} {_position.Y} {_position.Z}");
        writer.WriteLine($"{1} {1} {1}");
        writer.WriteLine($"{XRotation} {YRotation} {ZRotation}");
        for( int i = 0; i < _vertexs.Length; i++)
        {
            // position of the vector 
            string pos = $"{positionInRealitionToShapePosition[i].X} {positionInRealitionToShapePosition[i].Y} {positionInRealitionToShapePosition[i].Z}";
            // normal of the vector 
            string nor = $"{_vertexs[i].Normal.X} {_vertexs[i].Normal.Y} {_vertexs[i].Normal.Z}";
            // color of the vector 
            string col = $"{_vertexs[i].Color.R} {_vertexs[i].Color.G} {_vertexs[i].Color.B}";
            // texture position of the vector 
            string tex = $"{_vertexs[i].TextureCoordinate.X} {_vertexs[i].TextureCoordinate.Y}";
            writer.WriteLine($"{pos} {nor} {col} {tex}");
        }

    }
}
}