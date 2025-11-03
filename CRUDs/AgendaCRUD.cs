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
            
            // Exibir opções baseadas no tipo de usuário
            if (tipoUsuario == "Veterinário")
            {
                tela.ExibirOpcao("1", "Minha Agenda (Veterinário)");
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
                tela.ExibirOpcao("1", "Minha Agenda (Veterinário)");
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
        
        Console.WriteLine("Minha Agenda - Veterinário");
        Console.WriteLine();
        
        string idVeterinarioInput = tela.Perguntar("Digite seu ID de veterinário: ");
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
            
            tela.ExibirOpcao("1", "Listar Todos os Veterinários");
            tela.ExibirOpcao("2", "Escolher Veterinário Específico");
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
        
        var agendamentosHoje = agendamentos.Where(a => 
            a.dataHora >= hoje && a.dataHora < amanha &&
            (idVeterinario == null || a.idDoVeterinario == idVeterinario))
            .OrderBy(a => a.dataHora).ToList();
        
        Console.WriteLine($"Agenda de Hoje - {hoje:dd/MM/yyyy}");
        Console.WriteLine();
        
        if (agendamentosHoje.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento para hoje.");
        }
        else
        {
            string[] cabecalhos = { "Hora", "Paciente", "Veterinário", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var agendamento in agendamentosHoje)
            {
                var paciente = pacientes.FirstOrDefault(p => p.id == agendamento.idDoPaciente);
                var veterinario = veterinarios.FirstOrDefault(v => v.id == agendamento.idDoVeterinario);
                
                string status = agendamento.IsVencido() ? "VENCIDO" : agendamento.statusDetalhado;
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("HH:mm"),
                    paciente?.nome ?? "N/A",
                    veterinario?.nome ?? "N/A",
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
        
        var agendamentosSemana = agendamentos.Where(a => 
            a.dataHora >= inicioSemana && a.dataHora < fimSemana &&
            (idVeterinario == null || a.idDoVeterinario == idVeterinario))
            .OrderBy(a => a.dataHora).ToList();
        
        Console.WriteLine($"Agenda da Semana - {inicioSemana:dd/MM/yyyy} a {fimSemana.AddDays(-1):dd/MM/yyyy}");
        Console.WriteLine();
        
        if (agendamentosSemana.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento para esta semana.");
        }
        else
        {
            string[] cabecalhos = { "Data", "Hora", "Paciente", "Veterinário", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var agendamento in agendamentosSemana)
            {
                var paciente = pacientes.FirstOrDefault(p => p.id == agendamento.idDoPaciente);
                var veterinario = veterinarios.FirstOrDefault(v => v.id == agendamento.idDoVeterinario);
                
                string status = agendamento.IsVencido() ? "VENCIDO" : agendamento.statusDetalhado;
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("dd/MM"),
                    agendamento.dataHora.ToString("HH:mm"),
                    paciente?.nome ?? "N/A",
                    veterinario?.nome ?? "N/A",
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
        
        Console.WriteLine("Lista de Veterinários");
        Console.WriteLine();
        
        if (veterinarios.Count == 0)
        {
            Console.WriteLine("Nenhum veterinário cadastrado.");
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
        
        Console.WriteLine("Escolher Veterinário");
        Console.WriteLine();
        
        string idVeterinarioInput = tela.Perguntar("Digite o ID do veterinário: ");
        if (!int.TryParse(idVeterinarioInput, out int idVeterinario))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        var veterinario = veterinarios.FirstOrDefault(v => v.id == idVeterinario);
        if (veterinario == null)
        {
            tela.ExibirErro("Veterinário não encontrado!");
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
        
        var agendamentosDisponiveis = agendamentos.Where(a => 
            a.dataHora >= DateTime.Now && 
            (a.statusDetalhado == "Agendado" || a.statusDetalhado == "Confirmado"))
            .OrderBy(a => a.dataHora).ToList();
        
        if (agendamentosDisponiveis.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento disponível.");
        }
        else
        {
            string[] cabecalhos = { "Data/Hora", "Paciente", "Veterinário", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var agendamento in agendamentosDisponiveis)
            {
                var paciente = pacientes.FirstOrDefault(p => p.id == agendamento.idDoPaciente);
                var veterinario = veterinarios.FirstOrDefault(v => v.id == agendamento.idDoVeterinario);
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("dd/MM/yyyy HH:mm"),
                    paciente?.nome ?? "N/A",
                    veterinario?.nome ?? "N/A",
                    agendamento.tipoProcedimento,
                    agendamento.statusDetalhado
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }
}
