public class AgendaCRUD
{
    private List<Agendamento> agendamentos;
    private List<Veterinario> veterinarios;
    private List<Paciente> pacientes;

    public AgendaCRUD(List<Agendamento> agendamentos, List<Veterinario> veterinarios, List<Paciente> pacientes)
    {
        this.agendamentos = agendamentos;
        this.veterinarios = veterinarios;
        this.pacientes = pacientes;
    }

    private Paciente BuscarPacientePorId(int id)
    {
        for (int i = 0; i < this.pacientes.Count; i++)
        {
            if (this.pacientes[i].id == id)
            {
                return this.pacientes[i];
            }
        }
        return null;
    }

    private Veterinario BuscarVeterinarioPorId(int id)
    {
        for (int i = 0; i < this.veterinarios.Count; i++)
        {
            if (this.veterinarios[i].id == id)
            {
                return this.veterinarios[i];
            }
        }
        return null;
    }

    private void OrdenarAgendamentosPorData(List<Agendamento> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - 1 - i; j++)
            {
                if (lista[j].dataHora > lista[j + 1].dataHora)
                {
                    Agendamento temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }
    }

    public void ExecutarCRUD(string tipoUsuario)
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Sistema de Agenda");
            Console.WriteLine();
            
            // mostra opções baseado no tipo de usuario
            if (tipoUsuario == "veterinario")
            {
                tela.ExibirOpcao("1", "Minha Agenda (veterinario)");
                tela.ExibirOpcao("3", "Agenda Geral");
                opcoesValidas.AddRange(new List<char> { '1', '3' });
            }
            else if (tipoUsuario == "Recepcionista")
            {
                tela.ExibirOpcao("2", "Agenda dos Médicos (Recepcionista)");
                tela.ExibirOpcao("3", "Agenda Geral");
                opcoesValidas.AddRange(new List<char> { '2', '3' });
            }
            else if (tipoUsuario == "Gerente")
            {
                tela.ExibirOpcao("1", "Minha Agenda (veterinario)");
                tela.ExibirOpcao("2", "Agenda dos Médicos (Recepcionista)");
                tela.ExibirOpcao("3", "Agenda Geral");
                opcoesValidas.AddRange(new List<char> { '1', '2', '3' });
            }
            
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);

            if (opcao == '0') break;
            if (opcao == '1') this.MostrarMinhaAgenda();
            if (opcao == '2') this.MostrarAgendaMedicos();
            if (opcao == '3') this.MostrarAgendaGeral();
        }
    }

    public void MostrarMinhaAgenda()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Minha Agenda - veterinario");
        Console.WriteLine();
        
        string idVeterinarioInput = tela.Perguntar("Digite seu ID de veterinario: ");
        if (!int.TryParse(idVeterinarioInput, out int idVeterinario))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '0' };
        
        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Minha Agenda");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Agenda de Hoje");
            tela.ExibirOpcao("2", "Agenda da Semana");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);
            
            if (opcao == '0') break;
            if (opcao == '1') this.MostrarAgendaHoje(idVeterinario);
            if (opcao == '2') this.MostrarAgendaSemana(idVeterinario);
        }
    }

    public void MostrarAgendaMedicos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Agenda dos Médicos - Recepcionista");
        Console.WriteLine();
        
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '0' };
        
        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Agenda dos Médicos");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Listar Todos os veterinarios");
            tela.ExibirOpcao("2", "Escolher veterinario Específico");
            tela.ExibirOpcao("3", "Agendamentos Disponíveis");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);
            
            if (opcao == '0') break;
            if (opcao == '1') this.ListarVeterinarios();
            if (opcao == '2') this.EscolherVeterinario();
            if (opcao == '3') this.MostrarAgendamentosDisponiveis();
        }
    }

    public void MostrarAgendaGeral()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Agenda Geral");
        Console.WriteLine();
        
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '0' };
        
        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Agenda Geral");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Agenda de Hoje");
            tela.ExibirOpcao("2", "Agenda da Semana");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);
            
            if (opcao == '0') break;
            if (opcao == '1') this.MostrarAgendaHoje();
            if (opcao == '2') this.MostrarAgendaSemana();
        }
    }

    private void MostrarAgendaHoje(int? idVeterinario = null)
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        DateTime hoje = DateTime.Today;
        DateTime amanha = hoje.AddDays(1);
        
        List<Agendamento> agendamentosHoje = new List<Agendamento>();
        for (int i = 0; i < agendamentos.Count; i++)
        {
            bool dentroDoPeriodo = agendamentos[i].dataHora >= hoje && agendamentos[i].dataHora < amanha;
            bool filtroVeterinario = true;
            if (idVeterinario != null)
            {
                if (agendamentos[i].idDoVeterinario != idVeterinario)
                {
                    filtroVeterinario = false;
                }
            }
            
            if (dentroDoPeriodo && filtroVeterinario)
            {
                agendamentosHoje.Add(agendamentos[i]);
            }
        }
        
        OrdenarAgendamentosPorData(agendamentosHoje);
        
        Console.WriteLine($"Agenda de Hoje - {hoje:dd/MM/yyyy}");
        Console.WriteLine();
        
        if (agendamentosHoje.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento para hoje.");
        }
        else
        {
            string[] cabecalhos = { "Hora", "Paciente", "veterinario", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < agendamentosHoje.Count; i++)
            {
                Agendamento agendamento = agendamentosHoje[i];
                Paciente paciente = BuscarPacientePorId(agendamento.idDoPaciente);
                Veterinario veterinario = BuscarVeterinarioPorId(agendamento.idDoVeterinario);
                
                string status;
                if (agendamento.IsVencido())
                {
                    status = "VENCIDO";
                }
                else
                {
                    status = agendamento.statusDetalhado;
                }
                
                string nomePaciente;
                if (paciente != null)
                {
                    nomePaciente = paciente.nome;
                }
                else
                {
                    nomePaciente = "N/A";
                }
                
                string nomeVeterinario;
                if (veterinario != null)
                {
                    nomeVeterinario = veterinario.nome;
                }
                else
                {
                    nomeVeterinario = "N/A";
                }
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("HH:mm"),
                    nomePaciente,
                    nomeVeterinario,
                    agendamento.tipoProcedimento,
                    status
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }

    private void MostrarAgendaSemana(int? idVeterinario = null)
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        DateTime inicioSemana = DateTime.Today;
        DateTime fimSemana = inicioSemana.AddDays(7);
        
        List<Agendamento> agendamentosSemana = new List<Agendamento>();
        for (int i = 0; i < agendamentos.Count; i++)
        {
            bool dentroDoPeriodo = agendamentos[i].dataHora >= inicioSemana && agendamentos[i].dataHora < fimSemana;
            bool filtroVeterinario = true;
            if (idVeterinario != null)
            {
                if (agendamentos[i].idDoVeterinario != idVeterinario)
                {
                    filtroVeterinario = false;
                }
            }
            
            if (dentroDoPeriodo && filtroVeterinario)
            {
                agendamentosSemana.Add(agendamentos[i]);
            }
        }
        
        OrdenarAgendamentosPorData(agendamentosSemana);
        
        Console.WriteLine($"Agenda da Semana - {inicioSemana:dd/MM/yyyy} a {fimSemana.AddDays(-1):dd/MM/yyyy}");
        Console.WriteLine();
        
        if (agendamentosSemana.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento para esta semana.");
        }
        else
        {
            string[] cabecalhos = { "Data", "Hora", "Paciente", "veterinario", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < agendamentosSemana.Count; i++)
            {
                Agendamento agendamento = agendamentosSemana[i];
                Paciente paciente = BuscarPacientePorId(agendamento.idDoPaciente);
                Veterinario veterinario = BuscarVeterinarioPorId(agendamento.idDoVeterinario);
                
                string status;
                if (agendamento.IsVencido())
                {
                    status = "VENCIDO";
                }
                else
                {
                    status = agendamento.statusDetalhado;
                }
                
                string nomePaciente;
                if (paciente != null)
                {
                    nomePaciente = paciente.nome;
                }
                else
                {
                    nomePaciente = "N/A";
                }
                
                string nomeVeterinario;
                if (veterinario != null)
                {
                    nomeVeterinario = veterinario.nome;
                }
                else
                {
                    nomeVeterinario = "N/A";
                }
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("dd/MM"),
                    agendamento.dataHora.ToString("HH:mm"),
                    nomePaciente,
                    nomeVeterinario,
                    agendamento.tipoProcedimento,
                    status
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }

    private void ListarVeterinarios()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de veterinarios");
        Console.WriteLine();
        
        if (veterinarios.Count == 0)
        {
            Console.WriteLine("Nenhum veterinario cadastrado.");
        }
        else
        {
            string[] cabecalhos = { "ID", "Nome", "CRMV", "Especialidade" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var veterinario in veterinarios)
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
        
        tela.Pausar();
    }

    private void EscolherVeterinario()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Escolher veterinario");
        Console.WriteLine();
        
        string idVeterinarioInput = tela.Perguntar("Digite o ID do veterinario: ");
        if (!int.TryParse(idVeterinarioInput, out int idVeterinario))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        Veterinario veterinario = BuscarVeterinarioPorId(idVeterinario);
        if (veterinario == null)
        {
            tela.ExibirErro("veterinario não encontrado!");
            tela.Pausar();
            return;
        }
        
        Console.WriteLine($"\nAgenda do Dr(a). {veterinario.nome}");
        Console.WriteLine();
        
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '0' };
        
        while (true)
        {
            tela.LimparTela();
            Console.WriteLine($"Agenda do Dr(a). {veterinario.nome}");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Agenda de Hoje");
            tela.ExibirOpcao("2", "Agenda da Semana");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);
            
            if (opcao == '0') break;
            if (opcao == '1') this.MostrarAgendaHoje(idVeterinario);
            if (opcao == '2') this.MostrarAgendaSemana(idVeterinario);
        }
    }

    private void MostrarAgendamentosDisponiveis()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Agendamentos Disponíveis");
        Console.WriteLine();
        
        List<Agendamento> agendamentosDisponiveis = new List<Agendamento>();
        for (int i = 0; i < agendamentos.Count; i++)
        {
            bool dataValida = agendamentos[i].dataHora >= DateTime.Now;
            bool statusValido = false;
            if (agendamentos[i].statusDetalhado == "Agendado")
            {
                statusValido = true;
            }
            else if (agendamentos[i].statusDetalhado == "Confirmado")
            {
                statusValido = true;
            }
            
            if (dataValida && statusValido)
            {
                agendamentosDisponiveis.Add(agendamentos[i]);
            }
        }
        
        OrdenarAgendamentosPorData(agendamentosDisponiveis);
        
        if (agendamentosDisponiveis.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento disponível.");
        }
        else
        {
            string[] cabecalhos = { "Data/Hora", "Paciente", "veterinario", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < agendamentosDisponiveis.Count; i++)
            {
                Agendamento agendamento = agendamentosDisponiveis[i];
                Paciente paciente = BuscarPacientePorId(agendamento.idDoPaciente);
                Veterinario veterinario = BuscarVeterinarioPorId(agendamento.idDoVeterinario);
                
                string nomePaciente;
                if (paciente != null)
                {
                    nomePaciente = paciente.nome;
                }
                else
                {
                    nomePaciente = "N/A";
                }
                
                string nomeVeterinario;
                if (veterinario != null)
                {
                    nomeVeterinario = veterinario.nome;
                }
                else
                {
                    nomeVeterinario = "N/A";
                }
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("dd/MM/yyyy HH:mm"),
                    nomePaciente,
                    nomeVeterinario,
                    agendamento.tipoProcedimento,
                    agendamento.statusDetalhado
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }
}
