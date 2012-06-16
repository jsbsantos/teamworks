Web Api 
=

\label{sec:api}

A Api assenta sobre modelo de arquitectura ReSTful e todos os objectos da aplicação são considerados recursos e têm URL definido. 
A comunicação é feita utilizando o protocolo HTTP.

Em todos os pedidos à Api é comum a necessidade de aceder ao repositório de dados, para isso é usada uma instância de sessão do cliente RavenDB.
A instância é criada durante o processamento do pedido e no final deste as alterações feitas à sessão são persistidas na base de dados pela classe `RavenHandler`. Esta classe é adicionada ao pipeline HTTP e verifica se não ocorreu nenhum erro no processamento do pedido, persistindo as alterações em caso de sucesso, como demonstra o troço da lista \ref{code:savechanges}. 

\lstset{caption={Persistência das alterações à sessão RavenDB.},label={code:savechanges}}

````
using (var session = Global.Raven.CurrentSession) {  
  if (session != null && t.Result.IsSuccessStatusCode) {  
    session.SaveChanges();  
  }  
}  
````

Para abstrair os *controllers* da criação e obtenção da sessão estes implementam a classe `RavenApiController` que disponibiliza a propriedade `DbSession`. A classe `DbSession` retorna a sessão associada ao pedido actual.
O acesso a esta propriedade tem como resultado a chamada à propriedade `Global.Raven.CurrentSession` que retorna a sessão associada ao pedido ou cria uma nova caso ainda não tenha sido criada.

Segurança
- 

A autenticação na Api é feita utilizando HTTP *Basic authentication*, sendo as credenciais do utilizador enviadas convertidas em *Base64*. 
O pedido ao passar pelo *pipeline* de processamento de pedidos HTTP é analisado por uma instância da classe `BasicAuthenticationHandler`, que verifica se o *header* de nome `Authorization` tem como esquema de autenticação *Basic* e converte as credenciais para a forma original, `nome-de-utilizador:password`. 
A validação das credenciais enviadas para o servidor utiliza o processo descrito no domínio (ver secção \ref{sec:dominio}).

Se o utilizador não preencher o *header* de autenticação quando necessário o código da resposta é `401 Unauthorized`.


