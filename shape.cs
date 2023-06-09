using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Shapes
{
public class Shape
{
    public VertexPositionColorNormalTexture[] _vertexs;
    private Vector3[] positionInRealitionToShapePosition;
    ///<summary>the scale of the shape</summary>
    private float _scale = 1f;
    ///<summary>the scale of the shape</summary>
    public float Scale
    {
        get { return _scale; }
        set {SplitScaleInoXYZParts(Scale); Update(); _scale = value; }  
    }
    private float _scaleX = 1f;
    ///<summary>the X scale of the shape</summary>
    public float ScaleX
    {
        get { return _scaleX; }
        set {Update(); _scaleX = value; }  
    }
    private float _scaleY = 1f;
    ///<summary>the Y scale of the shape</summary>
    public float ScaleY
    {
        get { return _scaleY; }
        set {Update(); _scaleY = value; }  
    }
    private float _scaleZ = 1f;
    ///<summary>the Z scale of the shape</summary>
    public float ScaleZ
    {
        get { return _scaleZ; }
        set {Update(); _scaleZ = value; }  
    }
    ///<summary>The X rotation of the shape in radians</summary>
    public float XRotation {get; set;} = 0f;
    ///<summary>The Y rotation of the shape in radians</summary>
    public float YRotation {get; set;} = 0f;
    ///<summary>The Z rotation of the shape in radians</summary>
    public float ZRotation {get; set;} = 0f;
    ///<summary>the location of the front top right point</summary>
    private Vector3 _position = new Vector3(0, 0, 0);
    ///<summary>the location of the front top right point</summary>
    public Vector3 Position
    {
        get { return _position; }
        set {Update(); _position = value; }  
    }
    public Shape(string fileName)
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
                    positionInRealitionToShapePosition = new Vector3[numOfVertexs];
                    _vertexs = new VertexPositionColorNormalTexture[numOfVertexs];
                    break;
                case 1:
                    _position.X = float.Parse(splitLine[0]);
                    _position.Y = float.Parse(splitLine[1]);
                    _position.Z = float.Parse(splitLine[2]);
                    break;
                case 2:
                     _scaleX = float.Parse(splitLine[0]);
                    _scaleY = float.Parse(splitLine[1]);
                    _scaleZ = float.Parse(splitLine[2]);
                    break;
                case 3:
                    XRotation = float.Parse(splitLine[0]);
                    YRotation = float.Parse(splitLine[1]);
                    ZRotation = float.Parse(splitLine[2]);
                    break;
                default:
                    _vertexs[i].Position = new Vector3(float.Parse(splitLine[0]), float.Parse(splitLine[1]), float.Parse(splitLine[2]));
                    positionInRealitionToShapePosition[i] = _vertexs[i].Position;
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
        writer.WriteLine($"{_scaleX} {_scaleY} {_scaleZ}");
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
    private void SplitScaleInoXYZParts(float scale)
    {
        _scaleX = scale;
        _scaleY = scale;
        _scaleZ = scale;
    }
    public void Update()
    {
        Matrix rotationMatrix = Matrix.CreateRotationX(XRotation) * Matrix.CreateRotationY(YRotation)* Matrix.CreateRotationZ(ZRotation);
        for (int i = 0; i < _vertexs.Length; i++)
        {
            _vertexs[i].Position = Vector3.Transform((positionInRealitionToShapePosition[i] + Position) * new Vector3(_scaleX, _scaleY, _scaleZ), rotationMatrix) ;
        }
    }
    public void ReCalculateNormals()
    {
        for(int i = 0; i < _vertexs.Length; i += 3)
        {
            Vector3 normal = Vector3.Cross(_vertexs[i + 1].Position - _vertexs[i].Position, _vertexs[i + 2].Position - _vertexs[i].Position);
            _vertexs[i].Normal = normal;
            _vertexs[i + 1].Normal = normal;
            _vertexs[i + 2].Normal = normal;
        }
    }
}
}