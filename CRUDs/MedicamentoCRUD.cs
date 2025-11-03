public class MedicamentoCRUD
{
    private List<Medicamento> medicamentos;
    private Medicamento medicamento;
    private int indice;

    public MedicamentoCRUD()
    {
        this.medicamentos = new List<Medicamento>();
        this.medicamento = new Medicamento();
        this.indice = -1;
    }

    public List<Medicamento> GetMedicamentos()
    {
        return this.medicamentos;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Medicamentos");
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
        return this.medicamentos.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Medicamentos");
        Console.WriteLine();
        
        if (this.medicamentos.Count == 0)
        {
            Console.WriteLine("Nenhum medicamento cadastrado.");
        }
        else
        {
            foreach (var medicamento in this.medicamentos)
            {
                Console.WriteLine($"ID: {medicamento.id} | Nome: {medicamento.nome} | Controlado: {(medicamento.controlado ? "Sim" : "Não")}");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Medicamento");
        Console.WriteLine();
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.medicamentos.Add(new Medicamento(novoId, this.medicamento.nome, this.medicamento.controlado, this.medicamento.estoqueMinimo));
            Console.WriteLine("Medicamento cadastrado com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Medicamento");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do medicamento a alterar: ");
        if (!int.TryParse(idInput, out this.medicamento.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Medicamento não encontrado.");
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
                this.medicamentos[this.indice] = new Medicamento(this.medicamento.id, this.medicamento.nome, this.medicamento.controlado, this.medicamento.estoqueMinimo);
                Console.WriteLine("Medicamento alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Medicamento");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do medicamento a excluir: ");
        if (!int.TryParse(idInput, out this.medicamento.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Medicamento não encontrado.");
        }
        else
        {
            Console.WriteLine("\nMedicamento a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.medicamentos.RemoveAt(this.indice);
                Console.WriteLine("Medicamento excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        this.medicamento.nome = tela.Perguntar("Nome: ");
        
        string controladoInput = tela.Perguntar("Controlado (true/false): ");
        if (!bool.TryParse(controladoInput, out this.medicamento.controlado))
        {
            Console.WriteLine("Valor inválido para controlado!");
            return;
        }
        
        string estoqueInput = tela.Perguntar("Estoque Mínimo: ");
        if (!int.TryParse(estoqueInput, out this.medicamento.estoqueMinimo))
        {
            Console.WriteLine("Estoque mínimo inválido!");
            return;
        }
    }

    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.medicamentos.Count; i++)
        {
            if (this.medicamento.id == this.medicamentos[i].id)
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
        Console.WriteLine("Medicamento encontrado:");
        Console.WriteLine($"Nome: {this.medicamentos[this.indice].nome}");
        Console.WriteLine($"Controlado: {(this.medicamentos[this.indice].controlado ? "Sim" : "Não")}");
        Console.WriteLine($"Estoque Mínimo: {this.medicamentos[this.indice].estoqueMinimo}");
        Console.WriteLine($"Quantidade em Estoque: {this.medicamentos[this.indice].quantidadeEstoque}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Medicamento");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do medicamento: ");
        if (!int.TryParse(idInput, out this.medicamento.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Medicamento não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO MEDICAMENTO ===");
            this.MostrarDados();
            
            // Verificar estoque baixo
            var medicamento = this.medicamentos[this.indice];
            if (medicamento.quantidadeEstoque <= medicamento.estoqueMinimo)
            {
                tela.ExibirAviso($"ATENÇÃO: Estoque baixo! Quantidade atual ({medicamento.quantidadeEstoque}) está no limite mínimo ({medicamento.estoqueMinimo}).");
            }
            else
            {
                tela.ExibirSucesso($"Estoque OK: {medicamento.quantidadeEstoque} unidades disponíveis.");
            }
        }
        
        tela.Pausar();
    }
}
