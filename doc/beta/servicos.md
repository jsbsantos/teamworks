Serviços
=

\label{sec:servicos}

Os serviços disponibilizados pela infra-estrutura encontram-se no *namespace* `Teamworks.Core` e incluem serviços de acesso a dados e autenticação.

Acesso a Dados
-

\label{acesso-a-dados}

É comum para acesso a dados a utilização do padrão *Repository* \cite[pp.~322-327]{patterns}. Este padrão encapsula a lógica de persistência dos objectos de domínio de forma a desacoplar o modelo de domínio da lógica de acesso a dados. 

O cliente RavenDB implementa este padrão para comunicação com o servidor o que torna desnecessário a implementação deste padrão na solução da plataforma.

A inicialização e configuração do cliente é feita pela classe `Raven`, uma implementação do padrão *Singleton*, para garantir que todas as sessões são criadas a partir do mesmo cliente.
A cargo desta classe está também a criação de sessões que permitem a obtenção de dados da base de dados.

Os dados podem ser obtidos através do identificador do documento (utilizando o método `Load<T>`), da utilização de um índice ou fazendo uma *query* (método `Query`).

O código da plataforma obtém sessões do cliente através da propriedade `Global.Raven.CurrentSession` que abstrai a forma como é obtida e guardada a sessão actual. 

Segurança
- 

A plataforma impõe as políticas de acesso em conjunto com o *Authorization bundle*.
A autorização é feita quando se tenta aceder a um documento. Se um utilizador tentar aceder a um documento e não tiver permissões para o fazer é lançada uma excepção.

A configuração do cliente, para que este valide se é possível interagir com o documento, é feita utilizando métodos de extensão presentes no ficheiro `Raven.Client.Authorization.dll`.
