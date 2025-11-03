public class RelatoriosCRUD
{
    private List<Agendamento> agendamentos;
    private List<Paciente> pacientes;
    private List<Veterinario> veterinarios;
    private List<HistoricoClinico> historicos;
    private List<Medicamento> medicamentos;
    private List<Vacina> vacinas;

    public RelatoriosCRUD(List<Agendamento> agendamentos, List<Paciente> pacientes, List<Veterinario> veterinarios, 
        List<HistoricoClinico> historicos, List<Medicamento> medicamentos, List<Vacina> vacinas)
    {
        this.agendamentos = agendamentos;
        this.pacientes = pacientes;
        this.veterinarios = veterinarios;
        this.historicos = historicos;
        this.medicamentos = medicamentos;
        this.vacinas = vacinas;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Sistema de Relatórios");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Agendamentos por Período");
            tela.ExibirOpcao("2", "Pacientes mais Frequentes");
            tela.ExibirOpcao("3", "Veterinários - Atendimentos");
            tela.ExibirOpcao("4", "Estoque Baixo");
            tela.ExibirOpcao("5", "Status de Agendamentos");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);

            if (opcao == '0') break;
            if (opcao == '1') this.RelatorioAgendamentosPorPeriodo();
            if (opcao == '2') this.RelatorioPacientesMaisFrequentes();
            if (opcao == '3') this.RelatorioVeterinariosAtendimentos();
            if (opcao == '4') this.RelatorioEstoqueBaixo();
            if (opcao == '5') this.RelatorioStatusAgendamentos();
        }
    }

    private void RelatorioAgendamentosPorPeriodo()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Relatório - Agendamentos por Período");
        Console.WriteLine();
        
        string dataInicioInput = tela.Perguntar("Data de Início (dd/mm/aaaa): ");
        if (!DateTime.TryParse(dataInicioInput, out DateTime dataInicio))
        {
            tela.ExibirErro("Data inválida!");
            tela.Pausar();
            return;
        }
        
        string dataFimInput = tela.Perguntar("Data de Fim (dd/mm/aaaa): ");
        if (!DateTime.TryParse(dataFimInput, out DateTime dataFim))
        {
            tela.ExibirErro("Data inválida!");
            tela.Pausar();
            return;
        }
        
        dataFim = dataFim.AddDays(1).AddTicks(-1); // Incluir o dia inteiro
        
        var agendamentosPeriodo = agendamentos.Where(a => a.dataHora >= dataInicio && a.dataHora <= dataFim).ToList();
        
        Console.WriteLine($"\n=== RELATÓRIO DE AGENDAMENTOS ===");
        Console.WriteLine($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}");
        Console.WriteLine($"Total de Agendamentos: {agendamentosPeriodo.Count}");
        Console.WriteLine();
        
        if (agendamentosPeriodo.Count > 0)
        {
            string[] cabecalhos = { "Data", "Hora", "Paciente", "Veterinário", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var agendamento in agendamentosPeriodo.OrderBy(a => a.dataHora))
            {
                var paciente = pacientes.FirstOrDefault(p => p.id == agendamento.idDoPaciente);
                var veterinario = veterinarios.FirstOrDefault(v => v.id == agendamento.idDoVeterinario);
                
                dados.Add(new string[] {
                    agendamento.dataHora.ToString("dd/MM/yyyy"),
                    agendamento.dataHora.ToString("HH:mm"),
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

    private void RelatorioPacientesMaisFrequentes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Relatório - Pacientes mais Frequentes");
        Console.WriteLine();
        
        var pacientesFrequencia = historicos
            .GroupBy(h => h.idDoProntuario)
            .Select(g => new {
                IdProntuario = g.Key,
                QuantidadeAtendimentos = g.Count(),
                UltimoAtendimento = g.Max(h => h.dataAtendimento)
            })
            .OrderByDescending(p => p.QuantidadeAtendimentos)
            .Take(10)
            .ToList();
        
        Console.WriteLine("=== TOP 10 PACIENTES MAIS FREQUENTES ===");
        Console.WriteLine();
        
        if (pacientesFrequencia.Count == 0)
        {
            Console.WriteLine("Nenhum atendimento registrado.");
        }
        else
        {
            string[] cabecalhos = { "Posição", "Paciente", "Atendimentos", "Último Atendimento" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < pacientesFrequencia.Count; i++)
            {
                var prontuario = pacientesFrequencia[i];
                var paciente = pacientes.FirstOrDefault(p => p.id == prontuario.IdProntuario);
                
                dados.Add(new string[] {
                    (i + 1).ToString(),
                    paciente?.nome ?? "N/A",
                    prontuario.QuantidadeAtendimentos.ToString(),
                    prontuario.UltimoAtendimento.ToString("dd/MM/yyyy")
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }

    private void RelatorioVeterinariosAtendimentos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Relatório - Veterinários - Atendimentos");
        Console.WriteLine();
        
        var veterinariosAtendimentos = historicos
            .GroupBy(h => h.idDoVeterinario)
            .Select(g => new {
                IdVeterinario = g.Key,
                QuantidadeAtendimentos = g.Count(),
                UltimoAtendimento = g.Max(h => h.dataAtendimento)
            })
            .OrderByDescending(v => v.QuantidadeAtendimentos)
            .ToList();
        
        Console.WriteLine("=== ATENDIMENTOS POR VETERINÁRIO ===");
        Console.WriteLine();
        
        if (veterinariosAtendimentos.Count == 0)
        {
            Console.WriteLine("Nenhum atendimento registrado.");
        }
        else
        {
            string[] cabecalhos = { "Veterinário", "CRMV", "Especialidade", "Atendimentos", "Último Atendimento" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var vet in veterinariosAtendimentos)
            {
                var veterinario = veterinarios.FirstOrDefault(v => v.id == vet.IdVeterinario);
                
                dados.Add(new string[] {
                    veterinario?.nome ?? "N/A",
                    veterinario?.crmv ?? "N/A",
                    veterinario?.especialidade ?? "N/A",
                    vet.QuantidadeAtendimentos.ToString(),
                    vet.UltimoAtendimento.ToString("dd/MM/yyyy")
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }

    private void RelatorioEstoqueBaixo()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Relatório - Estoque Baixo");
        Console.WriteLine();
        
        // Simulando estoque baixo - em um sistema real, isso viria de uma tabela de estoque
        var medicamentosBaixo = medicamentos.Where(m => m.estoqueMinimo > 0).ToList();
        var vacinasBaixo = vacinas.Where(v => true).ToList(); // Todas as vacinas por enquanto
        
        Console.WriteLine("=== PRODUTOS COM ESTOQUE BAIXO ===");
        Console.WriteLine();
        
        if (medicamentosBaixo.Count == 0 && vacinasBaixo.Count == 0)
        {
            Console.WriteLine("Nenhum produto com estoque baixo.");
        }
        else
        {
            string[] cabecalhos = { "Tipo", "Nome", "Estoque Mínimo", "Status" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var medicamento in medicamentosBaixo)
            {
                dados.Add(new string[] {
                    "Medicamento",
                    medicamento.nome,
                    medicamento.estoqueMinimo.ToString(),
                    medicamento.controlado ? "Controlado" : "Livre"
                });
            }
            
            foreach (var vacina in vacinasBaixo)
            {
                dados.Add(new string[] {
                    "Vacina",
                    vacina.nome,
                    "N/A",
                    $"Periodicidade: {vacina.periodicidade} dias"
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }

    private void RelatorioStatusAgendamentos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Relatório - Status de Agendamentos");
        Console.WriteLine();
        
        var statusAgendamentos = agendamentos
            .GroupBy(a => a.statusDetalhado)
            .Select(g => new {
                Status = g.Key,
                Quantidade = g.Count(),
                Percentual = (double)g.Count() / agendamentos.Count * 100
            })
            .OrderByDescending(s => s.Quantidade)
            .ToList();
        
        Console.WriteLine("=== DISTRIBUIÇÃO POR STATUS ===");
        Console.WriteLine($"Total de Agendamentos: {agendamentos.Count}");
        Console.WriteLine();
        
        if (statusAgendamentos.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento cadastrado.");
        }
        else
        {
            string[] cabecalhos = { "Status", "Quantidade", "Percentual" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var status in statusAgendamentos)
            {
                dados.Add(new string[] {
                    status.Status,
                    status.Quantidade.ToString(),
                    $"{status.Percentual:F1}%"
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }
}
