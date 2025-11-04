public class HistoricoClinicoCRUD
{
    private List<HistoricoClinico> historicos;
    private HistoricoClinico historico;
    private int indice;
    private ProntuarioCRUD prontuarioCRUD;
    private AgendamentoCRUD agendamentoCRUD;
    private VeterinarioCRUD veterinarioCRUD;

    public HistoricoClinicoCRUD()
    {
        this.historicos = new List<HistoricoClinico>();
        this.historico = new HistoricoClinico();
        this.indice = -1;
        this.prontuarioCRUD = null;
        this.agendamentoCRUD = null;
        this.veterinarioCRUD = null;
    }

    public void SetProntuarioCRUD(ProntuarioCRUD prontuarioCRUD)
    {
        this.prontuarioCRUD = prontuarioCRUD;
    }

    public void SetAgendamentoCRUD(AgendamentoCRUD agendamentoCRUD)
    {
        this.agendamentoCRUD = agendamentoCRUD;
    }

    public void SetVeterinarioCRUD(VeterinarioCRUD veterinarioCRUD)
    {
        this.veterinarioCRUD = veterinarioCRUD;
    }

    public List<HistoricoClinico> GetHistoricos()
    {
        return this.historicos;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Histórico Clínico");
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
        return this.historicos.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Histórico Clínico");
        Console.WriteLine();
        
        if (this.historicos.Count == 0)
        {
            Console.WriteLine("Nenhum histórico cadastrado.");
        }
        else
        {
            string[] cabecalhos = { "ID", "Prontuário", "Agendamento", "Veterinário", "Data", "Diagnóstico" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var historico in this.historicos)
            {
                string diagnosticoTruncado;
                if (historico.diagnostico.Length > 30)
                {
                    diagnosticoTruncado = historico.diagnostico.Substring(0, 30) + "...";
                }
                else
                {
                    diagnosticoTruncado = historico.diagnostico;
                }
                
                dados.Add(new string[] {
                    historico.id.ToString(),
                    historico.idDoProntuario.ToString(),
                    historico.idDoAgendamento.ToString(),
                    historico.idDoVeterinario.ToString(),
                    historico.dataAtendimento.ToString("dd/MM/yyyy"),
                    diagnosticoTruncado
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        tela.Pausar();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Histórico Clínico");
        Console.WriteLine();
        
        // valida se tem prontuários
        if (prontuarioCRUD == null || prontuarioCRUD.GetProntuarios().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar histórico clínico: nenhum prontuário cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um prontuário antes de criar histórico clínico.");
            tela.Pausar();
            return;
        }
        
        // valida se tem agendamentos
        if (agendamentoCRUD == null || agendamentoCRUD.GetAgendamentos().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar histórico clínico: nenhum agendamento cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um agendamento antes de criar histórico clínico.");
            tela.Pausar();
            return;
        }
        
        // valida se tem veterinários
        if (veterinarioCRUD == null || veterinarioCRUD.GetVeterinarios().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar histórico clínico: nenhum veterinário cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um veterinário antes de criar histórico clínico.");
            tela.Pausar();
            return;
        }
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        if (tela.ConfirmarAcao("\nConfirma o cadastro (S/N)? "))
        {
            this.historicos.Add(new HistoricoClinico(novoId, this.historico.idDoProntuario, this.historico.idDoAgendamento, 
                this.historico.idDoVeterinario, this.historico.dataAtendimento, this.historico.diagnostico, 
                this.historico.tratamento, this.historico.observacoes, this.historico.medicamentosAplicados, 
                this.historico.vacinasAplicadas, this.historico.pesoAtual, this.historico.temperatura, 
                this.historico.frequenciaCardiaca));
            tela.ExibirSucesso("Histórico clínico cadastrado com sucesso!");
        }
        
        tela.Pausar();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Histórico Clínico");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do histórico a alterar: ");
        if (!int.TryParse(idInput, out this.historico.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Histórico não encontrado.");
        }
        else
        {
            Console.WriteLine("\nDados atuais:");
            this.MostrarDados();
            
            Console.WriteLine("Novos dados:");
            this.EntrarDados();
            
            if (tela.ConfirmarAcao("\nConfirma a alteração (S/N)? "))
            {
                this.historicos[this.indice] = new HistoricoClinico(this.historico.id, this.historico.idDoProntuario, 
                    this.historico.idDoAgendamento, this.historico.idDoVeterinario, this.historico.dataAtendimento, 
                    this.historico.diagnostico, this.historico.tratamento, this.historico.observacoes, 
                    this.historico.medicamentosAplicados, this.historico.vacinasAplicadas, this.historico.pesoAtual, 
                    this.historico.temperatura, this.historico.frequenciaCardiaca);
                tela.ExibirSucesso("Histórico clínico alterado com sucesso!");
            }
        }
        
        tela.Pausar();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Histórico Clínico");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do histórico a excluir: ");
        if (!int.TryParse(idInput, out this.historico.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Histórico não encontrado.");
        }
        else
        {
            Console.WriteLine("\nHistórico a ser excluído:");
            this.MostrarDados();
            
            if (tela.ConfirmarAcao("Confirma a exclusão (S/N)? "))
            {
                this.historicos.RemoveAt(this.indice);
                tela.ExibirSucesso("Histórico clínico excluído com sucesso!");
            }
        }
        
        tela.Pausar();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Histórico Clínico");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do histórico: ");
        if (!int.TryParse(idInput, out this.historico.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Histórico não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO HISTÓRICO CLÍNICO ===");
            this.MostrarDados();
        }
        
        tela.Pausar();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        // valida id do prontuário
        while (true)
        {
            string idProntuarioInput = tela.Perguntar("ID do Prontuário: ");
            if (!int.TryParse(idProntuarioInput, out this.historico.idDoProntuario))
            {
                tela.ExibirErro("ID do Prontuário inválido! Digite um número.");
                continue;
            }
            
            // verifica se o prontuário existe
            if (prontuarioCRUD != null)
            {
                bool prontuarioExiste = false;
                List<Prontuario> listaProntuarios = prontuarioCRUD.GetProntuarios();
                for (int i = 0; i < listaProntuarios.Count; i++)
                {
                    if (listaProntuarios[i].id == this.historico.idDoProntuario)
                    {
                        prontuarioExiste = true;
                        break;
                    }
                }
                if (!prontuarioExiste)
                {
                    tela.ExibirErro($"Prontuário com ID {this.historico.idDoProntuario} não encontrado!");
                    
                    // mostra os prontuários que tem
                    Console.WriteLine("\nProntuários disponíveis:");
                    if (prontuarioCRUD.GetProntuarios().Count > 0)
                    {
                        string[] cabecalhos = { "ID", "ID Paciente", "Data Abertura", "Status" };
                        List<string[]> dados = new List<string[]>();
                        
                        foreach (var prontuario in prontuarioCRUD.GetProntuarios())
                        {
                            string statusProntuario;
                            if (prontuario.ativo)
                            {
                                statusProntuario = "Ativo";
                            }
                            else
                            {
                                statusProntuario = "Inativo";
                            }
                            
                            dados.Add(new string[] {
                                prontuario.id.ToString(),
                                prontuario.idDoPaciente.ToString(),
                                prontuario.dataAbertura.ToString("dd/MM/yyyy"),
                                statusProntuario
                            });
                        }
                        
                        tela.ExibirTabela(cabecalhos, dados);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum prontuário cadastrado.");
                    }
                    Console.WriteLine();
                    continue;
                }
            }
            break;
        }
        
        // valida id do agendamento
        while (true)
        {
            string idAgendamentoInput = tela.Perguntar("ID do Agendamento: ");
            if (!int.TryParse(idAgendamentoInput, out this.historico.idDoAgendamento))
            {
                tela.ExibirErro("ID do Agendamento inválido! Digite um número.");
                continue;
            }
            
            // verifica se o agendamento existe
            if (agendamentoCRUD != null)
            {
                bool agendamentoExiste = agendamentoCRUD.ExisteAgendamento(this.historico.idDoAgendamento);
                if (!agendamentoExiste)
                {
                    tela.ExibirErro($"Agendamento com ID {this.historico.idDoAgendamento} não encontrado!");
                    
                    // mostra os agendamentos que tem
                    Console.WriteLine("\nAgendamentos disponíveis:");
                    if (agendamentoCRUD.GetAgendamentos().Count > 0)
                    {
                        string[] cabecalhos = { "ID", "Paciente", "Veterinário", "Data", "Procedimento" };
                        List<string[]> dados = new List<string[]>();
                        
                        foreach (var agendamento in agendamentoCRUD.GetAgendamentos())
                        {
                            dados.Add(new string[] {
                                agendamento.id.ToString(),
                                agendamento.idDoPaciente.ToString(),
                                agendamento.idDoVeterinario.ToString(),
                                agendamento.dataHora.ToString("dd/MM/yyyy HH:mm"),
                                agendamento.tipoProcedimento
                            });
                        }
                        
                        tela.ExibirTabela(cabecalhos, dados);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum agendamento cadastrado.");
                    }
                    Console.WriteLine();
                    continue;
                }
            }
            break;
        }
        
        // valida id do veterinário
        while (true)
        {
            string idVeterinarioInput = tela.Perguntar("ID do Veterinário: ");
            if (!int.TryParse(idVeterinarioInput, out this.historico.idDoVeterinario))
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
                    if (listaVeterinarios[i].id == this.historico.idDoVeterinario)
                    {
                        veterinarioExiste = true;
                        break;
                    }
                }
                if (!veterinarioExiste)
                {
                    tela.ExibirErro($"Veterinário com ID {this.historico.idDoVeterinario} não encontrado!");
                    
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
                    }
                    Console.WriteLine();
                    continue;
                }
            }
            break;
        }
        
        string dataInput = tela.Perguntar("Data do Atendimento (dd/mm/aaaa) ou Enter para hoje: ");
        if (string.IsNullOrWhiteSpace(dataInput))
        {
            this.historico.dataAtendimento = DateTime.Now;
        }
        else if (!DateTime.TryParse(dataInput, out this.historico.dataAtendimento))
        {
            tela.ExibirErro("Data inválida! Usando data atual.");
            this.historico.dataAtendimento = DateTime.Now;
        }
        
        this.historico.diagnostico = tela.Perguntar("Diagnóstico: ");
        this.historico.tratamento = tela.Perguntar("Tratamento: ");
        this.historico.observacoes = tela.Perguntar("Observações: ");
        this.historico.medicamentosAplicados = tela.Perguntar("Medicamentos Aplicados: ");
        this.historico.vacinasAplicadas = tela.Perguntar("Vacinas Aplicadas: ");
        
        string pesoInput = tela.Perguntar("Peso Atual (kg): ");
        if (!double.TryParse(pesoInput, out this.historico.pesoAtual))
        {
            this.historico.pesoAtual = 0;
        }
        
        this.historico.temperatura = tela.Perguntar("Temperatura: ");
        this.historico.frequenciaCardiaca = tela.Perguntar("Frequência Cardíaca: ");
    }

    public bool ProcurarCodigo()
    {
        this.indice = -1;
        bool encontrei = false;
        for (int i = 0; i < this.historicos.Count; i++)
        {
            if (this.historico.id == this.historicos[i].id)
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
        if (this.indice < 0 || this.indice >= this.historicos.Count)
        {
            Console.WriteLine("Erro: índice inválido.");
            return;
        }
        
        Console.WriteLine("Histórico encontrado:");
        Console.WriteLine($"ID: {this.historicos[this.indice].id}");
        Console.WriteLine($"ID do Prontuário: {this.historicos[this.indice].idDoProntuario}");
        Console.WriteLine($"ID do Agendamento: {this.historicos[this.indice].idDoAgendamento}");
        Console.WriteLine($"ID do Veterinário: {this.historicos[this.indice].idDoVeterinario}");
        Console.WriteLine($"Data do Atendimento: {this.historicos[this.indice].dataAtendimento:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Diagnóstico: {this.historicos[this.indice].diagnostico}");
        Console.WriteLine($"Tratamento: {this.historicos[this.indice].tratamento}");
        Console.WriteLine($"Observações: {this.historicos[this.indice].observacoes}");
        Console.WriteLine($"Medicamentos Aplicados: {this.historicos[this.indice].medicamentosAplicados}");
        Console.WriteLine($"Vacinas Aplicadas: {this.historicos[this.indice].vacinasAplicadas}");
        Console.WriteLine($"Peso Atual: {this.historicos[this.indice].pesoAtual} kg");
        Console.WriteLine($"Temperatura: {this.historicos[this.indice].temperatura}");
        Console.WriteLine($"Frequência Cardíaca: {this.historicos[this.indice].frequenciaCardiaca}");
        Console.WriteLine();
    }

    private void OrdenarHistoricoPorData(List<HistoricoClinico> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - 1 - i; j++)
            {
                if (lista[j].dataAtendimento > lista[j + 1].dataAtendimento)
                {
                    HistoricoClinico temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }
    }

    public List<HistoricoClinico> BuscarPorProntuario(int idProntuario)
    {
        List<HistoricoClinico> resultado = new List<HistoricoClinico>();
        for (int i = 0; i < this.historicos.Count; i++)
        {
            if (this.historicos[i].idDoProntuario == idProntuario)
            {
                resultado.Add(this.historicos[i]);
            }
        }
        
        OrdenarHistoricoPorData(resultado);
        return resultado;
    }

    public List<HistoricoClinico> BuscarPorVeterinario(int idVeterinario)
    {
        List<HistoricoClinico> resultado = new List<HistoricoClinico>();
        for (int i = 0; i < this.historicos.Count; i++)
        {
            if (this.historicos[i].idDoVeterinario == idVeterinario)
            {
                resultado.Add(this.historicos[i]);
            }
        }
        
        OrdenarHistoricoPorData(resultado);
        return resultado;
    }

    public void CriarEntradaAtendimento(int idProntuario, int idAgendamento, int idVeterinario, string diagnostico, string tratamento, string observacoes, string medicamentos, string vacinas, double peso, string temperatura, string frequencia)
    {
        int novoId = this.GerarNovoId();
        this.historicos.Add(new HistoricoClinico(novoId, idProntuario, idAgendamento, idVeterinario, DateTime.Now, 
            diagnostico, tratamento, observacoes, medicamentos, vacinas, peso, temperatura, frequencia));
    }
}
