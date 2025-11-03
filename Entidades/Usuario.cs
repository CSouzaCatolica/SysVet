public class Usuario
{
    public int id;
    public string nome;
    public string login;
    public string senha;
    public string tipoUsuario;

    public Usuario()
    {
        this.id = 0;
        this.nome = "";
        this.login = "";
        this.senha = "";
        this.tipoUsuario = "";
    }

    public Usuario(int id, string nome, string login, string senha, string tipoUsuario)
    {
        this.id = id;
        this.nome = nome;
        this.login = login;
        this.senha = senha;
        this.tipoUsuario = tipoUsuario;
    }
}
