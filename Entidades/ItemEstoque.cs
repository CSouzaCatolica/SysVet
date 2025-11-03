public class ItemEstoque
{
    public int id;
    public int idDoMedicamento;
    public string lote;
    public int quantidade;
    public DateTime dataValidade;

    public ItemEstoque()
    {
        this.id = 0;
        this.idDoMedicamento = 0;
        this.lote = "";
        this.quantidade = 0;
        this.dataValidade = DateTime.Now;
    }

    public ItemEstoque(int id, int idDoMedicamento, string lote, int quantidade, DateTime dataValidade)
    {
        this.id = id;
        this.idDoMedicamento = idDoMedicamento;
        this.lote = lote;
        this.quantidade = quantidade;
        this.dataValidade = dataValidade;
    }
}
