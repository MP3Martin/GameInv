namespace GameInv {
    public static class Program {
        public static void Main(string[] args) {
            new GameInv(
                new Inventory()
            ).Run();
        }
    }
}
