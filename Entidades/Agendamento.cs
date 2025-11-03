public class Agendamento
{
    public int id;
    public int idDoPaciente;
    public int idDoVeterinario;
    public int idDaSala;
    public DateTime dataHora;
    public int duracao;
    public string tipoProcedimento;
    public string status;
    public string statusDetalhado;

    public Agendamento()
    {
        this.id = 0;
        this.idDoPaciente = 0;
        this.idDoVeterinario = 0;
        this.idDaSala = 0;
        this.dataHora = DateTime.Now;
        this.duracao = 0;
        this.tipoProcedimento = "";
        this.status = "Agendado";
        this.statusDetalhado = "Agendado";
    }

    public Agendamento(int id, int idDoPaciente, int idDoVeterinario, int idDaSala, DateTime dataHora, int duracao, string tipoProcedimento, string status)
    {
        this.id = id;
        this.idDoPaciente = idDoPaciente;
        this.idDoVeterinario = idDoVeterinario;
        this.idDaSala = idDaSala;
        this.dataHora = dataHora;
        this.duracao = duracao;
        this.tipoProcedimento = tipoProcedimento;
        this.status = status;
        this.statusDetalhado = status;
    }

    public Agendamento(int id, int idDoPaciente, int idDoVeterinario, int idDaSala, DateTime dataHora, int duracao, string tipoProcedimento, string status, string statusDetalhado)
    {
        this.id = id;
        this.idDoPaciente = idDoPaciente;
        this.idDoVeterinario = idDoVeterinario;
        this.idDaSala = idDaSala;
        this.dataHora = dataHora;
        this.duracao = duracao;
        this.tipoProcedimento = tipoProcedimento;
        this.status = status;
        this.statusDetalhado = statusDetalhado;
    }

    public bool IsVencido()
    {
        return DateTime.Now > dataHora && statusDetalhado != "Atendido" && statusDetalhado != "Cancelado";
    }
}
