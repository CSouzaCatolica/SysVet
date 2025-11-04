public class PacienteFrequencia
{
    public int IdProntuario { get; set; }
    public int QuantidadeAtendimentos { get; set; }
    public DateTime UltimoAtendimento { get; set; }
}

public class VeterinarioAtendimento
{
    public int IdVeterinario { get; set; }
    public int QuantidadeAtendimentos { get; set; }
    public DateTime UltimoAtendimento { get; set; }
}

public class StatusAgendamento
{
    public string Status { get; set; }
    public int Quantidade { get; set; }
    public double Percentual { get; set; }
}

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

    private void OrdenarPacienteFrequenciaDesc(List<PacienteFrequencia> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - 1 - i; j++)
            {
                if (lista[j].QuantidadeAtendimentos < lista[j + 1].QuantidadeAtendimentos)
                {
                    PacienteFrequencia temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }
    }

    private void OrdenarVeterinarioAtendimentoDesc(List<VeterinarioAtendimento> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - 1 - i; j++)
            {
                if (lista[j].QuantidadeAtendimentos < lista[j + 1].QuantidadeAtendimentos)
                {
                    VeterinarioAtendimento temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }
    }

    private void OrdenarStatusAgendamentoDesc(List<StatusAgendamento> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - 1 - i; j++)
            {
                if (lista[j].Quantidade < lista[j + 1].Quantidade)
                {
                    StatusAgendamento temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }
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
            tela.ExibirOpcao("3", "veterinarios - Atendimentos");
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
        
        dataFim = dataFim.AddDays(1).AddTicks(-1); 
        
        List<Agendamento> agendamentosPeriodo = new List<Agendamento>();
        for (int i = 0; i < agendamentos.Count; i++)
        {
            if (agendamentos[i].dataHora >= dataInicio && agendamentos[i].dataHora <= dataFim)
            {
                agendamentosPeriodo.Add(agendamentos[i]);
            }
        }
        
        OrdenarAgendamentosPorData(agendamentosPeriodo);
        
        Console.WriteLine($"\n=== RELATÓRIO DE AGENDAMENTOS ===");
        Console.WriteLine($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}");
        Console.WriteLine($"Total de Agendamentos: {agendamentosPeriodo.Count}");
        Console.WriteLine();
        
        if (agendamentosPeriodo.Count > 0)
        {
            string[] cabecalhos = { "Data", "Hora", "Paciente", "veterinario", "Procedimento", "Status" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < agendamentosPeriodo.Count; i++)
            {
                Agendamento agendamento = agendamentosPeriodo[i];
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
                    agendamento.dataHora.ToString("dd/MM/yyyy"),
                    agendamento.dataHora.ToString("HH:mm"),
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

    private void RelatorioPacientesMaisFrequentes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Relatório - Pacientes mais Frequentes");
        Console.WriteLine();
        
        Dictionary<int, int> contadorAtendimentos = new Dictionary<int, int>();
        Dictionary<int, DateTime> ultimoAtendimento = new Dictionary<int, DateTime>();
        
        for (int i = 0; i < historicos.Count; i++)
        {
            int idProntuario = historicos[i].idDoProntuario;
            
            if (contadorAtendimentos.ContainsKey(idProntuario))
            {
                contadorAtendimentos[idProntuario] = contadorAtendimentos[idProntuario] + 1;
            }
            else
            {
                contadorAtendimentos[idProntuario] = 1;
            }
            
            if (ultimoAtendimento.ContainsKey(idProntuario))
            {
                if (historicos[i].dataAtendimento > ultimoAtendimento[idProntuario])
                {
                    ultimoAtendimento[idProntuario] = historicos[i].dataAtendimento;
                }
            }
            else
            {
                ultimoAtendimento[idProntuario] = historicos[i].dataAtendimento;
            }
        }
        
        List<PacienteFrequencia> pacientesFrequencia = new List<PacienteFrequencia>();
        foreach (int idProntuario in contadorAtendimentos.Keys)
        {
            PacienteFrequencia pf = new PacienteFrequencia();
            pf.IdProntuario = idProntuario;
            pf.QuantidadeAtendimentos = contadorAtendimentos[idProntuario];
            pf.UltimoAtendimento = ultimoAtendimento[idProntuario];
            pacientesFrequencia.Add(pf);
        }
        
        OrdenarPacienteFrequenciaDesc(pacientesFrequencia);
        
        List<PacienteFrequencia> top10 = new List<PacienteFrequencia>();
        int limite = 10;
        if (pacientesFrequencia.Count < limite)
        {
            limite = pacientesFrequencia.Count;
        }
        
        for (int i = 0; i < limite; i++)
        {
            top10.Add(pacientesFrequencia[i]);
        }
        
        Console.WriteLine("=== TOP 10 PACIENTES MAIS FREQUENTES ===");
        Console.WriteLine();
        
        if (top10.Count == 0)
        {
            Console.WriteLine("Nenhum atendimento registrado.");
        }
        else
        {
            string[] cabecalhos = { "Posição", "Paciente", "Atendimentos", "Último Atendimento" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < top10.Count; i++)
            {
                PacienteFrequencia prontuario = top10[i];
                Paciente paciente = BuscarPacientePorId(prontuario.IdProntuario);
                
                string nomePaciente;
                if (paciente != null)
                {
                    nomePaciente = paciente.nome;
                }
                else
                {
                    nomePaciente = "N/A";
                }
                
                dados.Add(new string[] {
                    (i + 1).ToString(),
                    nomePaciente,
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
        
        Console.WriteLine("Relatório - veterinarios - Atendimentos");
        Console.WriteLine();
        
        Dictionary<int, int> contadorAtendimentos = new Dictionary<int, int>();
        Dictionary<int, DateTime> ultimoAtendimento = new Dictionary<int, DateTime>();
        
        for (int i = 0; i < historicos.Count; i++)
        {
            int idVeterinario = historicos[i].idDoVeterinario;
            
            if (contadorAtendimentos.ContainsKey(idVeterinario))
            {
                contadorAtendimentos[idVeterinario] = contadorAtendimentos[idVeterinario] + 1;
            }
            else
            {
                contadorAtendimentos[idVeterinario] = 1;
            }
            
            if (ultimoAtendimento.ContainsKey(idVeterinario))
            {
                if (historicos[i].dataAtendimento > ultimoAtendimento[idVeterinario])
                {
                    ultimoAtendimento[idVeterinario] = historicos[i].dataAtendimento;
                }
            }
            else
            {
                ultimoAtendimento[idVeterinario] = historicos[i].dataAtendimento;
            }
        }
        
        List<VeterinarioAtendimento> veterinariosAtendimentos = new List<VeterinarioAtendimento>();
        foreach (int idVeterinario in contadorAtendimentos.Keys)
        {
            VeterinarioAtendimento va = new VeterinarioAtendimento();
            va.IdVeterinario = idVeterinario;
            va.QuantidadeAtendimentos = contadorAtendimentos[idVeterinario];
            va.UltimoAtendimento = ultimoAtendimento[idVeterinario];
            veterinariosAtendimentos.Add(va);
        }
        
        OrdenarVeterinarioAtendimentoDesc(veterinariosAtendimentos);
        
        Console.WriteLine("=== ATENDIMENTOS POR veterinario ===");
        Console.WriteLine();
        
        if (veterinariosAtendimentos.Count == 0)
        {
            Console.WriteLine("Nenhum atendimento registrado.");
        }
        else
        {
            string[] cabecalhos = { "veterinario", "CRMV", "Especialidade", "Atendimentos", "Último Atendimento" };
            List<string[]> dados = new List<string[]>();
            
            for (int i = 0; i < veterinariosAtendimentos.Count; i++)
            {
                VeterinarioAtendimento vet = veterinariosAtendimentos[i];
                Veterinario veterinario = BuscarVeterinarioPorId(vet.IdVeterinario);
                
                string nomeVeterinario;
                if (veterinario != null)
                {
                    nomeVeterinario = veterinario.nome;
                }
                else
                {
                    nomeVeterinario = "N/A";
                }
                
                string crmvVeterinario;
                if (veterinario != null)
                {
                    crmvVeterinario = veterinario.crmv;
                }
                else
                {
                    crmvVeterinario = "N/A";
                }
                
                string especialidadeVeterinario;
                if (veterinario != null)
                {
                    especialidadeVeterinario = veterinario.especialidade;
                }
                else
                {
                    especialidadeVeterinario = "N/A";
                }
                
                dados.Add(new string[] {
                    nomeVeterinario,
                    crmvVeterinario,
                    especialidadeVeterinario,
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
        
        List<Medicamento> medicamentosBaixo = new List<Medicamento>();
        for (int i = 0; i < medicamentos.Count; i++)
        {
            if (medicamentos[i].estoqueMinimo > 0)
            {
                medicamentosBaixo.Add(medicamentos[i]);
            }
        }
        
        List<Vacina> vacinasBaixo = new List<Vacina>();
        for (int i = 0; i < vacinas.Count; i++)
        {
            vacinasBaixo.Add(vacinas[i]);
        }
        
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
            
            for (int i = 0; i < medicamentosBaixo.Count; i++)
            {
                Medicamento medicamento = medicamentosBaixo[i];
                
                string statusMedicamento;
                if (medicamento.controlado)
                {
                    statusMedicamento = "Controlado";
                }
                else
                {
                    statusMedicamento = "Livre";
                }
                
                dados.Add(new string[] {
                    "Medicamento",
                    medicamento.nome,
                    medicamento.estoqueMinimo.ToString(),
                    statusMedicamento
                });
            }
            
            for (int i = 0; i < vacinasBaixo.Count; i++)
            {
                Vacina vacina = vacinasBaixo[i];
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
        
        Dictionary<string, int> contadorStatus = new Dictionary<string, int>();
        
        for (int i = 0; i < agendamentos.Count; i++)
        {
            string status = agendamentos[i].statusDetalhado;
            
            if (contadorStatus.ContainsKey(status))
            {
                contadorStatus[status] = contadorStatus[status] + 1;
            }
            else
            {
                contadorStatus[status] = 1;
            }
        }
        
        List<StatusAgendamento> statusAgendamentos = new List<StatusAgendamento>();
        int totalAgendamentos = agendamentos.Count;
        
        foreach (string status in contadorStatus.Keys)
        {
            StatusAgendamento sa = new StatusAgendamento();
            sa.Status = status;
            sa.Quantidade = contadorStatus[status];
            
            if (totalAgendamentos > 0)
            {
                sa.Percentual = (double)sa.Quantidade / totalAgendamentos * 100;
            }
            else
            {
                sa.Percentual = 0;
            }
            
            statusAgendamentos.Add(sa);
        }
        
        OrdenarStatusAgendamentoDesc(statusAgendamentos);
        
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
            
            for (int i = 0; i < statusAgendamentos.Count; i++)
            {
                StatusAgendamento status = statusAgendamentos[i];
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
