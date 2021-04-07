using TabuleiroN;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro t { get; private set; }
        public int turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            this.t = new Tabuleiro(8,8);
            this.turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            ColocarPecas();
        }

        private void ExecutaMovimeto(Posicao origem, Posicao destino)
        {
            Peca p = t.RemovePeca(origem);
            p.IncrMovimetos();
            Peca pCapturada = t.RemovePeca(destino);
            t.AddPeca(p, destino);
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca) JogadorAtual = Cor.Preta;
            else JogadorAtual = Cor.Branca;
        }

        public void ValidarPosOrigem(Posicao pos)
        {
            if (t.Peca(pos) == null) throw new TabuleiroException("Não existe peça nessa posição!");

            if (JogadorAtual != t.Peca(pos).Cor) throw new TabuleiroException("A peça de origem não é sua!");

            if (!t.Peca(pos).ExisteMovimentosPossiveis()) throw new TabuleiroException("Não há movimentos possiveis para a peça escolhida!");
        }

        public void ValidarPosDestino(Posicao origem, Posicao destino)
        {
            if (!t.Peca(origem).PodeMoverPara(destino)) throw new TabuleiroException("Posição de destino inválida!");
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimeto(origem, destino);
            turno++;
            MudaJogador();
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
