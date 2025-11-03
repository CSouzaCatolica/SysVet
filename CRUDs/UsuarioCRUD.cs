public class UsuarioCRUD
{
    private List<Usuario> usuarios;
    private Usuario usuario;
    private int indice;
    private VeterinarioCRUD veterinarioCRUD;

    public UsuarioCRUD()
    {
        this.usuarios = new List<Usuario>();
        this.usuario = new Usuario();
        this.indice = -1;
        this.veterinarioCRUD = null;
    }

    public List<Usuario> GetUsuarios()
    {
        return this.usuarios;
    }

    public void SetVeterinarioCRUD(VeterinarioCRUD veterinarioCRUD)
    {
        this.veterinarioCRUD = veterinarioCRUD;
    }

    public void ExecutarCRUD()
    {
        Tela tela = new Tela();
        char opcao;
        List<char> opcoesValidas = new List<char> { '1', '2', '3', '4', '0' };

        while (true)
        {
            tela.LimparTela();
            Console.WriteLine("Cadastro de Usuários");
            Console.WriteLine();
            
            tela.ExibirOpcao("1", "Listar Todos");
            tela.ExibirOpcao("2", "Cadastrar Novo");
            tela.ExibirOpcao("3", "Alterar");
            tela.ExibirOpcao("4", "Excluir");
            tela.ExibirOpcao("0", "Voltar");
            tela.ExibirLinhaVazia();
            
            opcao = tela.LerOpcaoValida(opcoesValidas);

            if (opcao == '0') break;
            if (opcao == '1') this.ListarTodos();
            if (opcao == '2') this.Cadastrar();
            if (opcao == '3') this.Alterar();
            if (opcao == '4') this.Excluir();
        }
    }

    public int GerarNovoId()
    {
        return this.usuarios.Count + 1;
    }

    public void ListarTodos()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Lista de Usuários");
        Console.WriteLine();
        
        if (this.usuarios.Count == 0)
        {
            Console.WriteLine("Nenhum usuário cadastrado.");
        }
        else
        {
            foreach (var usuario in this.usuarios)
            {
                string statusVeterinario = "Não";
                if (veterinarioCRUD != null)
                {
                    var veterinario = veterinarioCRUD.GetVeterinarios().FirstOrDefault(v => v.idDoUsuario == usuario.id);
                    statusVeterinario = veterinario != null ? "Sim" : "Não";
                }
                Console.WriteLine($"ID: {usuario.id} | Nome: {usuario.nome} | Login: {usuario.login} | Tipo: {usuario.tipoUsuario} | É Veterinário: {statusVeterinario}");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Cadastrar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Cadastrar Novo Usuário");
        Console.WriteLine();
        
        int novoId = this.GerarNovoId();
        this.EntrarDados();
        
        string confirma = tela.Perguntar("\nConfirma o cadastro (S/N)? ");
        if (confirma.ToLower() == "s")
        {
            this.usuarios.Add(new Usuario(novoId, this.usuario.nome, this.usuario.login, this.usuario.senha, this.usuario.tipoUsuario));
            Console.WriteLine("Usuário cadastrado com sucesso!");
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Alterar()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Alterar Usuário");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do usuário a alterar: ");
        if (!int.TryParse(idInput, out this.usuario.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Usuário não encontrado.");
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
                this.usuarios[this.indice] = new Usuario(this.usuario.id, this.usuario.nome, this.usuario.login, this.usuario.senha, this.usuario.tipoUsuario);
                Console.WriteLine("Usuário alterado com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void Excluir()
    {
        Tela tela = new Tela();
        tela.LimparTela();
        
        Console.WriteLine("Excluir Usuário");
        Console.WriteLine();
        
        string idInput = tela.Perguntar("Digite o ID do usuário a excluir: ");
        if (!int.TryParse(idInput, out this.usuario.id))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            Console.WriteLine("Usuário não encontrado.");
        }
        else
        {
            Console.WriteLine("\nUsuário a ser excluído:");
            this.MostrarDados();
            
            string confirma = tela.Perguntar("Confirma a exclusão (S/N)? ");
            if (confirma.ToLower() == "s")
            {
                this.usuarios.RemoveAt(this.indice);
                Console.WriteLine("Usuário excluído com sucesso!");
            }
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public void EntrarDados()
    {
        Tela tela = new Tela();
        
        this.usuario.nome = tela.Perguntar("Nome: ");
        this.usuario.login = tela.Perguntar("Login: ");
        this.usuario.senha = tela.Perguntar("Senha: ");
        this.usuario.tipoUsuario = tela.Perguntar("Tipo de Usuário: ");
    }

    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.usuarios.Count; i++)
        {
            if (this.usuario.id == this.usuarios[i].id)
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
        Console.WriteLine("Usuário encontrado:");
        Console.WriteLine($"Nome: {this.usuarios[this.indice].nome}");
        Console.WriteLine($"Login: {this.usuarios[this.indice].login}");
        Console.WriteLine($"Senha: {this.usuarios[this.indice].senha}");
        Console.WriteLine($"Tipo de Usuário: {this.usuarios[this.indice].tipoUsuario}");
        Console.WriteLine();
    }
}
