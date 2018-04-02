using System;
namespace Instagram
{
    class Program
    {
        static void Main(string[] args)
        {
            Go();
            Console.ReadKey();
        }
        static async void Go()
        {
            var instagram = new Instagram("563001bc245249ea947e19adc8a9ecf1", 
                "43676101507a4423ab4b59f6720b9a4f", 
                "http://localhost:50002");
            await instagram.Authorize();
            Console.WriteLine(String.Join(Environment.NewLine, await instagram.GetLatestMedia()));
        }
    }
}
