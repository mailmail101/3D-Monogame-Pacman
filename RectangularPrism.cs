using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
namespace Shapes
{
public class RectangularPrism : Shape
{
    public RectangularPrism(Vector3 topRightFrontPoint, Vector3 size, Color color)
    {

        this._vertexs = new VertexPositionColorNormalTexture[36];
        this.positionInRealitionToShapePosition = new Vector3[36];
        for(int i = 0; i < 36; i++)
        {
            this._vertexs[i].Color = color;
        }
        // front face
        this.positionInRealitionToShapePosition[0] = topRightFrontPoint;
        this.positionInRealitionToShapePosition[1] = topRightFrontPoint + new Vector3(- size.X, size.Y, 0);
        this.positionInRealitionToShapePosition[2] = topRightFrontPoint + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[3] = topRightFrontPoint;
        this.positionInRealitionToShapePosition[4] = topRightFrontPoint + new Vector3(- size.X, size.Y, 0);
        this.positionInRealitionToShapePosition[5] = topRightFrontPoint + new Vector3(-size.X, 0, 0);
        // back face
        this.positionInRealitionToShapePosition[6] = this.positionInRealitionToShapePosition[0] + new Vector3(0, 0 , size.Z);
        this.positionInRealitionToShapePosition[7] = this.positionInRealitionToShapePosition[2] + new Vector3(0, 0 , size.Z);
        this.positionInRealitionToShapePosition[8] = this.positionInRealitionToShapePosition[1] + new Vector3(0, 0 , size.Z);
        this.positionInRealitionToShapePosition[9] = this.positionInRealitionToShapePosition[3] + new Vector3(0, 0 , size.Z);
        this.positionInRealitionToShapePosition[10] = this.positionInRealitionToShapePosition[5] + new Vector3(0, 0 , size.Z);
        this.positionInRealitionToShapePosition[11] = this.positionInRealitionToShapePosition[4] + new Vector3(0, 0 , size.Z);
        // top face
        this.positionInRealitionToShapePosition[12] = topRightFrontPoint;
        this.positionInRealitionToShapePosition[13] = topRightFrontPoint + new Vector3(0, 0, size.Z);
        this.positionInRealitionToShapePosition[14] = topRightFrontPoint + new Vector3(- size.X, 0, size.Z);
        this.positionInRealitionToShapePosition[15] = topRightFrontPoint;
        this.positionInRealitionToShapePosition[16] = topRightFrontPoint + new Vector3(- size.X, 0, size.Z);
        this.positionInRealitionToShapePosition[17] = topRightFrontPoint + new Vector3(-size.X, 0, 0);
        // bottom face
        this.positionInRealitionToShapePosition[18] = this.positionInRealitionToShapePosition[12] + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[19] = this.positionInRealitionToShapePosition[14] + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[20] = this.positionInRealitionToShapePosition[13] + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[21] = this.positionInRealitionToShapePosition[15] + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[22] = this.positionInRealitionToShapePosition[17] + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[23] = this.positionInRealitionToShapePosition[16] + new Vector3(0, size.Y, 0);
        // right face
        this.positionInRealitionToShapePosition[24] = topRightFrontPoint;
        this.positionInRealitionToShapePosition[25] = topRightFrontPoint + new Vector3(0, size.Y, size.Z);
        this.positionInRealitionToShapePosition[26] = topRightFrontPoint + new Vector3(0, 0, size.Z);
        this.positionInRealitionToShapePosition[27] = topRightFrontPoint;
        this.positionInRealitionToShapePosition[28] = topRightFrontPoint + new Vector3(0, size.Y, 0);
        this.positionInRealitionToShapePosition[29] = topRightFrontPoint + new Vector3(0, size.Y, size.Z);
        // left face
        this.positionInRealitionToShapePosition[30] = this.positionInRealitionToShapePosition[24] + new Vector3(- size.X, 0, 0);
        this.positionInRealitionToShapePosition[31] = this.positionInRealitionToShapePosition[26] + new Vector3(- size.X, 0, 0);
        this.positionInRealitionToShapePosition[32] = this.positionInRealitionToShapePosition[25] + new Vector3(- size.X, 0, 0);
        this.positionInRealitionToShapePosition[33] = this.positionInRealitionToShapePosition[27] + new Vector3(- size.X, 0, 0);
        this.positionInRealitionToShapePosition[34] = this.positionInRealitionToShapePosition[29] + new Vector3(- size.X, 0, 0);
        this.positionInRealitionToShapePosition[35] = this.positionInRealitionToShapePosition[28] + new Vector3(- size.X, 0, 0);
        this.Update();
        this.ReCalculateNormals();
    }

}
}