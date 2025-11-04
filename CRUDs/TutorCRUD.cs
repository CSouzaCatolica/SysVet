public class TutorCRUD
{
    private List<Tutor> tutores;
    private Tutor tutor;
    private int indice;
    private PacienteCRUD pacienteCRUD;

    public TutorCRUD()
    {
        this.tutores = new List<Tutor>();
        this.tutor = new Tutor();
        this.indice = -1;
        this.pacienteCRUD = null;
    }

    public void SetPacienteCRUD(PacienteCRUD pacienteCRUD)
    {
        this.pacienteCRUD = pacienteCRUD;
    }

    public List<Tutor> GetTutores()
    {
        return this.tutores;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Tutores");
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
        return this.tutores.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Tutores");
        Console.WriteLine();
        
        if (this.tutores.Count == 0)
        {
            Console.WriteLine("Nenhum tutor cadastrado.");
        }
        else
        {
            foreach (var tutor in this.tutores)
            {
                Console.WriteLine($"ID: {tutor.id} | Nome: {tutor.nome} | CPF: {tutor.cpf}");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Tutor");
        Console.WriteLine();
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.tutores.Add(new Tutor(novoId, this.tutor.nome, this.tutor.cpf, this.tutor.telefone, this.tutor.email, this.tutor.endereco));
            Console.WriteLine("Tutor cadastrado com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Tutor");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do tutor a alterar: ");
        if (!int.TryParse(idInput, out this.tutor.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Tutor não encontrado.");
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
                this.tutores[this.indice] = new Tutor(this.tutor.id, this.tutor.nome, this.tutor.cpf, this.tutor.telefone, this.tutor.email, this.tutor.endereco);
                Console.WriteLine("Tutor alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Tutor");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do tutor a excluir: ");
        if (!int.TryParse(idInput, out this.tutor.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Tutor não encontrado.");
        }
        else
        {
            Console.WriteLine("\nTutor a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.tutores.RemoveAt(this.indice);
                Console.WriteLine("Tutor excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        this.tutor.nome = tela.Perguntar("Nome: ");
        
        // valida cpf
        while (true)
        {
            string cpfInput = tela.Perguntar("CPF (apenas números ou formato XXX.XXX.XXX-XX): ");
            
            // tira formatação pra validar
            string cpfLimpo = cpfInput.Replace(".", "").Replace("-", "").Replace(" ", "");
            
            if (ValidarCPF(cpfLimpo))
            {
                this.tutor.cpf = cpfInput;
                break;
            }
            else
            {
                tela.ExibirErro("CPF inválido! Digite 11 dígitos (ex: 12345678901 ou 123.456.789-01).");
            }
        }
        
        this.tutor.telefone = tela.Perguntar("Telefone: ");
        this.tutor.email = tela.Perguntar("Email: ");
        this.tutor.endereco = tela.Perguntar("Endereço: ");
    }

    private bool ValidarCPF(string cpf)
    {
        // verifica se tem exatamente 11 
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
        {
            return false;
        }
        
        
        // verifica se não são todos zeros
        if (cpf == "00000000000")
        {
            return false;
        }
        
        // aceita qualquer cpf com 11 digitos 
        return true;
    }

    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.tutores.Count; i++)
        {
            if (this.tutor.id == this.tutores[i].id)
            {
                encontrei = true;
                this.indice = i;
                this.tutor = this.tutores[i];
                break;
            }
        }
        return encontrei;
    }

    public void MostrarDados()
    {
        Console.WriteLine("Tutor encontrado:");
        Console.WriteLine($"Nome: {this.tutores[this.indice].nome}");
        Console.WriteLine($"CPF: {this.tutores[this.indice].cpf}");
        Console.WriteLine($"Telefone: {this.tutores[this.indice].telefone}");
        Console.WriteLine($"Email: {this.tutores[this.indice].email}");
        Console.WriteLine($"Endereço: {this.tutores[this.indice].endereco}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Tutor");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do tutor: ");
        if (!int.TryParse(idInput, out this.tutor.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Tutor não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO TUTOR ===");
            this.MostrarDados();
         
            List<Paciente> petsDoTutor = new List<Paciente>();
            List<Paciente> todosPacientes = pacienteCRUD.GetPacientes();
            for (int i = 0; i < todosPacientes.Count; i++)
            {
                if (todosPacientes[i].idDoTutor == this.tutor.id)
                {
                    petsDoTutor.Add(todosPacientes[i]);
                }
            }
            
            Console.WriteLine("\n=== PETS DO TUTOR ===");
            if (petsDoTutor.Count == 0)
            {
                Console.WriteLine("Nenhum pet cadastrado para este tutor.");
            }
            else
            {
                string[] cabecalhos = { "ID", "Nome", "Espécie", "Raça", "Peso" };
                List<string[]> dados = new List<string[]>();
                
                foreach (var pet in petsDoTutor)
                {
                    dados.Add(new string[] {
                        pet.id.ToString(),
                        pet.nome,
                        pet.especie,
                        pet.raca,
                        $"{pet.peso} kg"
                    });
                }
                
                tela.ExibirTabela(cabecalhos, dados);
            }

        }
        
        tela.Pausar();
    }
}
