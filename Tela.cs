public class Tela
{
    private int largura, altura;
    private ConsoleColor corTexto, corFundo;

    // New simplified constructor
    public Tela()
    {
        this.largura = 80;
        this.altura = 25;
        this.corTexto = ConsoleColor.Yellow;
        this.corFundo = ConsoleColor.Black;
    }

    // Backward compatibility constructors
    public Tela(int lar, int alt)
    {
        this.largura = lar;
        this.altura = alt;
        this.corTexto = ConsoleColor.Yellow;
        this.corFundo = ConsoleColor.Black;
    }

    public Tela(int lar, int alt, ConsoleColor txt, ConsoleColor fun)
    {
        this.largura = lar;
        this.altura = alt;
        this.corTexto = txt;
        this.corFundo = fun;
    }

    // New simplified methods
    public void LimparTela()
    {
        Console.BackgroundColor = this.corFundo;
        Console.ForegroundColor = this.corTexto;
        Console.Clear();
    }

    public void ExibirTitulo(string titulo)
    {
        Console.WriteLine($"\n{titulo}\n");
    }

    public void ExibirOpcao(string numero, string descricao)
    {
        Console.WriteLine($"{numero} - {descricao}");
    }

    public void ExibirLinhaVazia()
    {
        Console.WriteLine();
    }

    public char LerOpcaoValida(List<char> opcoesValidas)
    {
        char opcao;
        while (true)
        {
            Console.Write("Opção: ");
            string input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
                continue;
            }
            
            if (char.TryParse(input.Trim(), out opcao) && opcoesValidas.Contains(opcao))
            {
                return opcao;
            }
            
            Console.WriteLine("Opção inválida. Tente novamente.");
        }
    }

    public string Perguntar(string pergunta)
    {
        Console.Write(pergunta);
        return Console.ReadLine();
    }

    public void MostrarMensagem(string mensagem)
    {
        Console.WriteLine(mensagem);
    }

    // Backward compatibility methods
    public void PrepararTela()
    {
        Console.BackgroundColor = this.corFundo;
        Console.ForegroundColor = this.corTexto;
        Console.Clear();
        this.MontarMoldura(0, 0, this.largura, this.altura);
    }

    public void MontarMoldura(int colIni, int linIni, int colFin, int linFin)
    {
        for (int coluna = colIni; coluna <= colFin; coluna++)
        {
            Console.SetCursorPosition(coluna, linIni);
            Console.Write("═");
            Console.SetCursorPosition(coluna, linFin);
            Console.Write("═");
        }

        for (int linha = linIni; linha <= linFin; linha++)
        {
            Console.SetCursorPosition(colIni, linha);
            Console.Write("║");
            Console.SetCursorPosition(colFin, linha);
            Console.Write("║");
        }

        Console.SetCursorPosition(colIni, linIni);
        Console.Write("╔");
        Console.SetCursorPosition(colIni, linFin);
        Console.Write("╚");
        Console.SetCursorPosition(colFin, linIni);
        Console.Write("╗");
        Console.SetCursorPosition(colFin, linFin);
        Console.Write("╝");
    }

    public string MostrarMenu(List<string> opcoes, int col, int lin)
    {
        string opcaoEscolhida = "";
        int maiorLinha = 0;
        foreach (string op in opcoes)
        {
            if (op.Length > maiorLinha) maiorLinha = op.Length;
        }
        
        int colFin = col + maiorLinha + 1;
        int linFin = lin + opcoes.Count + 2;
        this.MontarMoldura(col, lin, colFin, linFin);

        col++;
        lin++;
        for (int i = 0; i < opcoes.Count; i++)
        {
            Console.SetCursorPosition(col, lin);
            Console.Write(opcoes[i]);
            lin++;
        }
        Console.SetCursorPosition(col, lin);
        Console.Write("Opção : ");
        opcaoEscolhida = Console.ReadLine();

        return opcaoEscolhida;
    }

    public void MontarTela(int col, int lin, List<string> dados, string titulo)
    {
        this.MontarMoldura(col, lin, col + this.largura, lin + this.altura);
        col++;
        lin++;
        Console.SetCursorPosition(col, lin);
        Console.Write(titulo);
        lin++;
        foreach (string pergunta in dados)
        {
            Console.SetCursorPosition(col, lin);
            Console.Write(pergunta);
            lin++;
        }
    }

    public void MostrarMensagem(int col, int lin, string msg)
    {
        Console.SetCursorPosition(col, lin);
        Console.Write(msg);
    }

    public string Perguntar(int coluna, int linha, string perg)
    {
        this.MostrarMensagem(coluna, linha, perg);
        string resposta = Console.ReadLine();
        return resposta;
    }

    // Métodos auxiliares para escalabilidade
    public void ExibirTabela(string[] cabecalhos, List<string[]> dados)
    {
        if (dados.Count == 0)
        {
            Console.WriteLine("Nenhum registro encontrado.");
            return;
        }

        // Calcular larguras das colunas
        int[] larguras = new int[cabecalhos.Length];
        for (int i = 0; i < cabecalhos.Length; i++)
        {
            larguras[i] = cabecalhos[i].Length;
            foreach (var linha in dados)
            {
                if (i < linha.Length && linha[i].Length > larguras[i])
                {
                    larguras[i] = linha[i].Length;
                }
            }
        }

        // Exibir cabeçalho
        ExibirLinhaSeparadora(larguras);
        for (int i = 0; i < cabecalhos.Length; i++)
        {
            Console.Write($"| {cabecalhos[i].PadRight(larguras[i])} ");
        }
        Console.WriteLine("|");
        ExibirLinhaSeparadora(larguras);

        // Exibir dados
        foreach (var linha in dados)
        {
            for (int i = 0; i < cabecalhos.Length; i++)
            {
                string valor = i < linha.Length ? linha[i] : "";
                Console.Write($"| {valor.PadRight(larguras[i])} ");
            }
            Console.WriteLine("|");
        }
        ExibirLinhaSeparadora(larguras);
    }

    private void ExibirLinhaSeparadora(int[] larguras)
    {
        Console.Write("+");
        foreach (int largura in larguras)
        {
            Console.Write(new string('-', largura + 2) + "+");
        }
        Console.WriteLine();
    }

    public bool ConfirmarAcao(string mensagem = "Confirma esta ação (S/N)? ")
    {
        while (true)
        {
            string resposta = Perguntar(mensagem);
            if (resposta.ToLower() == "s" || resposta.ToLower() == "sim")
                return true;
            if (resposta.ToLower() == "n" || resposta.ToLower() == "não" || resposta.ToLower() == "nao")
                return false;
            Console.WriteLine("Resposta inválida. Digite S para Sim ou N para Não.");
        }
    }

    public void ExibirLinha(char caractere = '-', int tamanho = 50)
    {
        Console.WriteLine(new string(caractere, tamanho));
    }

    public void Pausar()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void ExibirErro(string mensagem)
    {
        ConsoleColor corOriginal = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERRO: {mensagem}");
        Console.ForegroundColor = corOriginal;
    }

    public void ExibirSucesso(string mensagem)
    {
        ConsoleColor corOriginal = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"SUCESSO: {mensagem}");
        Console.ForegroundColor = corOriginal;
    }

    public void ExibirAviso(string mensagem)
    {
        ConsoleColor corOriginal = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"AVISO: {mensagem}");
        Console.ForegroundColor = corOriginal;
    }
}
