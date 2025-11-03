public class Vacina
{
    public int id;
    public string nome;
    public int periodicidade;
    public int quantidadeEstoque;

    public Vacina()
    {
        this.id = 0;
        this.nome = "";
        this.periodicidade = 0;
        this.quantidadeEstoque = 0;
    }

    public Vacina(int id, string nome, int periodicidade)
    {
        this.id = id;
        this.nome = nome;
        this.periodicidade = periodicidade;
        this.quantidadeEstoque = 0;
    }

    public Vacina(int id, string nome, int periodicidade, int quantidadeEstoque)
    {
        this.id = id;
        this.nome = nome;
        this.periodicidade = periodicidade;
        this.quantidadeEstoque = quantidadeEstoque;
    }
}
