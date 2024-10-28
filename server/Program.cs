
namespace GameInv {
    public static class Program {
        public static void Main(string[] args) {
            new GameInv(
                new Inventory()
            ).Run();
            
            var a = ushort.Parse("-1");
            Console.WriteLine(a);
            Thread.Sleep(-1);
        }
    }
}
