public class AplicacaoMedicamento
{
    public int id;
    public int idDoHistorico;
    public int idDoItemEstoque;
    public string dosagem;
    public string via;

    public AplicacaoMedicamento()
    {
        this.id = 0;
        this.idDoHistorico = 0;
        this.idDoItemEstoque = 0;
        this.dosagem = "";
        this.via = "";
    }

    public AplicacaoMedicamento(int id, int idDoHistorico, int idDoItemEstoque, string dosagem, string via)
    {
        this.id = id;
        this.idDoHistorico = idDoHistorico;
        this.idDoItemEstoque = idDoItemEstoque;
        this.dosagem = dosagem;
        this.via = via;
    }
}
