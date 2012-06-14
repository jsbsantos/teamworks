Serviços
=

\label{sec:servicos}

Os serviços disponibilizados pela infra-estrutura encontram-se no *namespace* `Teamworks.Core` e incluem serviço de acesso a dados e autenticação.

Acesso a Dados
-

\label{acesso-a-dados}


Para aceder aos dados da infra-estrutura é utilizado o cliente RavenDB como dito anteriomente. 
A inicialização, configuração e criação de sessões de comunicação com o servidor RavenDB é da responsabilidade desta camada. 
O cliente RavenDB desempenha o papel de repositório de dados devido ás características enunciadas (ver secção [sec:dados]()). E com a sessão do cliente é possível obter dados da base de dados através do identificador do documento, da utilização de um indice ou fazendo uma *query*.

O código da infra-estrutura obtem sessões do cliente através da propriedade `Global.Raven.CurrentSession` que abstrai a forma como é obtida e guardada a sessão currente. 

Segurança
- 

A infra-estrutura disponibiliza serviços para autenticação de utilizadores e impõe as politicas de acesso em conjunto com o *Authorization Bundle*.

### Autenticação

\label{sec:impl-autenticacao}

<span style="background-color=yellow" >Em falta.</span>

### Autorização

A autorização é feita quando um documento é obtido, alterado ou removido. Se um utilizador tentar fazer algumas destas acções e não tiver permissões para o fazer é lançada uma excepção.

A configuração do cliente para que este valide se é possível interagir com o documento é feita utilizando métodos de extensão ao cliente presentes no ficheiro `Raven.Client.Authorization.dll`.

 
