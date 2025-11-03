public class Veterinario
{
    public int id;
    public int idDoUsuario;
    public string nome;
    public string crmv;
    public string especialidade;

    public Veterinario()
    {
        this.id = 0;
        this.idDoUsuario = 0;
        this.nome = "";
        this.crmv = "";
        this.especialidade = "";
    }

    public Veterinario(int id, int idDoUsuario, string nome, string crmv, string especialidade)
    {
        this.id = id;
        this.idDoUsuario = idDoUsuario;
        this.nome = nome;
        this.crmv = crmv;
        this.especialidade = especialidade;
    }
}
