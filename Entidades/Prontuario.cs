public class Prontuario
{
    public int id;
    public int idDoPaciente;
    public DateTime dataAbertura;
    public bool ativo;

    public Prontuario()
    {
        this.id = 0;
        this.idDoPaciente = 0;
        this.dataAbertura = DateTime.Now;
        this.ativo = true;
    }

    public Prontuario(int id, int idDoPaciente, DateTime dataAbertura, bool ativo)
    {
        this.id = id;
        this.idDoPaciente = idDoPaciente;
        this.dataAbertura = dataAbertura;
        this.ativo = ativo;
    }
}
