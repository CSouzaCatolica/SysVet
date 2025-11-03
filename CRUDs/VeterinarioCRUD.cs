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
            Console.WriteLine("Cadastro de Veterinários");
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
        
        Console.WriteLine("Lista de Veterinários");
        Console.WriteLine();
        
        if (this.veterinarios.Count == 0)
        {
            Console.WriteLine("Nenhum veterinário cadastrado.");
        }
        else
        {
            // Usar tabela para melhor visualização
            string[] cabecalhos = { "ID", "Nome", "CRMV", "Especialidade", "Usuário Vinculado" };
            List<string[]> dados = new List<string[]>();
            
            foreach (var veterinario in this.veterinarios)
            {
                string nomeUsuario = "N/A";
                if (usuarioCRUD != null)
                {
                    var usuario = usuarioCRUD.GetUsuarios().FirstOrDefault(u => u.id == veterinario.idDoUsuario);
                    nomeUsuario = usuario?.nome ?? "N/A";
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
        
        Console.WriteLine("Cadastrar Novo Veterinário");
        Console.WriteLine();
        
        // Validação preventiva - verificar se existem usuários
        if (usuarioCRUD == null || usuarioCRUD.GetUsuarios().Count == 0)
        {
            tela.ExibirErro("Não é possível cadastrar veterinário: nenhum usuário cadastrado.");
            tela.ExibirAviso("Cadastre pelo menos um usuário antes de criar veterinários.");
            tela.Pausar();
            return;
        }
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.veterinarios.Add(new Veterinario(novoId, this.veterinario.idDoUsuario, this.veterinario.nome, this.veterinario.crmv, this.veterinario.especialidade));
            Console.WriteLine("Veterinário cadastrado com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Veterinário");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do veterinário a alterar: ");
        if (!int.TryParse(idInput, out this.veterinario.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Veterinário não encontrado.");
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
                Console.WriteLine("Veterinário alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Veterinário");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do veterinário a excluir: ");
        if (!int.TryParse(idInput, out this.veterinario.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Veterinário não encontrado.");
        }
        else
        {
            Console.WriteLine("\nVeterinário a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.veterinarios.RemoveAt(this.indice);
                Console.WriteLine("Veterinário excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        // Validação do ID do Usuário
        while (true)
        {
            string idUsuarioInput = tela.Perguntar("ID do Usuário: ");
            if (!int.TryParse(idUsuarioInput, out this.veterinario.idDoUsuario))
            {
                tela.ExibirErro("ID do Usuário inválido! Digite um número.");
                continue;
            }
            
            // Verificar se o usuário existe
            if (usuarioCRUD != null)
            {
                var usuarioExiste = usuarioCRUD.GetUsuarios().Any(u => u.id == this.veterinario.idDoUsuario);
                if (!usuarioExiste)
                {
                    tela.ExibirErro($"Usuário com ID {this.veterinario.idDoUsuario} não encontrado!");
                    
                    // Mostrar usuários disponíveis
                    Console.WriteLine("\nUsuários disponíveis:");
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
                        Console.WriteLine("Nenhum usuário cadastrado.");
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
        Console.WriteLine("Veterinário encontrado:");
        Console.WriteLine($"Nome: {this.veterinarios[this.indice].nome}");
        Console.WriteLine($"CRMV: {this.veterinarios[this.indice].crmv}");
        Console.WriteLine($"Especialidade: {this.veterinarios[this.indice].especialidade}");
        
        string nomeUsuario = "N/A";
        if (usuarioCRUD != null)
        {
            var usuario = usuarioCRUD.GetUsuarios().FirstOrDefault(u => u.id == this.veterinarios[this.indice].idDoUsuario);
            nomeUsuario = usuario?.nome ?? "N/A";
        }
        Console.WriteLine($"Usuário: {nomeUsuario}");
        Console.WriteLine();
    }

    public void VisualizarDetalhes()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Visualizar Detalhes do Veterinário");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do veterinário: ");
        if (!int.TryParse(idInput, out this.veterinario.id))
        {
            tela.ExibirErro("ID inválido!");
            tela.Pausar();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            tela.ExibirErro("Veterinário não encontrado.");
        }
        else
        {
            Console.WriteLine("\n=== DETALHES DO VETERINÁRIO ===");
            this.MostrarDados();
            
            var historicos = historicoClinicoCRUD.BuscarPorVeterinario(this.veterinario.id);
            
            Console.WriteLine("\n=== ESTATÍSTICAS DE ATENDIMENTOS ===");
            Console.WriteLine($"Total de Atendimentos: {historicos.Count}");
            
            if (historicos.Count > 0)
            {
                var ultimoAtendimento = historicos.OrderByDescending(h => h.dataAtendimento).FirstOrDefault();
                if (ultimoAtendimento != null)
                {
                    Console.WriteLine($"Último Atendimento: {ultimoAtendimento.dataAtendimento:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Diagnóstico: {ultimoAtendimento.diagnostico}");
                }
                
                // Contar pacientes únicos atendidos
                var pacientesAtendidos = historicos.Select(h => h.idDoProntuario).Distinct().Count();
                Console.WriteLine($"Pacientes Únicos Atendidos: {pacientesAtendidos}");
                
                // Mostrar últimos 5 atendimentos
                Console.WriteLine("\nÚltimos 5 Atendimentos:");
                var ultimosAtendimentos = historicos.OrderByDescending(h => h.dataAtendimento).Take(5).ToList();
                
                if (ultimosAtendimentos.Count > 0)
                {
                    string[] cabecalhos = { "Data", "Prontuário", "Agendamento", "Diagnóstico" };
                    List<string[]> dados = new List<string[]>();
                    
                    foreach (var historico in ultimosAtendimentos)
                    {
                        dados.Add(new string[] {
                            historico.dataAtendimento.ToString("dd/MM/yyyy HH:mm"),
                            historico.idDoProntuario.ToString(),
                            historico.idDoAgendamento.ToString(),
                            historico.diagnostico.Length > 30 ? historico.diagnostico.Substring(0, 30) + "..." : historico.diagnostico
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
