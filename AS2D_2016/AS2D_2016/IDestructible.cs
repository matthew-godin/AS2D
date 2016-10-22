/* Author :            Raphael Brule
   File :           IDestructible.cs
   Date :              05 October 2016
   Description :       This interface represents a destructible object.*/

namespace XNAProject
{
    interface IDestructible
    {
        bool ToDestroy { get; }
    }
}
