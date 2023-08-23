using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace Domain.Entity
{
    public class Usuario
    {
        public Usuario()
        {
            Id = Guid.NewGuid();
            EnderecoId = Guid.NewGuid();
            CodigoSeguranca = null;
            DataHoraCodigoSeguranca = null;
            DataCadastro = DateTime.Now;
            DataAtualizacao = null;
        }

        private DateTime dataDeNascimento;

        [Key]
        public Guid Id { get; set; }

        public Guid EnderecoId { get; set; }
        public DateTime DataDeNascimento 
        {
            get { return dataDeNascimento; }
            private set { dataDeNascimento = value; }
        }

        public void DefinirDataDeNascimento(string novaData)
        {
                this.dataDeNascimento = ConverterDataEmFomatoYYYMMDD(novaData);
        }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public string Senha { get; set; }
        public string Apelido { get; set; }
        public string Telefone { get; set; }
        
        public Guid? CodigoSeguranca { get; set; }
        
        public DateTime? DataHoraCodigoSeguranca { get; set; }
        public DateTime DataCadastro { get; set; }

        
        public DateTime? DataAtualizacao { get; set; }
        public int Sexo { get; set; }

        
        public Endereco Endereco { get; set; }

        private DateTime ConverterDataEmFomatoYYYMMDD(string dataInput)
        {
            return DateTime.ParseExact(dataInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public void AtrelarEnderecoAoUsuario(string cep, string bairro, string logradouro, string localidade, string uF, string? complemento, string? numero)
        {
            Endereco _endereco = new Endereco();
            _endereco.Id = EnderecoId;
            _endereco.Logradouro = logradouro;
            _endereco.Bairro = bairro;
            _endereco.Numero = numero;
            _endereco.Cep = cep;
            _endereco.Estado = uF;
            _endereco.Complemento = complemento;
            _endereco.Pais = "BR";
            _endereco.Cidade = localidade;

            this.Endereco = _endereco;
        }
    }
}
