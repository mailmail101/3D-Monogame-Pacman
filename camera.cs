using Microsoft.Xna.Framework;
namespace QuokkaEngine;
///<summary>Camera class</summary>
public class Camera
{
    ///<summary>The position of the camera</summary>
    public Vector3 Position{get; set;} = new Vector3(0f, 0f, 0f);
    ///<summary>The X rotation of the camera in radians</summary>
    public float XRotation {get; set;} = 0f;
    ///<summary>The Y rotation of the camera in radians</summary>
    public float YRotation {get; set;} = 0f;
    ///<summary>The Z rotation of the camera in radians</summary>
    public float ZRotation {get; set;} = 0f;
    ///<summary>direction vector that the camera is facing</summary>
    public Vector3 Facing {get; set;} = Vector3.Forward;
    private Vector3 FacingUp = Vector3.Up;
    ///<summary>hint put this into effect.view</summary>
    public Matrix View {get;}

    /// <summary>call to update camera psotion</summary>
    public Matrix Update()
    {
        Matrix rotationMatrix = Matrix.CreateRotationX(XRotation) * Matrix.CreateRotationY(YRotation)* Matrix.CreateRotationZ(ZRotation);
        // find facing
        Facing = Vector3.Transform(Vector3.Forward, rotationMatrix);
        // finds the up vector
        FacingUp = Vector3.Transform(Vector3.Up, rotationMatrix);
        // compute the view
        return Matrix.CreateLookAt(Position, Facing + Position, FacingUp);//Matrix.CreateTranslation(Position) * rotationMatrix;
    }
    ///<summary>moves the camera forward by distance</summary>
    public void Forward(float distance)
    {
        Position += Vector3.Normalize(Facing) * distance;
    }
    ///<summary>moves the camera back by distance</summary>
    public void Back(float distance)
    {
        Position += Vector3.Normalize(Facing) * -distance;
    }
    ///<summary>moves the camera up by distance</summary>
    public void Up(float distance)
    {
        Position += Vector3.Normalize(FacingUp) * -distance;
    }
    ///<summary>moves the camera down by distance</summary>
    public void Down(float distance)
    {
        Position += Vector3.Normalize(FacingUp) * distance;
    }
    ///<summary>moves the camera left by distance</summary>
    public void Left(float distance)
    {

        Position += Vector3.Normalize(Vector3.Cross(Facing - FacingUp, - FacingUp)) * distance;
    }
    ///<summary>moves the camera right by distance</summary>
    public void Right(float distance)
    {
        Position += Vector3.Normalize(Vector3.Cross(Facing - FacingUp, - FacingUp)) * -distance;
    }
    ///<summary>makes a new camera</summary>
    public Camera()
    {
        View =  Update();
    }
    ///<summary>makes a new camera</summary>
    ///<param name = "position">the position of the camera</param>
    public Camera(Vector3 position)
    {
        Position = position;
        View =  Update();
    }
    ///<summary>makes a new camera</summary>
    ///<param name = "position">the position of the camera</param>
    ///<param name = "facing">the direction that the camera is facing</param>
    public Camera(Vector3 position, Vector3 facing)
    {
        Position = position;
        Facing = facing;
        View =  Update();
    }
    ///<summary>makes a new camera</summary>
    ///<param name = "position">the position of the camera</param>
    ///<param name = "facing">the direction that the camera is facing</param>
    ///<param name = "up">the direction of the camera that is facing up</param>
    public Camera(Vector3 position, Vector3 facing, Vector3 up)
    {
        Position = position;
        Facing = facing;
        FacingUp = up;
        View =  Update();
    }
}