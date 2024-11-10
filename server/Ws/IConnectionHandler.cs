namespace GameInv.Ws {
    /// <remarks>
    ///     Set <see cref="GameInv" /> before calling <see cref="Start" />
    /// </remarks>
    public interface IConnectionHandler {
        public GameInv GameInv { set; }

        public void Start();

        public void Stop();
    }
}
