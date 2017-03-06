using System;

namespace AtelierXNA
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main(string[] args)
      {
         using (Atelier game = new Atelier())
         {
            game.Run();
         }
      }
   }
}

