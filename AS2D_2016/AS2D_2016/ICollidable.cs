﻿/* Author :            Raphael Brule
   File :           ICollidable.cs
   Date :              05 October 2016
   Description :       This interface represents a collidable object.*/

namespace XNAProject
{
    interface ICollidable
    {
        bool IsColliding(object otherObject);
    }
}
