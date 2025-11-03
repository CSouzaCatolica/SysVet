public class AplicacaoVacina
{
    public int id;
    public int idDoProntuario;
    public int idDaVacina;
    public string lote;
    public DateTime dataAplicacao;
    public DateTime dataProximaDose;

    public AplicacaoVacina()
    {
        this.id = 0;
        this.idDoProntuario = 0;
        this.idDaVacina = 0;
        this.lote = "";
        this.dataAplicacao = DateTime.Now;
        this.dataProximaDose = DateTime.Now;
    }

    public AplicacaoVacina(int id, int idDoProntuario, int idDaVacina, string lote, DateTime dataAplicacao, DateTime dataProximaDose)
    {
        this.id = id;
        this.idDoProntuario = idDoProntuario;
        this.idDaVacina = idDaVacina;
        this.lote = lote;
        this.dataAplicacao = dataAplicacao;
        this.dataProximaDose = dataProximaDose;
    }
}
