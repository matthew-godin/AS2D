/* Author :            Raphael Brule
   File :           ICollidable.cs
   Date :              05 October 2016
   Description :       This interface represents a collidable object.*/

namespace AS2D_2016
{
    interface ICollidable
    {
        //To implement
        bool IsColliding(object otherObject);
    }
}
