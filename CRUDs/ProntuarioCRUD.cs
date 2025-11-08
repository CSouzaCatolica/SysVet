public class ProntuarioCRUD
{
    private List<Prontuario> prontuarios;
    private Prontuario prontuario;
    private int indice;
    private HistoricoClinicoCRUD historicoClinicoCRUD;
    private PacienteCRUD pacienteCRUD;

    public ProntuarioCRUD()
    {
        this.prontuarios = new List<Prontuario>();
        this.prontuario = new Prontuario();
        this.indice = -1;
        this.historicoClinicoCRUD = null;
        this.pacienteCRUD = null;
    }

    public void SetHistoricoClinicoCRUD(HistoricoClinicoCRUD historicoClinicoCRUD)
    {
        this.historicoClinicoCRUD = historicoClinicoCRUD;
    }

    public void SetPacienteCRUD(PacienteCRUD pacienteCRUD)
    {
        this.pacienteCRUD = pacienteCRUD;
    }

    public List<Prontuario> GetProntuarios()
    {
        return this.prontuarios;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Prontuarios");
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
        return this.prontuarios.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Prontuarios");
        Console.WriteLine();
        
        if (this.prontuarios.Count == 0)
        {
            Console.WriteLine("Nenhum prontuario cadastrado.");
        }
        else
        {
            string[] cabecalhos = { "ID", "ID Paciente", "Data Abertura", "Status" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var prontuario in this.prontuarios)
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
        
        tela.Pausar();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Prontuario");
        Console.WriteLine();
        
        // tem pacientes
        if (pacienteCRUD == null || pacienteCRUD.GetPacientes().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar prontuario: nenhum paciente cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um paciente antes de criar prontuarios.");
            tela.Pausar();
            return;
        }
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        if (tela.ConfirmarAcao("\nConfirma o cadastro (S/N)? "))
        {
            this.prontuarios.Add(new Prontuario(novoId, this.prontuario.idDoPaciente, this.prontuario.dataAbertura, this.prontuario.ativo));
            tela.ExibirSucesso("Prontuario cadastrado com sucesso!");
        }
        
        tela.Pausar();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Prontuario");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do prontuario a alterar: ");
        if (!int.TryParse(idInput, out this.prontuario.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Prontuario não encontrado.");
        }
        else
        {
            Console.WriteLine("\nDados atuais:");
            this.MostrarDados();
            
            Console.WriteLine("Novos dados:");
            this.EntrarDados();
            
            if (tela.ConfirmarAcao("\nConfirma a alteração (S/N)? "))
            {
                this.prontuarios[this.indice] = new Prontuario(this.prontuario.id, this.prontuario.idDoPaciente, this.prontuario.dataAbertura, this.prontuario.ativo);
                tela.ExibirSucesso("Prontuario alterado com sucesso!");
            }
        }
        
        tela.Pausar();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Prontuario");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do prontuario a excluir: ");
        if (!int.TryParse(idInput, out this.prontuario.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Prontuario não encontrado.");
        }
        else
        {
            Console.WriteLine("\nProntuario a ser excluído:");
            this.MostrarDados();
            
            if (tela.ConfirmarAcao("Confirma a exclusão (S/N)? "))
            {
                this.prontuarios.RemoveAt(this.indice);
                tela.ExibirSucesso("Prontuario excluído com sucesso!");
            }
        }
        
        tela.Pausar();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Prontuario");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do prontuario: ");
        if (!int.TryParse(idInput, out this.prontuario.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Prontuario não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO PRONTUÁRIO ===");
            this.MostrarDados();
    
            var historicos = historicoClinicoCRUD.BuscarPorProntuario(this.prontuario.id);
            if (historicos.Count > 0)
            {
                Console.WriteLine("\n=== HISTÓRICO CLÍNICO ===");
                string[] cabecalhos = { "Data", "Diagnóstico", "Tratamento", "Peso", "Temp" };
                List<string[]> dados = new List<string[]>();
                
                foreach (var historico in historicos)
                {
                    string diagnosticoTruncado;
                    if (historico.diagnostico.Length > 20)
                    {
                        diagnosticoTruncado = historico.diagnostico.Substring(0, 20) + "...";
                    }
                    else
                    {
                        diagnosticoTruncado = historico.diagnostico;
                    }
                    
                    string tratamentoTruncado;
                    if (historico.tratamento.Length > 20)
                    {
                        tratamentoTruncado = historico.tratamento.Substring(0, 20) + "...";
                    }
                    else
                    {
                        tratamentoTruncado = historico.tratamento;
                    }
                    
                    dados.Add(new string[] {
                        historico.dataAtendimento.ToString("dd/MM/yyyy"),
                        diagnosticoTruncado,
                        tratamentoTruncado,
                        $"{historico.pesoAtual} kg",
                        historico.temperatura
                    });
                }
                
                tela.ExibirTabela(cabecalhos, dados);
                
                Console.WriteLine($"\nTotal de atendimentos: {historicos.Count}");
            }
            else
            {
                Console.WriteLine("\n=== HISTÓRICO CLÍNICO ===");
                Console.WriteLine("Nenhum histórico clínico encontrado para este prontuario.");
            }

        }
        
        tela.Pausar();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        // valida id do paciente
        while (true)
        {
            string idPacienteInput = tela.Perguntar("ID do Paciente: ");
            if (!int.TryParse(idPacienteInput, out this.prontuario.idDoPaciente))
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
                    if (listaPacientes[i].id == this.prontuario.idDoPaciente)
                    {
                        pacienteExiste = true;
                        break;
                    }
                }
                if (!pacienteExiste)
                {
                    tela.ExibirErro($"Paciente com ID {this.prontuario.idDoPaciente} não encontrado!");
                    
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
                    }
                    Console.WriteLine();
                    continue;
                }
            }
            break;
        }
        
        string dataInput = tela.Perguntar("Data de Abertura (dd/mm/aaaa) ou Enter para hoje: ");
        if (string.IsNullOrWhiteSpace(dataInput))
        {
            this.prontuario.dataAbertura = DateTime.Now;
        }
        else if (!DateTime.TryParse(dataInput, out this.prontuario.dataAbertura))
        {
            tela.ExibirErro("Data inválida! Usando data atual.");
            this.prontuario.dataAbertura = DateTime.Now;
        }
        
        string ativoInput = tela.Perguntar("Ativo (S/N): ");
        this.prontuario.ativo = ativoInput.ToLower() == "s" || ativoInput.ToLower() == "sim";
    }

    public bool ProcurarCodigo()
    {
        this.indice = -1;
        bool encontrei = false;
        for (int i = 0; i < this.prontuarios.Count; i++)
        {
            if (this.prontuario.id == this.prontuarios[i].id)
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
        if (this.indice < 0 || this.indice >= this.prontuarios.Count)
        {
            Console.WriteLine("Erro: índice inválido.");
            return;
        }
        
        Console.WriteLine("Prontuario encontrado:");
        Console.WriteLine($"ID: {this.prontuarios[this.indice].id}");
        Console.WriteLine($"ID do Paciente: {this.prontuarios[this.indice].idDoPaciente}");
        Console.WriteLine($"Data de Abertura: {this.prontuarios[this.indice].dataAbertura:dd/MM/yyyy}");
        string statusProntuario;
        if (this.prontuarios[this.indice].ativo)
        {
            statusProntuario = "Ativo";
        }
        else
        {
            statusProntuario = "Inativo";
        }
        Console.WriteLine($"Status: {statusProntuario}");
        Console.WriteLine();
    }

    public Prontuario BuscarPorPaciente(int idPaciente)
    {
        for (int i = 0; i < this.prontuarios.Count; i++)
        {
            if (this.prontuarios[i].idDoPaciente == idPaciente && this.prontuarios[i].ativo)
            {
                return this.prontuarios[i];
            }
        }
        return null;
    }

    public void CriarAutomatico(int idPaciente)
    {
        // tem prontuario ativo pra esse paciente
        if (BuscarPorPaciente(idPaciente) == null)
        {
            int novoId = this.GerarNovoId();
            this.prontuarios.Add(new Prontuario(novoId, idPaciente, DateTime.Now, true));
        }
    }
}
