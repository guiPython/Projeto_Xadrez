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
        public bool Xeque { get; private set; }


        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;

        public PartidaDeXadrez()
        {
            this.t = new Tabuleiro(8, 8);
            this.turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
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

        private Peca ExecutaMovimeto(Posicao origem, Posicao destino)
        {
            Peca p = t.RemovePeca(origem);
            p.IncrMovimetos();
            Peca pCapturada = t.RemovePeca(destino);
            t.AddPeca(p, destino);
            if (pCapturada != null) Capturadas.Add(pCapturada);
            return pCapturada;
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

        public void DesfazerMovimento(Posicao origem, Posicao destino, Peca pCapturada)
        {
            Peca p = t.RemovePeca(destino);
            p.DecrMovimentos();
            if (pCapturada != null)
            {
                t.AddPeca(pCapturada, destino);
                Capturadas.Remove(pCapturada);
            }
            t.AddPeca(p, origem);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pCapturada = ExecutaMovimeto(origem, destino);

            if (EstaEmCheque(JogadorAtual))
            {
                DesfazerMovimento(origem, destino, pCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (EstaEmCheque(Adversaria(JogadorAtual))) Xeque = true;
            else Xeque = false;

            if (TesteXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }

        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branca) return Cor.Preta;
            return Cor.Branca;
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca p in PecasEmJogo(cor))
            {
                if (p is Rei) return p;
            }
            return null;
        }

        public bool EstaEmCheque(Cor cor)
        {
            Peca rei = Rei(cor);
            if (rei == null) throw new TabuleiroException("Não existe um rei desta cor no tabuleiro!");
            foreach (Peca p in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = p.MovimentosPossiveis();
                if (mat[rei.Posicao.Linha, rei.Posicao.Coluna]) return true;
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmCheque(cor)) return false;
            foreach (Peca p in PecasEmJogo(cor))
            {
                bool[,] mat = p.MovimentosPossiveis();
                for (int i = 0; i < t.Linhas; i++)
                {
                    for (int j = 0; j < t.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = p.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pCapturada = ExecutaMovimeto(origem, destino);
                            bool testeXeque = EstaEmCheque(cor);
                            DesfazerMovimento(origem, destino, pCapturada);
                            if (!testeXeque) return false;
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char col, int lin, Peca p)
        {
            t.AddPeca(p, new PosicaoXadrez(col, lin).toPosicao());
            Pecas.Add(p);
        }
        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(t, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(t, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(t, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rainha(t, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(t, Cor.Branca));
            ColocarNovaPeca('f', 1, new Bispo(t, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(t, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(t, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('b', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('c', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('d', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('e', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('f', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('g', 2, new Peao(t, Cor.Branca));
            ColocarNovaPeca('h', 2, new Peao(t, Cor.Branca));

            ColocarNovaPeca('a', 8, new Torre(t, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(t, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(t, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rainha(t, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(t, Cor.Preta));
            ColocarNovaPeca('f', 8, new Bispo(t, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(t, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(t, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('b', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('c', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('d', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('e', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('f', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('g', 7, new Peao(t, Cor.Preta));
            ColocarNovaPeca('h', 7, new Peao(t, Cor.Preta));
        }
    }
}
