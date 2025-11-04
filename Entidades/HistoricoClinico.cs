public class HistoricoClinico
{
    public int id;
    public int idDoProntuario;
    public int idDoAgendamento;
    public int idDoVeterinario;
    public DateTime dataAtendimento;
    public string diagnostico;
    public string tratamento;
    public string observacoes;
    public string medicamentosAplicados;
    public string vacinasAplicadas;
    public double pesoAtual;
    public string temperatura;
    public string frequenciaCardiaca;

    public HistoricoClinico()
    {
        this.id = 0;
        this.idDoProntuario = 0;
        this.idDoAgendamento = 0;
        this.idDoVeterinario = 0;
        this.dataAtendimento = DateTime.Now;
        this.diagnostico = "";
        this.tratamento = "";
        this.observacoes = "";
        this.medicamentosAplicados = "";
        this.vacinasAplicadas = "";
        this.pesoAtual = 0;
        this.temperatura = "";
        this.frequenciaCardiaca = "";
    }

    public HistoricoClinico(int id, int idDoProntuario, int idDoAgendamento, int idDoVeterinario, DateTime dataAtendimento, string diagnostico, string tratamento, string observacoes): this(id, idDoProntuario, idDoAgendamento, idDoVeterinario, dataAtendimento, diagnostico, tratamento, observacoes, "", "", 0, "", ""){
    }

    public HistoricoClinico(int id, int idDoProntuario, int idDoAgendamento, int idDoVeterinario, DateTime dataAtendimento, string diagnostico, string tratamento, string observacoes, string medicamentosAplicados, string vacinasAplicadas, double pesoAtual, string temperatura, string frequenciaCardiaca)
    {
        this.id = id;
        this.idDoProntuario = idDoProntuario;
        this.idDoAgendamento = idDoAgendamento;
        this.idDoVeterinario = idDoVeterinario;
        this.dataAtendimento = dataAtendimento;
        this.diagnostico = diagnostico;
        this.tratamento = tratamento;
        this.observacoes = observacoes;
        this.medicamentosAplicados = medicamentosAplicados;
        this.vacinasAplicadas = vacinasAplicadas;
        this.pesoAtual = pesoAtual;
        this.temperatura = temperatura;
        this.frequenciaCardiaca = frequenciaCardiaca;
    }
}
