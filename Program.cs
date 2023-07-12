namespace PixelPalette
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GGame game = new();
                game.Run();
                game.start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}