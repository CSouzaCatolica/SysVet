public class AgendamentoCRUD
{
    private List<Agendamento> agendamentos;
    private Agendamento agendamento;
    private int indice;
    private PacienteCRUD pacienteCRUD;
    private VeterinarioCRUD veterinarioCRUD;

    public AgendamentoCRUD()
    {
        this.agendamentos = new List<Agendamento>();
        this.agendamento = new Agendamento();
        this.indice = -1;
        this.pacienteCRUD = null;
        this.veterinarioCRUD = null;
    }

    private Paciente BuscarPacientePorId(int id)
    {
        if (pacienteCRUD == null)
        {
            return null;
        }
        
        List<Paciente> listaPacientes = pacienteCRUD.GetPacientes();
        for (int i = 0; i < listaPacientes.Count; i++)
        {
            if (listaPacientes[i].id == id)
            {
                return listaPacientes[i];
            }
        }
        return null;
    }

    private Veterinario BuscarVeterinarioPorId(int id)
    {
        if (veterinarioCRUD == null)
        {
            return null;
        }
        
        List<Veterinario> listaVeterinarios = veterinarioCRUD.GetVeterinarios();
        for (int i = 0; i < listaVeterinarios.Count; i++)
        {
            if (listaVeterinarios[i].id == id)
            {
                return listaVeterinarios[i];
            }
        }
        return null;
    }

    public void SetPacienteCRUD(PacienteCRUD pacienteCRUD)
    {
        this.pacienteCRUD = pacienteCRUD;
    }

    public void SetVeterinarioCRUD(VeterinarioCRUD veterinarioCRUD)
    {
        this.veterinarioCRUD = veterinarioCRUD;
    }

    public List<Agendamento> GetAgendamentos()
    {
        return this.agendamentos;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Agendamentos");
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
        return this.agendamentos.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Agendamentos");
        Console.WriteLine();
        
        if (this.agendamentos.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento cadastrado.");
        }
        else
        {
            foreach (var agendamento in this.agendamentos)
            {
                Console.WriteLine($"ID: {agendamento.id} | Paciente: {agendamento.idDoPaciente} | Veterinário: {agendamento.idDoVeterinario} | Data: {agendamento.dataHora:dd/MM/yyyy HH:mm}");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Agendamento");
        Console.WriteLine();
        
        // valida se tem pacientes
        if (pacienteCRUD == null || pacienteCRUD.GetPacientes().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar agendamento: nenhum paciente cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um paciente antes de criar agendamentos.");
            tela.Pausar();
            return;
        }
        
        // valida se tem veterinários
        if (veterinarioCRUD == null || veterinarioCRUD.GetVeterinarios().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar agendamento: nenhum veterinário cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um veterinário antes de criar agendamentos.");
            tela.Pausar();
            return;
        }
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.agendamentos.Add(new Agendamento(novoId, this.agendamento.idDoPaciente, this.agendamento.idDoVeterinario, this.agendamento.idDaSala, this.agendamento.dataHora, this.agendamento.duracao, this.agendamento.tipoProcedimento, this.agendamento.status));
            Console.WriteLine("Agendamento cadastrado com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Agendamento");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do agendamento a alterar: ");
        if (!int.TryParse(idInput, out this.agendamento.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Agendamento não encontrado.");
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
                this.agendamentos[this.indice] = new Agendamento(this.agendamento.id, this.agendamento.idDoPaciente, this.agendamento.idDoVeterinario, this.agendamento.idDaSala, this.agendamento.dataHora, this.agendamento.duracao, this.agendamento.tipoProcedimento, this.agendamento.status);
                Console.WriteLine("Agendamento alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Agendamento");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do agendamento a excluir: ");
        if (!int.TryParse(idInput, out this.agendamento.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Agendamento não encontrado.");
        }
        else
        {
            Console.WriteLine("\nAgendamento a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.agendamentos.RemoveAt(this.indice);
                Console.WriteLine("Agendamento excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        // valida id do paciente
        while (true)
        {
            string idPacienteInput = tela.Perguntar("ID do Paciente: ");
            if (!int.TryParse(idPacienteInput, out this.agendamento.idDoPaciente))
            {
                tela.ExibirErro("ID do Paciente inválido! Digite um número.");
                continue;
            }
            
            // verifica se o paciente existe
            if (pacienteCRUD != null)
            {
                bool pacienteExiste = false;
                List<Paciente> listaPacientes = pacienteCRUD.GetPacientes();
                for (int i = 0; i < listaPacientes.Count; i++)
                {
                    if (listaPacientes[i].id == this.agendamento.idDoPaciente)
                    {
                        pacienteExiste = true;
                        break;
                    }
                }
                if (!pacienteExiste)
                {
                    tela.ExibirErro($"Paciente com ID {this.agendamento.idDoPaciente} não encontrado!");
                    
                    // mostra os pacientes que tem
                    Console.WriteLine("\nPacientes disponíveis:");
                    if (pacienteCRUD.GetPacientes().Count > 0)
                    {
                        string[] cabecalhos = { "ID", "Nome", "Espécie", "Tutor ID" };
                        List<string[]> dados = new List<string[]>();
                        
                        foreach (var paciente in pacienteCRUD.GetPacientes())
                        {
                            dados.Add(new string[] {
                                paciente.id.ToString(),
                                paciente.nome,
                                paciente.especie,
                                paciente.idDoTutor.ToString()
                            });
                        }
                        
                        tela.ExibirTabela(cabecalhos, dados);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum paciente cadastrado.");
                        Console.WriteLine("Não é possível continuar sem pacientes cadastrados.");
                        tela.Pausar();
                        return;
                    }
                }
            }
            break;
        }
        
        // valida id do veterinário
        while (true)
        {
            string idVeterinarioInput = tela.Perguntar("ID do Veterinário: ");
            if (!int.TryParse(idVeterinarioInput, out this.agendamento.idDoVeterinario))
            {
                tela.ExibirErro("ID do Veterinário inválido! Digite um número.");
                continue;
            }
            
            // verifica se o veterinário existe
            if (veterinarioCRUD != null)
            {
                bool veterinarioExiste = false;
                List<Veterinario> listaVeterinarios = veterinarioCRUD.GetVeterinarios();
                for (int i = 0; i < listaVeterinarios.Count; i++)
                {
                    if (listaVeterinarios[i].id == this.agendamento.idDoVeterinario)
                    {
                        veterinarioExiste = true;
                        break;
                    }
                }
                if (!veterinarioExiste)
                {
                    tela.ExibirErro($"Veterinário com ID {this.agendamento.idDoVeterinario} não encontrado!");
                    
                    // mostra os veterinários que tem
                    Console.WriteLine("\nVeterinários disponíveis:");
                    if (veterinarioCRUD.GetVeterinarios().Count > 0)
                    {
                        string[] cabecalhos = { "ID", "Nome", "CRMV", "Especialidade" };
                        List<string[]> dados = new List<string[]>();
                        
                        foreach (var veterinario in veterinarioCRUD.GetVeterinarios())
                        {
                            dados.Add(new string[] {
                                veterinario.id.ToString(),
                                veterinario.nome,
                                veterinario.crmv,
                                veterinario.especialidade
                            });
                        }
                        
                        tela.ExibirTabela(cabecalhos, dados);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum veterinário cadastrado.");
                        Console.WriteLine("Não é possível continuar sem veterinários cadastrados.");
                        tela.Pausar();
                        return;
                    }
                }
            }
            break;
        }
        
        string idSalaInput = tela.Perguntar("ID da Sala: ");
        if (!int.TryParse(idSalaInput, out this.agendamento.idDaSala))
        {
            tela.ExibirErro("ID da Sala inválido!");
            return;
        }
        
        string dataHoraInput = tela.Perguntar("Data/Hora (dd/mm/aaaa hh:mm): ");
        if (!DateTime.TryParse(dataHoraInput, out this.agendamento.dataHora))
        {
            tela.ExibirErro("Data/Hora inválida!");
            return;
        }
        
        string duracaoInput = tela.Perguntar("Duração (minutos): ");
        if (!int.TryParse(duracaoInput, out this.agendamento.duracao))
        {
            tela.ExibirErro("Duração inválida!");
            return;
        }
        
        this.agendamento.tipoProcedimento = tela.Perguntar("Tipo de Procedimento: ");
        this.agendamento.status = tela.Perguntar("Status: ");
    }

    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.agendamentos.Count; i++)
        {
            if (this.agendamento.id == this.agendamentos[i].id)
            {
                encontrei = true;
                this.indice = i;
                // atualiza os dados do agendamento que achou
                this.agendamento = this.agendamentos[i];
                break;
            }
        }
        return encontrei;
    }

    public bool ExisteAgendamento(int id)
    {
        for (int i = 0; i < this.agendamentos.Count; i++)
        {
            if (this.agendamentos[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void MostrarDados()
    {
        Console.WriteLine("Agendamento encontrado:");
        Console.WriteLine($"ID do Paciente: {this.agendamentos[this.indice].idDoPaciente}");
        Console.WriteLine($"ID do Veterinário: {this.agendamentos[this.indice].idDoVeterinario}");
        Console.WriteLine($"ID da Sala: {this.agendamentos[this.indice].idDaSala}");
        Console.WriteLine($"Data/Hora: {this.agendamentos[this.indice].dataHora}");
        Console.WriteLine($"Duração: {this.agendamentos[this.indice].duracao} minutos");
        Console.WriteLine($"Tipo: {this.agendamentos[this.indice].tipoProcedimento}");
        Console.WriteLine($"Status: {this.agendamentos[this.indice].status}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Agendamento");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do agendamento: ");
        if (!int.TryParse(idInput, out this.agendamento.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Agendamento não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO AGENDAMENTO ===");
            this.MostrarDados();
            
            Paciente paciente = BuscarPacientePorId(this.agendamento.idDoPaciente);
            if (paciente != null)
            {
                Console.WriteLine("\n=== DADOS DO PACIENTE ===");
                Console.WriteLine($"Nome: {paciente.nome}");
                Console.WriteLine($"Espécie: {paciente.especie}");
                Console.WriteLine($"Raça: {paciente.raca}");
                Console.WriteLine($"Peso: {paciente.peso} kg");
                Console.WriteLine($"Status: {paciente.status}");
                Console.WriteLine($"ID do Tutor: {paciente.idDoTutor}");
            }
            else
            {
                Console.WriteLine("\n=== DADOS DO PACIENTE ===");
                Console.WriteLine($"⚠️  Paciente com ID {this.agendamento.idDoPaciente} foi excluído.");
            }
    
            Veterinario veterinario = BuscarVeterinarioPorId(this.agendamento.idDoVeterinario);
            if (veterinario != null)
            {
                Console.WriteLine("\n=== DADOS DO VETERINÁRIO ===");
                Console.WriteLine($"Nome: {veterinario.nome}");
                Console.WriteLine($"CRMV: {veterinario.crmv}");
                Console.WriteLine($"Especialidade: {veterinario.especialidade}");
                
                // mostra dados do usuário se tiver
                if (veterinario.idDoUsuario > 0)
                {
                    Console.WriteLine($"ID do Usuário: {veterinario.idDoUsuario}");
                }
            }
            else
            {
                Console.WriteLine("\n=== DADOS DO veterinario ===");
                Console.WriteLine($"X veterinario com ID {this.agendamento.idDoVeterinario} foi excluído.");
            }
        }
        
        tela.Pausar();
    }
}

