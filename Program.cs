Tela tela = new Tela();

// inicizliza crds
TutorCRUD tutorCRUD = new TutorCRUD();
PacienteCRUD pacienteCRUD = new PacienteCRUD();
VeterinarioCRUD veterinarioCRUD = new VeterinarioCRUD();
UsuarioCRUD usuarioCRUD = new UsuarioCRUD();
AgendamentoCRUD agendamentoCRUD = new AgendamentoCRUD();
MedicamentoCRUD medicamentoCRUD = new MedicamentoCRUD();
VacinaCRUD vacinaCRUD = new VacinaCRUD();
ProntuarioCRUD prontuarioCRUD = new ProntuarioCRUD();
HistoricoClinicoCRUD historicoClinicoCRUD = new HistoricoClinicoCRUD();

// liga os cruds um com o outro pra ter uma exibicao melhor nos detalhes
tutorCRUD.SetPacienteCRUD(pacienteCRUD);
pacienteCRUD.SetTutorCRUD(tutorCRUD);
pacienteCRUD.SetProntuarioCRUD(prontuarioCRUD);
agendamentoCRUD.SetPacienteCRUD(pacienteCRUD);
agendamentoCRUD.SetVeterinarioCRUD(veterinarioCRUD);
prontuarioCRUD.SetHistoricoClinicoCRUD(historicoClinicoCRUD);
prontuarioCRUD.SetPacienteCRUD(pacienteCRUD);
veterinarioCRUD.SetUsuarioCRUD(usuarioCRUD);
usuarioCRUD.SetVeterinarioCRUD(veterinarioCRUD);
historicoClinicoCRUD.SetProntuarioCRUD(prontuarioCRUD);
historicoClinicoCRUD.SetAgendamentoCRUD(agendamentoCRUD);
historicoClinicoCRUD.SetVeterinarioCRUD(veterinarioCRUD);
veterinarioCRUD.SetHistoricoClinicoCRUD(historicoClinicoCRUD);

AgendaCRUD agendaCRUD = new AgendaCRUD(agendamentoCRUD.GetAgendamentos(), veterinarioCRUD.GetVeterinarios(), pacienteCRUD.GetPacientes());
RelatoriosCRUD relatoriosCRUD = new RelatoriosCRUD(agendamentoCRUD.GetAgendamentos(), pacienteCRUD.GetPacientes(), 
    veterinarioCRUD.GetVeterinarios(), historicoClinicoCRUD.GetHistoricos(), medicamentoCRUD.GetMedicamentos(), vacinaCRUD.GetVacinas());

// Seed inicial pra gente testar os dados
InicializarDados();

void InicializarDados()
{
    bool jaExistemDados = usuarioCRUD.GetUsuarios().Count > 0 || 
                          veterinarioCRUD.GetVeterinarios().Count > 0 ||
                          tutorCRUD.GetTutores().Count > 0 ||
                          pacienteCRUD.GetPacientes().Count > 0;
    
    if (jaExistemDados)
    {
        return;
    }
    
    usuarioCRUD.GetUsuarios().Add(new Usuario(1, "Dr. João Silva", "joao.silva", "123456", "veterinario"));
    veterinarioCRUD.GetVeterinarios().Add(new Veterinario(1, 1, "Dr. João Silva", "CRMV-12345", "Clínica Geral"));
    tutorCRUD.GetTutores().Add(new Tutor(1, "Maria Santos", "123.456.789-00", "(11) 98765-4321", "maria@email.com", "Rua A, 123"));
    tutorCRUD.GetTutores().Add(new Tutor(2, "Pedro Costa", "987.654.321-00", "(11) 91234-5678", "pedro@email.com", "Rua B, 456"));
    pacienteCRUD.GetPacientes().Add(new Paciente(1, 1, "Rex", "Cão", "Labrador", 25.5, "Ativo"));
    pacienteCRUD.GetPacientes().Add(new Paciente(2, 2, "Luna", "Gato", "Persa", 4.2, "Ativo"));
    prontuarioCRUD.CriarAutomatico(1);
    prontuarioCRUD.CriarAutomatico(2);
}

while (true)
{
    tela.LimparTela();
    
    tela.ExibirTitulo("Sistema Veterinario");
    tela.MostrarMensagem("Selecione para Entrar:");
    
    tela.ExibirOpcao("1", "Recepcionista");
    tela.ExibirOpcao("2", "veterinario");
    tela.ExibirOpcao("3", "Gerente");
    tela.ExibirOpcao("0", "Sair");
    tela.ExibirLinhaVazia();
    
    List<char> opcoesValidas = new List<char> { '1', '2', '3', '0' };
    char opcaoSelecionada = tela.LerOpcaoValida(opcoesValidas);
    
    if (opcaoSelecionada == '0') break;
    
    if (opcaoSelecionada == '1')
    {
        MenuRecepcionista();
    }
    else if (opcaoSelecionada == '2')
    {
        MenuVeterinario();
    }
    else if (opcaoSelecionada == '3')
    {
        MenuGerente();
    }
}


Paciente BuscarPacientePorIdProgram(int id)
{
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

Agendamento BuscarAgendamentoPorIdProgram(int id)
{
    List<Agendamento> listaAgendamentos = agendamentoCRUD.GetAgendamentos();
    for (int i = 0; i < listaAgendamentos.Count; i++)
    {
        if (listaAgendamentos[i].id == id)
        {
            return listaAgendamentos[i];
        }
    }
    return null;
}

Veterinario BuscarVeterinarioPorIdProgram(int id)
{
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

void OrdenarAgendamentosPorDataProgram(List<Agendamento> lista)
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

void RegistrarAtendimento()
{
    Tela tela = new Tela();
    tela.LimparTela();
    
    Console.WriteLine("Registrar Atendimento");
    Console.WriteLine();
    
    DateTime hoje = DateTime.Today;
    DateTime amanha = hoje.AddDays(1);
    
    List<Agendamento> todosAgendamentos = agendamentoCRUD.GetAgendamentos();
    List<Agendamento> agendamentosHoje = new List<Agendamento>();
    for (int i = 0; i < todosAgendamentos.Count; i++)
    {
        bool dentroDoPeriodo = todosAgendamentos[i].dataHora >= hoje && todosAgendamentos[i].dataHora < amanha;
        bool statusValido = true;
        if (todosAgendamentos[i].statusDetalhado == "Atendido")
        {
            statusValido = false;
        }
        else if (todosAgendamentos[i].statusDetalhado == "Cancelado")
        {
            statusValido = false;
        }
        
        if (dentroDoPeriodo && statusValido)
        {
            agendamentosHoje.Add(todosAgendamentos[i]);
        }
    }
    
    OrdenarAgendamentosPorDataProgram(agendamentosHoje);
    
    Console.WriteLine("Agendamentos de Hoje:");
    Console.WriteLine();
    
    string[] cabecalhos = { "ID", "Hora", "Paciente", "Procedimento" };
    List<string[]> dados = new List<string[]>();
    
    for (int i = 0; i < agendamentosHoje.Count; i++)
    {
        Agendamento agendamento = agendamentosHoje[i];
        Paciente pacienteInfo = BuscarPacientePorIdProgram(agendamento.idDoPaciente);
        
        string nomePaciente;
        if (pacienteInfo != null)
        {
            nomePaciente = pacienteInfo.nome;
        }
        else
        {
            nomePaciente = "N/A";
        }
        
        dados.Add(new string[] {
            agendamento.id.ToString(),
            agendamento.dataHora.ToString("HH:mm"),
            nomePaciente,
            agendamento.tipoProcedimento
        });
    }
    
    tela.ExibirTabela(cabecalhos, dados);
    
    string idAgendamentoInput = tela.Perguntar("\nDigite o ID do agendamento a atender: ");
    if (!int.TryParse(idAgendamentoInput, out int idAgendamento))
    {
        tela.ExibirErro("ID inválido!");
        tela.Pausar();
        return;
    }
    
    // busca agendamento na lista completa
    Agendamento agendamentoSelecionado = BuscarAgendamentoPorIdProgram(idAgendamento);
    if (agendamentoSelecionado == null)
    {
        tela.ExibirErro("Agendamento não encontrado!");
        tela.Pausar();
        return;
    }
    
    // valida se o paciente ainda existe
    Paciente paciente = BuscarPacientePorIdProgram(agendamentoSelecionado.idDoPaciente);
    if (paciente == null)
    {
        tela.ExibirErro($"Paciente com ID {agendamentoSelecionado.idDoPaciente} foi excluído!");
        tela.ExibirAviso("Não é possível registrar atendimento para um paciente inexistente.");
        tela.Pausar();
        return;
    }
    
    // valida se o veterinario ainda existe
    Veterinario veterinario = BuscarVeterinarioPorIdProgram(agendamentoSelecionado.idDoVeterinario);
    if (veterinario == null)
    {
        tela.ExibirErro($"veterinario com ID {agendamentoSelecionado.idDoVeterinario} foi excluído!");
        tela.ExibirAviso("Não é possível registrar atendimento sem um veterinario válido.");
        tela.Pausar();
        return;
    }
    
    // pega prontuario do paciente
    var prontuario = prontuarioCRUD.BuscarPorPaciente(agendamentoSelecionado.idDoPaciente);
    if (prontuario == null)
    {
        tela.ExibirErro("Prontuario não encontrado para este paciente!");
        tela.Pausar();
        return;
    }
    
    Console.WriteLine("\n=== DADOS DO ATENDIMENTO ===");
    Console.WriteLine($"Paciente: {paciente.nome}");
    Console.WriteLine($"Procedimento: {agendamentoSelecionado.tipoProcedimento}");
    Console.WriteLine();
    
    // pega dados do atendimento
    string diagnostico = tela.Perguntar("Diagnóstico: ");
    string tratamento = tela.Perguntar("Tratamento realizado: ");
    string observacoes = tela.Perguntar("Observações: ");
    string medicamentos = tela.Perguntar("Medicamentos aplicados: ");
    string vacinas = tela.Perguntar("Vacinas aplicadas: ");
    
    string pesoInput = tela.Perguntar("Peso atual (kg): ");
    if (!double.TryParse(pesoInput, out double peso))
    {
        peso = 0;
    }
    
    string temperatura = tela.Perguntar("Temperatura: ");
    string frequencia = tela.Perguntar("Frequência cardíaca: ");
    
    if (tela.ConfirmarAcao("\nConfirma o registro do atendimento (S/N)? "))
    {
        // entrada no histórico
        historicoClinicoCRUD.CriarEntradaAtendimento(
            prontuario.id, 
            agendamentoSelecionado.id, 
            agendamentoSelecionado.idDoVeterinario,
            diagnostico, 
            tratamento, 
            observacoes, 
            medicamentos, 
            vacinas, 
            peso, 
            temperatura, 
            frequencia
        );
        
        // atualiza  agendamento
        agendamentoSelecionado.statusDetalhado = "Atendido";
        agendamentoSelecionado.status = "Concluído";
        
        tela.ExibirSucesso("Atendimento registrado com sucesso!");
    }
    
    tela.Pausar();
}

void MenuRecepcionista()
{
    char opcao;
    List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '0' };

    while (true)
    {
        tela.LimparTela();
        tela.ExibirTitulo("Menu Recepcionista");
        
        tela.ExibirOpcao("1", "Tutores");
        tela.ExibirOpcao("2", "Pacientes");
        tela.ExibirOpcao("3", "Agendamentos");
        tela.ExibirOpcao("4", "Agenda Médicos");
        tela.ExibirOpcao("0", "Voltar");
        tela.ExibirLinhaVazia();
        
        opcao = tela.LerOpcaoValida(opcoesValidas);

        if (opcao == '0') break;
        if (opcao == '1') tutorCRUD.ExecutarCRUD();
        if (opcao == '2') pacienteCRUD.ExecutarCRUD();
        if (opcao == '3') agendamentoCRUD.ExecutarCRUD();
        if (opcao == '4') agendaCRUD.ExecutarCRUD("Recepcionista");
    }
}

void MenuVeterinario()
{
    char opcao;
    List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

    while (true)
    {
        tela.LimparTela();
        tela.ExibirTitulo("Menu veterinario");
        
        tela.ExibirOpcao("1", "Consultar Agendamentos");
        tela.ExibirOpcao("2", "Prontuarios");
        tela.ExibirOpcao("3", "Registrar Atendimento");
        tela.ExibirOpcao("4", "Minha Agenda");
        tela.ExibirOpcao("5", "Estoque");
        tela.ExibirOpcao("0", "Voltar");
        tela.ExibirLinhaVazia();
        
        opcao = tela.LerOpcaoValida(opcoesValidas);

        if (opcao == '0') break;
        if (opcao == '1') agendamentoCRUD.ExecutarCRUD();
        if (opcao == '2') prontuarioCRUD.ExecutarCRUD();
        if (opcao == '3') RegistrarAtendimento();
        if (opcao == '4') agendaCRUD.ExecutarCRUD("veterinario");
        if (opcao == '5') MenuEstoque();
    }
}

void MenuEstoque()
{
    char opcao;
    List<char> opcoesValidas = new List<char> { '1', '2', '0' };

    while (true)
    {
        tela.LimparTela();
        tela.ExibirTitulo("Menu Estoque");
        
        tela.ExibirOpcao("1", "Medicamentos");
        tela.ExibirOpcao("2", "Vacinas");
        tela.ExibirOpcao("0", "Voltar");
        tela.ExibirLinhaVazia();
        
        opcao = tela.LerOpcaoValida(opcoesValidas);

        if (opcao == '0') break;
        if (opcao == '1') medicamentoCRUD.ExecutarCRUD();
        if (opcao == '2') vacinaCRUD.ExecutarCRUD();
    }
}

void MenuGerente()
{
    char opcao;
    List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '6', '7', '0' };

    while (true)
    {
        tela.LimparTela();
        tela.ExibirTitulo("Menu Gerente");
        
        tela.ExibirOpcao("1", "Tutores");
        tela.ExibirOpcao("2", "Pacientes");
        tela.ExibirOpcao("3", "veterinarios");
        tela.ExibirOpcao("4", "usuarios");
        tela.ExibirOpcao("5", "Estoque");
        tela.ExibirOpcao("6", "Relatórios");
        tela.ExibirOpcao("7", "Agenda");
        tela.ExibirOpcao("0", "Voltar");
        tela.ExibirLinhaVazia();
        
        opcao = tela.LerOpcaoValida(opcoesValidas);

        if (opcao == '0') break;
        if (opcao == '1') tutorCRUD.ExecutarCRUD();
        if (opcao == '2') pacienteCRUD.ExecutarCRUD();
        if (opcao == '3') veterinarioCRUD.ExecutarCRUD();
        if (opcao == '4') usuarioCRUD.ExecutarCRUD();
        if (opcao == '5') MenuEstoque();
        if (opcao == '6') relatoriosCRUD.ExecutarCRUD();
        if (opcao == '7') agendaCRUD.ExecutarCRUD("Gerente");
    }
}
