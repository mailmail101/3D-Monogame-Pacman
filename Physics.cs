using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Shapes;
namespace QuokkaEngine;
/// <summary>a point that draws all other objs towards it</summary>
public struct PointOfGravity
{
     /// <summary>Creates a point of gravity at <paramref name="position"/> with given<paramref name="strength"/> 
     ///, <paramref name="decay"/> is the rate that the <paramref name="strength"/> falls off set as one as default
     /// and <paramref name="distanceOfEffect"/> is the point that it no longer has an effect set to -1 for infinite(may cause bad performance)</summary>
    /// <param name="position">the position of the point</param>
    /// <param name="strength">the strength of gravity</param>
    /// <param name="decay">the rate of decay</param>
    /// <param name="distanceOfEffect">the cut off distance of effect</param>
    public PointOfGravity(Vector3 position, float strength, float decay, float distanceOfEffect)
    {
        Position = position;
        Strength = strength;
        Decay = decay;
        DistanceOfEffect = distanceOfEffect;
    }
    ///<summary>the position of the point</summary>
    public Vector3 Position {get; set;}
    ///<summary>the strength of gravity</summary>
    public float Strength {get; set;}
    ///<summary>the rate of decay higher is faster</summary>
    public float Decay{get; set;}
    ///<summary>the cut off distance of effect</summary>
    public float DistanceOfEffect {get; set;}
}
public class Physics
{
    public List<Shape> _shapes;
    public Vector3 _universalGravity = new Vector3(0, 0, 0);
    public List<PointOfGravity> _gravityObjects; 
    public Physics(List<Shape> shapes)
    {
        _shapes = shapes;
    }
    ///<summary>apply's gravity to all obj in list does require each shape to be updated for chage in position</summary>
    public List<Shape> ApplyGravity(List<Shape> shapes)
    {
        foreach(Shape shape in shapes)
        {
            shape.velocity += _universalGravity;
            foreach(PointOfGravity gravPoint in _gravityObjects)
            {
                Vector3 vectordistance = shape.Position -gravPoint.Position;
                float distance =(float) Math.Sqrt((Math.Pow((double)vectordistance.X, 2)) + 
                                                                    (Math.Pow((double)vectordistance.Y, 2)) + 
                                                                    (Math.Pow((double)vectordistance.Z, 2)));
                if (distance <= gravPoint.DistanceOfEffect)
                {
                    float forceOfGravity = (float)(gravPoint.Strength * 100000 / (gravPoint.Decay * Math.Pow((double)distance, 2)));
                    shape.velocity += Vector3.Normalize(vectordistance) * forceOfGravity;
                }
            }
        }
        return shapes;
    }
    public bool Collision(Shape shapeToCheck, Shape[] shapesToCheckAgainst)
    {
        List<BoundingBox> boxs = new List<BoundingBox>();
        foreach(Shape shape in shapesToCheckAgainst){boxs.Add(shape.boundingBox);}
        foreach(BoundingBox box in boxs)
        {
            if( shapeToCheck.boundingBox.Min.X <= box.Max.X &&
                shapeToCheck.boundingBox.Max.X >= box.Min.X &&
                shapeToCheck.boundingBox.Min.Y <= box.Max.Y &&
                shapeToCheck.boundingBox.Max.Y >= box.Min.Y &&
                shapeToCheck.boundingBox.Min.Z <= box.Max.Z &&
                shapeToCheck.boundingBox.Max.Z >= box.Min.Z)
            {
                shapeToCheck.velocity *= -1;
                shapeToCheck.Update();
                return true;
            }
        }
        return false;
    }

}