public class VeterinarioCRUD
{
    private List<Veterinario> veterinarios;
    private Veterinario veterinario;
    private int indice;
    private UsuarioCRUD usuarioCRUD;
    private HistoricoClinicoCRUD historicoClinicoCRUD;

    public VeterinarioCRUD()
    {
        this.veterinarios = new List<Veterinario>();
        this.veterinario = new Veterinario();
        this.indice = -1;
        this.usuarioCRUD = null;
        this.historicoClinicoCRUD = null;
    }

    private Usuario BuscarUsuarioPorId(int id)
    {
        if (usuarioCRUD == null)
        {
            return null;
        }
        
        List<Usuario> listaUsuarios = usuarioCRUD.GetUsuarios();
        for (int i = 0; i < listaUsuarios.Count; i++)
        {
            if (listaUsuarios[i].id == id)
            {
                return listaUsuarios[i];
            }
        }
        return null;
    }

    private void OrdenarHistoricoPorDataDesc(List<HistoricoClinico> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - 1 - i; j++)
            {
                if (lista[j].dataAtendimento < lista[j + 1].dataAtendimento)
                {
                    HistoricoClinico temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }
    }

    public void SetUsuarioCRUD(UsuarioCRUD usuarioCRUD)
    {
        this.usuarioCRUD = usuarioCRUD;
    }

    public void SetHistoricoClinicoCRUD(HistoricoClinicoCRUD historicoClinicoCRUD)
    {
        this.historicoClinicoCRUD = historicoClinicoCRUD;
    }

    public List<Veterinario> GetVeterinarios()
    {
        return this.veterinarios;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '5', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de veterinarios");
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
        return this.veterinarios.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de veterinarios");
        Console.WriteLine();
        
        if (this.veterinarios.Count == 0)
        {
            Console.WriteLine("Nenhum veterinario cadastrado.");
        }
        else
        {
            // usa tabela pra ver melhor
            string[] cabecalhos = { "ID", "Nome", "CRMV", "Especialidade", "usuario Vinculado" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var veterinario in this.veterinarios)
            {
                string nomeUsuario = "N/A";
                if (usuarioCRUD != null)
                {
                    Usuario usuario = BuscarUsuarioPorId(veterinario.idDoUsuario);
                    if (usuario != null)
                    {
                        nomeUsuario = usuario.nome;
                    }
                    else
                    {
                        nomeUsuario = "N/A";
                    }
                }
                
                dados.Add(new string[] {
                    veterinario.id.ToString(),
                    veterinario.nome,
                    veterinario.crmv,
                    veterinario.especialidade,
                    nomeUsuario
                });
            }
            
            tela.ExibirTabela(cabecalhos, dados);
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo veterinario");
        Console.WriteLine();
        
        // valida se tem usuarios
        if (usuarioCRUD == null || usuarioCRUD.GetUsuarios().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar veterinario: nenhum usuario cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um usuario antes de criar veterinarios.");
            tela.Pausar();
            return;
        }
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.veterinarios.Add(new Veterinario(novoId, this.veterinario.idDoUsuario, this.veterinario.nome, this.veterinario.crmv, this.veterinario.especialidade));
            Console.WriteLine("veterinario cadastrado com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar veterinario");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do veterinario a alterar: ");
        if (!int.TryParse(idInput, out this.veterinario.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("veterinario não encontrado.");
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
                this.veterinarios[this.indice] = new Veterinario(this.veterinario.id, this.veterinario.idDoUsuario, this.veterinario.nome, this.veterinario.crmv, this.veterinario.especialidade);
                Console.WriteLine("veterinario alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir veterinario");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do veterinario a excluir: ");
        if (!int.TryParse(idInput, out this.veterinario.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("veterinario não encontrado.");
        }
        else
        {
            Console.WriteLine("\nveterinario a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.veterinarios.RemoveAt(this.indice);
                Console.WriteLine("veterinario excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        // valida id do usuario
        while (true)
        {
            string idUsuarioInput = tela.Perguntar("ID do usuario: ");
            if (!int.TryParse(idUsuarioInput, out this.veterinario.idDoUsuario))
            {
                tela.ExibirErro("ID do usuario inválido! Digite um número.");
                continue;
            }
            
            // verifica se o usuario existe
            if (usuarioCRUD != null)
            {
                bool usuarioExiste = false;
                List<Usuario> listaUsuarios = usuarioCRUD.GetUsuarios();
                for (int i = 0; i < listaUsuarios.Count; i++)
                {
                    if (listaUsuarios[i].id == this.veterinario.idDoUsuario)
                    {
                        usuarioExiste = true;
                        break;
                    }
                }
                if (!usuarioExiste)
                {
                    tela.ExibirErro($"usuario com ID {this.veterinario.idDoUsuario} não encontrado!");
                    
                    // mostra os usuarios que tem
                    Console.WriteLine("\nusuarios disponíveis:");
                    if (usuarioCRUD.GetUsuarios().Count > 0)
                    {
                        string[] cabecalhos = { "ID", "Nome", "Login", "Tipo" };
                        List<string[]> dados = new List<string[]>();
                        
                        foreach (var usuario in usuarioCRUD.GetUsuarios())
                        {
                            dados.Add(new string[] {
                                usuario.id.ToString(),
                                usuario.nome,
                                usuario.login,
                                usuario.tipoUsuario
                            });
                        }
                        
                        tela.ExibirTabela(cabecalhos, dados);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum usuario cadastrado.");
                    }
                    Console.WriteLine();
                    continue;
                }
            }
            break;
        }
        
        this.veterinario.nome = tela.Perguntar("Nome: ");
        this.veterinario.crmv = tela.Perguntar("CRMV: ");
        this.veterinario.especialidade = tela.Perguntar("Especialidade: ");
    }

    public bool ProcurarCodigo()
    {
        this.indice = -1;
        bool encontrei = false;
        for (int i = 0; i < this.veterinarios.Count; i++)
        {
            if (this.veterinario.id == this.veterinarios[i].id)
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
        if (this.indice < 0 || this.indice >= this.veterinarios.Count)
        {
            Console.WriteLine("Erro: índice inválido.");
            return;
        }
        
        Console.WriteLine("veterinario encontrado:");
        Console.WriteLine($"Nome: {this.veterinarios[this.indice].nome}");
        Console.WriteLine($"CRMV: {this.veterinarios[this.indice].crmv}");
        Console.WriteLine($"Especialidade: {this.veterinarios[this.indice].especialidade}");
        
        string nomeUsuario = "N/A";
        if (usuarioCRUD != null)
        {
            Usuario usuario = BuscarUsuarioPorId(this.veterinarios[this.indice].idDoUsuario);
            if (usuario != null)
            {
                nomeUsuario = usuario.nome;
            }
            else
            {
                nomeUsuario = "N/A";
            }
        }
        Console.WriteLine($"usuario: {nomeUsuario}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do veterinario");
        Console.WriteLine();
        
        this.indice = -1;
        string idInput = tela.Perguntar("Digite o ID do veterinario: ");
        if (!int.TryParse(idInput, out this.veterinario.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("veterinario não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO veterinario ===");
            this.MostrarDados();
            
            var historicos = historicoClinicoCRUD.BuscarPorVeterinario(this.veterinario.id);
            
            Console.WriteLine("\n=== ESTATÍSTICAS DE ATENDIMENTOS ===");
            Console.WriteLine($"Total de Atendimentos: {historicos.Count}");
            
            if (historicos.Count > 0)
            {
                List<HistoricoClinico> historicosOrdenados = new List<HistoricoClinico>();
                for (int i = 0; i < historicos.Count; i++)
                {
                    historicosOrdenados.Add(historicos[i]);
                }
                OrdenarHistoricoPorDataDesc(historicosOrdenados);
                
                if (historicosOrdenados.Count > 0)
                {
                    HistoricoClinico ultimoAtendimento = historicosOrdenados[0];
                    Console.WriteLine($"Último Atendimento: {ultimoAtendimento.dataAtendimento:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Diagnóstico: {ultimoAtendimento.diagnostico}");
                }
                
                Dictionary<int, bool> pacientesUnicos = new Dictionary<int, bool>();
                for (int i = 0; i < historicos.Count; i++)
                {
                    int idProntuario = historicos[i].idDoProntuario;
                    if (!pacientesUnicos.ContainsKey(idProntuario))
                    {
                        pacientesUnicos[idProntuario] = true;
                    }
                }
                int pacientesAtendidos = pacientesUnicos.Count;
                Console.WriteLine($"Pacientes Únicos Atendidos: {pacientesAtendidos}");
                
                Console.WriteLine("\nÚltimos 5 Atendimentos:");
                List<HistoricoClinico> ultimosAtendimentos = new List<HistoricoClinico>();
                int limite = 5;
                if (historicosOrdenados.Count < limite)
                {
                    limite = historicosOrdenados.Count;
                }
                
                for (int i = 0; i < limite; i++)
                {
                    ultimosAtendimentos.Add(historicosOrdenados[i]);
                }
                
                if (ultimosAtendimentos.Count > 0)
                {
                    string[] cabecalhos = { "Data", "Prontuario", "Agendamento", "Diagnóstico" };
                    List<string[]> dados = new List<string[]>();
                    
                    for (int i = 0; i < ultimosAtendimentos.Count; i++)
                    {
                        HistoricoClinico historico = ultimosAtendimentos[i];
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
                            historico.dataAtendimento.ToString("dd/MM/yyyy HH:mm"),
                            historico.idDoProntuario.ToString(),
                            historico.idDoAgendamento.ToString(),
                            diagnosticoTruncado
                        });
                    }
                    
                    tela.ExibirTabela(cabecalhos, dados);
                }
            }
            else
            {
                Console.WriteLine("Nenhum atendimento registrado ainda.");
            }
        }
        
        tela.Pausar();
    }
}
