public class Medicamento
{
    public int id;
    public string nome;
    public bool controlado;
    public int estoqueMinimo;
    public int quantidadeEstoque;

    public Medicamento()
    {
        this.id = 0;
        this.nome = "";
        this.controlado = false;
        this.estoqueMinimo = 0;
        this.quantidadeEstoque = 0;
    }

    public Medicamento(int id, string nome, bool controlado, int estoqueMinimo)
    {
        this.id = id;
        this.nome = nome;
        this.controlado = controlado;
        this.estoqueMinimo = estoqueMinimo;
        this.quantidadeEstoque = 0;
    }

    public Medicamento(int id, string nome, bool controlado, int estoqueMinimo, int quantidadeEstoque)
    {
        this.id = id;
        this.nome = nome;
        this.controlado = controlado;
        this.estoqueMinimo = estoqueMinimo;
        this.quantidadeEstoque = quantidadeEstoque;
    }
}
