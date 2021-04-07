using TabuleiroN;
using System.Collections.Generic;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro t { get; private set; }
        public int turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;

        public PartidaDeXadrez()
        {
            this.t = new Tabuleiro(8, 8);
            this.turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in Capturadas) if (p.Cor == cor) aux.Add(p);
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in Pecas) if (p.Cor == cor) aux.Add(p);
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private void ExecutaMovimeto(Posicao origem, Posicao destino)
        {
            Peca p = t.RemovePeca(origem);
            p.IncrMovimetos();
            Peca pCapturada = t.RemovePeca(destino);
            t.AddPeca(p, destino);
            if (pCapturada != null) Capturadas.Add(pCapturada);
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

        public void ColocarNovaPeca(char col, int lin, Peca p)
        {
            t.AddPeca(p, new PosicaoXadrez(col, lin).toPosicao());
            Pecas.Add(p);
        }
        private void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(t, Cor.Branca));
            ColocarNovaPeca('c', 2, new Torre(t, Cor.Branca));
            ColocarNovaPeca('d', 2, new Torre(t, Cor.Branca));
            ColocarNovaPeca('e', 2, new Torre(t, Cor.Branca));
            ColocarNovaPeca('e', 1, new Torre(t, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rei(t, Cor.Branca));


            ColocarNovaPeca('c', 7, new Torre(t, Cor.Preta));
            ColocarNovaPeca('c', 8, new Torre(t, Cor.Preta));
            ColocarNovaPeca('d', 7, new Torre(t, Cor.Preta));
            ColocarNovaPeca('e', 7, new Torre(t, Cor.Preta));
            ColocarNovaPeca('e', 8, new Torre(t, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rei(t, Cor.Preta)); ;
        }
    }
}
