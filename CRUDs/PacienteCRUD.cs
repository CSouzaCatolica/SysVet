public class PacienteCRUD
{
    private List<Paciente> pacientes;
    private Paciente paciente;
    private int indice;
    private TutorCRUD tutorCRUD;
    private ProntuarioCRUD prontuarioCRUD;

    public PacienteCRUD()
    {
        this.pacientes = new List<Paciente>();
        this.paciente = new Paciente();
        this.indice = -1;
        this.tutorCRUD = null;
        this.prontuarioCRUD = null;
    }

    public void SetTutorCRUD(TutorCRUD tutorCRUD)
    {
        this.tutorCRUD = tutorCRUD;
    }

    public void SetProntuarioCRUD(ProntuarioCRUD prontuarioCRUD)
    {
        this.prontuarioCRUD = prontuarioCRUD;
    }

    public List<Paciente> GetPacientes()
    {
        return this.pacientes;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Pacientes");
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
        return this.pacientes.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Pacientes");
        Console.WriteLine();
        
        if (this.pacientes.Count == 0)
        {
            Console.WriteLine("Nenhum paciente cadastrado.");
        }
        else
        {
            foreach (var paciente in this.pacientes)
            {
                Console.WriteLine($"ID: {paciente.id} | Nome: {paciente.nome} | Espécie: {paciente.especie} | Tutor ID: {paciente.idDoTutor}");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Paciente");
        Console.WriteLine();
        
        // Validação preventiva - verificar se existem tutores
        if (tutorCRUD == null || tutorCRUD.GetTutores().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar paciente: nenhum tutor cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um tutor antes de criar pacientes.");
            tela.Pausar();
            return;
        }
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.pacientes.Add(new Paciente(novoId, this.paciente.idDoTutor, this.paciente.nome, this.paciente.especie, this.paciente.raca, this.paciente.peso, this.paciente.status));
            
            // Criar prontuário automaticamente
            if (prontuarioCRUD != null)
            {
                prontuarioCRUD.CriarAutomatico(novoId);
            }
            
            tela.ExibirSucesso("Paciente cadastrado com sucesso!");
        }
        
        tela.Pausar();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Paciente");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do paciente a alterar: ");
        if (!int.TryParse(idInput, out this.paciente.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Paciente não encontrado.");
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
                this.pacientes[this.indice] = new Paciente(this.paciente.id, this.paciente.idDoTutor, this.paciente.nome, this.paciente.especie, this.paciente.raca, this.paciente.peso, this.paciente.status);
                Console.WriteLine("Paciente alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Paciente");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do paciente a excluir: ");
        if (!int.TryParse(idInput, out this.paciente.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Paciente não encontrado.");
        }
        else
        {
            Console.WriteLine("\nPaciente a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.pacientes.RemoveAt(this.indice);
                Console.WriteLine("Paciente excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        // Validação do ID do Tutor
        while (true)
        {
            string idTutorInput = tela.Perguntar("ID do Tutor: ");
            if (!int.TryParse(idTutorInput, out this.paciente.idDoTutor))
            {
                tela.ExibirErro("ID do Tutor inválido! Digite um número.");
                continue;
            }
            
            // Verificar se o tutor existe
            if (tutorCRUD != null)
            {
                var tutorExiste = tutorCRUD.GetTutores().Any(t => t.id == this.paciente.idDoTutor);
                if (!tutorExiste)
                {
                    tela.ExibirErro($"Tutor com ID {this.paciente.idDoTutor} não encontrado!");
                    
                    // Mostrar tutores disponíveis
                    Console.WriteLine("\nTutores disponíveis:");
                    if (tutorCRUD.GetTutores().Count > 0)
                    {
                        string[] cabecalhos = { "ID", "Nome", "CPF" };
                        List<string[]> dados = new List<string[]>();
                        
                        foreach (var tutor in tutorCRUD.GetTutores())
                        {
                            dados.Add(new string[] {
                                tutor.id.ToString(),
                                tutor.nome,
                                tutor.cpf
                            });
                        }
                        
                        tela.ExibirTabela(cabecalhos, dados);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum tutor cadastrado.");
                    }
                    Console.WriteLine();
                    continue;
                }
            }
            break;
        }
        
        this.paciente.nome = tela.Perguntar("Nome: ");
        this.paciente.especie = tela.Perguntar("Espécie: ");
        this.paciente.raca = tela.Perguntar("Raça: ");
        
        // Validação do peso com loop
        while (true)
        {
            string pesoInput = tela.Perguntar("Peso (kg): ");
            
            // Remover "kg" se presente e converter vírgula para ponto
            string pesoLimpo = pesoInput.Replace("kg", "").Replace("KG", "").Replace(" ", "").Replace(",", ".");
            
            if (double.TryParse(pesoLimpo, out double peso))
            {
                if (peso > 0)
                {
                    this.paciente.peso = peso;
                    break;
                }
                else
                {
                    tela.ExibirErro("Peso deve ser maior que zero!");
                }
            }
            else
            {
                tela.ExibirErro("Peso inválido! Digite um número válido (ex: 25.5 ou 25,5)");
            }
        }
        
        this.paciente.status = tela.Perguntar("Status: ");
    }

    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.pacientes.Count; i++)
        {
            if (this.paciente.id == this.pacientes[i].id)
            {
                encontrei = true;
                this.indice = i;
                // Atualizar o objeto paciente com os dados encontrados
                this.paciente = this.pacientes[i];
                break;
            }
        }
        return encontrei;
    }

    public void MostrarDados()
    {
        Console.WriteLine("Paciente encontrado:");
        Console.WriteLine($"ID do Tutor: {this.pacientes[this.indice].idDoTutor}");
        Console.WriteLine($"Nome: {this.pacientes[this.indice].nome}");
        Console.WriteLine($"Espécie: {this.pacientes[this.indice].especie}");
        Console.WriteLine($"Raça: {this.pacientes[this.indice].raca}");
        Console.WriteLine($"Peso: {this.pacientes[this.indice].peso} kg");
        Console.WriteLine($"Status: {this.pacientes[this.indice].status}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Paciente");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do paciente: ");
        if (!int.TryParse(idInput, out this.paciente.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Paciente não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO PACIENTE ===");
            this.MostrarDados();

            var prontuario = prontuarioCRUD.BuscarPorPaciente(this.paciente.id);
            if (prontuario != null)
            {
                Console.WriteLine("\n=== PRONTUÁRIO ===");
                Console.WriteLine($"ID do Prontuário: {prontuario.id}");
                Console.WriteLine($"Data de Abertura: {prontuario.dataAbertura:dd/MM/yyyy}");
                Console.WriteLine($"Status: {(prontuario.ativo ? "Ativo" : "Inativo")}");
                
                // Mostrar histórico clínico recente (últimas 3 entradas)
                Console.WriteLine("\n=== HISTÓRICO CLÍNICO RECENTE ===");
                Console.WriteLine("(Para visualizar histórico completo, use o menu Prontuários)");
            }
            else
            {
                Console.WriteLine("\n=== PRONTUÁRIO ===");
                Console.WriteLine("Nenhum prontuário encontrado para este paciente.");
            }

        }
        
        tela.Pausar();
    }
}
