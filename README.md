# API WEB DO ASP .NET CORE 6 
## ChallengeBRQ - Usuarios

# 📌 Tópicos

<!--ts-->
- [Visão geral](###-Visão-geral)
- [Pré-requisitos](###-Pré-requisitos)
- [Instalação](###-Instalação)
- [Exemplar de Usuario](###-Exemplar-de-Usuario)
- [Endpoints](###-Endpoints)
- [Suporte](###-Suporte)
- [Histórico de versões](###-Histórico-de-versões)

<!--te-->
### Visão geral

O objetivo geral do projeto `.NET API de gerenciamento de usuário` é fornecer um microsserviço confiável e escalável para gerenciar as operações básicas relacionadas a usuários, como criação, atualização, detalhamento, listagem e exclusão de usuários.

### Pré-requisitos
- SDK do .NET 6: o SDK (kit de desenvolvimento de software) 
- IDE: você precisará de uma IDE (ambiente de desenvolvimento integrado)
- Gerenciador de pacotes: o .NET 6 usa o gerenciador de pacotes NuGet

### Instalação
- Execute o comando `dotnet restore` para restaurar as dependências do projeto. Isso baixará e instalará todas as bibliotecas e pacotes necessários para a aplicação funcionar corretamente.

Finalmente, execute o comando `dotnet run` para iniciar a aplicação. Isso iniciará o servidor web da aplicação e você poderá acessar a aplicação no seu navegador web em http://localhost..., onde uma nova página no `Swagger` será gerada para identificar os endpoints e necessidades dos dados.

### Exemplar de Usuario
```
{
  "Cpf": "09282249500",
  "email": "ryanpablosilvaaraujo@gmail.com",
  "DataDeNascimento": "2002-11-11",
  "sexo": 1,
  "nomeCompleto": "Ryan Pablo Silva Araujo",
  "senha": "ryan@1",
  "apelido": "fazopix",
  "telefone": "99822912",
  "endereco": {
    "logradouro": "Rua manoel alves texeira",
    "numero": "199",
    "bairro": "osorio ferraz",
    "cidade": "vitoria da conquista",
    "estado": "ba",
    "pais": "Brasil",
    "cep": "45140000"
  }
}
```

### Endpoints
`POST` - /challengebrq/v1/usuarios

### Suporte
[Linkedin](https://www.linkedin.com/in/ryanpsa/)

### Histórico de versões
- Versão 1 - [Em andamento]
