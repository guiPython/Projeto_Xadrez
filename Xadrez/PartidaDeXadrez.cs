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
        public Peca VulneravelEnPassant { get; private set; }

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
            VulneravelEnPassant = null;
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

            // # Jogada Especial Roque Pequeno

            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = t.RemovePeca(origemT);
                T.IncrMovimetos();
                t.AddPeca(T, destinoT);

            }

            // # Jogada Especial Roque Grande

            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = t.RemovePeca(origemT);
                T.IncrMovimetos();
                t.AddPeca(T, destinoT);

            }


            // #Jogada Especial En Passant

            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca) posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    else posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    pCapturada = t.RemovePeca(posP);
                    Capturadas.Add(pCapturada);
                }
            }

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

            // #Jogada Especial Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = t.RemovePeca(destinoT);
                T.DecrMovimentos();
                t.AddPeca(T, origemT);

            }

            // #Jogada Especial Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = t.RemovePeca(destinoT);
                T.DecrMovimentos();
                t.AddPeca(T, origemT);

            }

            // #Jogada Especial En Passant

            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pCapturada == VulneravelEnPassant)
                {
                    Peca peao = t.RemovePeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    t.AddPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pCapturada = ExecutaMovimeto(origem, destino);

            Peca p = t.Peca(destino);

            // #Jogada Especial Promoção

            if( p is Peao)
            {
                if((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7))
                {
                    p = t.RemovePeca(destino);
                    Pecas.Remove(p);
                    Peca rainha = new Rainha(t, p.Cor);
                    t.AddPeca(rainha,destino);
                    Pecas.Add(rainha);
                }
            }

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

            // #Jogada Especial En Passant

            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            }
            else VulneravelEnPassant = null;
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
            ColocarNovaPeca('e', 1, new Rei(t, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(t, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(t, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(t, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(t, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(t, Cor.Branca, this));

            ColocarNovaPeca('a', 8, new Torre(t, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(t, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(t, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rainha(t, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(t, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(t, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(t, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(t, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(t, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(t, Cor.Preta, this));
        }
    }
}
