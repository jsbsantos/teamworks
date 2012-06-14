Web Api 
=

\label{sec:api}

De forma a disponibilizar aos utilizadores uma forma de interagir e integrar aplicações externas com a infra-estrutura foi desenvolvida um serviço web disponibilizado através de uma api web. 

A api web assenta sobre modelo de arquitectura ReST^[Representational state transfer] em que todos os objectos da aplicação são considerados recursos e têm URL definido. 
A comunicação é feita utilizando o protocolo HTTP[#http]().

Em todos os pedidos à api é comum a necessidade de aceder ao repositório de dados, para isso é usada uma instância de sesssão do cliente RavenDB.
A instância é criada durante o processamento do pedido e no final as alterações feitas à sessão são persistidas na base de dados pela classe `RavenHandler`. Esta classe é adicionada ao pipeline e HTTP, verifica se não ocorreu nenhum erro no processamento do pedido e persiste as alterações em caso de sucesso, como demonstra o troço da lista [savechanges](). 

````[Persistência das alterações à sessão RavenDB.](savechanges)
using (var session = Global.Raven.CurrentSession) {  
  if (session != null && t.Result.IsSuccessStatusCode) {  
    session.SaveChanges();  
  }  
}  
````

Para abstrair os *controllers* da criação e obtenção da sessão estes implementam a classe `RavenApiController` que diponibiliza a propriedade `DbSession`. A classe `DbSession` retorna a sessão associada ao pedido actual.
O acesso a esta propriedade tem como resultado a chamada à propriedade `Global.Raven.CurrentSession` que retorna a sessão associada ao pedido ou cria uma nova caso ainda não tenha sido criada.

Segurança
-

A autenticação na api é feita utilizando HTTP *Basic authentication*[#basic]() em que as credenciais do utilizador são enviadas convertidas em *Base64*. 
O pedido ao passar pelo *pipeline* de processamento de pedidos HTTP é tratado por uma instância da classe `BasicAuthenticationHandler` que verifica se o *header* de nome *Authorization*^[http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.8] tem como esquema de autenticação *Basic* e converte as credencias para a forma original, `nome-de-utilizador:password`. A validação das credenciais enviadas para o servidor utiliza o serviço de autenticação (ver secção [sec:impl-autenticacao]()).

Se o utilizador não preencher o *header* de autenticação quando necessário o código da resposta é `401 Unauthorized`.