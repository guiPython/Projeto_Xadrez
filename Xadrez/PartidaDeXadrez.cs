using TabuleiroN;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro t { get; private set; }
        private int turno;
        private Cor JogadorAtual;
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            this.t = new Tabuleiro(8,8);
            this.turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            ColocarPecas();
        }

        public void ExecutaMovimeto(Posicao origem, Posicao destino)
        {
            Peca p = t.RemovePeca(origem);
            p.IncrMovimetos();
            Peca pCapturada = t.RemovePeca(destino);
            t.AddPeca(p, destino);
        }

        private void ColocarPecas()
        {
            t.AddPeca(new Torre(t, Cor.Branca), new PosicaoXadrez('c',1).toPosicao());
            t.AddPeca(new Torre(t, Cor.Branca), new PosicaoXadrez('c',2).toPosicao());
            t.AddPeca(new Torre(t, Cor.Branca), new PosicaoXadrez('d',2).toPosicao());
            t.AddPeca(new Torre(t, Cor.Branca), new PosicaoXadrez('e',2).toPosicao());
            t.AddPeca(new Torre(t, Cor.Branca), new PosicaoXadrez('e',1).toPosicao());
            t.AddPeca(new Rei(t, Cor.Branca), new PosicaoXadrez('d', 1).toPosicao());


            t.AddPeca(new Torre(t, Cor.Preta), new PosicaoXadrez('c', 7).toPosicao());
            t.AddPeca(new Torre(t, Cor.Preta), new PosicaoXadrez('c', 8).toPosicao());
            t.AddPeca(new Torre(t, Cor.Preta), new PosicaoXadrez('d', 7).toPosicao());
            t.AddPeca(new Torre(t, Cor.Preta), new PosicaoXadrez('e', 7).toPosicao());
            t.AddPeca(new Torre(t, Cor.Preta), new PosicaoXadrez('e', 8).toPosicao());
            t.AddPeca(new Rei(t, Cor.Preta), new PosicaoXadrez('d', 8).toPosicao());
        }
    }
}
