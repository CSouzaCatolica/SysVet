Tela tela = new Tela();

// Inicializar CRUDs
TutorCRUD tutorCRUD = new TutorCRUD();
PacienteCRUD pacienteCRUD = new PacienteCRUD();
VeterinarioCRUD veterinarioCRUD = new VeterinarioCRUD();
UsuarioCRUD usuarioCRUD = new UsuarioCRUD();
AgendamentoCRUD agendamentoCRUD = new AgendamentoCRUD();
MedicamentoCRUD medicamentoCRUD = new MedicamentoCRUD();
VacinaCRUD vacinaCRUD = new VacinaCRUD();
ProntuarioCRUD prontuarioCRUD = new ProntuarioCRUD();
HistoricoClinicoCRUD historicoClinicoCRUD = new HistoricoClinicoCRUD();

// Configurar referências entre CRUDs
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

// Inicializar dados hardcoded
InicializarDados();

void InicializarDados()
{
    // Verificar se já existem dados para evitar duplicatas
    bool jaExistemDados = usuarioCRUD.GetUsuarios().Count > 0 || 
                          veterinarioCRUD.GetVeterinarios().Count > 0 ||
                          tutorCRUD.GetTutores().Count > 0 ||
                          pacienteCRUD.GetPacientes().Count > 0;
    
    if (jaExistemDados)
    {
        return; // Dados já existem, não inicializar novamente
    }
    
    // Criar 1 Usuário para Veterinário
    usuarioCRUD.GetUsuarios().Add(new Usuario(1, "Dr. João Silva", "joao.silva", "123456", "Veterinário"));
    
    // Criar 1 Veterinário
    veterinarioCRUD.GetVeterinarios().Add(new Veterinario(1, 1, "Dr. João Silva", "CRMV-12345", "Clínica Geral"));
    
    // Criar 2 Tutores
    tutorCRUD.GetTutores().Add(new Tutor(1, "Maria Santos", "123.456.789-00", "(11) 98765-4321", "maria@email.com", "Rua A, 123"));
    tutorCRUD.GetTutores().Add(new Tutor(2, "Pedro Costa", "987.654.321-00", "(11) 91234-5678", "pedro@email.com", "Rua B, 456"));
    
    // Criar 2 Pacientes
    pacienteCRUD.GetPacientes().Add(new Paciente(1, 1, "Rex", "Cão", "Labrador", 25.5, "Ativo"));
    pacienteCRUD.GetPacientes().Add(new Paciente(2, 2, "Luna", "Gato", "Persa", 4.2, "Ativo"));
    
    // Criar prontuários automáticos para os pacientes
    prontuarioCRUD.CriarAutomatico(1);
    prontuarioCRUD.CriarAutomatico(2);
}

while (true)
{
    tela.LimparTela();
    
    tela.ExibirTitulo("Sistema Veterinario");
    tela.MostrarMensagem("Selecione para Entrar:");
    
    tela.ExibirOpcao("1", "Recepcionista");
    tela.ExibirOpcao("2", "Veterinário");
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

// Função InicializarDados() removida - dados não são mais recriados automaticamente

void RegistrarAtendimento()
{
    Tela tela = new Tela();
    tela.LimparTela();
    
    Console.WriteLine("Registrar Atendimento");
    Console.WriteLine();
    
    // Listar agendamentos do dia
    DateTime hoje = DateTime.Today;
    DateTime amanha = hoje.AddDays(1);
    
    var agendamentosHoje = agendamentoCRUD.GetAgendamentos()
        .Where(a => a.dataHora >= hoje && a.dataHora < amanha && a.statusDetalhado != "Atendido" && a.statusDetalhado != "Cancelado")
        .OrderBy(a => a.dataHora)
        .ToList();
    
    Console.WriteLine("Agendamentos de Hoje:");
    Console.WriteLine();
    
    string[] cabecalhos = { "ID", "Hora", "Paciente", "Procedimento" };
    List<string[]> dados = new List<string[]>();
    
    foreach (var agendamento in agendamentosHoje)
    {
        var pacienteInfo = pacienteCRUD.GetPacientes().FirstOrDefault(p => p.id == agendamento.idDoPaciente);
        dados.Add(new string[] {
            agendamento.id.ToString(),
            agendamento.dataHora.ToString("HH:mm"),
            pacienteInfo?.nome ?? "N/A",
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
    
    // Buscar agendamento na lista completa
    var agendamentoSelecionado = agendamentoCRUD.GetAgendamentos().FirstOrDefault(a => a.id == idAgendamento);
    if (agendamentoSelecionado == null)
    {
        tela.ExibirErro("Agendamento não encontrado!");
        tela.Pausar();
        return;
    }
    
    // Validar se o paciente ainda existe
    var paciente = pacienteCRUD.GetPacientes().FirstOrDefault(p => p.id == agendamentoSelecionado.idDoPaciente);
    if (paciente == null)
    {
        tela.ExibirErro($"Paciente com ID {agendamentoSelecionado.idDoPaciente} foi excluído!");
        tela.ExibirAviso("Não é possível registrar atendimento para um paciente inexistente.");
        tela.Pausar();
        return;
    }
    
    // Validar se o veterinário ainda existe
    var veterinario = veterinarioCRUD.GetVeterinarios().FirstOrDefault(v => v.id == agendamentoSelecionado.idDoVeterinario);
    if (veterinario == null)
    {
        tela.ExibirErro($"Veterinário com ID {agendamentoSelecionado.idDoVeterinario} foi excluído!");
        tela.ExibirAviso("Não é possível registrar atendimento sem um veterinário válido.");
        tela.Pausar();
        return;
    }
    
    // Obter prontuário do paciente
    var prontuario = prontuarioCRUD.BuscarPorPaciente(agendamentoSelecionado.idDoPaciente);
    if (prontuario == null)
    {
        tela.ExibirErro("Prontuário não encontrado para este paciente!");
        tela.Pausar();
        return;
    }
    
    Console.WriteLine("\n=== DADOS DO ATENDIMENTO ===");
    Console.WriteLine($"Paciente: {paciente.nome}");
    Console.WriteLine($"Procedimento: {agendamentoSelecionado.tipoProcedimento}");
    Console.WriteLine();
    
    // Coletar dados do atendimento
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
        // Criar entrada no histórico clínico
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
        
        // Atualizar status do agendamento
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
        tela.ExibirTitulo("Menu Veterinário");
        
        tela.ExibirOpcao("1", "Consultar Agendamentos");
        tela.ExibirOpcao("2", "Prontuários");
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
        if (opcao == '4') agendaCRUD.ExecutarCRUD("Veterinário");
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
        tela.ExibirOpcao("3", "Veterinários");
        tela.ExibirOpcao("4", "Usuários");
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
