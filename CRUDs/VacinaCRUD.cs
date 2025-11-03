public class VacinaCRUD
{
    private List<Vacina> vacinas;
    private Vacina vacina;
    private int indice;

    public VacinaCRUD()
    {
        this.vacinas = new List<Vacina>();
        this.vacina = new Vacina();
        this.indice = -1;
    }

    public List<Vacina> GetVacinas()
    {
        return this.vacinas;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Vacinas");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Listar Todos");
            tela.ExibirOpcao("2", "Cadastrar Novo");
            tela.ExibirOpcao("3", "Alterar");
            tela.ExibirOpcao("4", "Excluir");
            tela.ExibirOpcao("5", "Visualizar Detalhes");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);

            if (opcao == '0') break;
            if (opcao == '1') this.ListarTodos();
            if (opcao == '2') this.Cadastrar();
            if (opcao == '3') this.Alterar();
            if (opcao == '4') this.Excluir();
            if (opcao == '5') this.VisualizarDetalhes();
        }
    }

    public int GerarNovoId()
    {
        return this.vacinas.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Vacinas");
        Console.WriteLine();
        
        if (this.vacinas.Count == 0)
        {
            Console.WriteLine("Nenhuma vacina cadastrada.");
        }
        else
        {
            foreach (var vacina in this.vacinas)
            {
                Console.WriteLine($"ID: {vacina.id} | Nome: {vacina.nome} | Periodicidade: {vacina.periodicidade} dias");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Nova Vacina");
        Console.WriteLine();
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.vacinas.Add(new Vacina(novoId, this.vacina.nome, this.vacina.periodicidade));
            Console.WriteLine("Vacina cadastrada com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Vacina");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID da vacina a alterar: ");
        if (!int.TryParse(idInput, out this.vacina.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Vacina não encontrada.");
        }
        else
        {
            Console.WriteLine("\nDados atuais:");
            this.MostrarDados();
            
            Console.WriteLine("Novos dados:");
            this.EntrarDados();
            
            string confirma = tela.Perguntar("\nConfirma a alteração (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.vacinas[this.indice] = new Vacina(this.vacina.id, this.vacina.nome, this.vacina.periodicidade);
                Console.WriteLine("Vacina alterada com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Vacina");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID da vacina a excluir: ");
        if (!int.TryParse(idInput, out this.vacina.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Vacina não encontrada.");
        }
        else
        {
            Console.WriteLine("\nVacina a ser excluída:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.vacinas.RemoveAt(this.indice);
                Console.WriteLine("Vacina excluída com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        this.vacina.nome = tela.Perguntar("Nome: ");
        
        string periodicidadeInput = tela.Perguntar("Periodicidade (dias): ");
        if (!int.TryParse(periodicidadeInput, out this.vacina.periodicidade))
        {
            Console.WriteLine("Periodicidade inválida!");
            return;
        }
    }

    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.vacinas.Count; i++)
        {
            if (this.vacina.id == this.vacinas[i].id)
            {
                encontrei = true;
                this.indice = i;
                break;
            }
        }
        return encontrei;
    }

    public void MostrarDados()
    {
        Console.WriteLine("Vacina encontrada:");
        Console.WriteLine($"Nome: {this.vacinas[this.indice].nome}");
        Console.WriteLine($"Periodicidade: {this.vacinas[this.indice].periodicidade} dias");
        Console.WriteLine($"Quantidade em Estoque: {this.vacinas[this.indice].quantidadeEstoque}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes da Vacina");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID da vacina: ");
        if (!int.TryParse(idInput, out this.vacina.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Vacina não encontrada.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DA VACINA ===");
            this.MostrarDados();
            
            var vacina = this.vacinas[this.indice];
            if (vacina.quantidadeEstoque <= 5) 
            {
                tela.ExibirAviso($"ATENÇÃO: Estoque baixo! Apenas {vacina.quantidadeEstoque} unidades disponíveis.");
            }
            else
            {
                tela.ExibirSucesso($"Estoque OK: {vacina.quantidadeEstoque} unidades disponíveis.");
            }
        }
        
        tela.Pausar();
    }
}
