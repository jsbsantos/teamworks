Serviços
=

\label{sec:servicos}

Os serviços disponibilizados pela infra-estrutura encontram-se no *namespace* `Teamworks.Core` e incluem serviços de acesso a dados e autenticação.

Acesso a Dados
-

\label{acesso-a-dados}

É comum para acesso a dados a utilização do padrão *Repository* \cite[pp.~322-327]{patterns}. Este padrão encapsula a lógica de persistência dos objectos de domínio de forma a desacoplar o modelo de domínio da lógica de acesso a dados. Na solução *Teamworks* a implementação do padrão *Repository* é feita pelo cliente *RavenDB*

A inicialização e configuração do cliente (`IDocumentStore`) está presente na propriedade `Database` da classe `Global` e a sua inicialização deve ser feita no ínicio da aplicação.


Segurança
- 

\label{seguranca}

A plataforma impõe as políticas de acesso em conjunto com o *Authorization bundle*.
A autorização é feita quando se tenta aceder a um documento. Se um utilizador tentar aceder a um documento e não tiver permissões para o fazer é lançada uma excepção.

A configuração do cliente, para que este valide se é possível interagir com o documento, é feita utilizando métodos de extensão presentes no `assembly` `Raven.Client.Authorization.dll`.
