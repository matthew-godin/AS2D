using System;

namespace XNAProject
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main(string[] args)
      {
         using (GameProject game = new GameProject())
         {
            game.Run();
         }
      }
   }
}

