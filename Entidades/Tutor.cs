public class Tutor
{
    public int id;
    public string nome;
    public string cpf;
    public string telefone;
    public string email;
    public string endereco;

    public Tutor()
    {
        this.id = 0;
        this.nome = "";
        this.cpf = "";
        this.telefone = "";
        this.email = "";
        this.endereco = "";
    }

    public Tutor(int id, string nome, string cpf, string telefone, string email, string endereco)
    {
        this.id = id;
        this.nome = nome;
        this.cpf = cpf;
        this.telefone = telefone;
        this.email = email;
        this.endereco = endereco;
    }
}
