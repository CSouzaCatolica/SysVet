public class Paciente
{
    public int id;
    public int idDoTutor;
    public string nome;
    public string especie;
    public string raca;
    public double peso;
    public string status;

    public Paciente()
    {
        this.id = 0;
        this.idDoTutor = 0;
        this.nome = "";
        this.especie = "";
        this.raca = "";
        this.peso = 0;
        this.status = "";
    }

    public Paciente(int id, int idDoTutor, string nome, string especie, string raca, double peso, string status)
    {
        this.id = id;
        this.idDoTutor = idDoTutor;
        this.nome = nome;
        this.especie = especie;
        this.raca = raca;
        this.peso = peso;
        this.status = status;
    }
}
